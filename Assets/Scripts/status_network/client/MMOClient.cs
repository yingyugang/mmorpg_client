using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace MMO
{
	//the message types from server,
	//1.message of monster pos and rot.(以后同步方向和速度就好了，在方向改变的时候同步位置和朝向)
	//2.actions of monster.
	//3.message of player pos and rot.(像魔兽世界一样，是每个主控端向服务器端同步控制玩家的pos和rot，以后同步方向和速度就好了，在方向改变的时候同步位置和朝向)
	//4.actions of player.
	//5.hit infos.
	//6.chat infos.
	//7.unit infos. (HP,Buffs,Status)
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
			client.RegisterHandler (MessageConstant.PLAYER_VOICE, OnRecievePlayerVoice);

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

		public void SendAction(StatusInfo action){
			Send (MessageConstant.PLAYER_ACTION, action);
		}

		public void SendVoice(float[] data){
			VoiceInfo voice = new VoiceInfo ();
			voice.voice = data;
			Send (MessageConstant.PLAYER_VOICE,voice);
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
			AssetbundleManager.Instance.ClearAssetBundles ();
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
			StatusInfo mmoAction = msg.ReadMessage<StatusInfo> ();
			MMOController.Instance.DoClientPlayerAction (mmoAction);
		}

		void OnRecievePlayerVoice(NetworkMessage msg){
			VoiceInfos voices = msg.ReadMessage<VoiceInfos> ();
			SoundManager.Instance.PlayVoice (voices);
		}

		void OnRecieveMessage (NetworkMessage msg)
		{
			if (onRecieveMessage != null)
				onRecieveMessage (msg);
		}
	}
}