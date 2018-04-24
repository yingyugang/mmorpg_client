using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScene : MonoBehaviour {

	public Vector3 p0 = Vector3.zero;
	public Vector3 p1 = new Vector3 (5,5,0);
	public Vector3 p2= new Vector3 (10,0,0);
	public float length = 100;
	public float angle = 45;
	public LineRenderer lineRenderer;
	public Material mat;
	public float scrollSpeed = -2f;
	public float radio = 0.6f;

	void Awake(){
		lineRenderer = gameObject.AddComponent<LineRenderer> ();
		lineRenderer.material = mat;
	}

	void Update () {
		lineRenderer.positionCount = Mathf.CeilToInt(length);
		float offset = Time.time * scrollSpeed;
		mat.SetTextureOffset ("_MainTex", new Vector2(offset, 0));
		mat.SetTextureScale("_MainTex", new Vector2(length / 2, 1));
		p0 = transform.position;
		float amend = 1;
		if(angle < 45){
			amend = Mathf.Tan (angle / 180 * Mathf.PI);
		}
		float forward = length * Mathf.Cos (angle / 180 * Mathf.PI) * amend;
		p1 = p0 + new Vector3(0,length * Mathf.Sin(angle / 180 * Mathf.PI) * amend,0) + transform.forward * forward * radio;
		p2 = p0 + transform.forward * forward;
		for(int i = 0;i<length;i++){
			Vector3 targetPos = BezierUtility.Bezier2 (i / (float)length, p0, p1, p2);
			if(i>0){
				RaycastHit hit;
				Vector3 direct = (targetPos - lineRenderer.GetPosition (i - 1)).normalized;
				float length = (targetPos - lineRenderer.GetPosition (i - 1)).magnitude;
				if(Physics.Raycast(lineRenderer.GetPosition(i-1),direct,out hit,length,1<<LayerConstant.LAYER_GROUND)){
					lineRenderer.SetPosition (i,hit.point);
					lineRenderer.positionCount = i + 1;
					break;
				}
			}
			lineRenderer.SetPosition (i,targetPos);
		}
	}


}
