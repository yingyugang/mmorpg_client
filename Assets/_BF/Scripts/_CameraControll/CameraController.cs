using UnityEngine;
using System.Collections;

public class CameraController : SingleMonoBehaviour<CameraController>
{

	public Camera mainCamera;
	public bool moveAble = true;
	public Vector3 defaultCameraPos;
	public float defaultmainOrthographicSize;
	public float speed = 20;

	protected override void Awake ()
	{
		base.Awake ();
		defaultCameraPos = mainCamera.transform.position;
		defaultmainOrthographicSize = mainCamera.orthographicSize;
	}

	void Update ()
	{
		if (moveAble) {
			float realSpeed = speed;
			if (Input.GetKey (KeyCode.LeftShift)) {
				realSpeed = speed / 4;
			}
			if (Input.GetKey (KeyCode.D)) {
				mainCamera.transform.position += mainCamera.transform.right * Time.unscaledDeltaTime * realSpeed;
			}
			if (Input.GetKey (KeyCode.A)) {
				mainCamera.transform.position -= mainCamera.transform.right * Time.unscaledDeltaTime * realSpeed;
			}
			if (Input.GetKey (KeyCode.W)) {
				mainCamera.transform.position += mainCamera.transform.up * Time.unscaledDeltaTime * realSpeed;
			}
			if (Input.GetKey (KeyCode.S)) {
				mainCamera.transform.position -= mainCamera.transform.up * Time.unscaledDeltaTime * realSpeed;
			}
			if (Input.GetKey (KeyCode.Q)) {
				mainCamera.orthographicSize -= Time.unscaledDeltaTime * realSpeed;
			}
			if (Input.GetKey (KeyCode.E)) {
				mainCamera.orthographicSize += Time.unscaledDeltaTime * realSpeed;
			}
			mainCamera.orthographicSize = Mathf.Max (0.1f,mainCamera.orthographicSize);
			if (Input.GetKey (KeyCode.R)) {
				mainCamera.transform.position = defaultCameraPos;
				mainCamera.orthographicSize = defaultmainOrthographicSize;
			}
		}
	}

	void OnGUI(){
		float startY = 50;
		GUI.Label (new Rect (10, startY, 200, 30), "D - move right");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "A - move left");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "W - move up");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "S - move down");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "Q - scale camera big");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "E - scale camera small");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "LeftShift - slow move or scale");
		GUI.Label (new Rect (10, startY += 30, 200, 30), "R - reset to begin");

	}

}
