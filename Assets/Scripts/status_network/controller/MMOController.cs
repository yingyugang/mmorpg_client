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
		public SimpleRpgCamera rpgCamera;
		public List<ShootObject> shootPrefabs;
		public List<GameObject> unitPrefabs;

		SimpleRpgAnimator mSimpleRpgAnimator;
		public SimpleRpgPlayerController simpleRpgPlayerController;
		Dictionary<int,GameObject> mUnitDic;
		Dictionary<int,GameObject> mPlayerDic;
		Dictionary<int,GameObject> mMonsterDic;
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
			mPlayerDic = new Dictionary<int, GameObject> ();
			mOtherPlayerIds = new List<int> ();
			mMonsterDic = new Dictionary<int, GameObject> ();
			mUnitDic = new Dictionary<int, GameObject> ();
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

		//TODO
		public void SendChat(string chat){
			playerInfo.chat = chat;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
			playerInfo.chat = "";
		}

		//TODO
		public void SendUseSkill(int skillId){
			playerInfo.skillId = skillId;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
			playerInfo.skillId = 0;
		}

		public void RecieveUseSkill(int unitId,int skillId){
		
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
			TransferData transferData = msg.ReadMessage<TransferData> ();
			HashSet<int> activedPlayerIds = new HashSet<int> ();
			for (int i = 0; i < transferData.playerDatas.Length; i++) {
				if (!mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
					GameObject playerGO = InstantiateUnit (0);
					mPlayerDic.Add (transferData.playerDatas [i].playerId, playerGO);
					mPlayerDic [transferData.playerDatas [i].playerId].SetActive (true);
					mOtherPlayerIds.Add (transferData.playerDatas [i].playerId);
					if(mUnitDic.ContainsKey(transferData.playerDatas [i].unitInfo.attribute.unitId)){
						mUnitDic.Add (transferData.playerDatas [i].unitInfo.attribute.unitId, playerGO);
					}
				}
				if (transferData.playerDatas [i].playerId != mPlayerId) {
					activedPlayerIds.Add (transferData.playerDatas [i].playerId);
					mPlayerDic [transferData.playerDatas [i].playerId].transform.position = transferData.playerDatas [i].unitInfo.transform.playerPosition;
					mPlayerDic [transferData.playerDatas [i].playerId].transform.forward = transferData.playerDatas [i].unitInfo.transform.playerForward;
					mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().Action = transferData.playerDatas [i].unitInfo.animation.action;
					mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().SetSpeed (transferData.playerDatas [i].unitInfo.animation.animSpeed);
				}
				if(!string.IsNullOrEmpty(transferData.playerDatas [i].chat)){
					if (onChat != null) {
						if(transferData.playerDatas [i].playerId != mPlayerId)
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", transferData.playerDatas [i].unitInfo.attribute.unitName, transferData.playerDatas [i].chat));
						else
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", "you", transferData.playerDatas [i].chat));
					}
				}
			}
			for (int i = 0; i < mOtherPlayerIds.Count; i++) {
				if (!activedPlayerIds.Contains (mOtherPlayerIds [i])) {
					int id = mOtherPlayerIds [i];
					Destroy (mPlayerDic [id]);
					mOtherPlayerIds.Remove (id);
					mPlayerDic.Remove (id);
					i--;
				}
			}
		}

		void OnRecieveMonsterInfos(NetworkMessage msg){
			TransferData data = msg.ReadMessage<TransferData> ();
			for (int i = 0; i < data.monsterDatas.Length; i++) {
				if (!mMonsterDic.ContainsKey ( data.monsterDatas [i].attribute.unitId)) {
					GameObject monsterGo = InstantiateUnit (0);
					mMonsterDic.Add(data.monsterDatas [i].attribute.unitId, monsterGo);
					mMonsterDic[data.monsterDatas [i].attribute.unitId].SetActive (true);
					mUnitDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
				}
				UnitInfo unitInfo = data.monsterDatas [i];
				MMOUnit monster = mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit>();
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

		GameObject InstantiateUnit (int unitType)
		{
			unitType = Mathf.Clamp (unitType, 0, unitPrefabs.Count - 1);
			GameObject unitPrebfab = unitPrefabs [unitType].gameObject;
			unitPrebfab.SetActive (false);
			GameObject unitGo = Instantiate (unitPrebfab) as GameObject;
			MMOUnitSkill mmoUnitSkill = unitGo.GetOrAddComponent<MMOUnitSkill> ();
			unitGo.SetActive (true);
			return unitGo;
		}

	}
}
