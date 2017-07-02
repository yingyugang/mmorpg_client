using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace BaseLib
{
	class LanguageFile
	{
        public LanguageModuleMgr mDictMgr = new LanguageModuleMgr();

        public LanguageFile()
        {
        }

        public void release()
        {
            mDictMgr.release();
        }

        public LanguageModuleMgr dictMgr
        {
            get { return mDictMgr; }
        }

        //读取ini配置文件
        public bool init(string strPath,string lang)
        {
            mDictMgr.release();
            iniReader reader = new iniReader();
            if (!reader.loadFile(strPath))
                return false;
            if (!reader.parseIni())
                return false;

            foreach (KeyValuePair<string,IniSection> item in reader.mIniDict)
            {
                //节指定了语言
                string strLang = item.Value.getString(LanguageMgr.strLang);
                if (strLang== null || strLang == string.Empty)
                {
                    //节没有指定语言，文件有指定
                    IniSection sec;
                    if(reader.mIniDict.TryGetValue(LanguageModuleMgr.strDefault,out sec))
                        strLang = sec.getString(LanguageMgr.strLang);
                }
                //指定语言与目标语言不符时
                if (strLang != null && strLang != string.Empty)
                {
                    if(!strLang.Equals(lang))
                        continue;
                }
                LanguageModule module = new LanguageModule();
                module.name = item.Key;
                module.lang = lang;
                foreach (KeyValuePair<string, string> line in item.Value.mDict)
                    module.addString(line.Key,line.Value);
                mDictMgr.addModule(module);
            }
            reader.release();
            return true;
        }
	}
}
