using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TestGL : MonoBehaviour {

	public Material mat;
	private Vector3 startVertex;
	private Vector3 mousePos;
	void Update() {
		mousePos = Input.mousePosition;
		if (Input.GetMouseButton(0))
		{
			startVertex = new Vector3(mousePos.x / Screen.width, mousePos.y / Screen.height, 0);
			if(vertexs.Count % 2 == 0 && vertexs.Count!=0)
			{
				vertexs.Add(vertexs[vertexs.Count-1]);
			}
			vertexs.Add(startVertex);
		}
			
	}
	void OnPostRender() {
		if (!mat) {
			Debug.LogError("Please Assign a material on the inspector");
			return;
		}
		GL.PushMatrix();
		mat.SetPass(0);
		GL.LoadOrtho();
		GL.Begin(GL.LINES);
		GL.Color(Color.red);
		foreach(Vector3 pos in vertexs)
		{
			GL.Vertex(pos);
		}
		GL.End();
		GL.PopMatrix();
	}

	List<Vector3> vertexs;
	void Start()
	{
		vertexs = new List<Vector3>();
//		vertexs.Add(Vector3.zero);
//		vertexs.Add(new Vector3(10,10,0));
//		vertexs.Add(new Vector3(10,50,0));
//		vertexs.Add(new Vector3(50,50,0));
	}

	void Example() {
		startVertex = new Vector3(0, 0, 0);
	}

}
