using UnityEngine;
using System.Collections;

public class rightArrowPoint : MonoBehaviour
{
	public Vector3 getPosition(){
		return transform.position;
	}

	public void addChild(GameObject obj){
		obj.transform.parent = transform;
	}
	public void removeChild(GameObject obj){
		obj.transform.parent = null;
	}
//		public void removeChild(GameObject obj,Quaternion trim){
//			obj.transform.parent = null;
//			obj.transform.rotation = obj.transform.rotation * trim;
//		}
}


