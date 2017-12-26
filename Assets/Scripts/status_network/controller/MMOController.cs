using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.EventSystems;

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
		public GameObject minimap;
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
			KGFMapSystem kgf = FindObjectOfType<KGFMapSystem> ();
			if (kgf != null)
				minimap = kgf.gameObject;
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
				if (player.position != mPrePosition || player.forward != mPreForward || mSimpleRpgAnimator.Action != mPreAction ||
				    simpleRpgPlayerController._animation_speed != mPreSpeed || playerInfo.skillId > -1) {
					mPrePosition = player.position;
					mPreForward = player.forward;
					mPreAction = mSimpleRpgAnimator.Action;
					mPreSpeed = simpleRpgPlayerController._animation_speed;
//					SendPlayerMessage (player);
					playerInfo.unitInfo.transform.playerForward = player.forward;
					playerInfo.unitInfo.transform.playerPosition = player.position;
					playerInfo.unitInfo.animation.action = mPreAction;
					playerInfo.unitInfo.animation.animSpeed = mPreSpeed;
					client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
					playerInfo.skillId = -1;
				}
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
		}

		public void Connect (string ip, int port)
		{
			client.Connect (ip, port, OnConnected, OnRecievePlayerInfo, OnRecieveMessage);
		}

		public void SendChat (string chat)
		{
			playerInfo.chat = chat;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
			playerInfo.chat = "";
		}

		//		public void SendUseSkill(int skillId){
		//			Debug.Log ("SendUseSkill");
		//			playerInfo.unitInfo.action.attackType = skillId;
		//			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
		//			playerInfo.unitInfo.action.attackType = 0;
		//		}
		//
		//		public void RecieveUseSkill(int unitId,int skillId){
		//			RecieveUseSkill(unitId,skillId);
		//			RecieveUseSkill(int unitId,int skillId);
		//			playerInfo.unitInfo.action.attackType = 0;
		//			playerInfo.unitInfo.action.targetPos = Vector3.one;
		//			playerInfo.unitInfo.action.attackType = 1;
		//			playerInfo.unitInfo.action.targetPos = Vector3.zero;
		//		}

		void OnConnected (NetworkMessage msg)
		{

		}

		//TODO 把通信数据放在一个主对象的不同参数里面，这个容易理解很保存数据。
		//开发量也相对较少，否则分开的化需要不同的对象，方法，action，数量多，时间长不易于管理。
		//主要是玩家自己的操作
		void OnRecievePlayerInfo (NetworkMessage msg)
		{
			playerInfo = msg.ReadMessage<PlayerInfo> ();
			mPlayerId = playerInfo.playerId;
			rpgCamera.enabled = true;
			player.gameObject.SetActive (true);
			playerInfo.unitInfo.attribute.unitName = playerName;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, playerInfo);
			PanelManager.Instance.mainInterfacePanel.gameObject.SetActive (true);
			PanelManager.Instance.chatPanel.gameObject.SetActive (true);
			if (minimap != null)
				minimap.SetActive (true);
		}

		void OnRecieveMessage (NetworkMessage msg)
		{
			TransferData transferData = msg.ReadMessage<TransferData> ();
			HashSet<int> activedPlayerIds = new HashSet<int> ();
			for (int i = 0; i < transferData.playerDatas.Length; i++) {
				if (!mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
					GameObject playerGO;
					if (transferData.playerDatas [i].playerId != mPlayerId) {
						playerGO = InstantiateUnit (0);
					} else {
						playerGO = MMOController.Instance.player.gameObject;
					}
					mPlayerDic.Add (transferData.playerDatas [i].playerId, playerGO);
					mPlayerDic [transferData.playerDatas [i].playerId].SetActive (true);
					mOtherPlayerIds.Add (transferData.playerDatas [i].playerId);
					if (!mUnitDic.ContainsKey (transferData.playerDatas [i].unitInfo.attribute.unitId)) {
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
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.animation = transferData.playerDatas [i].unitInfo.animation;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.attribute = transferData.playerDatas [i].unitInfo.attribute;
//				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.transform = transferData.playerDatas [i].unitInfo.transform;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.action = transferData.playerDatas [i].unitInfo.action;
				if (!string.IsNullOrEmpty (transferData.playerDatas [i].chat)) {
					if (onChat != null) {
						if (transferData.playerDatas [i].playerId != mPlayerId)
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", transferData.playerDatas [i].unitInfo.attribute.unitName, transferData.playerDatas [i].chat));
						else
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", "you", transferData.playerDatas [i].chat));
					}
				}
				MMOUnit mmoUnit = mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ();
				if (mmoUnit.unitInfo.action.attackType > 0) {
					Debug.Log (mmoUnit.unitInfo.action.attackType);
					mmoUnit.GetComponent<MMOUnitSkill> ().PlayServerSkill (mmoUnit.unitInfo.action.attackType);
					mmoUnit.unitInfo.action.attackType = 0;
				}
			}

//			for (int i = 0; i < mOtherPlayerIds.Count; i++) {
//				int id = mOtherPlayerIds [i];
//				Destroy(mPlayerDic[i]);
//			}
		}

		void OnRecieveMonsterInfos (NetworkMessage msg)
		{
			TransferData data = msg.ReadMessage<TransferData> ();
			for (int i = 0; i < data.monsterDatas.Length; i++) {
				if (!mMonsterDic.ContainsKey (data.monsterDatas [i].attribute.unitId)) {
					GameObject monsterGo = InstantiateUnit (data.monsterDatas [i].attribute.unitType);
					mMonsterDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
					mMonsterDic [data.monsterDatas [i].attribute.unitId].SetActive (true);
					if (!mUnitDic.ContainsKey (data.monsterDatas [i].attribute.unitId))
						mUnitDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
				}
				UnitInfo unitInfo = data.monsterDatas [i];
				MMOUnit monster = mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit> ();
				monster.transform.position = data.monsterDatas [i].transform.playerPosition;
				monster.transform.forward = data.monsterDatas [i].transform.playerForward;
				monster.SetAnimation (data.monsterDatas [i].animation.action, data.monsterDatas [i].animation.animSpeed);
				if (data.monsterDatas [i].action.attackType >= 0) {
					monster.GetComponent<MMOUnitSkill> ().PlayServerSkill (data.monsterDatas [i].action.attackType);
					data.monsterDatas [i].action.attackType = -1;
				}
				//data.monsterDatas[i].action.attackType = -1;
				//data.monsterDatas[i].action.attackType = -2;
				//data.monsterDatas[i].action.attackType = -3;
				//monster.GetComponent<MMOUnitSkill>().PlayServerSkill

			}
		}

		GameObject InstantiateUnit (int unitType)
		{
			unitType = Mathf.Clamp (unitType, 0, unitPrefabs.Count - 1);
			GameObject unitPrebfab = unitPrefabs [unitType].gameObject;
			unitPrebfab.SetActive (false);
			GameObject unitGo = Instantiate (unitPrebfab) as GameObject;
			unitGo.GetOrAddComponent<MMOUnitSkill> ();
			unitGo.SetActive (true);
			return unitGo;
		}

	}
}
