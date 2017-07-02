#define FORCE_SYSTEM_LOG
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading;
using BaseLib;


#if UNITY_EDITOR
using UnityEditor;
#endif

public class Logger : Singleton<Logger>
{
    // 日志层级
    enum LogLevel
    {
        Error = 0,
        Assert = 1,
        Warning = 2,
        Log = 3,
        Debug = 3,
        Exception = 4,
    }

    public string Filename = "Logger.log";
    public bool IsRemote = false;
    public bool IsAppend = false;

#if !UNITY_ANDROID && !UNITY_IPHONE
    public string encodName = "utf-8";
#endif

    public int PoolSize = 100;

    public string FullPath
    {
        get
        {
#if UNITY_EDITOR
            if (EditorApplication.isPlaying && IsRemote == false)
                return Application.dataPath + "/../" + Filename;
#endif

#if UNITY_ANDROID || UNITY_IPHONE
            return Application.temporaryCachePath + "/" + Filename;
#else
            return Application.dataPath + "/../" + Filename;
#endif
        }
    }

    //bool IsInit = false;
    private Logger()
    {
//         if (IsInit)
//             return;
// 
//         IsInit = true;
        string file = FullPath;
        if (IsAppend == false && File.Exists(file))
            File.Delete(file);
        //ConsoleSelf.me.addText("Log:{0}", file);

        try
        {
#if !UNITY_ANDROID && !UNITY_IPHONE
            m_streamWriter = new StreamWriter(file, IsAppend, System.Text.Encoding.GetEncoding(encodName));
#else
            m_streamWriter = new StreamWriter(file, IsAppend, System.Text.Encoding.UTF8);
#endif
            LogData logData = new LogData();
            logData.time = System.DateTime.Now;
            m_streamWriter.WriteLine(logData.Time);
            m_streamWriter.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            m_streamWriter.WriteLine("+                                                                             +");
            m_streamWriter.WriteLine("+                         Logger singleton created.                           +");
            m_streamWriter.WriteLine("+                                                     	                    +");
            m_streamWriter.WriteLine("+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+-+");
            m_streamWriter.Flush();

            bool bThread = true;
            Application.RegisterLogCallback(LogCallback);

#if UNITY_EDITOR
            bThread = false;
#endif
            if (bThread)
            {
                m_thread = new Thread(UpdateThread);
                m_thread.Start();
            }
        }
        catch (System.Exception ex)
        {
            ConsoleSelf.me.addText("Init Error!{0}", ex.Message);
            IsEnd = true;
        }
    }

    public void release()
    {
        IsEnd = true;
        if (m_thread != null)
        {
            m_thread.Abort();
            m_thread = null;
        }
    }

    ~Logger()
    {
        release();
    }

    // 系统日志回调
    static void LogCallback(string condition, string stackTrace, LogType type)
    {
        if (IsSelf == false)
        {
            Logger.me.Log((LogLevel)type, "condition:{0}, stackTrace:{1}", condition, stackTrace);
        }
    }

    static bool IsSelf = false;
    public static void LogDebug(string format, params object[] objs)
    {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR && !FORCE_SYSTEM_LOG
        Logger.me.Log(LogLevel.Debug, format, objs);
#else
        IsSelf = true;
        Logger.me.Log(LogLevel.Debug, format, objs);
        Debug.Log(string.Format(format, objs));
        IsSelf = false;
#endif
    }

    public static void LogWarning(string format, params object[] objs)
    {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR && !FORCE_SYSTEM_LOG
        Logger.me.Log(LogLevel.Warning, format, objs);
#else
        IsSelf = true;
        Logger.me.Log(LogLevel.Warning, format, objs);
        Debug.LogWarning(string.Format(format, objs));
        IsSelf = false;
#endif
    }

    public static void LogError(string format, params object[] objs)
    {
#if (UNITY_ANDROID || UNITY_IPHONE) && !UNITY_EDITOR && !FORCE_SYSTEM_LOG
        Logger.me.Log(LogLevel.Error, format, objs);
#else
        IsSelf = true;
        Logger.me.Log(LogLevel.Error, format, objs);
        Debug.LogError(string.Format(format, objs));
        IsSelf = false;
#endif
    }

    public void Destroy()
    {
        Application.RegisterLogCallback(null);

        m_streamWriter.Close();
        IsEnd = true;
        m_thread = null;
    }

    bool IsEnd = false;
    void UpdateThread()
    {
        while (!IsEnd)
        {
            if (m_writeList.Count == 0)
            {
                Thread.Sleep(1000);
                continue;
            }

            List<LogData> ldList = null;
            {
                lock (m_writeList)
                {
                    ldList = m_writeList;
                    m_writeList = new List<LogData>();
                }
            }

            string text;
            LogData ld;
            for (int i = 0; i < ldList.Count; ++i)
            {
                ld = ldList[i];
                text = string.Format("tid:{3} {0}:{1} {2}", ld.level, ld.Time, ld.text, ld.threadid);
                m_streamWriter.WriteLine(text);
                ld.text = "";
            }

            lock (m_poolList)
            {
                foreach (LogData l in ldList)
                {
                    m_poolList.Add(l);
                }
            }
            m_streamWriter.Flush();
        }
    }

    StreamWriter m_streamWriter;

    class LogData
    {
        public LogLevel level;
        public string text;
        public DateTime time;
        public int threadid;

        public string Time
        {
            get
            {
                return string.Format("{0}-{1}-{2},{3}:{4}:{5}", time.Year, time.Month, time.Day, time.Hour, time.Minute, time.Second);
            }
        }
    }

    void Log(LogLevel level, string format, params object[] objs)
    {
        LogData ld = GetEmpty();
        ld.level = level;
        ld.text = string.Format(format, objs);
        ld.time = System.DateTime.Now;
        ld.threadid = Thread.CurrentThread.ManagedThreadId;

        lock (m_writeList)
        {
            m_writeList.Add(ld);
        }
    }

    List<LogData> m_poolList = new List<LogData>();
    List<LogData> m_writeList = new List<LogData>();

    LogData GetEmpty()
    {
        {
            lock (m_poolList)
            {
                if (m_poolList.Count != 0)
                {
                    LogData ld = m_poolList[m_poolList.Count - 1];
                    m_poolList.RemoveAt(m_poolList.Count - 1);
                    return ld;
                }
            }
        }

        return new LogData();
    }

    Thread m_thread;
}