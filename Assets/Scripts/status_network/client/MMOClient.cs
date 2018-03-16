using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MMO
{
	public class MMOClient : SingleMonoBehaviour<MMOClient>
	{
		NetworkClient client;
		UnityAction<NetworkMessage> onConnect;
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
			client.RegisterHandler (MessageConstant.PLAYER_ACTION, OnRecievePlayerAction);
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

		public void SendRespawn(){
			MMORespawn respawn = new MMORespawn ();
			Send (MessageConstant.CLIENT_TO_SERVER_PLAYER_RESPAWN,respawn);
		}

		public void SendAction(MMOAction action){
			Send (MessageConstant.PLAYER_ACTION, action);
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

		//TODO
		void OnDisconnect (NetworkMessage nm)
		{
			MessageReciever.Instance.StopReceive ();
			Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene ();
			UnityEngine.SceneManagement.SceneManager.LoadScene (currentScene.name);
			Debug.logger.Log ("<color=red>Disconnect</color>");
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

		void OnRecievePlayerAction(NetworkMessage msg){
			MMOAction mmoAction = msg.ReadMessage<MMOAction> ();
			MMOController.Instance.DoClientPlayerAction (mmoAction);
		}

		void OnRecieveMessage (NetworkMessage msg)
		{
			if (onRecieveMessage != null)
				onRecieveMessage (msg);
		}
	}
}