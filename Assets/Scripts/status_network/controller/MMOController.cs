﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;

namespace MMO
{
	public enum PlayType
	{
RPG = 0,
TPS = 1

	}

	public class MMOController : SingleMonoBehaviour<MMOController>
	{
		public MMOClient client;
		public bool isStart;

		public Transform player;
		public Camera playerCamera;
		public Camera uiCamera;
		//if Serialized this while not run the construction function when game start;
		PlayerInfo mPlayerInfo;
		public string playerName;

		public string targetIp;
		public KGFMapSystem minimap;
		public UnityAction<string> onChat;
		public GameObject handleSelectRing;
		public MMOUnit selectedUnit;

		public GameObject rpgPlayer;
		public GameObject tpsPlayer;

		public BaseCameraController cameraController;
		public BasePlayerController playerController;

		HeadUIBase mHeadUIPrefab;
		Dictionary<int,GameObject> mUnitDic;
		Dictionary<int,GameObject> mPlayerDic;
		Dictionary<int,GameObject> mMonsterDic;
		List<int> mOtherPlayerIds;
		int mPlayerId;
		Vector3 mPrePosition;
		Vector3 mPreForward;
		int mPreSelectId = -1;
		float mPreSpeed;
		Terrain mTerrain;
		//0:rpg 1:tps
		public PlayType playType;

		void Start ()
		{
			mPlayerDic = new Dictionary<int, GameObject> ();
			mOtherPlayerIds = new List<int> ();
			mMonsterDic = new Dictionary<int, GameObject> ();
			mUnitDic = new Dictionary<int, GameObject> ();
			client.onRecieveMonsterInfos = OnRecieveServerActions;
			mHeadUIPrefab = Resources.Load<GameObject> ("UnitUI/HeadRoot").GetComponent<HeadUIBase> ();
			GameObject terrainPrefab;
			GameObject terrainPrefabT4M;
			ResourcesManager.Instance.GetTerrain ("FarmTerrain", out terrainPrefab, out terrainPrefabT4M);
			mTerrain = Instantiate (terrainPrefab).GetComponent<Terrain> ();
			mTerrain.drawHeightmap = true;
			mTerrain.gameObject.layer = LayerConstant.LAYER_GROUND;
			//TODO the T4M terrain's 精度不够，需要重新制作。
//			if(terrainPrefabT4M!=null)
//				Instantiate (terrainPrefabT4M);
			GameObject terrainObjectPrefab = ResourcesManager.Instance.GetTerrainObjects ("FarmTerrianObjects");
			Instantiate (terrainObjectPrefab).GetComponent<GameObject> ();
		}

		void InitPlayer ()
		{
			switch (playType) {
			case PlayType.RPG:
				InitRPGPlayerAndInterface ();
				break;
			case PlayType.TPS:
				InitTPSPlayerAndInterface ();
				break;
			default :
				break;
			}
		}

		void InitRPGPlayerAndInterface ()
		{
			rpgPlayer.SetActive (false);
			player = Instantiate (rpgPlayer).transform;
			RPGCameraController rpgCameraController = playerCamera.gameObject.GetOrAddComponent<RPGCameraController> ();
			rpgCameraController.target = player;
			cameraController = rpgCameraController;
			minimap.SetTarget (player.gameObject);
			RPGPlayerController rpgPlayerController = player.gameObject.GetOrAddComponent<RPGPlayerController> ();
			rpgPlayerController.rpgCameraController = rpgCameraController;
			playerController = rpgPlayerController;
			PanelManager.Instance.mainInterfacePanel.bulletGroup.gameObject.SetActive (false);
		}

		void InitTPSPlayerAndInterface ()
		{
			tpsPlayer.SetActive (false);
			player = Instantiate (tpsPlayer).transform;
			TPSCameraController tpsCameraController = playerCamera.gameObject.GetOrAddComponent<TPSCameraController> ();
			tpsCameraController.target = player;
			cameraController = tpsCameraController;
			minimap.SetTarget (player.gameObject);
			TPSPlayerController tpsPlayerController = player.gameObject.GetOrAddComponent<TPSPlayerController> ();
			tpsPlayerController.tpsCameraController = tpsCameraController;
			playerController = tpsPlayerController;
			PanelManager.Instance.mainInterfacePanel.bulletGroup.gameObject.SetActive (true);
			PanelManager.Instance.mainInterfacePanel.bulletGroup.SetWeapon (BattleConst.DEFAULT_BULLET_COUNT);
		}

		void Update ()
		{
			if (isStart) {
				int mCurrentSelectId = selectedUnit == null ? -1 : selectedUnit.unitInfo.attribute.unitId;
				//Postion and rotation を同期する
				//TODO 动画状态同步
				if (player.position != mPrePosition || player.forward != mPreForward || mPreSelectId != mCurrentSelectId) {
					//TODO 通信を減ったら
					mPrePosition = player.position;
					mPreForward = player.forward;
					mPreSelectId = mCurrentSelectId;
					mPlayerInfo.unitInfo.transform.forward = IntVector3.ToIntVector3 (player.forward);
					mPlayerInfo.unitInfo.transform.position = IntVector3.ToIntVector3 (player.position);
					mPlayerInfo.targetId = mCurrentSelectId;
					client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, mPlayerInfo);
				}
			}
			if (Input.GetKeyDown (KeyCode.Escape)) {
				Application.Quit ();
			}
			if (Input.GetMouseButtonDown (0)) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, Mathf.Infinity, 1 << LayerConstant.LAYER_UNIT | 1 << LayerConstant.LAYER_PLAYER)) {
					SelectUnit (hit);
				} else {
					if (!EventSystem.current.IsPointerOverGameObject ()) {
						selectedUnit = null;
						this.handleSelectRing.transform.position = new Vector3 (0, -1000f, 0);
					}
				}
			}
			if (selectedUnit != null) {
				if (playType == PlayType.RPG)
					handleSelectRing.gameObject.SetActive (true);
				handleSelectRing.transform.position = selectedUnit.transform.position;
			} else {
				handleSelectRing.gameObject.SetActive (false);
			}

			if (minimap != null) {
				if (minimap.GetFullscreen ())
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

		public void SendVoice (float[] data)
		{
			client.SendVoice (data);
		}

		void SelectUnit (RaycastHit hit)
		{
			MMOUnit mmoUnit = hit.transform.GetComponent<MMOUnit> ();
			selectedUnit = mmoUnit;
//			handleSelectRing.transform.SetParent (mmoUnit.transform);
//			handleSelectRing.transform.localPosition = new Vector3 (0, 0.1f, 0);//大会集材
		}

		public MMOUnit GetUnitByUnitId (int unitId)
		{
			if (mUnitDic.ContainsKey (unitId)) {
				return mUnitDic [unitId].GetComponent<MMOUnit> ();
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
			Debug.Log ("OnRecieveGameStartInfo");
			GameInitInfo gameInitInto = msg.ReadMessage<GameInitInfo> ();
			mPlayerInfo = gameInitInto.playerInfo;
			mPlayerId = mPlayerInfo.playerId;
			mPlayerInfo.unitInfo.attribute.unitName = playerName;
			PanelManager.Instance.mainInterfacePanel.gameObject.SetActive (true);
			playType = (PlayType)gameInitInto.playType;
			InitPlayer ();
			player.gameObject.SetActive (true);
			if (minimap != null)
				minimap.gameObject.SetActive (true);
			isStart = true;
		}

		void OnRecievePlayerMessage (NetworkMessage msg)
		{
			if (!isStart)
				return;
			TransferData transferData = msg.ReadMessage<TransferData> ();
			HashSet<int> activedPlayerIds = new HashSet<int> ();
			for (int i = 0; i < transferData.playerDatas.Length; i++) {
				if (!mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
					GameObject playerGO;
					if (transferData.playerDatas [i].playerId != mPlayerId) {
						playerGO = InstantiateUnit (transferData.playerDatas [i].unitInfo.attribute.unitType, transferData.playerDatas [i].unitInfo);
					} else {
						playerGO = player.gameObject;
						playerGO.layer = LayerConstant.LAYER_PLAYER;
						MMOUnit playerUnit = playerGO.GetComponent<MMOUnit> ();
						playerUnit.onDeath = () => {
							OnCurrentPlayerDeath ();
						};
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
				} else {
					if (mPlayerInfo != null)
						mPlayerInfo.unitInfo = transferData.playerDatas [i].unitInfo;
				}

				//TODO temp change the mmounit when respawn;
				if (!mUnitDic.ContainsKey (transferData.playerDatas [i].unitInfo.attribute.unitId)) {
					if (mPlayerDic.ContainsKey (transferData.playerDatas [i].playerId)) {
						//TODO need remove old unit.
						mUnitDic.Add (transferData.playerDatas [i].unitInfo.attribute.unitId, mPlayerDic [transferData.playerDatas [i].playerId]);
					}
				}
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.attribute = transferData.playerDatas [i].unitInfo.attribute;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.action = transferData.playerDatas [i].unitInfo.action;
				mPlayerDic [transferData.playerDatas [i].playerId].GetComponent<MMOUnit> ().unitInfo.unitSkillIds = transferData.playerDatas [i].unitInfo.unitSkillIds;
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
					if (!mmoUnitSkill.IsInitted) {
						mmoUnitSkill.InitSkills ();
						PanelManager.Instance.InitSkillIcons (mmoUnitSkill);
					}
				}
			}
		}

		void OnCurrentPlayerDeath ()
		{
			playerController.enabled = false;
			MMOUnit playerUnit = player.gameObject.GetComponent<MMOUnit> ();
			playerUnit.isDead = true;
			if (player.gameObject.GetComponent<BasePlayerController> () != null)
				player.gameObject.GetComponent<BasePlayerController> ().enabled = false;
			PerformManager.Instance.ShowCurrentPlayerDeathEffect (playerUnit);
			switch (this.playType) {
			case PlayType.RPG:
				break;
			case PlayType.TPS:
				PanelManager.Instance.mainInterfacePanel.HideAims ();
				Cursor.lockState = CursorLockMode.Confined;
				break;
			default:
				break;
			}
			PanelManager.Instance.mainInterfacePanel.btn_respawn.gameObject.SetActive (true);
			PanelManager.Instance.mainInterfacePanel.btn_respawn.onClick.AddListener (() => {
				PanelManager.Instance.mainInterfacePanel.btn_respawn.gameObject.SetActive (false);
				MMOClient.Instance.SendRespawn ();
			});
		}

		public bool IsPlayer (MMOUnit mmoUnit)
		{
			if (mPlayerInfo != null && mPlayerInfo.unitInfo != null) {
				return mmoUnit.unitInfo.attribute.unitId == mPlayerInfo.unitInfo.attribute.unitId;
			}
			return false;
		}

		public bool IsPlayer (int unitId)
		{
			if (mPlayerInfo != null && mPlayerInfo.unitInfo != null) {
				return unitId == mPlayerInfo.unitInfo.attribute.unitId;
			}
			return false;
		}

		int mCurrentFrame = 0;
		public bool isDebug;

		void OnRecieveServerActions (NetworkMessage msg)
		{
			if (!isStart)
				return;
			TransferData data = msg.ReadMessage<TransferData> ();
			mCurrentFrame++;
			for (int i = 0; i < data.monsterDatas.Length; i++) {
				if (!mMonsterDic.ContainsKey (data.monsterDatas [i].attribute.unitId)) {
					GameObject monsterGo = InstantiateUnit (data.monsterDatas [i].attribute.unitType, data.monsterDatas [i]);
					mMonsterDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
					mMonsterDic [data.monsterDatas [i].attribute.unitId].SetActive (true);
					if (!mUnitDic.ContainsKey (data.monsterDatas [i].attribute.unitId))
						mUnitDic.Add (data.monsterDatas [i].attribute.unitId, monsterGo);
					//不显示已经死了的怪物
					if (data.monsterDatas [i].attribute.currentHP <= 0) {
						monsterGo.SetActive (false);
					}
				}
				mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit> ().frame = mCurrentFrame;
				UnitInfo unitInfo = data.monsterDatas [i];
				MMOUnit monster = mMonsterDic [data.monsterDatas [i].attribute.unitId].GetComponent<MMOUnit> ();
				monster.transform.position = IntVector3.ToVector3 (data.monsterDatas [i].transform.position);
				monster.transform.forward = IntVector3.ToVector3 (data.monsterDatas [i].transform.forward);
				monster.unitInfo = unitInfo;
			}
			List<int> removeList = new List<int> ();
			foreach (int id in mMonsterDic.Keys) {
				if (mMonsterDic [id].GetComponent<MMOUnit> ().frame != mCurrentFrame) {
					removeList.Add (id);
				}
			}

			for (int i = 0; i < removeList.Count; i++) {
				Destroy (mMonsterDic [removeList [i]]);
				mMonsterDic.Remove (removeList [i]);
				mUnitDic.Remove (removeList [i]);
			}

			UpdateHits (data.hitDatas);
			UpdateActions (data.actions);
			UpdateShoots (data.shoots);
		}

		void OnRecievePlayerStatus (NetworkMessage msg)
		{
			StatusInfo statusInfo = msg.ReadMessage<StatusInfo> ();
			if (isDebug)
				Debug.Log (JsonUtility.ToJson (statusInfo));
			DoClientPlayerAction (statusInfo);
		}

		void UpdateActions (StatusInfo[] actions)
		{
			if (actions.Length > 0) {
				for (int i = 0; i < actions.Length; i++) {
					if (isDebug)
						Debug.Log (JsonUtility.ToJson (actions [i]));
					DoClientPlayerAction (actions [i]);
				}
			}
		}

		void UpdateHits (HitInfo[] hitInfos)
		{
			if (hitInfos.Length > 0) {
				for (int i = 0; i < hitInfos.Length; i++) {
					if (isDebug)
						Debug.Log (JsonUtility.ToJson (hitInfos [i]));
					OnHit (hitInfos [i]);
				}
			}
		}

		void UpdateShoots (ShootInfo[] shootInfos)
		{
			if (shootInfos.Length > 0) {
				for (int i = 0; i < shootInfos.Length; i++) {
					if (isDebug)
						Debug.Log (JsonUtility.ToJson (shootInfos [i]));
					ActionManager.Instance.DoShoot (shootInfos [i]);
				}
			}
		}

		void OnHit (HitInfo hitInfo)
		{
//			Debug.Log (JsonUtility.ToJson (hitInfo));
			PerformManager.Instance.ShowHitInfo (hitInfo, mUnitDic);
		}

		Dictionary<int,GameObject> mCachedUnitPrefabs;

		GameObject InstantiateUnit (int unitType, UnitInfo unitInfo, bool isLocal = false)
		{
			if (mCachedUnitPrefabs == null)
				mCachedUnitPrefabs = new Dictionary<int, GameObject> ();
			MUnit mUnit = CSVManager.Instance.GetUnit (unitInfo.attribute.unitType);
			GameObject unitPrebfab;

			//TODO need amend players load to assetbundle.
//			if (unitType == 0) {
//				unitPrebfab = Resources.Load<GameObject> ("Units/Player");
//			} else {
			if (mCachedUnitPrefabs.ContainsKey (unitType)) {
				unitPrebfab = mCachedUnitPrefabs [unitType];
			} else {
				if (isLocal) {
					unitPrebfab = ResourcesManager.Instance.GetUnitFromLocal (mUnit.resource_name);
				} else {
					unitPrebfab = ResourcesManager.Instance.GetUnit (mUnit.assetbundle, mUnit.resource_name);
				}
				mCachedUnitPrefabs.Add (unitType, unitPrebfab);
			}
//			}
			Debug.Log (string.Format ("{0}||{1}", mUnit.assetbundle, mUnit.resource_name));
			unitPrebfab.SetActive (false);
			GameObject unitGo = Instantiate (unitPrebfab) as GameObject;
			unitGo.GetOrAddComponent<MMOUnitSkill> ();
			unitGo.layer = LayerConstant.LAYER_UNIT;
			MMOUnit mmoUnit = unitGo.GetOrAddComponent<MMOUnit> ();
			mmoUnit.unitInfo = unitInfo;
			MiniMapManager.Instance.SetMiniIcon (mmoUnit);
			GameObject go = Instantiate (mHeadUIPrefab.gameObject);
			go.GetComponent<HeadUIBase> ().SetUnit (mmoUnit);
			unitGo.SetActive (true);
			return unitGo;
		}

		public PlayerInfo playerInfo {
			get {
				return mPlayerInfo;
			}
		}

		public Vector3 GetTerrainPos (Vector3 pos)
		{
			Vector3 terrainPos = new Vector3 (pos.x, mTerrain.SampleHeight (pos), pos.z);
			return terrainPos;
		}

		//Send the status to the server.
		//例えば　遷移とか、待機どか。
		//为了更好的用户体验，这些操作都在客户端进行。
		//ユーザー体験の為にする。
		public void SendPlayerAction (int actionType, int actionId, IntVector3 targetPos)
		{
			StatusInfo action = new StatusInfo ();
			action.status = actionType;
			action.actionId = actionId;
			action.position = targetPos;
			if (selectedUnit != null)
				action.targetId = selectedUnit.unitInfo.attribute.unitId;
			MMOClient.Instance.SendAction (action);
		}

		public void SendPlayerAction (StatusInfo statusInfo)
		{
			MMOClient.Instance.SendAction (statusInfo);
		}

		//Do the action from server.
		public void DoClientPlayerAction (StatusInfo action)
		{
			ActionManager.Instance.DoAction (action);
		}

		public void DoRespawn (int unitId)
		{
			if (IsPlayer (unitId)) {
//				ReleaseControll ();
				PanelManager.Instance.HideCommonDialog ();
				PerformManager.Instance.HideCurrentPlayerDeathEffect ();
				MMOUnit playerUnit = player.GetComponent<MMOUnit> ();
				if (playerUnit.GetComponent<BasePlayerController> () != null)
					playerUnit.GetComponent<BasePlayerController> ().enabled = true;
				switch (this.playType) {
				case PlayType.RPG:
					break;
				case PlayType.TPS:
					PanelManager.Instance.mainInterfacePanel.ShowAims ();
					Cursor.lockState = CursorLockMode.Locked;
					break;
				default:
					break;
				}
			} else {
				//TODO Do other monster respawn;
			}
			MMOUnit mmoUnit = GetUnitByUnitId (unitId);
			mmoUnit.unitAnimator.ResetTriggers ();
			mmoUnit.isDead = false;
			PerformManager.Instance.ShowRespawnEffect (mmoUnit.transform.position);
		}

	}
}
