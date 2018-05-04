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
		UnityAction<NetworkMessage> onRecievePlayerInitInfo;
		public UnityAction<NetworkMessage> onRecieveMonsterInfos;

		void Start ()
		{
			if(client==null){
				Init();
			}
		}

		void Init(){
			client = new NetworkClient ();
			client.RegisterHandler (MsgType.Connect, OnConnect);
			client.RegisterHandler (MsgType.Disconnect, OnDisconnect);
			client.RegisterHandler (MessageConstant.SERVER_TO_CLIENT_MONSTER_INFO, OnRecieveMonsterInfos);
			client.RegisterHandler (MessageConstant.PLYAER_INIT_INFO, OnRecievePlayerInitInfo);
			client.RegisterHandler (MessageConstant.SERVER_TO_CLIENT_MSG, OnRecieveMessage);
			client.RegisterHandler (MessageConstant.PLAYER_ACTION, OnRecievePlayerAction);
			client.RegisterHandler (MessageConstant.PLAYER_VOICE, OnRecievePlayerVoice);
			client.RegisterHandler (MessageConstant.PLAYER_RESPAWN, OnRecievePlayerRespawn);
			client.RegisterHandler (MessageConstant.PLAYER_CONTROLL, OnRecievePlayerControll);
			client.RegisterHandler (MessageConstant.PLAYER_ENTER, OnRecievePlayerEnter);
			//TODO これはDisconnectと違う。たまでキャラクターを選ぶこと。
			client.RegisterHandler (MessageConstant.PLAYER_LEAVE, OnRecievePlayerLeave);
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
			SimplePlayerInfo respawn = new SimplePlayerInfo ();
			Send (MessageConstant.PLAYER_RESPAWN,respawn);
		}

		public void SendAction(StatusInfo action){
			Send (MessageConstant.PLAYER_ACTION, action);
		}

		public void SendVoice(float[] data){
			VoiceInfo voice = new VoiceInfo ();
			voice.voice = data;
			Send (MessageConstant.PLAYER_VOICE,voice);
		}

		public void Connect (string ip, int port, UnityAction<NetworkMessage> onRecieveMessage)
		{
			if(client==null){
				Init();
			}
			Debug.Log (string.Format ("{0},{1}", ip, port));
//			this.onConnect = onConnect;
//			this.onRecievePlayerInitInfo = onRecievePlayerInitInfo;
			this.onRecieveMessage = onRecieveMessage;
			client.Connect (ip, port);
		}

		void OnConnect (NetworkMessage nm)
		{
			Debug.logger.Log ("<color=green>Connect</color>,new send the player information.");
//			if (onConnect != null)
//				onConnect (nm);
			SendPlayerRegister(MMOController.Instance.playerName);
		}

		//TODO これで若干APIが必要だと思う。
		//SendPlayerToken;this is get the user information (e.g. character list). now there is the "SendPlayerRegister" to instant it.
		//SendSelectCharacter;
		//SendCreateCharacter;

		//Send the player info to server.
		//例えば:プレーヤーの名前。PLAYER_REGISTER
		public void SendPlayerRegister(string playerName){
			FullPlayerInfo playerInitInfo = new FullPlayerInfo ();
			playerInitInfo.playerName = playerName;
			Send (MessageConstant.PLAYER_REGISTER,playerInitInfo);
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

		void OnRecievePlayerInitInfo (NetworkMessage msg)
		{
			GameInitInfo gameInitInfo = msg.ReadMessage<GameInitInfo> ();
			MMOController.Instance.GameStart (gameInitInfo);
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

		void OnRecievePlayerRespawn(NetworkMessage msg){
			SimplePlayerInfo respawnInfo = msg.ReadMessage<SimplePlayerInfo> ();
			ActionManager.Instance.DoRespawn (respawnInfo.unitId);
		}

		void OnRecievePlayerControll(NetworkMessage msg){
			PlayerControllInfo playerControll = msg.ReadMessage<PlayerControllInfo> ();
			MMOController.Instance.DoPlayerControll (playerControll);
		}

		void OnRecievePlayerLeave(NetworkMessage msg){
			SimplePlayerInfo playerInfo = msg.ReadMessage<SimplePlayerInfo> ();
			MMOController.Instance.DoRemovePlayer (playerInfo);
		}

		void OnRecievePlayerEnter(NetworkMessage msg){
			FullPlayerInfo fullPlayerInfo = msg.ReadMessage<FullPlayerInfo> ();
			MMOController.Instance.AddPlayer (fullPlayerInfo);
		}

		void OnRecieveMessage (NetworkMessage msg)
		{
			if (onRecieveMessage != null)
				onRecieveMessage (msg);
		}
	}
}