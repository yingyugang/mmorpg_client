using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

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

		public Canvas joystickCanvas;
		public ETCJoystick etcJoystick;


		protected override void Awake ()
		{
			base.Awake ();
			mCharacterController = GetComponent<CharacterController> ();
			_animator = GetComponent<SimpleRpgAnimator> ();
			mTrans = transform;
			etcJoystick = MMO.PlatformController.Instance.etcJoystick.GetComponentInChildren<ETCJoystick> (true);
		}

		void Update ()
		{
			mInputX = Input.GetAxis ("Horizontal");
			mInputY = Input.GetAxis ("Vertical");

			if (etcJoystick != null && etcJoystick.gameObject.activeInHierarchy) {
				mInputX = etcJoystick.axisX.axisValue;
				mInputY = etcJoystick.axisY.axisValue;
			}

			Vector3 forward = rpgCameraController.transform.forward;
			forward = new Vector3 (forward.x, 0, forward.z).normalized;

			Vector3 plus = new Vector3 (mInputX, 0, mInputY).normalized;
			float angle = Vector3.Dot (plus, new Vector3 (1, 0, 0));
			int d = 1;
			if (mInputY < 0) {
				d = -1;
			}
			float angleY = Mathf.Acos (angle) * 180 / Mathf.PI * d - 90;
			mTrans.forward = Quaternion.AngleAxis (-angleY, new Vector3 (0, 1, 0)) * forward;
			if (mInputX != 0 || mInputY != 0) {
				_animator.SetMoveSpeed (3f);
			} else {
				_animator.SetMoveSpeed (0);
			}
			RaycastHit hit;
			if (Physics.Raycast (mTrans.position, -Vector3.up, out hit, Mathf.Infinity)) {
				mTrans.position = new Vector3 (mTrans.position.x, hit.point.y, mTrans.position.z);
			}
			if (_animator.IsRun ()) {
				mCharacterController.Move (mTrans.forward * Time.deltaTime * speed);
			}
		}

		void UpdateETCJoystickPos(){
			if(etcJoystick.activated){
				Vector2 touchPos = RectTransformUtility.PixelAdjustPoint(Input.GetTouch(etcJoystick.pointId).position,this.joystickCanvas.transform,this.joystickCanvas);
//				etcJoystick.pointId

			}
		}

	}
}