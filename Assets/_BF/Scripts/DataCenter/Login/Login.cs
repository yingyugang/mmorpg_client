using UnityEngine;
using System.Collections;
using System;
using BaseLib;

namespace DataCenter
{
	public class Login
	{
        public Login()
		{
            this.gameId = 1;
            this.sign = 1;
            this.sign2 = 2;
            this.sessionKey = "11111";
            this.checkCode = "11111";
            this.version = 1;
        }

        UInt64 _iggid;

        public string loginIp { get; set; }
        public uint loginPort { get; set; }
        public string gameIP{ get; set; }
        public uint gamePort { get; set; }
        public string checkCode { get; set; }
        public string sessionKey { get; set; }
        public UInt32 gameId { get; set; }
        public UInt32 sign { get; set; }
        public UInt32 sign2 { get; set; }
        public UInt32 version { get; set; }
        public UInt64 iggid
        {
            get { return _iggid; }
            set { _iggid = value; }
        }

        public string strIggid
        {
            set
            {
                _iggid = Convert.ToUInt64(value);
            }
        }
        public bool login()
		{
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_LOGIN_LS_VALIDATE, onAuth, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_LOGIN_RESPONSE, onLogin, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_USERINFO_STAR, onLoadData, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)MSG_TYPE._MSG_CLIENT_USERINFO_FINISH, onLoginFinish, (int)DataCenter.EVENT_GROUP.packet);
            EventSystem.register((int)EVENT_GLOBAL.net_connSucc, onConnSucc);
            //iggLogin();

            //连接登陆服务器
            NetworkMgr.connect(this.loginIp, (int)this.loginPort, SEVER_TYPE.LOGIN_SERVER);
            return true;
		} 

        void iggLogin()
        {
            IggLogin igglogin = new IggLogin();
            igglogin.succFunc = this.onIggSucc;
            igglogin.failedFunc = this.onIggFailed;
            igglogin.login();
        }

        void doAuth()
        {
            UnityEngine.Debug.Log("Auth...");

            MSG_CLIENT_LOGINT_2_LS msg = new MSG_CLIENT_LOGINT_2_LS();
            msg.sessionKey = this.sessionKey;
            msg.userid = this._iggid;
            msg.version = this.version;
            NetworkMgr.sendData(msg,SEVER_TYPE.LOGIN_SERVER);
        }

        void onAuth(int nEvent, System.Object param)
        {
            MSG_CLIENT_LOGIN_LS_VALIDATE response = (MSG_CLIENT_LOGIN_LS_VALIDATE)param;
            if (response == null)
                return;
            if (response.userid == this._iggid)
            {
                this.gamePort = response.port;
                this.gameIP = response.servip;
                this.checkCode = response.checkCode;
                UnityEngine.Debug.Log("Auth SUCC");
                //连接登陆服务器
                NetworkMgr.connect(this.gameIP, (int)this.gamePort, SEVER_TYPE.GAME_SERVER);
            }
        }

        void onLogin(int nEvent, System.Object param)
        {
            MSG_SERVER_LOGIN_RESPONSE response = (MSG_SERVER_LOGIN_RESPONSE)param;
            Debug.Log(response.ret);
            if (response.ret == LOGIN_RESULT.SUCC)
            {
                MSG_CLIENT_USERINFO_REQUEST msg = new MSG_CLIENT_USERINFO_REQUEST();
                msg.userid = this._iggid;
                NetworkMgr.sendData(msg);
            }
            else
            {
                onFailed();
            }
        }

        void doLogin()
        {
            MSG_CLIENT_LOGIN_2_GS msg = new MSG_CLIENT_LOGIN_2_GS();
            msg.userid = this.iggid;
            msg.checkCode = this.checkCode;
            msg.sessionKey = this.sessionKey;
            msg.version = this.version;
            msg.gameid = this.gameId;
            msg.clientSign = this.sign;
            msg.clientSign2 = this.sign2;
            NetworkMgr.sendData(msg);
        }

        void onIggSucc(IggLogin igg)
        {
            if (igg != null)
            {
                if (this.iggid==null || this.iggid.Equals(string.Empty))
                    this.strIggid = igg.m_strIggId;
                this.sessionKey = igg.m_strToken;
            }

            Debug.Log("IggAuthSucc");
            //连接登陆服务器
            NetworkMgr.connect(this.loginIp, (int)this.loginPort, SEVER_TYPE.LOGIN_SERVER);
        }

        
        void onConnSucc(int nEvent,System.Object param)
        {
            NetResult ret = (NetResult)param;
            if (ret.server.nType == SEVER_TYPE.LOGIN_SERVER)
            {
                doAuth();
            }
            else
                doLogin();
        }
        
        void onIggFailed(IggLogin igg)
        {
            onFailed();
            Debug.Log("IggAuthFailed");
        }

        void onFailed()
        {
            EventSystem.sendEvent((int)EVENT_GLOBAL.sys_loginFailed);
        }

        void onSucc()
        {
            Debug.Log("Login Succ,waitting user info...");
        }

        void onLoadData(int nEvent, System.Object param)
        {
        }

        void onLoginFinish(int nEvent, System.Object param)
        {
            MSG_CLIENT_CLIENTINITOVER_REQUEST msg = new MSG_CLIENT_CLIENTINITOVER_REQUEST();
            NetworkMgr.sendData(msg);
            EventSystem.sendEvent((int)EVENT_GLOBAL.sys_loginSucc);
        }
    }
}