using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour
{
		private Animator enemyAnimator;
	public string AttackID;
		// Use this for initialization
		void Start ()
		{
				enemyAnimator = GameObject.FindGameObjectWithTag ("Enemy").GetComponent<Animator> ();
		}
	
		// Update is called once per frame
	AnimatorStateInfo stateInfo;
		void Update ()
		{
			 stateInfo = enemyAnimator.GetCurrentAnimatorStateInfo (0);  
				if (!stateInfo.IsName ("014idle"))  
		{
						enemyAnimator.SetBool (AttackID, false);
	
				}
		}

		void OnTriggerStay (Collider girl)
		{
			
				if (girl.tag.CompareTo ("Player") == 0) {
				
						enemyAnimator.SetBool (AttackID, true);
				
				}
		}
}
