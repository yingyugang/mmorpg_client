using UnityEngine;
using System.Collections;

public class UVAnim : MonoBehaviour {

	Mesh mesh;

	// Use this for initialization
	void Start () {
		mesh = new Mesh();
		Vector3[] vertexs = new Vector3[4];
		vertexs[0] = new Vector3(0,0,0);
		vertexs[1] = new Vector3(0,3,0);
		vertexs[2] = new Vector3(3,3,0);
		vertexs[3] = new Vector3(3,0,0);
		mesh.vertices = vertexs;
		mesh.triangles = new int[]{0,1,2,2,3,0};
		Vector2[] uvs = new Vector2[4];
		uvs[0] = new Vector2(0,0);

		uvs[1] = new Vector2(0,0.99f);
		uvs[2] = new Vector2(0.99f,0.99f);
		uvs[3] = new Vector2(0.99f,0);
		mesh.uv = uvs;

		mesh.RecalculateNormals();
		GetComponent<MeshFilter>().mesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2[] uvs = mesh.uv;
		for(int i =0;i <uvs.Length;i++)
		{
			uvs[i] =uvs[i] + new Vector2(-0.005f,0);
		}
		mesh.uv = uvs;
	}
}
