using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace MMO
{

	[Serializable]
	public class HitInfo:MessageBase
	{
		public int casterId;
		public int[] hitIds;
		public int[] damages;
		public int[] hitObjectIds;
		public IntVector3[] hitPositions;
	}

	[Serializable]
	public class UnitInfo : MessageBase
	{
		public MMOAttribute attribute;
		public MMOTransform transform;
		public MMOAnimation animation;
		public MMOAction action;
		public int[] skillIds;

		public UnitInfo ()
		{
			attribute = new MMOAttribute ();
			transform = new MMOTransform ();
			animation = new MMOAnimation ();
			action = new MMOAction ();
			skillIds = new int[0];
		}
	}

	[Serializable]
	public class PlayerInfo : MessageBase
	{
		public int playerId;
		public string chat;
		public int skillId;
		public int targetId;
		public UnitInfo unitInfo;

		public PlayerInfo(){
			skillId = -1;
			targetId = -1;
		}
	}

	[Serializable]
	public class TransferData : MessageBase
	{
		public PlayerInfo[] playerDatas;
		public UnitInfo[] monsterDatas;
		public HitInfo[] hitDatas;

		public TransferData ()
		{
			playerDatas = new PlayerInfo[0];
			monsterDatas = new UnitInfo[0];
			hitDatas = new HitInfo[0];
		}
	}

	[System.Serializable]
	public class MMOAttribute:MessageBase
	{
		public int unitId;
		public int unitType;
		public string unitName;
		public int currentHP;
		public int maxHP;
		public int level;
		public int currentExp;
		public int maxExp;
	}

	[System.Serializable]
	public class MMOTransform:MessageBase
	{
		public IntVector3 playerPosition;
		public IntVector3 playerForward;

		public MMOTransform(){
			playerPosition = new IntVector3 ();
			playerForward = new IntVector3 ();
		}
	}

	[System.Serializable]
	public class MMOAnimation:MessageBase
	{
		public string action;
		public float animSpeed;
	}

	[System.Serializable]
	public class MMOAction:MessageBase
	{
		//when  is the player and is send from client to server, the unitId is not be used,because it will be get the unit by the connectid.
		public int casterId;
		//the actionId of caster;(must)
		public int actionId;
		//1:unit skill,2:other action;(must)
		public int actionType;

		//the cast target unit id;(maybe)
		public int targetId;
		//the cast target position;(maybe)
		public IntVector3 targetPos;

		public MMOAction(){
			actionId = -1;
			actionType = 1;
			targetPos = new IntVector3 ();
		}
	}

	[System.Serializable]
	public class MMORespawn:MessageBase
	{
		public int playerId;
	}


	[System.Serializable]
	public struct IntVector3
	{
		public int x;
		public int y;
		public int z;

		public const int MULTIPLE = 1000;

		public static IntVector3 ToIntVector3 (Vector3 vector)
		{
			IntVector3 pos = new IntVector3 ();
			pos.x = (int)(vector.x * MULTIPLE);
			pos.y = (int)(vector.y * MULTIPLE);
			pos.z = (int)(vector.z * MULTIPLE);
			return pos;
		}

		public static Vector3 ToVector3 (IntVector3 intPos)
		{
			Vector3 pos = new Vector3 (((float)intPos.x) / MULTIPLE, ((float)intPos.y) / MULTIPLE, ((float)intPos.z) / MULTIPLE);
			return pos;
		}
	}
}