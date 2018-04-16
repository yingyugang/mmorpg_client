using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace MMO
{
	public class RPGPlayerController : BasePlayerController
	{

		public float slopeLimit = 55;
		public RPGCameraController rpgCameraController;
		CharacterController mCharacterController;
		float mInputX;
		float mInputY;
		Vector3 mVelocity;
		Transform mTrans;
		public float speed = 7;
		private UnitAnimator _animator;

		public Canvas joystickCanvas;
		public ETCJoystick etcJoystick;


		void Awake ()
		{
			mCharacterController = GetComponent<CharacterController> ();
			_animator = GetComponent<UnitAnimator> ();
			mTrans = transform;
			etcJoystick = MMO.PlatformController.Instance.etcJoystick.GetComponentInChildren<ETCJoystick> (true);
			etcJoystick.onMoveStart.AddListener (()=>{
				isPause = false;
			});
		}

		bool mIsRuning;
		public bool isPause;
		void Update ()
		{
			if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)){
				isPause = false;
			}
			if (isPause) {
				_animator.SetMoveSpeed (0);
				return;
			}
			mInputX = Input.GetAxis ("Horizontal");
			mInputY = Input.GetAxis ("Vertical");

			if (etcJoystick != null && etcJoystick.gameObject.activeInHierarchy) {
				mInputX = etcJoystick.axisX.axisValue;
				mInputY = etcJoystick.axisY.axisValue;
			}

			if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W)){
				_animator.ResetAllAttackTriggers ();
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
				//TODO 暂时只能同步move和idle，attack无法同步。
				if (!mIsRuning) {
					mIsRuning = true;
					MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.MOVE, -1,IntVector3.zero);
				}
			} else {
				if (mIsRuning) {
					mIsRuning = false;
					MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.STANDBY, -1,IntVector3.zero);
				}
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