using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;
using System.Text;

namespace MMO
{
	public class MMOServer : NetworkManager
	{

		public bool isBattleBegin;
		int mFrame = 0;
		float mNextFrameTime;
		float mFrameInterval;

		Dictionary<int,PlayerData> dic_player_data;

		int mCurrentMaxId = 0;
		public TransferData data;
		public const int FRAME_RATE = 10;//10回per1秒。

		void Awake ()
		{
			//			Reset ();
			this.networkPort = NetConstant.LISTENE_PORT;
			this.StartServer ();
			dic_player_data = new Dictionary<int, PlayerData> ();
			connectionConfig.SendDelay = 1;
			NetworkServer.RegisterHandler (MsgType.Connect, OnClientConnect);
			NetworkServer.RegisterHandler (MsgType.Disconnect, OnClientDisconnect);
			NetworkServer.RegisterHandler (MessageConstant.CLIENT_TO_SERVER_MSG,OnRecievePlayerMessage);
			NetworkServer.maxDelay = 0;
			mFrameInterval = 1f / FRAME_RATE;

			//			NetworkServer.RegisterHandler (MessageConstant.CLIENT_READY,OnRecieveClientReady);
			//			NetworkServer.RegisterHandler (MessageConstant.CLIENT_PLAYER_HANDLE,OnRecievePlayerHandle);
			//			NetworkServer.RegisterHandler (MessageConstant.CLIENT_REQUEST_FRAMES, OnRecievePlayerFrameRequest);
			//			NetworkServer.RegisterHandler (MessageConstant.CLIENT_RESOURCE_READY, OnRecieveClientResourceReady);
			//			NetworkServer.RegisterHandler (MessageConstant.CLIENT_TO_SERVER_MSG, OnRecieveClientResourceReady);
			//Environmentでシステムのパラメーターをセートする
			//			string[] commandLineArgs = Environment.GetCommandLineArgs();
			//			for(int i=0;i<commandLineArgs.Length;i++){
			//				if(commandLineArgs[i].ToLower().IndexOf("playercount")!=-1){
			//					string[] countStrs = commandLineArgs[i].Split(new char[]{'='});
			//					if(countStrs.Length>1){
			//						int playerCount = 0;
			//						if(int.TryParse(countStrs[1],out playerCount)){
			//							if (playerCount > 0) {
			//								NetConstant.max_player_count = playerCount;
			//							}
			//						}
			//					}
			//				}
			//			}
		}

		//サーバーをリセットーする
		//		public void Reset(){
		//			isBattleBegin = false;
		//			mConnections = new Dictionary<int, PlayerStatus> ();
		//			mHandleMessages = new Dictionary<int, HandleMessage> ();
		//			mFrame = 0;
		//			mNextFrameTime = 0;
		//		}

		void Update ()
		{
			if (!isBattleBegin) {
				return;
			}
			SendFrame ();
			//			if(this.mConnections.Count == 0){
			//				isBattleBegin = false;
			//				mFrame = 0;
			//			}
		}

		//		public int ConnectionCount{
		//			get{ 
		//				if (mConnections != null)
		//					return mConnections.Count;
		//				else
		//					return 0;
		//			}
		//		}

		#region 1.Send
		//当有玩家加入或者退出或者准备的場合
		//プレイヤーを入る、去るとか、準備できた時とか、メセージを送る
		//		void SendPlayerStatus(){
		//			List<PlayerStatus> pss = new List<PlayerStatus> ();
		//			foreach(PlayerStatus ps in mConnections.Values){
		//				pss.Add (ps);
		//			}
		//			PlayerStatusArray psa = new PlayerStatusArray ();
		//			psa.playerStatus = pss.ToArray ();
		//			NetworkServer.SendToAll (MessageConstant.SERVER_CLIENT_STATUS,psa);
		//		}

		//告诉客户端创建人物
		//クライアントにキャラクターを作成する
		//		void SendBattleBegin(){
		//			CreatePlayer cp = new CreatePlayer ();
		//			List<int> playerIds = new List<int> ();
		//			foreach(NetworkConnection nc in NetworkServer.connections){
		//				Debug.Log (nc);
		//				if(nc!=null)
		//					playerIds.Add (nc.connectionId);
		//			}
		//			cp.playerIds = playerIds.ToArray ();
		//			NetworkServer.SendToAll (MessageConstant.CLIENT_READY,cp);
		//		}

		//メセージをクライアントに送る
		void SendFrame(){
			//这样做能保证帧率恒定不变
			while(mNextFrameTime <= Time.fixedUnscaledTime){
				mNextFrameTime += mFrameInterval;
				SendFrameMessage ();
			}
		}

		//メセージをクライアントに送る
		void SendFrameMessage(){
			//			ServerMessage currentMessage = new ServerMessage ();
			//			ConstructFrameMessageAndIncreaseFrameIndex (currentMessage);
			//			NetworkServer.SendUnreliableToAll (MessageConstant.SERVER_TO_CLIENT_MSG, currentMessage);
		}

		//メセージを構造して、フレーム番号が増える
		//		void ConstructFrameMessageAndIncreaseFrameIndex(ServerMessage currentMessage){
		//			currentMessage.frame = mFrame;
		//			List<HandleMessage> handleMessages = new List<HandleMessage> ();
		//			//			foreach(int playerId in mHandleMessages.Keys){
		//			if(mHandleMessages.ContainsKey(1))
		//				handleMessages.Add (mHandleMessages [1]);
		//			//				mHandleMessages [playerId] = null;
		//			//			}
		//			currentMessage.handleMessages = handleMessages.ToArray();
		//			mFrame++;
		//		}
		#endregion

		#region 2.Recieve
		void OnClientConnect (NetworkMessage nm)
		{
			Debug.logger.Log ("OnClientConnect");
			PlayerInfo playerInfo = new PlayerInfo ();
			playerInfo.playerId = mCurrentMaxId;
			NetworkServer.SendToClient (nm.conn.connectionId,MessageConstant.SERVER_TO_CLIENT_PLAYER_INFO,playerInfo);
			mCurrentMaxId++;
			//			NetworkConnection conn = nm.conn;
			//			if (isBattleBegin || mConnections.Count >= NetConstant.max_player_count) {
			//				conn.Disconnect ();
			//			}else {
			//				PlayerStatus ps = new PlayerStatus ();
			//				ps.playerId = conn.connectionId;
			//				ps.isReady = false;
			//				mConnections.Add (conn.connectionId,ps);
			//				SendPlayerStatus ();
			//			}
		}

		void OnClientDisconnect (NetworkMessage nm)
		{
			Debug.logger.Log ("OnClientDisconnect");
			//			NetworkConnection conn = nm.conn;
			//			mConnections.Remove(conn.connectionId);
			//			if (mConnections.Count == 0) {
			//				Reset ();
			//			} else {
			//				SendPlayerStatus ();
			//			}
		}

		//收到用户准备准备完毕
		//ユーザーを準備できたメセージを受信する
		void OnRecieveClientReady(NetworkMessage msg){
			Debug.logger.Log ("OnRecieveClientReady");
			//			if(mConnections.ContainsKey(msg.conn.connectionId)){
			//				mConnections [msg.conn.connectionId].isReady = true;
			//			}
			//			SendPlayerStatus ();
			//			int count = 0;
			//			foreach(PlayerStatus ps in mConnections.Values){
			//				if (ps.isReady) {
			//					count++;
			//				} 
			//			}
			//			if (count >= NetConstant.max_player_count) {
			//				SendBattleBegin ();
			//			} 
		}

		void OnRecievePlayerMessage(NetworkMessage msg){
			PlayerData playerHandle = msg.ReadMessage<PlayerData> ();
			playerHandle.playerId = msg.conn.connectionId;
			if (!dic_player_data.ContainsKey (msg.conn.connectionId)) {
				dic_player_data.Add (msg.conn.connectionId, playerHandle);
			} 
			dic_player_data [msg.conn.connectionId] = playerHandle;
			TransferData data = new TransferData ();
			data.playerDatas = new PlayerData[dic_player_data.Count];
			int i = 0;
			foreach(int id in dic_player_data.Keys){
				data.playerDatas [i] = dic_player_data [id];
				i++;
			}
			Debug.Log (JsonUtility.ToJson(playerHandle));
			NetworkServer.SendUnreliableToAll (MessageConstant.SERVER_TO_CLIENT_MSG, data);
			//			ServerMessage currentMessage = new ServerMessage ();
			//			ConstructFrameMessageAndIncreaseFrameIndex (currentMessage);
			//			NetworkServer.SendUnreliableToAll (MessageConstant.SERVER_TO_CLIENT_MSG, currentMessage);
		}
		#endregion
	}

}

