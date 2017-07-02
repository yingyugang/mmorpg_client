using System.Collections.Generic;
using UnityEngine;

namespace BaseLib
{
	class ResourceCenter
	{
        static public string configPath = "Configs/";
        static public string audioPath = "Audios/";

        public static T LoadAsset<T>(string strPath) where T : Object
        {
            object obj = Resources.Load(strPath,typeof(T));
            if(obj==null)
            {
                Logger.LogError("load resource {0} failed", strPath);
                return null;
            }
            return obj as T;
        }
    }
}
