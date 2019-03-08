using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;
using BlueNoah.Event;
using TPS.CameraControl;
using MMO;

namespace TPS.PlayerControl
{
    public class TPSPlayerController : BasePlayerController
    {
        public float slopeLimit = 55;
        public float gravity = 20;
        public float jumpPower = 8;
        Vector3 _velocity;
        bool _grounded;
        public TPSCameraController tpsCameraController;
        public CharacterEffectUtility characterEffectUtility;
        CharacterController mCharacterController;
        float mInputX;
        float mInputY;
        Vector3 mVelocity;
        Transform mTrans;
        public float speed = 7;
        public Canvas joystickCanvas;
        public ETCJoystick etcJoystick;
        UnitAnimator mUnitAnimator;
        MMOUnit mMMOUnit;
        MMOUnitSkill mMMOUnitSkill;
        AudioSource mPlayerAudioSource;

        void Awake()
        {
            mCharacterController = GetComponent<CharacterController>();
            mUnitAnimator = GetComponent<UnitAnimator>();
            mMMOUnit = GetComponent<MMOUnit>();
            characterEffectUtility = GetComponentInChildren<CharacterEffectUtility>(true);
            mPlayerAudioSource = gameObject.GetOrAddComponent<AudioSource>();
            mPlayerAudioSource.spatialBlend = 1;
            mTrans = transform;
            etcJoystick = MMO.PlatformController.Instance.etcJoystick.GetComponentInChildren<ETCJoystick>(true);
            etcJoystick.onMoveStart.AddListener(() =>
            {
                isPause = false;
            });
            Cursor.lockState = CursorLockMode.Locked;
            MMOController.Instance.weaponId = 1;
#if (UNITY_IOS || UNITY_ANDROID) && !UNITY_EDITOR
//之所以用全局事件，是因为任意finger的触点都有可能成为移动的触点
            EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.TouchBegin, OnTouchBegin);
            // EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.TwoFingerBegin, OnTouchBegin);
            EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.Touch, OnTouch);
            //TODO TwoFinger没有实现。
            // EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.TwoFinger, OnTouch);
            EasyInput.Instance.AddGlobalListener(BlueNoah.Event.TouchType.TouchEnd, OnTouchEnd);
#endif

        }

        bool mMove;
        int mMoveFinger;
        // Vector2 mStartPos;

        void OnTouchBegin(EventData eventData)
        {
            if (!mMove && !eventData.currentTouch.isPointerOnGameObject)
            {
                if (eventData.currentTouch.startTouch.position.x < Screen.width / 2f)
                {
                    mMove = true;
                    mMoveFinger = eventData.currentTouch.startTouch.fingerId;
                    // mStartPos = eventData.currentTouch.startTouch.position;
                }
            }
        }

        void OnTouch(EventData eventData)
        {
            if (mMove && eventData.currentTouch.touch.fingerId == mMoveFinger)
            {
                isPause = false;
                Vector3 distance = eventData.currentTouch.touch.position - eventData.currentTouch.startTouch.position;
                mInputX = Mathf.Clamp(distance.x / Screen.dpi * 3, -1, 1);
                if (Mathf.Abs(mInputX) < 0.2f)
                {
                    mInputX = 0;
                }
                mInputY = Mathf.Clamp(distance.y / Screen.dpi * 3, -1, 1);
                if (Mathf.Abs(mInputY) < 0.2f)
                {
                    mInputY = 0;
                }
                mUnitAnimator.ResetAllAttackTriggers();
            }
        }

        void OnTouchEnd(EventData eventData)
        {
            Debug.Log("OnTouchEnd");
            if (mMove && eventData.currentTouch.touch.fingerId == mMoveFinger)
            {
                mMove = false;
                mInputX = 0;
                mInputY = 0;
                mMoveFinger = -1;
            }
        }

        public bool isPause;
        float mNextShoot;
        bool mIsFire;

        void Update()
        {
            if (!MMOController.Instance.isStart)
            {
                return;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.Confined;
            }
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (Cursor.lockState != CursorLockMode.Locked)
                {
                    Cursor.lockState = CursorLockMode.Locked;
                    return;
                }
            }
#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
            //TODO mobileを対応することが必要だと思う。
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
            {
                if (MMOController.Instance.weaponId == 1)
                {
                    mIsFire = true;
                    mUnitAnimator.StartFire();
                    MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.FIRE, -1, new IntVector3());
                }
                else if (MMOController.Instance.weaponId == 2)
                {
                    ShootInfo shootInfo = new ShootInfo();
                    shootInfo.casterId = MMOController.Instance.playerInfo.unitInfo.attribute.unitId;
                    shootInfo.targetId = Random.Range(1, 10);
                    shootInfo.unitSkillId = 18;
                    ActionManager.Instance.DoShoot(shootInfo);
                    Throw();
                }
            }
#endif
            if (Input.GetMouseButton(0) && mIsFire)
            {
                Shoot(null);
            }
            if (Input.GetMouseButtonUp(0))
            {
                UnShoot(null);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                MMOController.Instance.weaponId = 1;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                MMOController.Instance.weaponId = 2;
            }

            if (Input.GetKeyDown(KeyCode.V))
            {

            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftCommand))
            {
                Squat();
            }

            if (Input.GetKeyDown(KeyCode.Z))
            {
                Lying();
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Reload();
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                isPause = false;
            }
            if (isPause)
            {
                mUnitAnimator.SetMoveSpeed(0);
                return;
            }

#if UNITY_EDITOR || (!UNITY_IOS && !UNITY_ANDROID)
            mInputX = Input.GetAxis("Horizontal");
            mInputY = Input.GetAxis("Vertical");

            if (etcJoystick != null && etcJoystick.gameObject.activeInHierarchy)
            {
                mInputX = etcJoystick.axisX.axisValue;
                mInputY = etcJoystick.axisY.axisValue;
            }
#endif
            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.W))
            {
                mUnitAnimator.ResetAllAttackTriggers();
            }

            Vector3 forward = tpsCameraController.transform.forward;
            forward = new Vector3(forward.x, 0, forward.z).normalized;

            Vector3 plus = new Vector3(mInputX, 0, mInputY).normalized;
            float angle = Vector3.Dot(plus, new Vector3(1, 0, 0));
            int d = 1;
            if (mInputY < 0)
            {
                d = -1;
            }
            float angleY = Mathf.Acos(angle) * 180 / Mathf.PI * d - 90;
            Vector3 moveDirection = Quaternion.AngleAxis(-angleY, new Vector3(0, 1, 0)) * forward;
            if (mInputX != 0 || mInputY != 0)
            {
                mUnitAnimator.SetMoveSpeed(mInputY);
                mUnitAnimator.SetRight(mInputX);
                //				if (!mIsRuning) {
                //					mIsRuning = true;
                //					MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.MOVE, -1, IntVector3.zero);
                //				}
            }
            else
            {
                mUnitAnimator.SetMoveSpeed(0);
                mUnitAnimator.SetRight(0);
                //				if (mIsRuning) {
                //					mIsRuning = false;
                //					MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.STANDBY, -1, IntVector3.zero);
                //				}
            }
            MMOController.Instance.SendPlayerControll(mInputY, mInputX);
            RaycastHit hit;
            if (_grounded && Physics.Raycast(mTrans.position + new Vector3(0, 2, 0), -Vector3.up, out hit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND | 1 << LayerConstant.LAYER_DEFAULT))
            {
                mTrans.position = new Vector3(mTrans.position.x, hit.point.y, mTrans.position.z);
            }
            if (mUnitAnimator.IsIdle() || mUnitAnimator.IsFire() || mUnitAnimator.IsJump() || !_grounded)
            {
                if (mInputY < 0)
                {
                    mInputY = mInputY / 4f;
                }
                Vector3 direct = moveDirection * Time.deltaTime * speed * new Vector3(mInputX * 0.5f, 0, mInputY).magnitude;
                if (_grounded)
                {
                    mCharacterController.Move(direct);
                }
                else
                {
                    mCharacterController.Move(direct + new Vector3(0, _velocity.y * Time.deltaTime, 0));
                }
                //TODO 暂时只能同步move和idle，attack无法同步。
            }
            _velocity.y -= gravity * Time.deltaTime;
        }

        void OnControllerColliderHit(ControllerColliderHit col)
        {
            // This keeps the player from sticking to walls
            float angle = col.normal.y * 90;
            if (angle < slopeLimit)
            {
                _velocity = Vector3.zero;
            }
            else
            {
                _grounded = true;
                _velocity.y = 0;
            }
        }

        void LateUpdate()
        {
            Vector3 forward = tpsCameraController.transform.forward;
            forward = new Vector3(forward.x, 0, forward.z).normalized;
            mTrans.forward = forward;
        }

        //TODO 
        void UpdateETCJoystickPos()
        {
            if (etcJoystick.activated)
            {
                //				Vector2 touchPos = RectTransformUtility.PixelAdjustPoint (Input.GetTouch (etcJoystick.pointId).position, this.joystickCanvas.transform, this.joystickCanvas);
            }
        }

        bool CheckShootAble()
        {
            return true;
        }

        public void Shoot(BaseEventData eventData)
        {
            if (mNextShoot < Time.time)
            {
                if (CheckShootAble())
                {
                    if (mMMOUnitSkill == null)
                    {
                        mMMOUnitSkill = MMOController.Instance.player.GetComponent<MMOUnitSkill>();
                    }
                    if (PanelManager.Instance.mainInterfacePanel.bulletGroup.Shoot())
                    {
                        mMMOUnitSkill.skillList[0].position = IntVector3.ToIntVector3(tpsCameraController.transform.position);
                        mMMOUnitSkill.skillList[0].forward = IntVector3.ToIntVector3(tpsCameraController.transform.forward);
                        if (mMMOUnitSkill.skillList[0].Play())
                        {
                            if (characterEffectUtility)
                            {
                                characterEffectUtility.ShowSlash();
                            }
                            PanelManager.Instance.mainInterfacePanel.Shoot();
                            SoundManager.Instance.PlayShoot(this.mPlayerAudioSource);
                        }
                    }
                    else
                    {
                        SoundManager.Instance.PlayEmpty(this.mPlayerAudioSource);
                        if (mUnitAnimator.IsFireBool())
                        {
                            mUnitAnimator.StopFire();
                            MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.UNFIRE, -1, new IntVector3());
                        }
                    }
                }
                mNextShoot = Time.time + 0.1f;
            }
        }

        public void UnShoot(BaseEventData eventData)
        {
            mIsFire = false;
            mUnitAnimator.StopFire();
            MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.UNFIRE, -1, new IntVector3());
        }

        Coroutine mToggleOffsetCoroutine;

        float mToggleDuration = 1f;

        IEnumerator _ToggleOffset(Vector3 target)
        {
            float t = 0;
            Vector3 startOffset = tpsCameraController.targetOffset;
            while (t < 1)
            {
                t += Time.deltaTime / mToggleDuration;
                tpsCameraController.targetOffset = Vector3.Lerp(startOffset, target, t);
                yield return null;
            }
        }

        void ToggleOffset(Vector3 target)
        {
            if (mToggleOffsetCoroutine != null)
                StopCoroutine(mToggleOffsetCoroutine);
            mToggleOffsetCoroutine = StartCoroutine(_ToggleOffset(target));
        }

        void Throw()
        {
            MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.THROW, -1, new IntVector3());


            mUnitAnimator.SetTrigger(AnimationConstant.UNIT_ANIMATION_PARAMETER_THROW);
        }

        public void Squat()
        {
            if (mUnitAnimator.Squat())
            {
                MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.SQUAT, -1, new IntVector3());
                ToggleOffset(new Vector3(0, 1.8f, 0));
            }
            else
            {
                MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.UNSQUAT, -1, new IntVector3());
                ToggleOffset(new Vector3(0, 2.3f, 0));
            }
        }

        public void Lying()
        {
            if (mUnitAnimator.Lying())
            {
                MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.LYING, -1, new IntVector3());
                ToggleOffset(new Vector3(0, 1.2f, 0));
            }
            else
            {
                MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.UNLYING, -1, new IntVector3());
                ToggleOffset(new Vector3(0, 2.3f, 0));
            }
        }
        //TODO need to check wheather can be reloaded.
        public void Reload()
        {
            mUnitAnimator.Reload();
            PanelManager.Instance.mainInterfacePanel.bulletGroup.Clear();
            StartCoroutine(_Reload());
            MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.RELOAD, -1, new IntVector3());
            SoundManager.Instance.PlayReload(mPlayerAudioSource);
        }
        //TODO
        IEnumerator _Reload()
        {
            yield return new WaitForSeconds(2);
            PanelManager.Instance.mainInterfacePanel.bulletGroup.Reload();
        }

        void Melee()
        {
            MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.MELEE, -1, new IntVector3());
        }
        //Jump 正式一点跳跃应该是3个动作
        public void Jump()
        {
            if (_grounded)
            {
                mUnitAnimator.SetTrigger(AnimationConstant.UNIT_ANIMATION_PARAMETER_JUMP);
                _velocity.y = jumpPower;
                _grounded = false;
                MMOController.Instance.SendPlayerAction(BattleConst.UnitMachineStatus.JUMP, -1, new IntVector3());
            }
        }

        public float force = 10;

        IEnumerator _Jump()
        {
            mUnitAnimator.GetComponent<Rigidbody>().velocity = new Vector3(0, 100, 0);
            mUnitAnimator.SetTrigger(AnimationConstant.UNIT_ANIMATION_PARAMETER_JUMP);
            while (true)
            {
                yield return null;
            }
        }

    }
}