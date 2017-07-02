using System;
using System.Collections.Generic;
using System.Reflection;
using System.ComponentModel;
using UnityEngine;

namespace BaseLib
{
    public class LanguageMgr : Singleton<LanguageMgr>
	{
        const string strLangPath = "language/";
        const string strLangCfg = "language";
        public static string strLang = "Language";

        Dictionary<string, List<string>> _langFileList = new Dictionary<string, List<string>>();
        Dictionary<string, string> _langList = new Dictionary<string,string>();
        string mLang = "";
        LanguageModuleMgr mDictMgr = new LanguageModuleMgr();

        private LanguageMgr()
        {
        }

        public void release()
        {
            mDictMgr.release();
        }
        
        string language
        {
            get { return mLang; }
        }

        protected override void init()
        {
            //读取配置文件
            if (!loadCfgFile(BaseLib.ResourceCenter.configPath + strLangPath + strLangCfg))
                return;
            //读取语言配置
            //mLang = PlayerPrefs.GetString(strLang);
            if (mLang == null || mLang==string.Empty)
            {
                if (_langList.Count == 0)
                    return;
                foreach (KeyValuePair<string, string> item in this._langList)
                {
                    mLang = item.Key;
                    break;
                }
            }
            this.selectLang(mLang);
        }

        bool selectLang(string lang)
        {
            if (lang == null || lang == string.Empty)
                return false;
            if (!this.initLang(lang))
                return false;
            //保存
            mLang = lang;
            //PlayerPrefs.SetString(strLang,mLang);
            return true;
        }

        static public string getString(string strKey)
        {
            LanguageModule module = LanguageMgr.me.mDictMgr.getModule();
            if (module != null)
                return module.getString(strKey);
            return string.Empty;
        }

        static public string getString(int key)
        {
            LanguageModule module = LanguageMgr.me.mDictMgr.getModule();
            if (module != null)
                return module.getString(key);
            return string.Empty;
        }

        public string getString(string strKey,string strModule)
        {
            LanguageModule module = mDictMgr.getModule(strModule);
            if (module != null)
                return module.getString(strKey);
            return string.Empty;
        }

        public string getString(int key, string strModule)
        {
            LanguageModule module = mDictMgr.getModule(strModule);
            if (module != null)
                return module.getString(key);
            return string.Empty;
        }

        //读取配置文件
        bool loadCfgFile(string strPath)
        {
            iniReader reader = new iniReader();
            if (!reader.loadFile(strPath))
                return false;
            reader.parseIni();
            IniSection langList;
            if (!reader.mIniDict.TryGetValue(strLang,out langList))
                return false;
            foreach (KeyValuePair<string,string> item in langList.mDict)
            {
                //默认值
                if (item.Key.Equals("default"))
                {
                    mLang = item.Value;
                    continue;
                }
                //语言名
                this._langList[item.Key] = item.Value;
                //语言文件列表
                List<string> fileList;
                if (reader.mDict.TryGetValue(item.Key, out fileList))
                {
                    List<string> theFileList = new List<string>();
                    foreach (string file in fileList)
                        theFileList.Add(file);
                    this._langFileList[item.Key] = theFileList;
                }
            }

            reader.release();
            return true;
        }

        bool initLang(string strLang)
        {
            mDictMgr.release();
            if (strLang == null || strLang== string.Empty)
                return false;
            List<string> fileList;
            if (!_langFileList.TryGetValue(strLang, out fileList))
                return false;
            foreach (string strItem in fileList)
            {
                LanguageFile file = new LanguageFile();
                if (file.init(BaseLib.ResourceCenter.configPath + strLangPath + strItem, strLang))
                {
                    this.mDictMgr.addModuleMgr(file.dictMgr);
                    file.release();
                }
            }
            return true;
        }
	}
}
