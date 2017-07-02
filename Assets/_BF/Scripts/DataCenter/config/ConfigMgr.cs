using System;
using System.Collections.Generic;
using BaseLib;

namespace DataCenter
{
    class ConfigMgr : Singleton<ConfigMgr>
	{
        BaseLib.Config<CONFIG_MODULE> _config = new BaseLib.Config<CONFIG_MODULE>();

        static public ConfigTable getConfig(CONFIG_MODULE module)
        {
            return ConfigMgr.me._config.getCfg<CONFIG_MODULE>(module);
        }

        static public void releaseConfig(CONFIG_MODULE module)
        {
            ConfigMgr.me._config.releaseCfg(module);
        }

        private ConfigMgr()
        {
        }

        static public void initPath(bool bFlag,string strPath)
        {
            ConfigMgr.me._config.externalPath = strPath;
            ConfigMgr.me._config.initExternal = bFlag;
        }
	}
}
