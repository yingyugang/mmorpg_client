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
            mTPSPlayerController = FindObjectOfType<TPSPlayerController>();
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerExit;
            entry.callback.AddListener(mTPSPlayerController.Shoot);
            entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener(mTPSPlayerController.UnShoot);
            mBtnReload.onClick.AddListener(mTPSPlayerController.Reload);
            mBtnSquat.onClick.AddListener(mTPSPlayerController.Squat);
            mBtnJump.onClick.AddListener(mTPSPlayerController.Jump);
            mBtnLying.onClick.AddListener(mTPSPlayerController.Lying);
        }
    }
}

