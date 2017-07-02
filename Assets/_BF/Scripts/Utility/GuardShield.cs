using UnityEngine;
using System.Collections;

public class GuardShield : MonoBehaviour {

	public GameObject flashObj;

	public void ShowFlash()
	{
		flashObj.SetActive(false);
		flashObj.SetActive(true);
	} 

}
