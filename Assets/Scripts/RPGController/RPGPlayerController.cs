using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace MMO
{
	public class RPGPlayerController : SingleMonoBehaviour<RPGPlayerController>
	{

		public float slopeLimit = 55;
		public RPGCameraController rpgCameraController;
		CharacterController mCharacterController;
		float mInputX;
		float mInputY;
		Vector3 mVelocity;
		Transform mTrans;
		public float speed = 5;
		private SimpleRpgAnimator _animator;
//		public UnityAction onRun;
		ETCJoystick mETCJoystick;

		protected override void Awake ()
		{
			base.Awake ();
			mCharacterController = GetComponent<CharacterController> ();
			_animator = GetComponent<SimpleRpgAnimator> ();
			mTrans = transform;
			mETCJoystick = MMO.PlatformController.Instance.etcJoystick.GetComponentInChildren<ETCJoystick> (true);
		}

		void Update ()
		{
			mInputX = Input.GetAxis ("Horizontal");
			mInputY = Input.GetAxis ("Vertical");

			if (mETCJoystick != null && mETCJoystick.gameObject.activeInHierarchy) {
				mInputX = mETCJoystick.axisX.axisValue;
				mInputY = mETCJoystick.axisY.axisValue;
			}

			//以camera的forward为基准。
			Vector3 forward = rpgCameraController.transform.forward;
			forward = new Vector3 (forward.x, 0, forward.z).normalized;

			Vector3 plus = new Vector3 (mInputX, 0, mInputY).normalized;
			float angle = Vector3.Dot (plus, new Vector3 (1, 0, 0));
			int d = 1;
			if (mInputY < 0) {
				d = -1;
			}
//		Debug.Log (Mathf.Acos(angle) * 180 /Mathf.PI * d - 90);
			float angleY = Mathf.Acos (angle) * 180 / Mathf.PI * d - 90;
			mTrans.forward = Quaternion.AngleAxis (-angleY, new Vector3 (0, 1, 0)) * forward;
			if (mInputX != 0 || mInputY != 0) {
//			if(string.IsNullOrEmpty(_animator.Action) ||  _animator.Action=="idle")
//				if (onRun != null)
//					onRun ();
				_animator.SetMoveSpeed (3f);//_animator.SetSpeed(3f) .Action = "run";
			} else {
//				if (string.IsNullOrEmpty (_animator.Action) || _animator.Action == "run") {
//					_animator.Action = "idle";
					_animator.SetMoveSpeed(0);
//				}
			}
			RaycastHit hit;
			if (Physics.Raycast (mTrans.position, -Vector3.up, out hit, Mathf.Infinity)) {
				mTrans.position = new Vector3 (mTrans.position.x, hit.point.y, mTrans.position.z);
			}
			if (_animator.IsRun ()) {
				mCharacterController.Move (mTrans.forward * Time.deltaTime * speed);
			}
		}
	}
}