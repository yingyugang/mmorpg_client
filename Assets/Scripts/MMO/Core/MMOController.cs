using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;

namespace MMO
{
	public class MMOController : SingleMonoBehaviour<MMOController>
	{
		public MMOClient client;
		public Transform player;
		public string targetIp;
		public GameObject playerPrefab;
		public SimpleRpgCamera rpgCamera;
		public List<ShootObject> shootPrefabs;

		SimpleRpgAnimator mSimpleRpgAnimator;
		public SimpleRpgPlayerController simpleRpgPlayerController;
		Dictionary<int,GameObject> mOtherPlayers;
		List<int> mOtherPlayerIds;
		int mPlayerId;
		Vector3 mPrePosition;
		Vector3 mPreForward;
		string mPreAction;
		float mPreSpeed;
		public PlayerInfo playerInfo;
		public string playerName;
		public UnityAction<string> onChat;

		void Start ()
		{
			mSimpleRpgAnimator = player.GetComponentInChildren<SimpleRpgAnimator> (true);
			simpleRpgPlayerController = player.GetComponentInChildren<SimpleRpgPlayerController> (true);
			mOtherPlayers = new Dictionary<int, GameObject> ();
			mOtherPlayerIds = new List<int> ();
			mMonsters = new Dictionary<int, GameObject> ();
			client.onRecieveMonsterInfos = OnRecieveMonsterInfos;
		}

		void Update ()
		{
			if (client.IsConnected) {
				if (player.position != mPrePosition || player.forward != mPreForward || mSimpleRpgAnimator.Action != mPreAction || simpleRpgPlayerController._animation_speed != mPreSpeed) {
					mPrePosition = player.position;
					mPreForward = player.forward;
					mPreAction = mSimpleRpgAnimator.Action;
					mPreSpeed = simpleRpgPlayerController._animation_speed;
					SendMessage (player);
				}
			}
			if(Input.GetKeyDown(KeyCode.Escape)){
				Application.Quit ();
			}
		}

		public void Connect (string ip, int port)
		{
			client.Connect (ip, port, OnConnected, OnRecievePlayerInfo, OnRecieveMessage);
		}

		public void SendMessage (Transform player)
		{
			playerInfo.unitInfo.transform.playerForward = player.forward;
			playerInfo.unitInfo.transform.playerPosition = player.position;
			playerInfo.unitInfo.animation.action = mPreAction;
			playerInfo.unitInfo.animation.animSpeed = mPreSpeed;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
		}

		public void SendChat(string chat){
			playerInfo.chat = chat;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
			playerInfo.chat = "";
		}

		void OnConnected (NetworkMessage msg)
		{
			
		}

		//TODO 把通信数据放在一个主对象的不同参数里面，这个容易理解很保存数据。
		//开发量也相对较少，否则分开的化需要不同的对象，方法，action，数量多，时间长不易于管理。
		void OnRecievePlayerInfo (NetworkMessage msg)
		{
			playerInfo = msg.ReadMessage<PlayerInfo> ();
			mPlayerId = playerInfo.playerId;
			rpgCamera.enabled = true;
			player.gameObject.SetActive (true);
			playerInfo.unitInfo.attribute.unitName = playerName;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
		}

		void OnRecieveMessage (NetworkMessage msg)
		{
			TransferData playerHandle = msg.ReadMessage<TransferData> ();
			HashSet<int> activedPlayerIds = new HashSet<int> ();
			for (int i = 0; i < playerHandle.playerDatas.Length; i++) {
				if (playerHandle.playerDatas [i].playerId != mPlayerId) {
					if (!mOtherPlayers.ContainsKey (playerHandle.playerDatas [i].playerId)) {
						mOtherPlayers.Add (playerHandle.playerDatas [i].playerId, Instantiate (playerPrefab));
						mOtherPlayers [playerHandle.playerDatas [i].playerId].SetActive (true);
						mOtherPlayerIds.Add (playerHandle.playerDatas [i].playerId);
					}
					activedPlayerIds.Add (playerHandle.playerDatas [i].playerId);
					mOtherPlayers [playerHandle.playerDatas [i].playerId].transform.position = playerHandle.playerDatas [i].unitInfo.transform.playerPosition;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].transform.forward = playerHandle.playerDatas [i].unitInfo.transform.playerForward;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().Action = playerHandle.playerDatas [i].unitInfo.animation.action;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().SetSpeed (playerHandle.playerDatas [i].unitInfo.animation.animSpeed);
				}

				if(!string.IsNullOrEmpty(playerHandle.playerDatas [i].chat)){
					if (onChat != null) {
						if(playerHandle.playerDatas [i].playerId != mPlayerId)
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", playerHandle.playerDatas [i].unitInfo.attribute.unitName, playerHandle.playerDatas [i].chat));
						else
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", "you", playerHandle.playerDatas [i].chat));
					}
				}
			}

			for (int i = 0; i < mOtherPlayerIds.Count; i++) {
				if (!activedPlayerIds.Contains (mOtherPlayerIds [i])) {
					int id = mOtherPlayerIds [i];
					Destroy (mOtherPlayers [id]);
					mOtherPlayerIds.Remove (id);
					mOtherPlayers.Remove (id);
					i--;
				}
			}
		}

		Dictionary<int,GameObject> mMonsters;
		void OnRecieveMonsterInfos(NetworkMessage msg){
			TransferData data = msg.ReadMessage<TransferData> ();
			for (int i = 0; i < data.monsterDatas.Length; i++) {
				if (!mMonsters.ContainsKey ( data.monsterDatas [i].attribute.unitId)) {
					mMonsters.Add ( data.monsterDatas [i].attribute.unitId, Instantiate (playerPrefab));
					mMonsters [ data.monsterDatas [i].attribute.unitId].SetActive (true);
				}
				UnitInfo unitInfo = data.monsterDatas [i];
				MMOUnit monster = mMonsters [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit>();
				monster.transform.position = data.monsterDatas [i].transform.playerPosition;
				monster.transform.forward = data.monsterDatas [i].transform.playerForward;
				monster.GetComponent<SimpleRpgAnimator> ().Action = data.monsterDatas [i].animation.action;
				monster.GetComponent<SimpleRpgAnimator> ().SetSpeed (data.monsterDatas [i].animation.animSpeed);
				if(data.monsterDatas [i].attack.attackType>=0){
					GameObject shootPrefab = shootPrefabs [data.monsterDatas [i].attack.attackType].gameObject;
					GameObject shootGo = Instantiater.Spawn (false, shootPrefab, monster.transform.position + new Vector3 (0, 1, 0), monster.transform.rotation * Quaternion.Euler (60, 0, 0));
					ShootObject so = shootGo.GetComponent<ShootObject> ();
					so.Shoot (monster,unitInfo.attack.targetPos,Vector3.zero);
				}
			}
		}
	}
}
