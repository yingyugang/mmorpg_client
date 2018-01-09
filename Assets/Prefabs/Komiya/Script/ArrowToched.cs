using UnityEngine;
using System.Collections;


	public class ArrowToched : MonoBehaviour
	{
		private bool efficient = true;

		public GameObject Effect1;
		public GameObject Effect2;
		public GameObject particle;

		public int style = 0;

		public void Awake(){
		if (Effect1 != null) {
			Effect1.SetActive (false);
		}
		if (Effect2 != null) {
			Effect2.SetActive (false);
		}
		if (particle != null) {
			particle.SetActive (false);
		}
	}

	void Update(){
		if (transform.position.y < -1.0) {
			Destroy (this.gameObject);
		}

	}

	void OnCollisionEnter(Collision other) {
		
	}

	public void startParticle(){
		if (particle != null) {
			particle.SetActive (true);
		}
	}

	private void dostay(){
		Rigidbody rg = GetComponentInChildren<Rigidbody> (true);
		rg.useGravity = false;
		rg.isKinematic = true;
		rg.velocity = new Vector3 (0, 0, 0);
		rg.angularVelocity = Vector3.zero;

	}

	void OnTriggerEnter(Collider other) {
		efficient = false;
		Destroy (this.gameObject, 0.1f);
	}
	
}


