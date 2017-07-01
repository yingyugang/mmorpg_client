using UnityEngine;
using System.Collections;

public enum BattlePerspective{AroundBoss,StayBehind,Fixed};
public enum HitBehaviour{Default,Fail,Fly};
public enum BattleStatus{Begin,Win,Fail};
//public enum PlayerState{Idle,IdleFree,Run,Roll,RollFree,PowerLock,Defense,Collect,Other};
public enum EnemyState{Idle,Battle,Escape};
public enum ItemType{Item0,Item1};

public static class CommonUtility {

	public static string battleA = "BattleA";
	public static string battleB = "BattleB";
	public static string villages = "Villages";

	
	public static float GetDirection(Vector3 direction0,Vector3 direction1)
	{
		return Vector3.Dot (direction0.normalized,direction1.normalized);
	}

	public static float GetDirection(Transform origin,Transform target)
	{
		return Vector3.Dot (origin.forward.normalized, target.forward.normalized);
	}

	public static bool GetAngleDirection(Transform origin,Transform target)
	{
		Vector3 crossAngle = Vector3.Cross (origin.forward,(target.position - origin.position).normalized);
		return crossAngle.y > 0 ? false : true;
	}

}

