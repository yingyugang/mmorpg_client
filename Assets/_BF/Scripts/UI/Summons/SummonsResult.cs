using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;
using UI;

public class SummonsResult : MonoBehaviour
{
    GameObject skipBtn;

    // Use this for initialization
	void Start () {

        skipBtn = PanelTools.findChild(gameObject, "Title/skipButton");

        if (skipBtn != null)
        {
            UIEventListener.Get(skipBtn).onClick = onReturn;
        }
        
	}

    void onReturn(GameObject go)
    {
        gameObject.SetActive(false);
    }
}
