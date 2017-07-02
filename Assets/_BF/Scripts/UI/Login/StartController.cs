using UnityEngine;
using System.Collections;

public class StartController : MonoBehaviour
{
    public GameObject logoObj;
    public GameObject loginObj;
    public float logoShowTime = 1f;

    float _showLogoStartTime;
    bool _bShowLogo = false;
    
	void Start () 
    {
        _bShowLogo = showLogo(true);
        showLogin(!_bShowLogo);
	}
	
	void Update () 
    {
        if (_bShowLogo)
        {
            if (Input.GetMouseButtonDown(0) ||
            Input.GetMouseButtonDown(1) ||
            Input.touchCount > 0 ||
            Time.time - _showLogoStartTime > logoShowTime)
            {
                showLogo(false);
                showLogin(true);
                _bShowLogo = false;
            }
        }
	}

    bool showLogo(bool bFlag)
    {
        if (this.logoObj == null)
            return false;
        this.logoObj.SetActive(bFlag);
        _showLogoStartTime = Time.time;
        return true;
    }

    bool showLogin(bool bFlag)
    {
        if (this.loginObj!=null)
            this.loginObj.SetActive(bFlag);
        return false;
    }
}
