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
		public IntVector3 hitOriginPosition;
		public IntVector3 hitNormal;
		public int hitLayer;
		public int unitSkillId;
		public int[] hitIds;
		public int[] nums;
		public int[] hitObjectIds;
		public IntVector3[] hitPositions;
	}

	[Serializable]
	public class ShootInfo:MessageBase
	{
		public int casterId;
		//-1 means hit immediatly.
		public int unitSkillId;
		public int targetId;
		public IntVector3 position;
		public IntVector3 forward;
		public static ShootInfo Instance(int casterId,int unitSkillId,int targetId,IntVector3 targetPos,IntVector3 forward){
			ShootInfo shootInfo = new ShootInfo ();
			shootInfo.casterId = casterId;
			shootInfo.unitSkillId = unitSkillId;
			shootInfo.targetId = targetId;
			shootInfo.position = targetPos;
			shootInfo.forward = forward;
			return shootInfo;
		}

	}

	[Serializable]
	public class UnitInfo : MessageBase
	{
		public MMOAttribute attribute;
		public MMOTransform transform;
		public StatusInfo action;
		public int[] unitSkillIds;
		public int camp;
		public bool isPlayer;

		public UnitInfo ()
		{
			attribute = new MMOAttribute ();
			transform = new MMOTransform ();
			action = new StatusInfo ();
			unitSkillIds = new int[0];
			camp = -1;
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
	public class PlayerControll{
		public int unitId;
		public float forward;
		public float right;
	}

	[Serializable]
	public class GameInitInfo: MessageBase
	{
		public int playType;
		public PlayerInfo playerInfo;
	}

	[Serializable]
	public class VoiceInfos : MessageBase
	{
		public VoiceInfo[] voices;
	}

	[Serializable]
	public class VoiceInfo : MessageBase
	{
		public int sender;
		public float[] voice;
	}

	[Serializable]
	public class TransferData : MessageBase
	{
		//PlayerInfo do not send better.replace as other parameter.
		public PlayerInfo[] playerDatas;
		public UnitInfo[] monsterDatas;
		public HitInfo[] hitDatas;
		public StatusInfo[] actions;
		public ShootInfo[] shoots;

		public TransferData ()
		{
			playerDatas = new PlayerInfo[0];
			monsterDatas = new UnitInfo[0];
			hitDatas = new HitInfo[0];
			actions = new StatusInfo[0];
			shoots = new ShootInfo[0];
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
		public IntVector3 position;
		public IntVector3 forward;

		public MMOTransform(){
			position = new IntVector3 ();
			forward = new IntVector3 ();
		}
	}

	[System.Serializable]
	public class MMOAnimation:MessageBase
	{
		public string action;
		public float animSpeed;
	}

	[System.Serializable]
	public class StatusInfo:MessageBase
	{
		//when  is the player and is send from client to server, the unitId is not be used,because it will be get the unit by the connectid.
		public int casterId;
		//the actionId of caster;(must)
		public int actionId;
		//1:unit skill,2:create projectile object;3:create hit object(must)
		public int status;
		//the cast target unit id;(maybe)
		public int targetId;
		//the cast target position or shoot begin position.
		public IntVector3 position;

		public IntVector3 forward;

		public StatusInfo(){
			actionId = -1;
			status = 1;
			targetId = -1;
			position = new IntVector3 ();
			forward = new IntVector3 ();
		}
	}

	[System.Serializable]
	public class RespawnInfo:MessageBase
	{
		public int playerId;
		public int unitId;
	}

	[System.Serializable]
	public struct IntVector3
	{
		public int x;
		public int y;
		public int z;

		public const int MULTIPLE = 1000;

		public IntVector3(int x,int y,int z){
			this.x = x;
			this.y = y;
			this.z = z;
		}

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

		public static IntVector3 zero{
			get{
				return new IntVector3 (0, 0, 0);
			}
		}

		public static IntVector3 operator+ (IntVector3 b, IntVector3 c) {
			IntVector3 v = new IntVector3();
			v.x = b.x + c.x;
			v.y = b.y + c.y;
			v.z = b.z + c.z;
			return v;
		}

		public static IntVector3 operator* (IntVector3 b, int c) {
			IntVector3 v = new IntVector3();
			v.x = b.x * c;
			v.y = b.y * c;
			v.z = b.z * c;
			return v;
		}

		public static IntVector3 operator/ (IntVector3 b, int c) {
			IntVector3 v = new IntVector3();
			v.x = b.x / c;
			v.y = b.y / c;
			v.z = b.z / c;
			return v;
		}
	}
}