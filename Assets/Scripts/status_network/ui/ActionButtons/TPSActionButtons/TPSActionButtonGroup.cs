using System.Collections;
using System.Collections.Generic;
using TPS.PlayerControl;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TPS.UI
{
    public class TPSActionButtonGroup : MonoBehaviour
    {
        EventTrigger mTriggerFire;
        Button mBtnReload;
        Button mBtnSquat;
        Button mBtnJump;
        Button mBtnLying;
        TPSPlayerController mTPSPlayerController;
        void Awake()
        {

            mBtnReload = transform.Find("btn_normal/container_skill1/btn_sub").GetComponent<Button>();
            mBtnSquat = transform.Find("btn_normal/container_skill2/btn_sub").GetComponent<Button>();
            mBtnJump = transform.Find("btn_normal/container_skill3/btn_sub").GetComponent<Button>();
            mBtnLying = transform.Find("btn_normal/container_skill4/btn_sub").GetComponent<Button>();
            mTriggerFire = transform.Find("btn_normal").GetComponent<EventTrigger>();

        }
        IEnumerator Start()
        {
            yield return new WaitForSeconds(1);
            mTPSPlayerController = FindObjectOfType<TPSPlayerController>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((BaseEventData eventData) =>
            {
                StartCoroutine("_Shoot");
            });
            mTriggerFire.triggers.Add(entry);

            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((BaseEventData eventData) =>
            {
                StopCoroutine("_Shoot");
                mTPSPlayerController.UnShoot(null);
            });
            mTriggerFire.triggers.Add(entry);

            mBtnReload.onClick.AddListener(mTPSPlayerController.Reload);
            mBtnSquat.onClick.AddListener(mTPSPlayerController.Squat);
            mBtnJump.onClick.AddListener(mTPSPlayerController.Jump);
            mBtnLying.onClick.AddListener(mTPSPlayerController.Lying);
        }



        IEnumerator _Shoot()
        {
            while (true)
            {
                mTPSPlayerController.Shoot(null);
                yield return null;
            }
        }
    }
}

