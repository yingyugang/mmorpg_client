using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BaseLib;

public enum SEVER_TYPE
{
    LOGIN_SERVER = 0,
    GAME_SERVER
}

public class NetworkMgr : SingletonMonoBehaviourNoCreate<NetworkMgr>
{
    NetClient[] _clientList = new NetClient[2];
    //发送队列
    Dictionary<SEVER_TYPE, List<IPacket>> _sendList = new Dictionary<SEVER_TYPE, List<IPacket>>();
    //接收队列
    Dictionary<SEVER_TYPE, List<RecvPacket>> _recvList = new Dictionary<SEVER_TYPE, List<RecvPacket>>();
    //解包完成待分发队列
    List<IUnPacket> _unpacketList = new List<IUnPacket>();

    public bool offLine = false;

    public static void connect(string strIP, int port, SEVER_TYPE type, bool autoReconn = false)
    {
        servInfo serv = new servInfo();
        serv.strIp = strIP;
        serv.nPort = port;
        serv.nType = type;
        serv.autoReconn = autoReconn;

        string strMsg = string.Format("connetting {0}:{1}...", strIP, port);
        UnityEngine.Debug.Log(strMsg);

        if (NetworkMgr.me.offLine)
        {
            NetResult ret = new NetResult();
            ret.server = serv;
            EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_connSucc, ret);
            return;
        }

        if (NetworkMgr.me._clientList[(int)type] == null)
            NetworkMgr.me._clientList[(int)type] = new NetClient();
        NetworkMgr.me._clientList[(int)type].connect(serv);
    }

    protected override void Init()
    {
        //创建网络主线程
        ThreadMgr.createThread(this.threadMain);
    }

    public static void sendData(IPacket data, SEVER_TYPE type = SEVER_TYPE.GAME_SERVER)
    {
        if (NetworkMgr.me.offLine)
        {
            EventSystem.sendEvent((int)DataCenter.EVENT_GLOBAL.net_sendData,data);
            return;
        }
        if (!NetworkMgr.me._sendList.ContainsKey(type))
            NetworkMgr.me._sendList[type] = new List<IPacket>();
        lock (NetworkMgr.me._sendList[type])
        {
            NetworkMgr.me._sendList[type].Add(data);
        }
    }

    public static void recvData(RecvPacket packet, SEVER_TYPE type = SEVER_TYPE.GAME_SERVER)
    {
        if (!NetworkMgr.me._recvList.ContainsKey(type))
            NetworkMgr.me._recvList[type] = new List<RecvPacket>();
        lock (NetworkMgr.me._recvList[type])
        {
            NetworkMgr.me._recvList[type].Add(packet);
        }
    }

    public static void release()
    {
        for (int n = 0; n < NetworkMgr.me._clientList.Length; n++)
        {
            if (NetworkMgr.me._clientList[n] != null)
            {
                NetworkMgr.me._clientList[n].close();
                NetworkMgr.me._clientList[n] = null;
            }
        }
    }

    public void addUnpacketRet(IUnPacket unpacket)
    {
        if (unpacket != null)
        {
            lock (_unpacketList)
            {
                _unpacketList.Add(unpacket);
            }
        }
    }

    void Update()
    {
        //分发接收到的报文
        DispatchPack();
    }

    void DispatchPack()
    {
        if (_unpacketList.Count == 0)
            return;
        lock (_unpacketList)
        {
            List<IUnPacket> theList = _unpacketList;
            _unpacketList = new List<IUnPacket>();
            foreach (IUnPacket item in theList)
            {
                if (item != null)
                    EventSystem.sendEvent((int)item.msgid, item, (int)DataCenter.EVENT_GROUP.packet);
            }
        }
    }

    TRHEAD_RESULT threadMain()
    {
        if (NetworkMgr.me.offLine)
            return TRHEAD_RESULT.STOP;
        try
        {
            //发送
            threadSendPack();
            //收包
            threadRecvPack();
            //解包分发
            threadUnpackList();
            return TRHEAD_RESULT.SLEEP;
        }
        catch (System.Exception ex)
        {
            UnityEngine.Debug.Log("threadMain error:" + ex.Message);
            return TRHEAD_RESULT.SLEEP;
        }
    }

    void threadRecvPack()
    {
        //收包
        foreach (NetClient item in NetworkMgr.me._clientList)
        {
            if (item != null)
                item.recvData();
        }
    }

    //发送线程
    void threadSendPack()
    {
        foreach (KeyValuePair<SEVER_TYPE, List<IPacket>> item in _sendList)
        {
            NetClient client = _clientList[(int)item.Key];
            if (client == null || !client.isConn || client.isSending)
                continue;

            List<IPacket> sendList = item.Value;
            if (sendList == null || sendList.Count == 0)
                continue;
            lock (NetworkMgr.me._sendList)
            {
                List<IPacket> tempList = new List<IPacket>();
                tempList.AddRange(sendList);
                sendList.Clear();
                sendList = tempList;
            }
            foreach (IPacket packet in sendList)
            {
                //打包
                if (!packet.pack())
                {
                    packet.release();
                    continue;
                }
                //如果可以发送则添加流水号等待发送，不能发则继续投入到发送队列等待发送
                if (client.canWrite)
                {
                    packet.addSequence();
                    UnityEngine.Debug.Log(string.Format("send pack:{0}:{1}",PacketMgr.me.getPacketId(packet),packet.length));
                    //发送
                    client.sendData(packet.data,packet.length);
                    packet.release();
                }
            }
        }
    }

    void threadUnpackList()
    {
        //解包
        foreach (KeyValuePair<SEVER_TYPE, List<RecvPacket>> item in _recvList)
        {
            List<RecvPacket> unpackList = item.Value;
            if (unpackList == null || unpackList.Count == 0)
                continue;

            lock (NetworkMgr.me._recvList)
            {
                List<RecvPacket> tempList = new List<RecvPacket>();
                tempList.AddRange(unpackList);
                unpackList.Clear();
                unpackList = tempList;
            }

            //解包
            foreach (RecvPacket itemPacket in unpackList)
            {
                //解包后插入报文
                IUnPacket unpackt = UnPacketMgr.me.getUnPacket(itemPacket.nMsgId);
                if (unpackt != null)
                {
                    unpackt.unpack(itemPacket.data,itemPacket.nLen);
                    this.addUnpacketRet(unpackt);
                }
                else
                {
                    UnityEngine.Debug.Log(string.Format("msgid ={0} cant find unpack function...",itemPacket.nMsgId));
                }
            }
        }
    }
}
