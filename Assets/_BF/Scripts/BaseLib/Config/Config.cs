using System;
using System.Collections.Generic;

namespace BaseLib
{
    public class Config<T> where T : struct, IConvertible
	{
        Dictionary<T, ConfigTable> _cfgList = new Dictionary<T, ConfigTable>();

        bool _initExternal;
        string _externalPath = "";

        //是否启用外部路径
        public bool initExternal
        {
            get { return _initExternal; }
            set
            {
                _initExternal = value;
                this.release();
                if (!value)
                    externalPath = null;
            } 
        }
        public string externalPath
        {
            get { return _externalPath; }
            set 
            {
                _externalPath = value;
                if (_externalPath != null && _externalPath.Equals(string.Empty))
                {
                    _externalPath.Replace('\\', '/');
                }
            } 
        }

        public Config()
        {
            externalPath = null;
            this.initExternal = false;
        }

        public void release()
        {
            this._cfgList.Clear();
        }

        //获取指定模块的配置对象
        public ConfigTable getCfg(T key)
        {
            ConfigTable config = null;
            if(this._cfgList.TryGetValue(key,out config))
                return config;
            //加载配置文件
            return this.initCfg(key);
        }

        //自动校验字段个数是否和枚举匹配
        public ConfigTable getCfg<T1>(T key) where T1 : struct, IConvertible
        {
            ConfigTable config = this.getCfg(key);
            if (config == null)
                return null;
            if (config.checkColCount<T1>())
            {
                Logger.LogError("read config {0}  failed!", key);
                return null;
            }
            return config;
        }

        public void releaseCfg(T key)
        {
            this._cfgList.Remove(key);
        }

        private ConfigTable initCfg(T key)
        {
            ConfigTable config = new ConfigTable();
            string strFileName = Tools.GetDescription(key as System.Enum);
            if(strFileName==null)
                return null;

            //如果启用了外部路径
            if(this.initExternal)
            {
                string strPath = this.externalPath + "/" + strFileName + ".csv";
                if (config.init(strPath, true))
                {
                    this._cfgList[key] = config;
                    return config;
                }
                //外部路径配置读取失败后继续读取默认配置
            }
            if (config.init(strFileName))
            {
                this._cfgList[key] = config;
                return config;
            }
            else
                Logger.LogError("read config {0}  failed!", strFileName);
            return null;
        }
	}
}
