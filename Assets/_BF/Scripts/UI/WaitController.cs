using UnityEngine;
using System.Collections;

public delegate void TimeOutFunc();

public class WaitController : SingletonMonoBehaviourNoCreate<WaitController>
{
    UICamera _curCamera = null;
    bool _bEnable = false;
    string _strMsg = "";
    Texture2D _loadBackPic = null;
    float _waitTime = 0;
    float _startTime = 0;
    bool _pause = false;

    TimeOutFunc _callFunc = null;

    void Start()
    { 
        _loadBackPic = CommonUtility.InitTexture2D(10, 10, Color.black);
    }

	// Update is called once per frame
	void Update () 
    {
	}

    void disable()
    {
        if (_curCamera != null)
        {
            _curCamera.useMouse = true;
            _curCamera.useTouch = true;
            _curCamera.useKeyboard = true;
            _curCamera.useController = true;
            _curCamera = null;
            _strMsg = "";
            _callFunc = null;
            _waitTime = 0;
            _startTime = 0;
        }
    }

    void enable()
    {
        disable();
        _curCamera = UICamera.current;
        if (_curCamera != null)
        {
            _curCamera.useMouse = false;
            _curCamera.useTouch = false;
            _curCamera.useKeyboard = false;
            _curCamera.useController = false;
            _startTime = Time.time;
        }
    }

    public void showWaitMsg(string strMsg, float waitTime = 0,TimeOutFunc func=null)
    {
        _bEnable = true;
        enable();
        _waitTime = waitTime;
        _strMsg = strMsg;
        _callFunc = func;
    }

    public void hide()
    {
        _bEnable = false;
        disable();
    }

    public void pause()
    {
        _pause = true;
    }

    public void resume()
    {
        _pause = false;
    }

    void OnGUI()
    {
        if (_waitTime > 0)
        {
            if (Time.time - _startTime > _waitTime)
            {
                if (_callFunc != null)
                    _callFunc();
                hide();
                return;
            }
        }
        if (_pause)
            return;
        if (_bEnable && _strMsg != null)
        {
            GUIStyle style = new GUIStyle(GUI.skin.box);
            style.fontStyle = FontStyle.Bold;
            style.fontSize = 40;
            if (_loadBackPic != null)
                GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _loadBackPic);
            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2, 300, 60), _strMsg, style);
        }
    }
}
