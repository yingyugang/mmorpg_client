using System.Collections;
using System.Collections.Generic;
using BlueNoah.Event;
using UnityEngine;
using UnityEngine.Events;

namespace BlueNoah.Event
{
    public class StandardInputService : BaseInputService
    {
        protected override void CheckTouchDown(Dictionary<TouchType, List<UnityAction<EventData>>> globalActionDic, Dictionary<int, Dictionary<TouchType, List<UnityAction<EventData>>>> actionDic, EventData eventData)
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (eventData.StartTouch(CreateTouchByMouseEvent(0)))
                {
                    OnTouchDown(globalActionDic, actionDic, eventData);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                if (eventData.StartTouch(CreateTouchByMouseEvent(1)))
                {
                    OnTouchDown(globalActionDic, actionDic, eventData);
                }
            }
        }

        protected override void CheckTouch(Dictionary<TouchType, List<UnityAction<EventData>>> globalActionDic, Dictionary<int, Dictionary<TouchType, List<UnityAction<EventData>>>> actionDic, EventData eventData)
        {
            if (Input.GetMouseButton(0))
            {
                Touch touch = CreateTouchByMouseEvent(0);
                touch.deltaPosition = (Vector2)Input.mousePosition - eventData.currentTouch.touch.position;
                if (eventData.UpdateTouch(touch))
                {
                    OnTouch(globalActionDic, actionDic, eventData);
                }
            }
            if (Input.GetMouseButton(1))
            {
                Touch touch = CreateTouchByMouseEvent(1);
                touch.deltaPosition = (Vector2)Input.mousePosition - eventData.currentTouch.touch.position;
                if (eventData.UpdateTouch(touch))
                {
                    OnTouch(globalActionDic, actionDic, eventData);
                }
            }
        }
        protected override void CheckTouchUp(Dictionary<TouchType, List<UnityAction<EventData>>> globalActionDic, Dictionary<int, Dictionary<TouchType, List<UnityAction<EventData>>>> actionDic, EventData eventData)
        {
            if (Input.GetMouseButtonUp(0))
            {
                if (eventData.EndTouch(CreateTouchByMouseEvent(0)))
                {
                    OnTouchUp(globalActionDic, actionDic, eventData);
                }
            }
            if (Input.GetMouseButtonUp(1))
            {
                if (eventData.EndTouch(CreateTouchByMouseEvent(1)))
                {
                    OnTouchUp(globalActionDic, actionDic, eventData);
                }
            }
        }
        Touch CreateTouchByMouseEvent(int mouseButtonIndex)
        {
            Touch touch = new Touch();
            touch.fingerId = mouseButtonIndex;
            touch.deltaTime = Time.deltaTime;
            touch.position = Input.mousePosition;
            return touch;
        }
    }
}
