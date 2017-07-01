using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{
	public Transform boss;
	public bool isInBoss;

	void OnTriggerStay(Collider coll)
	{
		if(coll.transform.root == boss)
		{
			isInBoss = true;
		}
	}
	/*void OnTriggerEnter (Collider other)
	{
	
			if (other.tag == "Ememy") {
					print ("hit");

			}
	}*/
}
