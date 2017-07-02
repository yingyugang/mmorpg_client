using UnityEngine;
using System.Collections;
using BaseLib;
using DataCenter;

public class MainController : SingletonMonoBehaviourNoCreate<MainController>
{
    protected override void Init()
    {
        Screen.SetResolution(640, 960, false);
        this.gameObject.AddComponent<NetworkMgr>();
        this.gameObject.AddComponent<WaitController>();
    }

	void Start()
	{
        DataManager.CreateInstance();
	}

	bool mArean;
	void OnGUI()
	{
//		if(!mArean && GUI.Button(new Rect(10,10,100,30),"Arean"))
//		{
//			Application.LoadLevel("Arean");
//			mArean = true;
//		}
	}

    void OnDestroy()
    {
        if (MainController.me == this)
        {
            Logger.me.Destroy();
            ThreadMgr.release();
            NetworkMgr.release();
        }
        //EventSystem.sendEvent((int)EVENT_GLOBAL.sys_quit, null, (int)EVENT_GROUP.globalEvent);
    }
}
