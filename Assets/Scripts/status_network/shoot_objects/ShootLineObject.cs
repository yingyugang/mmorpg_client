using UnityEngine;
using System.Collections;

namespace MMO{
//Line direction shoot ,with physics check.
public class ShootLineObject : ShootObject {

	public float maxDistance = 100;

		public override void Shoot(MMOUnit attacker,Transform target,Vector3 offset)
	{
		base.Shoot (attacker,target,offset);
		this.target = target;
		this.offset = offset;
		Vector3 direction = (target.position + offset - transform.position).normalized;
		StartCoroutine (_Shoot(direction));
	}

		public override void Shoot(MMOUnit attacker,Vector3 direction){
		base.Shoot (attacker,direction);
		StartCoroutine (_Shoot(direction));
	}

	IEnumerator _Shoot(Vector3 direction){
		if (shootAudio != null)
			AudioSource.PlayClipAtPoint (shootAudio,transform.position);
		transform.LookAt (transform.position + direction);
		Vector3 startPos = transform.position;
		Vector3 prePos = transform.position;
		Vector3 tmpPos;
		bool hit = false;
		RaycastHit rayHit;

		int targetLayer = 1 << targetLayers[0];
		for(int i=1;i<targetLayers.Length;i++)
		{
			targetLayer = targetLayer | (1 << targetLayers[i]);
		}
		while(!hit){
			prePos = transform.position;
			tmpPos = prePos + direction * speed * Time.deltaTime;
			//Raycast from prePos to curPos to check if hit;
			if(Physics.Raycast(prePos,direction,out rayHit,Vector3.Distance(tmpPos,prePos),targetLayer))
			{
				hit = true;//break out this loop;
				tmpPos = rayHit.point;
				if(hitObject!=null)
				{
					GameObject hitObj = Instantiater.Spawn(true,hitObject.gameObject,rayHit.point,Quaternion.identity);
//					hitObj.GetComponent<HitObject>().damageObj = damageObj;
//					hitObj.GetComponent<HitObject>().Hit(rayHit.transform);
				}
			}
			transform.position = tmpPos;
			yield return null;
		}
		yield return null;
		Instantiater.UnSpawn (true,gameObject);
	}
	}

}
