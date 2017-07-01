using UnityEngine;
using System.Collections;

public class ShootObject : MonoBehaviour {

	public UnitController attacker;

	void OnTriggerEnter(Collider other)
	{
		Transform trans = other.transform.root;
		if (trans.GetComponent<PlayerController> () != null) {
			trans.GetComponent<PlayerController> ().OnHit(2,-0.1f,attacker,false);
			gameObject.SetActive(false);
		}
	}

}
