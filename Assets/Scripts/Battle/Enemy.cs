using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
	public Animator enemyAnimator;
	public Animator playerAnimator;
	public PlayerController player = null;
	public EnemyController enemy;
	public BaseAttribute enemyAttribute;
	protected Transform mTransform;

	public BodyType bodyType = BodyType.Head;
	public enum BodyType{Head,Leg,Body};

	void Start ()
	{
		mTransform = this.transform;
	}

	void AttackSpeedReset ()
	{
		playerAnimator.speed = 1;
		enemyAnimator.speed = 1;
	}

	void OnTriggerStay (Collider sword)
	{

		if(enemy.isCanAttack && sword.transform.root == player.transform && !player.pm.Fsm.GetFsmBool("isDead").Value)
		{
			enemy.isCanAttack = false;
			player.IsCanAttack = false;
			float durection = CommonUtility.GetDirection(transform.forward, player.transform.forward);
			if(durection<0)
			{
				player.OnHit(0,-0.05f,enemy,true);
			}
			else
			{
				player.OnHit(1,-0.1f,enemy,true);
			}
			enemyAnimator.speed = 1;
			Invoke ("AttackSpeedReset", 0.3f);
		}

		if (sword.tag.CompareTo ("Weapon") == 0 && player.IsCanAttack == true) {
			player.IsCanAttack = false;
			enemy.isCanAttack = false;
			GameObject blood = BattleController.SingleTon().spawnManager.SpawnHitPrefab0(mTransform.position,Quaternion.identity);
			blood.transform.parent = mTransform;
			blood.transform.localPosition = Vector3.zero;
			blood.transform.localRotation = Quaternion.identity;
			blood.transform.localScale = Vector3.one;
			enemyAttribute = BattleController.SingleTon().enemyAttribute;
			enemyAttribute.AdjustHealthByPercent(-0.1f);
			int index = Random.Range(0,3);
			enemy.OnHit(index,-0.2f,player);
			playerAnimator.speed = 1;
			Debug.Log("playerAnimator.speed :"+ playerAnimator.speed );
			Invoke ("AttackSpeedReset", 0.3f);
		}
	}

}