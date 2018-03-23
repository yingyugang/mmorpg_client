using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace MMO
{
	public class MMOController : SingleMonoBehaviour<MMOController>
	{
		public MMOClient client;
		public bool isStart;

		public Transform player;
		//if Serialized this while not run the construction function when game start;
		PlayerInfo mPlayerInfo;
		public string playerName;

		public string targetIp;
//		public SimpleRpgCamera rpgCamera;
		//TODO csvに遷移するが必要。
		public List<ShootObject> shootPrefabs;
		//TODO ResourcesManager に移動する必要だ。
		public List<GameObject> unitPrefabs;
//		public SimpleRpgPlayerController simpleRpgPlayerController;
		public GameObject minimap;
		public UnityAction<string> onChat;
		public GameObject handleSelectRing;
		public MMOUnit selectedUnit;
		public KGFMapSystem miniMap;

		HeadUIBase mHeadUIPrefab;
		SimpleRpgAnimator mSimpleRpgAnimator;
		Dictionary<int,GameObject> mUnitDic;
		Dictionary<int,GameObject> mPlayerDic;
		Dictionary<int,GameObject> mMonsterDic;
		List<int> mOtherPlayerIds;
		int mPlayerId;
		Vector3 mPrePosition;
		Vector3 mPreForward;
		string mPreAction;
		float mPreSpeed;

		void Start ()
		{
			KGFMapSystem kgf = FindObjectOfType<KGFMapSystem> ();
			if (kgf != null)
				minimap = kgf.gameObject;
			mSimpleRpgAnimator = player.GetComponentInChildren<SimpleRpgAnimator> (true);
//			simpleRpgPlayerController = player.GetComponentInChildren<SimpleRpgPlayerController> (true);
			mPlayerDic = new Dictionary<int, GameObject> ();
			mOtherPlayerIds = new List<int> ();
			mMonsterDic = new Dictionary<int, GameObject> ();
			mUnitDic = new Dictionary<int, GameObject> ();
			client.onRecieveMonsterInfos = OnRecieveServerActions;
			mHeadUIPrefab = Resources.Load<GameObject> ("UnitUI/HeadRoot").GetComponent<HeadUIBase> ();
			if (miniMap == null)
				miniMap = FindObjectOfType<KGFMapSystem> ();
			gameObject.GetOrAddComponent<AssetbundleManager> ();
		}

		void Update ()
		{
			if (isStart) {
				if (player.position != mPrePosition || player.forward != mPreForward || mSimpleRpgAnimator.Action != mPreAction ||
				     mPlayerInfo.skillId > -1) {
					mPrePosition = player.position;
					mPreForward = player.forward;
					mPreAction = mSimpleRpgAnimator.Action;
//					mPreSpeed = simpleRpgPlayerController._animation_speed;
//					SendPlayerMessage (player);
					mPlayerInfo.unitInfo.transform.forward = IntVector3.ToIntVector3 (player.forward);
					mPlayerInfo.unitInfo.transform.position = IntVector3.ToIntVector3 (player.position);
					if (selectedUnit != null) {
						mPlayerInfo.targetId = selectedUnit.unitInfo.attribute.unitId;
					} else {
						mPlayerInfo.targetId = -1;
					}
					mPlayerInfo.unitInfo.animation.action = mPreAction;
					mPlayerInfo.unitInfo.animation.animSpeed = mPreSpeed;
					client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, mPlayerInfo);
					mPlayerInfo.skillId = -1;
				}
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerConstant.LAYER_UNIT)) {
					SelectUnit (hit);
				} else {
					if (!EventSystem.current.IsPointerOverGameObject ()) {
						selectedUnit = null;
						this.handleSelectRing.transform.position = new Vector3 (0, -1000f, 0);
					}
				}
			}
			if (selectedUnit != null) {
				handleSelectRing.gameObject.SetActive (true);
				handleSelectRing.transform.position = selectedUnit.transform.position;
			} else {
				handleSelectRing.gameObject.SetActive (false);
			}

			if(miniMap!=null){
				if(miniMap.GetFullscreen ())
					PanelManager.Instance.ShowBigMapMask ();
				else
					PanelManager.Instance.HideBigMapMask ();
			}

		}

		public void Connect (string ip, int port)
		{
			client.Connect (ip, port, OnConnected, OnRecieveGameStartInfo, OnRecievePlayerMessage);
		}

		public void SendChat (string chat)
		{
			mPlayerInfo.chat = chat;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, mPlayerInfo);
			mPlayerInfo.chat = "";
		}

		void SelectUnit (RaycastHit hit)
		{
			MMOUnit mmoUnit = hit.transform.GetComponent<MMOUnit> ();
			selectedUnit = mmoUnit;
//			handleSelectRing.transform.SetParent (mmoUnit.transform);
//			handleSelectRing.transform.localPosition = new Vector3 (0, 0.1f, 0);//大会集材
		}

		public MMOUnit GetUnitByUnitId(int unitId){
			if (mUnitDic.ContainsKey (unitId)) {
				return mUnitDic [unitId].GetComponent<MMOUnit>();
			}
			return null;
		}

		void OnConnected (NetworkMessage msg)
		{
			Debug.Log ("OnConnected");
		}

		//TODO 把通信数据放在一个主对象的不同参数里面，这个容易理解很保存数据。
		//开发量也相对较少，否则分开的化需要不同的对象，方法，action，数量多，时间长不易于管理。
		//主要是玩家自己的操作
		void OnRecieveGameStartInfo (NetworkMessage msg)
		{
			mPlayerInfo = msg.ReadMessage<PlayerInfo> ();
			mPlayerId = mPlayerInfo.playerId;
//			rpgCamera.enabled = true;
			player.gameObject.SetActive (true);
			mPlayerInfo.unitInfo.attribute.unitName = playerName;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, mPlayerInfo);
			PanelManager.Instance.mainInterfacePanel.gameObject.SetActive (true);
			//TODO
//			PanelManager.Instance.chatPanel.gameObject.SetActive (true);
			if (minimap != null)
				minimap.SetActive (true);
			isStart = true;
		}

		void OnRecievePlayerMessage (NetworkMessage msg)
		{
			TransferData transferData = msg.ReadMessage<TransferData> ();
			HashSet<int> activedPlayerIds = new HashSet<int> ();
			for (int i = 0; i < transferData.playerDatas.Length; i++) {
				if (!mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
					GameObject playerGO;
					if (transferData.playerDatas [i].playerId != mPlayerId) {
						playerGO = InstantiateUnit (0, transferData.playerDatas [i].unitInfo);
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
					mPlayerDic [transferData.playerDatas [i].playerId].transform.position = IntVector3.ToVector3 (transferData.playerDatas [i].unitInfo.transform.position);
					mPlayerDic [transferData.playerDatas [i].playerId].transform.forward = IntVector3.ToVector3 (transferData.playerDatas [i].unitInfo.transform.forward);
					mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().SetAnimation (transferData.playerDatas [i].unitInfo.animation.action, transferData.playerDatas [i].unitInfo.animation.animSpeed);
				} else {
					SetCurrentPlayer (transferData.playerDatas [i]);
					if(mPlayerInfo!=null)
						mPlayerInfo.unitInfo = transferData.playerDatas [i].unitInfo;
				}

				//TODO temp change the mmounit when respawn;
				if (!mUnitDic.ContainsKey (transferData.playerDatas [i].unitInfo.attribute.unitId)) {
					if (mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
						//TODO need remove old unit.
						mUnitDic.Add (transferData.playerDatas [i].unitInfo.attribute.unitId,mPlayerDic [transferData.playerDatas [i].playerId]);
					}
				}

				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.animation = transferData.playerDatas [i].unitInfo.animation;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.attribute = transferData.playerDatas [i].unitInfo.attribute;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.action = transferData.playerDatas [i].unitInfo.action;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.skillIds = transferData.playerDatas [i].unitInfo.skillIds;
				if (!string.IsNullOrEmpty (transferData.playerDatas [i].chat)) {
					if (onChat != null) {
						if (transferData.playerDatas [i].playerId != mPlayerId)
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", transferData.playerDatas [i].unitInfo.attribute.unitName, transferData.playerDatas [i].chat));
						else
							onChat (string.Format ("<color=yellow>{0}</color>:{1}", "you", transferData.playerDatas [i].chat));
					}
				}
				MMOUnit mmoUnit = mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ();
				if (transferData.playerDatas [i].playerId == mPlayerId) {
					MMOUnitSkill mmoUnitSkill = mmoUnit.GetComponent<MMOUnitSkill> ();
					if(!mmoUnitSkill.IsInitted){
						mmoUnitSkill.InitSkills ();
						PanelManager.Instance.InitSkillIcons (mmoUnitSkill);
					}
				}
				if (mmoUnit.unitInfo.action.actionId > 0) {
					mmoUnit.GetComponent<MMOUnitSkill> ().PlayServerSkill (mmoUnit.unitInfo.action.actionId);
					mmoUnit.unitInfo.action.actionId = -1;
				}
			}
		}

		void SetCurrentPlayer(PlayerInfo playInfo){
			MMOUnit playerUnit = mPlayerDic [playInfo.playerId].GetComponent<MMOUnit> ();
//			simpleRpgPlayerController.enabled = false;
			//TODO イベントの形になればいい。
			if (playInfo.unitInfo.attribute.currentHP <= 0) {
				StopControll ();
				PerformManager.Instance.ShowCurrentPlayerDeathEffect (playerUnit);
				PanelManager.Instance.ShowCommonDialog ("Death", "you are killed", "復活", () => {
					MMOClient.Instance.SendRespawn ();
				});
			} else {
				ReleaseControll ();
				PanelManager.Instance.HideCommonDialog ();
				PerformManager.Instance.HideCurrentPlayerDeathEffect ();
			}
		}

		int mCurrentFrame = 0;
		//TODO 以后需要跟playerinfo合并，只保留更新其他npc和玩家自身两个api。
		void OnRecieveServerActions (NetworkMessage msg)
		{
			TransferData data = msg.ReadMessage<TransferData> ();
			mCurrentFrame ++;
			for (int i = 0; i < data.monsterDatas.Length; i++) {
				if (!mMonsterDic.ContainsKey (data.monsterDatas [i].attribute.unitId)) {
					GameObject monsterGo = InstantiateUnit (data.monsterDatas [i].attribute.unitType, data.monsterDatas [i]);
					mMonsterDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
					mMonsterDic [data.monsterDatas [i].attribute.unitId].SetActive (true);
					if (!mUnitDic.ContainsKey (data.monsterDatas [i].attribute.unitId))
						mUnitDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
				}
				mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit> ().frame = mCurrentFrame;
				UnitInfo unitInfo = data.monsterDatas [i];
				MMOUnit monster = mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit> ();
				monster.transform.position = IntVector3.ToVector3 (data.monsterDatas [i].transform.position);
				monster.transform.forward = IntVector3.ToVector3 (data.monsterDatas [i].transform.forward);
				monster.unitInfo = unitInfo;
				if (monster.unitInfo.attribute.currentHP <= 0) {
					monster.Death ();
				}
				monster.SetAnimation (data.monsterDatas [i].animation.action, data.monsterDatas [i].animation.animSpeed);
				if (data.monsterDatas [i].action.actionId >= 0) {
					monster.GetComponent<MMOUnitSkill> ().PlayServerSkill (data.monsterDatas [i].action.actionId);
					data.monsterDatas [i].action.actionId = -1;
				}
			}
			List<int> removeList = new List<int> ();
			foreach(int id in mMonsterDic.Keys){
				if(mMonsterDic[id].GetComponent<MMOUnit>().frame!=mCurrentFrame){
					removeList.Add (id);
				}
			}

			for(int i=0;i<removeList.Count;i++){
				Destroy (mMonsterDic [removeList[i]]);
				mMonsterDic.Remove (removeList[i]);
				mUnitDic.Remove (removeList[i]);
			}

			if (data.hitDatas.Length > 0) {
				for (int i = 0; i < data.hitDatas.Length; i++) {
					OnHit (data.hitDatas [i]);
				}
			}
			if(data.actions.Length > 0){
				for(int i = 0; i < data.actions.Length; i++) {
					DoClientPlayerAction (data.actions[i]);
				}
			}
		}

		void OnHit (HitInfo hitInfo)
		{
			PerformManager.Instance.ShowHitInfo (hitInfo,mUnitDic);
		}

		GameObject InstantiateUnit (int unitType, UnitInfo unitInfo)
		{
			MUnit mUnit = CSVManager.Instance.GetUnit (unitInfo.attribute.unitType);
			GameObject unitPrebfab = Resources.Load<GameObject> (mUnit.resource_name);
			unitPrebfab.SetActive (false);
			GameObject unitGo = Instantiate (unitPrebfab) as GameObject;
			unitGo.GetOrAddComponent<MMOUnitSkill> ();
			unitGo.SetActive (true);
			MMOUnit mmoUnit = unitGo.GetComponent<MMOUnit> ();
			mmoUnit.unitInfo = unitInfo;
			GameObject go = Instantiate (mHeadUIPrefab.gameObject);
			go.GetComponent<HeadUIBase> ().SetUnit (mmoUnit);
			return unitGo;
		}
	
		public PlayerInfo playerInfo{
			get{
				return mPlayerInfo;
			}
		}

		public void StopControll(){
//			simpleRpgPlayerController.enabled = false;
		}

		public void ReleaseControll(){
//			simpleRpgPlayerController.enabled = true;
		}

		//Send the action to the server.
		public void DoServerPlayerAction(int actionType,int actionId){
			MMOAction action = new MMOAction ();
			action.actionType = actionType;
			action.actionId = actionId;
			if(selectedUnit!=null)
				action.targetId = selectedUnit.unitInfo.attribute.unitId;
			MMOClient.Instance.SendAction (action);
		}

		//Do the action from server.
		public void DoClientPlayerAction(MMOAction action){
			ActionManager.Instance.DoAction (action);
		}

	}
}
