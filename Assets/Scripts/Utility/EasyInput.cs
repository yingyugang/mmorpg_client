using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace BlueNoah.Event
{
    public enum TouchType { TouchBegin, TouchEnd, Touch, Click, DoubleClick, LongPressBegin, TwoFingerBegin, TwoFinger, TwoFingerEnd };

    public class EventData
    {
        public Vector3 touchStartPos0;
        public Vector3 touchPos0;
        public Vector3 deltaTouchPos0;
        public float touchStartTime0;
        public float touchEndTime0;

        public bool isPointerOnGameObject;

        public Vector3 touchStartPos1;
        public Vector3 touchPos1;
        public Vector3 deltaTouchPos1;
        public float touchStartTime1;

        public float deltaAngle;
        public float pinchDistance;
        public float deltaPinchDistance;
        public bool isLongPressed;

        public float preClickTime;
    }

    //説明：GetTouchDownなどの名前と仕組みは、Input.GetMouseBottonDown(0)と近いてる。
    public class EasyInput : SimpleSingleMonoBehaviour<EasyInput>
    {
        //Click interval max(same as ugui system)
        //Double click interval 0.3f~0.5f
        Dictionary<TouchType, List<UnityAction<EventData>>> mTouchActionDic;

        Dictionary<TouchType, List<UnityAction<EventData>>> mLateTouchActionDic;

        EventData mEventData;

        EventData mLateEventData;

        const float MIN_LONGPRESS_DURATION = 1f;

        const float MIN_LONGPRESS_DISTANCE = 0.02f;

        protected override void Awake()
        {
            base.Awake();

            if (mTouchActionDic == null)
            {
                Init();
            }

        }

        void Init()
        {
            mEventData = new EventData();
            mLateEventData = new EventData();
            mTouchActionDic = new Dictionary<TouchType, List<UnityAction<EventData>>>();
            mTouchActionDic.Add(TouchType.TouchBegin, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.TouchEnd, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.Touch, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.Click, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.LongPressBegin, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.TwoFingerBegin, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.TwoFinger, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.TwoFingerEnd, new List<UnityAction<EventData>>());
            mTouchActionDic.Add(TouchType.DoubleClick, new List<UnityAction<EventData>>());

            mLateTouchActionDic = new Dictionary<TouchType, List<UnityAction<EventData>>>();
            mLateTouchActionDic.Add(TouchType.TouchBegin, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.TouchEnd, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.Touch, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.Click, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.LongPressBegin, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.TwoFingerBegin, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.TwoFinger, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.TwoFingerEnd, new List<UnityAction<EventData>>());
            mLateTouchActionDic.Add(TouchType.DoubleClick, new List<UnityAction<EventData>>());

            SceneManager.sceneUnloaded += OnUnloaded;
        }

        void OnUnloaded(Scene scene)
        {
            foreach (TouchType touchType in mTouchActionDic.Keys)
            {
                mTouchActionDic[touchType].Clear();
            }

            foreach (TouchType touchType in mLateTouchActionDic.Keys)
            {
                mLateTouchActionDic[touchType].Clear();
            }
        }

        public void RemoveAllListener(TouchType touchType)
        {
            if (mTouchActionDic.ContainsKey(touchType))
            {
                mTouchActionDic.Remove(touchType);
            }
        }

        public void AddListener(TouchType touchType, UnityAction<EventData> unityAction)
        {
            if (mTouchActionDic == null)
                Init();
            mTouchActionDic[touchType].Add(unityAction);
        }

        public void AddLateUpdateListener(TouchType touchType, UnityAction<EventData> unityAction)
        {
            if (mLateTouchActionDic == null)
                Init();
            mLateTouchActionDic[touchType].Add(unityAction);
        }

        bool GetTouchDown()
        {
            bool isDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButtonDown(0))
            {
                isDown = true;
            }
#else
            if (Input.touchCount == 1 && Input.touches[0].phase == TouchPhase.Began)
            {
                isDown = true;
            }
#endif
            return isDown;
        }

        bool GetTouch()
        {
            bool isPress = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButton(0))
            {
                isPress = true;
            }
#else
            if (Input.touchCount == 1 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary))
            {
                isPress = true;
            }
#endif
            return isPress;
        }

        bool GetLongPress(EventData eventData)
        {
            if (GetTouch() && !eventData.isLongPressed && Time.realtimeSinceStartup - eventData.touchStartTime0 > MIN_LONGPRESS_DURATION)
            {
                float distance = Vector3.Distance(eventData.touchStartPos0, GetTouchPosition(0));
                if (distance / Screen.height < 0.02f)
                {
                    return true;
                }
            }
            return false;
        }

        bool GetTouchUp()
        {
            bool isUp = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetMouseButtonUp(0))
            {
                isUp = true;
            }
#else
            if (Input.touchCount == 1 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled))
            {
                isUp = true;
            }
#endif
            return isUp;
        }

        public Vector3 GetTouchPosition(int index)
        {
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (index == 0)
            {
                return Input.mousePosition;
            }
            else
            {
                return Vector3.zero;
            }
#else
            return (Vector3)Input.touches[index].position;
#endif
        }

        bool GetMouseClick(EventData eventData)
        {
            //範囲の判断はもっと確認したい。
            bool isClick = false;
            float distance = Vector3.Distance(eventData.touchStartPos0, GetTouchPosition(0));
            if (distance / Screen.height < 0.02f)
            {
                isClick = true;
            }
            return isClick;
        }

        bool GetDoubleMouseClick(EventData eventData)
        {
            bool isClick = GetMouseClick(eventData);
            if (Time.realtimeSinceStartup - eventData.preClickTime > 0.3f)
            {
                isClick = false;
            }
            return isClick;
        }

        bool GetTwoFingerBegin()
        {
            bool isTwoFingerDown = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if ((Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.Q) && Input.GetMouseButtonDown(0))
               || (Input.GetKeyDown(KeyCode.Q) && Input.GetMouseButtonDown(0)))
            {
                isTwoFingerDown = true;

            }
#else
            if ((Input.touches.Length == 2 && Input.touches[1].phase == TouchPhase.Began && Input.touches[0].phase != TouchPhase.Ended && Input.touches[0].phase != TouchPhase.Canceled) 
                || (Input.touches.Length == 2 && Input.touches[0].phase == TouchPhase.Began && Input.touches[1].phase != TouchPhase.Ended && Input.touches[1].phase != TouchPhase.Canceled))
            {
                isTwoFingerDown =  true;
            }
#endif
            return isTwoFingerDown;
        }

        bool GetTwoFinger()
        {
            bool isTwoFinger = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if (Input.GetKey(KeyCode.Q) && Input.GetMouseButton(0))
            {
                isTwoFinger = true;
            }
#else
            if(Input.touches.Length == 2 && (Input.touches[0].phase == TouchPhase.Moved || Input.touches[0].phase == TouchPhase.Stationary || Input.touches[1].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Stationary)){
                isTwoFinger = true;
            }
#endif
            return isTwoFinger;
        }

        bool GetTwoFingerEnd()
        {
            bool isTwoFingerUp = false;
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
            if ((Input.GetKeyUp(KeyCode.Q) && Input.GetMouseButton(0)) || (Input.GetKey(KeyCode.Q) && Input.GetMouseButtonUp(0))
                || (Input.GetKeyUp(KeyCode.Q) && Input.GetMouseButtonUp(0)))
            {
                isTwoFingerUp = true;
            }
#else
            if ((Input.touches.Length == 2 && (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)) 
                || (Input.touches.Length == 2 && (Input.touches[1].phase == TouchPhase.Ended || Input.touches[1].phase == TouchPhase.Canceled)))
            {
                isTwoFingerUp =  true;
            }
#endif
            return isTwoFingerUp;
        }

        void OnActions(List<UnityAction<EventData>> unityActions, EventData eventData)
        {
            for (int i = 0; i < unityActions.Count; i++)
            {
                if (unityActions[i] != null)
                    unityActions[i](eventData);
            }
        }

        void Update()
        {
            OnUpdate(mTouchActionDic, mEventData);
        }

        void LateUpdate()
        {
            OnUpdate(mLateTouchActionDic, mLateEventData);
        }

        void OnUpdate(Dictionary<TouchType, List<UnityAction<EventData>>> actionDic, EventData eventData)
        {
            if (GetTouchDown())
            {
                OnTouchDown(eventData);
                OnActions(actionDic[TouchType.TouchBegin], eventData);
            }
            if (GetTouch())
            {
                OnTouch(eventData);
                OnActions(actionDic[TouchType.Touch], eventData);
            }
            if (GetTouchUp())
            {
                OnTouchUp(eventData);
                if (GetDoubleMouseClick(eventData))
                {
                    OnDoubleClick(eventData);
                    OnActions(actionDic[TouchType.DoubleClick], eventData);
                }
                if (GetMouseClick(eventData))
                {
                    OnClick(eventData);
                    OnActions(actionDic[TouchType.Click], eventData);
                }
                OnActions(actionDic[TouchType.TouchEnd], eventData);
            }
            if (GetLongPress(eventData))
            {
                OnLongPress(eventData);
                OnActions(actionDic[TouchType.LongPressBegin], eventData);
            }
            if (GetTwoFingerBegin())
            {
                OnTwoFingerBegin(eventData);
                OnActions(actionDic[TouchType.TwoFingerBegin], eventData);
            }
            if (GetTwoFinger())
            {
                OnTwoFinger(eventData);
                OnActions(actionDic[TouchType.TwoFinger], eventData);
            }
            if (GetTwoFingerEnd())
            {
                OnTwoFingerEnd();
                OnActions(actionDic[TouchType.TwoFingerEnd], eventData);
            }
        }

        void OnTouchDown(EventData eventData)
        {
            eventData.touchStartPos0 = GetTouchPosition(0);
            eventData.touchPos0 = eventData.touchStartPos0;
            eventData.touchStartTime0 = Time.realtimeSinceStartup;
            eventData.isLongPressed = false;
            eventData.isPointerOnGameObject = IsPointerOverUIObject();
        }

        bool IsPointerOverUIObject()
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            if (EventSystem.current != null)
            {
                EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
                return results.Count > 0;
            }
            else
            {
                return false;
            }
        }

        bool CheckOverGameObject()
        {
#if UNITY_EDITOR
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                return true;
            }
#else
            //Debug.Log(Input.touchCount + "||" + EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId));
            if (EventSystem.current != null && Input.touchCount > 0)
            {

                if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                    return true;
            }
#endif
            return false;
        }

        void OnTouch(EventData eventData)
        {
            eventData.deltaTouchPos0 = GetTouchPosition(0) - eventData.touchPos0;
            eventData.touchPos0 = GetTouchPosition(0);
        }

        void OnTouchUp(EventData eventData)
        {
            eventData.deltaTouchPos0 = GetTouchPosition(0) - eventData.touchPos0;
            eventData.touchPos0 = GetTouchPosition(0);
            eventData.touchEndTime0 = Time.realtimeSinceStartup;
            eventData.isLongPressed = false;
        }

        void OnTwoFingerBegin(EventData eventData)
        {
            eventData.touchPos0 = GetTouchPosition(0);
            eventData.touchPos1 = GetTouchPosition(1);
            eventData.touchStartPos1 = GetTouchPosition(1);
            eventData.pinchDistance = Vector3.Distance(eventData.touchPos0, eventData.touchPos1);
        }

        void OnTwoFinger(EventData eventData)
        {
            eventData.deltaTouchPos0 = GetTouchPosition(0) - eventData.touchPos0;
            eventData.deltaTouchPos1 = GetTouchPosition(1) - eventData.touchPos1;
            eventData.deltaAngle = Vector3.SignedAngle(GetTouchPosition(1) - GetTouchPosition(0), eventData.touchPos1 - eventData.touchPos0, new Vector3(0, 0, 1));
            eventData.touchPos0 = GetTouchPosition(0);
            eventData.touchPos1 = GetTouchPosition(1);
            float currentDistance = Vector3.Distance(eventData.touchPos0, eventData.touchPos1);
            eventData.deltaPinchDistance = currentDistance - eventData.pinchDistance;
            eventData.pinchDistance = currentDistance;
        }

        void OnTwoFingerEnd()
        {

        }

        void OnLongPress(EventData eventData)
        {
            eventData.isLongPressed = true;
        }

        void OnClick(EventData eventData)
        {
            eventData.preClickTime = Time.realtimeSinceStartup;
        }

        void OnDoubleClick(EventData eventData)
        {
            eventData.preClickTime = 0;
        }
    }
}
