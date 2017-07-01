using UnityEngine;
using System.Collections;

public class Gate : MonoBehaviour {

	public string gateName = "Gate0";
	public string nextScene = "BattleA";
	public string nextGate = "Gate0";
	public Transform spawnPoint;

	public static string currentNextGate;

	void OnTriggerEnter(Collider other)
	{
		if (other.transform.root.tag == "Player") 
		{
			currentNextGate = nextGate;
			Application.LoadLevel(nextScene);
		}
	}

}
