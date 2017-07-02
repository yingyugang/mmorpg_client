//IGG
using UnityEngine;
using System.Collections;
using ThinksquirrelSoftware.Utilities;


public class CameraShakeLauncher : MonoBehaviour {
	public float delayTime=1;
	public int shakeNumber=1;
	public Vector3 shakeAmount = new Vector3 (0,10,0);
	public Vector3 rotationAmount = new Vector3 (0, 0, 0);

	[Range(0,0.1f)]
	public float shakeDistance=0.05f;
	[Range(0,100.0f)]
	public float shakeSpeed=50;
	[Range(0,1.0f)]
	public float shakeDecay=0.3f;

	#region Private variables
	private CameraShake shake;
	private bool shakeGUI;
	private bool shake1;
	private bool shake2;
	private bool multiShake;
	#endregion
	
	#region MonoBehaviour methods
	void Start()
	{
		shake = CameraShake.instance;

		StartCoroutine ("ShakeCamera");
	}

	#endregion



	IEnumerator ShakeCamera(){

		shake.numberOfShakes = shakeNumber;
		shake.shakeAmount = shakeAmount;
		shake.rotationAmount = rotationAmount;
		shake.distance = shakeDistance;
		shake.speed = shakeSpeed;
		shake.decay = shakeDecay;


		yield return new WaitForSeconds (delayTime);
		CameraShake.Shake();
		yield return null;
	}


	
	#region Manual shakes
	void Update()
	{

	}
	#endregion

}
