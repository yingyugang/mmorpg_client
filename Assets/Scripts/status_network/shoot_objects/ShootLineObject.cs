using UnityEngine;
using System.Collections;

namespace MMO
{
	//途中の物理チェックが不要だ
	public class ShootLineObject : ShootObject
	{

		public override void Shoot (MMOUnit attacker, Vector3 targetPos, Vector3 offset)
		{
			base.Shoot (attacker, targetPos, offset);
			StartCoroutine (_Shoot ());
		}

		public override void Shoot (MMOUnit attacker, MMOUnit target, Vector3 offset)
		{
			base.Shoot (attacker, target, offset);
			StartCoroutine (_Shoot ());
		}

		public override void Shoot (MMOUnit attacker, Transform target, Vector3 offset)
		{
			base.Shoot (attacker, target, offset);
			StartCoroutine (_Shoot ());
		}

		IEnumerator _Shoot ()
		{
			Debug.Log ("_Shoot");
			if (target != null)
				targetPos = target.position + offset;
			if(targetPos!=thisT.position)
				thisT.LookAt (targetPos);

			float sqrHitThreshold = hitThreshold * hitThreshold;
			bool hit = false;
			while (!hit) {
				if(target != null){
					targetPos = target.position + offset;
					if(targetPos!=thisT.position)
						thisT.LookAt (targetPos);
				}
				thisT.position += thisT.forward * speed * Time.deltaTime;
				float sqrCurrentDistance = (targetPos - thisT.position).sqrMagnitude;
				float sqrDeltaMove = speed * speed * Time.deltaTime * Time.deltaTime;
				if(sqrDeltaMove >= sqrCurrentDistance || sqrCurrentDistance <= sqrHitThreshold){
					thisT.position = targetPos;
					hit = true;
				}
				yield return null;
			}
			yield return null;
			OnHit ();
		}

	}

}
