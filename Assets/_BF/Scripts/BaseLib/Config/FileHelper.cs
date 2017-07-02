using System;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#pragma warning disable 0168
#pragma warning disable 0219
#pragma warning disable 0414
#pragma warning disable 0618
#pragma warning disable 0108
#pragma warning disable 0162
#pragma warning disable 0649
#pragma warning disable 0472
public class CConstance
{
    public const int INEXISTENCE_ID = 0;
    public const int DEFAULT_ID = 0;
    public const int INVALID_ID = -1;
    public const int LEVEL_ID = 1;
    public const int STAR_ID = 0;

    public const string ERROR_STR = "error";
    public const string EMPTY_STR = "";
}

public class FileHelper
{
	#region enum field

	public class stTrunkData
	{
		public List<string> listData;

		public stTrunkData()
		{
			listData = new List<string>();
		}
	}

	#endregion

	#region define variable

	private Dictionary<int,stTrunkData> m_mapInfo = null;
	public Dictionary<int,stTrunkData> mapData{ get{ return m_mapInfo; } }
	public int rowAmount{ get{ return m_mapInfo.Count; } }

	private List<string> m_listColumns = null;
	public List<string> listColumnName{ get{ return m_listColumns; } }
	public int columnAmount{ get{ return m_listColumns.Count; } }

    private char mSplitMark = ',';

    public string PathFile { get; set; }

	#endregion

	#region define function

	public FileHelper()
	{
        _Initialize();
	}

    public FileHelper(char splitMark)
    {
        _Initialize();

        mSplitMark = splitMark;
    }

    void _Initialize()
    {
        m_mapInfo = new Dictionary<int, stTrunkData>();
        m_listColumns = new List<string>();
    }

    public void close()
    {
        mapData.Clear();
        listColumnName.Clear();
    }

    public bool openFile(string strFileName)
    {
        if (strFileName.Equals(""))
            return false;

        try
        {
			FileStream fs = new FileStream(strFileName, System.IO.FileMode.Open, System.IO.FileAccess.Read,System.IO.FileShare.ReadWrite);
            StreamReader sr = new StreamReader(fs, Encoding.Default);
            bool bRet = open(sr);
            sr.Close();
            fs.Close();
            return bRet;
        }
        catch (System.Exception ex)
        {
            return false;
        }
    }

	public bool open(string strFileName)
	{
        if (strFileName.Equals(""))
            return false;

		string strPath = BaseLib.ResourceCenter.configPath + strFileName;

        try
        {
            TextAsset buildText = BaseLib.ResourceCenter.LoadAsset<TextAsset>(strPath.ToLower());
            StreamReader sr = new StreamReader(new MemoryStream(buildText.bytes)/*, Encoding.ASCII*/);
            bool bRet = open(sr);
            sr.Close();
            this.PathFile = strPath;
            return bRet;
        }
        catch (System.Exception ex)
        {
            return false;
        }
	}

    public bool open(StreamReader sr)
    {
        //记录每次读取的一行记录
        string strLine = "";
        //记录读取每一个Trunk数据
        string strTemp = "";
        //标示列数
        int columnCount = 0;
        //标示是否是读取的第一行
        bool IsFirst = true;

        //逐行读取CSV中的数据
        do
        {
            strLine = sr.ReadLine();
            if (strLine == null)
                break;

            if (strLine.Length >= 2 && strLine[0] == '/' && strLine[1] == '/')
                continue;

            strTemp = "";
            if (IsFirst)
            {
                IsFirst = false;

                for (int i = 0; i < strLine.Length; i++)
                {
                    char c = strLine[i];
                    char cEx = '\0'; // 空值
                    if ((i + 1) < strLine.Length)
                        cEx = strLine[i + 1];

                    if (c == ',')
                    {
                        m_listColumns.Add(strTemp);
                        strTemp = "";
                    }
                    else
                        strTemp += c;

                    if ((i == strLine.Length - 1) || (c == 0x0D && cEx == 0x0A) || c == 0x0A)
                    {
                        m_listColumns.Add(strTemp);
                        strTemp = "";
                    }
                }
            }
            else
            {
                stTrunkData trunk = new stTrunkData();

                for (int i = 0; i < strLine.Length; i++)
                {
                    char c = strLine[i];
                    char cEx = '\0'; // 空值
                    if ((i + 1) < strLine.Length)
                        cEx = strLine[i + 1];

                    if (c == mSplitMark || (i == strLine.Length - 1)) // read field   // 
                    {
                        if (i == strLine.Length - 1)
                            strTemp += c;

                        trunk.listData.Add(strTemp);

                        strTemp = "";
                    }
                    else
                        strTemp += c;

                    // add trunk data
                    if ((i == strLine.Length - 1) || (c == 0x0D && cEx == 0x0A) || c == 0x0A)
                    {
                        m_mapInfo.Add(m_mapInfo.Count, trunk);
                    }

                }

            }

        } while (true);

        if (rowAmount == 0)
        {
            return false;
        }
        return true;
    }

	public bool save(string strTitle, string strDirectory, string strExtensionName, string strDefaultName = "")
	{
		if(strDefaultName == "")
		{
			DateTime dt = DateTime.Now;
			strDefaultName = dt.Year.ToString() + "-" + dt.Month.ToString() + "-" + dt.Day.ToString() + "-" +
						dt.Hour.ToString() + ":" + dt.Minute.ToString() + ":" + dt.Second.ToString();
		}

		string strPath = "";
		//#if UNITY_EDITOR
		//strPath = EditorUtility.SaveFilePanel( strTitle, strDirectory, strDefaultName, strExtensionName);
		//#else
		strPath = getAppPath + "/Data/Configs/" + strTitle;
#if UNITY_WEBPLAYER
        bool bRet = false;
        Debug.LogError("Can't save on webplayer");
#else
        bool bRet = save(strPath);
#endif
       
		return bRet;
	}

#if UNITY_WEBPLAYER

#else
    public bool save(string strFile)
    {
        if(this.listColumnName.Count == 0)
            return false;
        
        if(this.m_mapInfo.Count == 0)
            return false;
        
        FileInfo fi = new FileInfo(strFile);
        if (!fi.Directory.Exists)
        {
            fi.Directory.Create();
        }
        
        FileStream fs = new FileStream(strFile, System.IO.FileMode.Create, System.IO.FileAccess.Write);
        
        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.Default);
        //StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);
        StreamWriter sw = new StreamWriter(fs, Encoding.UTF8);
        string data = "";
        
        // write column name
        foreach (string item in this.m_listColumns)
        {
            data += item;
            data += mSplitMark;
        }
        data += 0x0A; // 加换行符
        sw.WriteLine(data);
        
        // write data
        foreach(KeyValuePair<int,stTrunkData> trunkItem in m_mapInfo)
        {
            data = "";
            stTrunkData st = trunkItem.Value;
            
            // each column data
            foreach (var colItem in st.listData)
            {
                if(colItem != null)
                {
                    data += colItem;
                    data += ",";
                }
            }
            
            data += 0x0A; // 加换行符
            sw.WriteLine(data);
        }
        sw.Close();
        fs.Close();
        return true;
    }
#endif


	public bool isHaveColumnName(string strName)
	{
		if(strName.Equals(""))
			return false;

		if (this.listColumnName.Count == 0)
			return false;

		foreach (var item in this.listColumnName)
		{
			if (item.Equals(strName)) 
			{
				return true;
			}
		}

		return false;
	}

	bool _GetRowDataByIndex(int nIndex, ref stTrunkData refData)
	{
		if(nIndex >= this.mapData.Count || nIndex < 0)
			return false;

		bool bRet = this.mapData.ContainsKey(nIndex);
		if(!bRet)
			return false;

		refData = this.mapData[nIndex];

		return bRet;
	}

	bool _GetDataByRowIndexAndColIndex(int nRowIndex, int nColIndex, out string refData)
	{
		bool bRet = false;

        refData = "";
		stTrunkData rowData = new stTrunkData();
        bRet = _GetRowDataByIndex(nRowIndex, ref rowData);
		if(!bRet)
			return false;

		if( nColIndex >= rowData.listData.Count || nColIndex < 0)
			return false;

		for (int i = 0; i < rowData.listData.Count; i++)
		{
			if(i == nColIndex)
			{
				refData = rowData.listData[i];
				return true;
			}
		}

		return false;
	}

    public string getStr(int nRowIndex, int nColIndex, string strDefault = CConstance.ERROR_STR)
    {
        string str = "";
        bool bRet = _GetDataByRowIndexAndColIndex(nRowIndex, nColIndex, out str);
        if (bRet)
            return str;

        return strDefault;
    }

    public int getInt(int nRowIndex, int nColIndex, int nDefault = 0)
    {
        string str = "";
        bool bRet = _GetDataByRowIndexAndColIndex(nRowIndex, nColIndex, out str);
//         if (bRet)
//             return Convert.ToInt32(str);
        if (bRet)
        {
            int n = nDefault;
            bRet = System.Int32.TryParse(str, out n);
            if (bRet)
                return n;
        }

        return nDefault;
    }

    public float getFloat(int nRowIndex, int nColIndex, float fDefault = 0.0f)
    {
        string str = "";
        bool bRet = _GetDataByRowIndexAndColIndex(nRowIndex, nColIndex, out str);
        if (bRet)
        {
            float f = fDefault;
            bRet = System.Single.TryParse(str, out f);
            if(bRet)
                return f;
        }
        return fDefault;
    }

    public bool getBool(int nRowIndex, int nColIndex, bool bIs = false)
    {
        string str = "";
        bool bRet = _GetDataByRowIndexAndColIndex(nRowIndex, nColIndex, out str);
        if (bRet)
        {
            bool b = bIs;
            if (str == "1")
                return true;
            else if(str == "true")
                return true;
            //bRet = System.Boolean.TryParse(str, out b);
            //if (bRet)
            //    return b;
        }
        return bIs;
    }

	static public string getAppPath
	{
		get
		{
			return Application.dataPath;
		}
	}

	// public static void 
	#endregion

}
