using UnityEngine;
using System.Collections;

namespace MMO
{
	public class ShootProjectileObject : ShootObject
	{

		public float maxShootAngle = 30;

		public bool hitWater = false;
		int waterLayer = 4;
		public GameObject hitWaterPrefab;

		public override void Shoot(MMOUnit attacker,Vector3 targetPos, Vector3 offset){
			base.Shoot (attacker, target, offset);
			this.targetPos =  targetPos + offset;
			this.target = null;
			this.offset = offset;
			hitWater = false;
			StartCoroutine (ProjectileRoutine ());
		}

		public override void Shoot (MMOUnit attacker, Transform target, Vector3 offset)
		{
			base.Shoot (attacker, target, offset);
			this.target = target;
			this.offset = offset;
			hitWater = false;
			StartCoroutine (ProjectileRoutine ());
		}

		IEnumerator ProjectileRoutine ()
		{
			if (shootEffect != null)
				Instantiater.Spawn (true, shootEffect, thisT.position, thisT.rotation);
			calculatedLayer = CalculateLayer ();//重新计算目标层
			float timeShot = Time.time;
			bool hit = false;
			if (target != null)
				targetPos = target.position + offset;
			
			//make sure the shootObject is facing the target and adjust the projectile angle
			thisT.LookAt (targetPos);
			float angle = Mathf.Min (1, Vector3.Distance (thisT.position, targetPos) / maxShootRange) * maxShootAngle;
			//clamp the angle magnitude to be less than 45 or less the dist ratio will be off
			thisT.rotation = thisT.rotation * Quaternion.Euler (-angle, 0, 0);
			Vector3 startPos = thisT.position;
			float iniRotX = thisT.rotation.eulerAngles.x;
			float y = Mathf.Min (targetPos.y, startPos.y);
			float totalDist = Vector3.Distance (startPos, targetPos);
			RaycastHit rayHit;
			Vector3 prePos = thisT.transform.position;
			//while the shootObject havent hit the target
			while (!hit) {
				if (target != null) {
					Vector3 targetMove = (target.position + offset) - targetPos;
					thisT.position += targetMove;
					targetPos = target.position + offset;
				}
				//calculating distance to targetPos
				Vector3 curPos = thisT.position;
				curPos.y = y;
				float currentDist = Vector3.Distance (curPos, targetPos);
				float curDist = Vector3.Distance (thisT.position, targetPos);
				if (Time.time - timeShot < 3.5f) {
					//calculate ratio of distance covered to total distance
					float invR = 1 - Mathf.Min (1, currentDist / totalDist);
					//use the distance information to set the rotation, 
					//as the projectile approach target, it will aim straight at the target
					Vector3 wantedDir = targetPos - thisT.position;
					if (wantedDir != Vector3.zero) {
						Quaternion wantedRotation = Quaternion.LookRotation (wantedDir);
						float rotX = Mathf.LerpAngle (iniRotX, wantedRotation.eulerAngles.x, invR);
						//make y-rotation always face target
						thisT.rotation = Quaternion.Euler (rotX, wantedRotation.eulerAngles.y, wantedRotation.eulerAngles.z);
					}
				} else {
					//this shoot time exceed 3.5sec, abort the trajectory and just head to the target
					thisT.LookAt (targetPos);
				}

				//TODO
//				if (checkType == ShootCheckType.LineCast && Vector3.Distance (transform.position, prePos) > hitThreshold) {
//					if (Physics.Raycast (prePos, transform.forward, out rayHit, Vector3.Distance (transform.position, prePos), 1 << waterLayer)) {
//						targetPos = rayHit.point;
//						hit = true;
//						hitWater = true;
//						if (hitWaterPrefab != null) {
//							Instantiater.Spawn (true, hitWaterPrefab, rayHit.point, Quaternion.identity);
//						}
//						break;
//					} else if (Physics.Raycast (prePos, transform.forward, out rayHit, Vector3.Distance (transform.position, prePos), calculatedLayer)) {
//						targetPos = rayHit.point;
//						hit = true;
//						break;
//					}
//				}

				if (curDist <= hitThreshold && !hit) {
					hit = true;
					break;
				}
				prePos = thisT.position;
				//move forward
				thisT.Translate (Vector3.forward * Mathf.Min (speed * Time.deltaTime, curDist));
//				Debug.Log (string.Format( "targetPos:{0}",targetPos.ToString()));
				yield return null;
			}
			if (hitObject != null && !hitWater) {
//			if(!hit)
//			{
//				GameObject hitObj = Instantiater.Spawn(true,hitObject.gameObject,targetPos,Quaternion.identity);
//
//				//如果没有击中,则表示没击中的特效。
//				hitObj.GetComponent<HitObject>().Hit(null);
//			}
//			else
//			{
				GameObject hitObj = Instantiater.Spawn (true, hitObject.gameObject, targetPos, Quaternion.identity);
				//TODO
//				if (damageAble) {
//					hitObj.GetComponent<HitObject> ().damageObj = damageObj;
//					hitObj.GetComponent<HitObject> ().Hit (target);
//				} else
//					hitObj.GetComponent<HitObject> ().Hit (null);
//			}
			}
//			Instantiater.UnSpawn (true, gameObject);
			Destroy (gameObject,3);
		}
	}
}