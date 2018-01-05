using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System;

namespace MMO
{
	[Serializable]
	public class SkillInfo:MessageBase
	{
		public int skillId;
		public int[] targetIds;
		public Vector3 targetPos;
	}

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

		public UnitInfo ()
		{
			attribute = new MMOAttribute ();
			transform = new MMOTransform ();
			animation = new MMOAnimation ();
			action = new MMOAction ();
		}
	}

	[Serializable]
	public class PlayerInfo : MessageBase
	{
		public int playerId;
		public string chat;
		public int skillId;
		public UnitInfo unitInfo;
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
		public Vector3 playerPosition;
		public Vector3 playerForward;
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
		public int attackType;
		public Vector3 targetPos;
	}

	[System.Serializable]
	public class IntVector3:MessageBase
	{
		public int x;
		public int y;
		public int z;

		public static IntVector3 GetInstance (Vector3 vector)
		{
			IntVector3 pos = new IntVector3 ();
			pos.x = (int)(vector.x * 1000);
			pos.y = (int)(vector.y * 1000);
			pos.z = (int)(vector.z * 1000);
			return pos;
		}

		public static Vector3 ToVector3 (IntVector3 vector)
		{
			Vector3 pos = new Vector3 ();
			pos.x = vector.x / 1000f;
			pos.y = vector.y / 1000f;
			pos.z = vector.z / 1000f;
			return pos;
		}
	}
}