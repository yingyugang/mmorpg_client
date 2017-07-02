using UnityEngine;
using System.Collections;

public class DissolveShaderController : MonoBehaviour {
	public float delayTime=0.5f;
	public float aniTime=10.0f;
	private Material _mat;
	// Use this for initialization
	void Start () {
		_mat = gameObject.GetComponent<Renderer>().material;
		StartCoroutine (setMat ());

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator setMat()
	{	
		yield return new WaitForSeconds (delayTime);
		float t = 0;
		
		while (t<aniTime) {
			t += Time.deltaTime;
			_mat.SetFloat ("_Amount", Mathf.Lerp (0, 1, t/aniTime));
			//Debug.Log("t="+t+"        "+Mathf.Lerp (0, 1, t/aniTime));
			yield return null;
		}

	}

	
}
