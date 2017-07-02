using System;
using System.Collections.Generic;
using UnityEngine;

namespace BaseLib
{
	class LanguageModule
	{
        Dictionary<string, string> mDict = new Dictionary<string, string>();
        string strName = string.Empty;

        public LanguageModule()
        {
        }

        public string name
        {
            get { return strName; }
            set { strName = value; }
        }

        public string lang
        {
            get;
            set;
        }

        public string getString(int nKey)
        {
            return this.getString(nKey.ToString());
        }

        public string getString(string strKey)
        {
            string strOut;
            if (mDict.TryGetValue(strKey, out strOut))
                return strOut;
            Logger.LogWarning("language key {0} is not exist !", strKey);
            return string.Empty;
        }

        public void release()
        {
            mDict.Clear();
        }

        public void addString(string strKey, string strValue)
        {
            mDict[strKey] = strValue;
        }

        public void addModule(LanguageModule newModule)
        {
            if (newModule != null)
            {
                if (newModule.lang == this.lang && newModule.name.Equals(this.name))
                {
                    foreach (KeyValuePair<string, string> item in newModule.mDict)
                    {
                        this.addString(item.Key, item.Value);
                    }
                }
            }
        }
	}

    class LanguageModuleMgr
    {
        public static string strDefault = "default";

        Dictionary<string, LanguageModule> mDict = new Dictionary<string, LanguageModule>();

        public LanguageModuleMgr()
        {
        }

        public void release()
        {
            foreach (KeyValuePair<string, LanguageModule> item in this.mDict)
                item.Value.release();
            mDict.Clear();
        }

        public LanguageModule getModule(string strModule = null)
        {
            string strKey = string.Empty;
            if (strModule == null || strModule == string.Empty)
                strKey = strDefault;
            else
                strKey = strModule;

            LanguageModule outModule;
            if (mDict.TryGetValue(strKey, out outModule))
                return outModule;
            return null;
        }

        public void addModule(LanguageModule module)
        {
            if (module == null || module.name == string.Empty)
                return;

            LanguageModule existModuele = getModule(module.name);
            if (existModuele == null)
            {
                existModuele = new LanguageModule();
                existModuele.name = module.name;
                existModuele.lang = module.lang;
                mDict[module.name] = existModuele;
            }
            existModuele.addModule(module);
        }

        public void addModuleMgr(LanguageModuleMgr newMgr)
        {
            foreach(KeyValuePair<string, LanguageModule> item in newMgr.mDict)
            {
                this.addModule(item.Value);
            }
        }
    }
}
