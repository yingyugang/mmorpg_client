using UnityEngine;
using System.Collections;
using StatusMachines;

public class DeathAction : UnitBaseAction {

	public float destoryDelay = 2;
	float mDestoryTime = 0;

	public override void OnEnter ()
	{
		base.OnEnter ();
//		if(BattleController.SingleTon().HandleTarget == hero)
//			BattleController.SingleTon().NextHandleTarget();
//		unit.Play(UnitAnimationClipName.death.ToString());
//		if(hero.Btn!=null)
//			hero.Btn.ShowDeathMask();
		if(unit.side == _Side.Enemy)
		{
//			hero.heroRes.GetComponent<Collider2D>().enabled = false;
			BattleManager.GetInstance().leftUnits.Remove (unit);
//			BattleController.SingleTon().LeftHeroes.Remove(hero);
		}
		else
		{
			BattleManager.GetInstance().rightUnits.Remove (unit);
//			BattleController.SingleTon().RightHeroes.Remove(hero);
//			BattleController.SingleTon().DeadHeroes.Add(hero);
		}
//		if(hero.healthBar!=null)
//			hero.healthBar.gameObject.SetActive(false);
//		BattleController.SingleTon().DeadHeroes.Add(hero);
		mDestoryTime = destoryDelay + Time.time;
	}

	public override void OnUpdate ()
	{
		base.OnUpdate ();
		if(mDestoryTime<Time.time)
			GO.SetActive (false);
	}

}
