using UnityEngine;
using System.Collections;
using DataCenter;
using BaseLib;

public class LoadSceneMgr : SingletonMonoBehaviour<LoadSceneMgr>
{
    public static int SCENE_LOGIN = 0;
    public static int SCENE_MAIN = 1;
    public static int SCENE_BATTLE = 2;
	public static int SCENE_ARENA = 3;
    //string _curSceneName = "";
    AsyncOperation _async = null;
 
    // Use this for initialization
	void Start () 
    {
        EventSystem.register((int)EVENT_GLOBAL.sys_chgScene,onChange);
    }

    public void onChange(int nEvent,System.Object param)
    {
        int nSecene = (int)param;
        if (Application.loadedLevel == nSecene)
            return;
        EventSystem.releaseGroup((int)EVENT_GROUP.mainUI);
        WaitController.me.showWaitMsg("Loading...");
        StartCoroutine(loadSence(nSecene));
    }

    IEnumerator loadSence(int nSceneid)
    {
        _async = Application.LoadLevelAsync(nSceneid);
        _async.allowSceneActivation = false;
        while (_async!=null&&!_async.isDone && _async.progress < 0.9f)
        {
            yield return null;
        }
        loadFinish();
        yield return _async;
    }

    void loadFinish()
    {
        this._async.allowSceneActivation = true;
        this._async = null;
    }

    void Update()
    {
        if (!Application.isLoadingLevel)
        {
            WaitController.me.hide();
        }
    }
}
