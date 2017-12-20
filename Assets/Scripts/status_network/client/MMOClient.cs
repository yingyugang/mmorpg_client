using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace MMO
{
	public class MMOClient : SingleMonoBehaviour<MMOClient>
	{
		NetworkClient client;
		UnityAction<NetworkMessage> onConnect;
		UnityAction<NetworkMessage> onLogin;
		UnityAction<NetworkMessage> onRecieveMessage;
		UnityAction<NetworkMessage> onRecievePlayerInfo;
		public UnityAction<NetworkMessage> onRecieveMonsterInfos;

		void Start ()
		{
			client = new NetworkClient ();
			client.RegisterHandler (MsgType.Connect, OnConnect);
			client.RegisterHandler (MsgType.Disconnect, OnDisconnect);
			client.RegisterHandler (MessageConstant.SERVER_TO_CLIENT_MONSTER_INFO, OnRecieveMonsterInfos);
			client.RegisterHandler (MessageConstant.SERVER_TO_CLIENT_PLAYER_INFO, OnRecievePlayerInfo);
			client.RegisterHandler (MessageConstant.SERVER_TO_CLIENT_MSG, OnRecieveMessage);
		}

		public bool IsConnected {
			get { 
				return client.isConnected;
			}
		}

		public void Send (short msgType, MessageBase msg)
		{
			client.Send (msgType, msg);
		}

		public void Connect (string ip, int port, UnityAction<NetworkMessage> onConnect, UnityAction<NetworkMessage> onRecievePlayerInfo, UnityAction<NetworkMessage> onRecieveMessage)
		{
			Debug.Log (string.Format ("{0},{1}", ip, port));
			this.onConnect = onConnect;
			this.onRecievePlayerInfo = onRecievePlayerInfo;
			this.onRecieveMessage = onRecieveMessage;
			client.Connect (ip, port);
		}

		void OnConnect (NetworkMessage nm)
		{
			Debug.logger.Log ("<color=green>Connect</color>");
			if (onConnect != null)
				onConnect (nm);
		}

		void OnLogin(NetworkMessage nm){
			Debug.logger.Log ("<color=green>OnLogin</color>");
			if (onLogin != null)
				onLogin (nm);
		}

		void OnDisconnect (NetworkMessage nm)
		{
			Debug.logger.Log ("<color=red>Disconnect</color>");
			//BattleClientController.Instance.Reset ();
		}

		void OnRecievePlayerInfo (NetworkMessage msg)
		{
			if (onRecievePlayerInfo != null)
				onRecievePlayerInfo (msg);
		}

		void OnRecieveMonsterInfos(NetworkMessage msg){
			if (onRecieveMonsterInfos != null)
				onRecieveMonsterInfos (msg);
		}


		void OnRecieveMessage (NetworkMessage msg)
		{
			if (onRecieveMessage != null)
				onRecieveMessage (msg);
		}
	}
}