using UnityEngine;
using System.Collections;

public enum PSRAxis
{
	AXIX_X,
	AXIX_Y,
	AXIX_Z,
}

[AddComponentMenu("VStar/Effect/ParticleSelfRotating")]
public class ParticleSelfRotating : MonoBehaviour
{
	public bool loop = true;
	public float startTime = 0.0f;
	public float endTime = 0.0f;
	public float angle = 0.0f;
	public PSRAxis axis = PSRAxis.AXIX_X;
	private Transform m_transform;
	private ParticleSystem m_particleSystem;
	private bool m_bRotated = false;
	private float m_anglePerSecond = 0.0f;
	private float m_elaspe = 0.0f;
	private Vector3 m_axis;

	void Awake()
	{
		m_transform = transform;
		m_particleSystem = GetComponent<ParticleSystem>();
		if (null == m_particleSystem || Mathf.Abs(angle) < 0.01f)
		{
			enabled = false;
		}
		else
		{
			float deltaT = endTime - startTime;
			if (deltaT > 0.0f)
			{
				m_anglePerSecond = angle / deltaT;
			}
			else
			{
				m_anglePerSecond = angle;
			}
		}
		if (PSRAxis.AXIX_X == axis)
		{
			m_axis = Vector3.right;
		}
		else if (PSRAxis.AXIX_Y == axis)
		{
			m_axis = Vector3.up;
		}
		else if (PSRAxis.AXIX_Z == axis)
		{
			m_axis = Vector3.forward;
		}
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		float deltaTime = Time.deltaTime;
		m_elaspe += deltaTime;
		if (!m_bRotated)
		{
			if (m_elaspe >= startTime)
			{
				m_bRotated = true;
				deltaTime = m_elaspe - startTime;

			}
			else
			{
				return;
			}
		}
		else
		{
			if (m_elaspe >= endTime)
			{
				enabled = false;
			}
		}

		m_transform.Rotate(m_axis, m_anglePerSecond * deltaTime);
	}
}
