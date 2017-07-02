using System;
using System.Collections.Generic;
using System.Threading;

namespace BaseLib
{
    public enum TRHEAD_RESULT
    {
        NORMAL,
        SLEEP,
        STOP
    }

    public delegate TRHEAD_RESULT threadProc();

    class ThreadMgr : Singleton<ThreadMgr>
	{
        List<Thread> _threadList = new List<Thread>();
        static bool bQuit = false;

        private ThreadMgr()
        {

        }

        static public void createThread(threadProc func)
        {
            Thread newThread = new Thread(new ParameterizedThreadStart(ThreadMgr.me.threadMain));
            newThread.Start(func);
            ThreadMgr.me._threadList.Add(newThread);
        }

        public void threadMain(object obj)
        {
            threadProc func = (threadProc)obj;
            if (func == null)
                return;
            while (!bQuit)
            {
                TRHEAD_RESULT ret = func();
                if (ret == TRHEAD_RESULT.SLEEP)
                    Thread.Sleep(10);
                else if (ret == TRHEAD_RESULT.STOP)
                    return;
            }
        }

        static public void release()
        {
            bQuit = true;
            foreach (Thread item in ThreadMgr.me._threadList)
            {
                if (item != null)
                {
                    item.Abort();
                }
            }
            ThreadMgr.me._threadList.Clear();
        }
	}
}
