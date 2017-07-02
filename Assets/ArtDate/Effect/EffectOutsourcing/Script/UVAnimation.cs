using UnityEngine;
using System.Collections;

[AddComponentMenu("VStar/Effect/UVAnimation")]
public class UVAnimation : MonoBehaviour
{
	public Vector3[] uvs;
	private Material m_mat;
	private float m_elaspe = 0.0f;
	private int m_uvCount;
	private int m_nowCount;
	private float m_allTime;
	private float[] m_factors;

	// Use this for initialization
	void Start()
	{
		m_uvCount = uvs.Length;
		if (m_uvCount <= 1)
		{
			enabled = false;
			return;
		}

		m_nowCount = 1;
		m_allTime = uvs[m_uvCount - 1].x;
		if (m_allTime <= 0.0f)
		{
			enabled = false;
			return;
		}

		Renderer r = GetComponent<Renderer>();
		if (r)
		{
			m_mat = r.material;
		}

		if (!m_mat)
		{
			enabled = false;
		}

		m_factors = new float[m_uvCount - 1];
		for (int i = 1; i < m_uvCount; i++)
		{
			m_factors[i - 1] = 1.0f / (uvs[i].x - uvs[i - 1].x);
		}
	}

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;
		m_elaspe += deltaTime;

		if (m_elaspe >= uvs[m_nowCount].x)
		{
			++m_nowCount;
		}

		if (m_nowCount >= m_uvCount)
		{
			m_nowCount = 1;
			m_elaspe = Mathf.Repeat(m_elaspe, m_allTime);
			Vector3 v0 = uvs[0];
			m_mat.mainTextureOffset = new Vector2(v0.y, v0.z);
			return;
		}

		Vector3 vLast = uvs[m_nowCount - 1];
		Vector3 vNow = uvs[m_nowCount];

		m_mat.mainTextureOffset = Vector2.Lerp(new Vector2(vLast.y, vLast.z), new Vector2(vNow.y, vNow.z), (m_elaspe - vLast.x) * m_factors[m_nowCount - 1]);
	}

	void OnDestroy()
	{
		if (m_mat)
		{
			DestroyImmediate(m_mat);
		}
	}
}
