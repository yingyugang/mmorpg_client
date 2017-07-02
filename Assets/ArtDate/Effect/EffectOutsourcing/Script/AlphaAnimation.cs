using UnityEngine;
using System.Collections;

[AddComponentMenu("VStar/Effect/AlphaAnimation")]
public class AlphaAnimation : MonoBehaviour
{
	public bool loop = true;
	public string colorName;
	public Vector2[] alphas;
	private Material m_mat;
	private float m_elaspe = 0.0f;
	private int m_alphaCount;
	private int m_nowCount;
	private float m_allTime;
	private float[] m_factors;
	private Color m_color;

	// Use this for initialization
	void Start()
	{
		m_alphaCount = alphas.Length;
		if (m_alphaCount <= 1)
		{
			enabled = false;
			return;
		}

		m_nowCount = 1;
		m_allTime = alphas[m_alphaCount - 1].x;
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
		else
		{
			//colorName = string.Format("_{0}", colorName);
			if (m_mat.HasProperty(colorName))
			{
				m_color = m_mat.GetColor(colorName);
			}
			else
			{
				enabled = false;
			}
		}

		m_factors = new float[m_alphaCount - 1];
		for (int i = 1; i < m_alphaCount; i++)
		{
			m_factors[i - 1] = 1.0f / (alphas[i].x - alphas[i - 1].x);
		}
	}

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;
		m_elaspe += deltaTime;

		if (m_elaspe >= alphas[m_nowCount].x)
		{
			++m_nowCount;
		}

		if (m_nowCount >= m_alphaCount)
		{
			if (!loop)
			{
				Vector2 vFinal = alphas[m_nowCount - 1];
				m_color.a = vFinal.y;
				m_mat.SetColor(colorName, m_color);
				enabled = false;
				return;
			}
			m_nowCount = 1;
			m_elaspe = Mathf.Repeat(m_elaspe, m_allTime);
			Vector2 v0 = alphas[0];
			m_color.a = v0.y;
			m_mat.SetColor(colorName, m_color);
			return;
		}

		Vector2 vLast = alphas[m_nowCount - 1];
		Vector2 vNow = alphas[m_nowCount];

		m_color.a = Mathf.Lerp(vLast.y, vNow.y, (m_elaspe - vLast.x) * m_factors[m_nowCount - 1]);
		m_mat.SetColor(colorName, m_color);
	}

	void OnDestroy()
	{
		if (m_mat)
		{
			DestroyImmediate(m_mat);
		}
	}
}
