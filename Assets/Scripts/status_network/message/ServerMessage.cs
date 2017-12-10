using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace MMO
{

	public class PlayerInfo : MessageBase{
		public int playerId;
	}

	[Serializable]
	public class TransferData : MessageBase{
		public PlayerData[] playerDatas;
	}

	[Serializable]
	public class PlayerData : MessageBase{
		public int playerId;
		public Vector3 playerPosition;
		public Vector3 playerForward;
		public string action;
		public float animSpeed;
	}

}