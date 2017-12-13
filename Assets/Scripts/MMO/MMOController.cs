using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace MMO
{
	public class MMOController : SingleMonoBehaviour<MMOController>
	{
		public MMOClient client;
		public Transform player;
		public string targetIp;
		public GameObject playerPrefab;
		public SimpleRpgCamera rpgCamera;

		SimpleRpgAnimator mSimpleRpgAnimator;
		SimpleRpgPlayerController mSimpleRpgPlayerController;
		Dictionary<int,GameObject> mOtherPlayers;
		List<int> mOtherPlayerIds;
		int mPlayerId;
		Vector3 mPrePosition;
		Vector3 mPreForward;
		string mPreAction;
		float mPreSpeed;


		void Start ()
		{
			mSimpleRpgAnimator = player.GetComponentInChildren<SimpleRpgAnimator> (true);
			mSimpleRpgPlayerController = player.GetComponentInChildren<SimpleRpgPlayerController> (true);
			mOtherPlayers = new Dictionary<int, GameObject> ();
			mOtherPlayerIds = new List<int> ();
		}

		void Update ()
		{
			if (client.IsConnected) {
				if (player.position != mPrePosition || player.forward != mPreForward || mSimpleRpgAnimator.Action != mPreAction || mSimpleRpgPlayerController._animation_speed != mPreSpeed) {
					mPrePosition = player.position;
					mPreForward = player.forward;
					mPreAction = mSimpleRpgAnimator.Action;
					mPreSpeed = mSimpleRpgPlayerController._animation_speed;
					SendMessage (player);
				}
			}
		}

		public void Connect (string ip, int port)
		{
			client.Connect (ip, port, OnConnected, OnRecievePlayerInfo, OnRecieveMessage);
		}

		public void SendMessage (Transform player)
		{
			PlayerData data = new PlayerData ();
			data.playerId = mPlayerId;
			data.playerForward = player.forward;
			data.playerPosition = player.position;
			data.action = mPreAction;
			data.animSpeed = mPreSpeed;
			client.Send (MessageConstant.CLIENT_TO_SERVER_MSG, data);
		}

		void OnConnected (NetworkMessage msg)
		{
			
		}

		void OnRecievePlayerInfo (NetworkMessage msg)
		{
			PlayerInfo playerInfo = msg.ReadMessage<PlayerInfo> ();
			mPlayerId = playerInfo.playerId;
			rpgCamera.enabled = true;
			player.gameObject.SetActive (true);
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
					mOtherPlayers [playerHandle.playerDatas [i].playerId].transform.position = playerHandle.playerDatas [i].playerPosition;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].transform.forward = playerHandle.playerDatas [i].playerForward;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().Action = playerHandle.playerDatas [i].action;
					mOtherPlayers [playerHandle.playerDatas [i].playerId].GetComponent<SimpleRpgAnimator> ().SetSpeed (playerHandle.playerDatas [i].animSpeed);
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
	}
}
