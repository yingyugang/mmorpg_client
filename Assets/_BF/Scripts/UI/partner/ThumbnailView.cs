using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThumbnailView : MonoBehaviour
{
    static GameObject cameraRoot;
    public GameObject effect;
    public GameObject showObject;
    public UITexture uiTexture;
    public Shader ghostShader;
    public Camera _camera;
    public RenderTexture renderTexture;
    public Material SpritesDefault;


    private GameObject heroModel = null;
    bool isChange = false;
    // Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

//         if (_camera != null && showObject != null)
//         {
//             _camera.RenderWithShader(ghostShader, "");
//         }

        if (heroModel != null && !isChange)
        {
            ChangShare(heroModel);
        }
	}

    private void CreateCamera()
    {
        _camera = this.gameObject.AddComponent<Camera>();
        _camera.orthographicSize = 6;
        _camera.orthographic = true;
        _camera.depth = 4;
        _camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
        _camera.clearFlags = CameraClearFlags.SolidColor;
        _camera.cullingMask = 13;
        _camera.targetTexture = renderTexture;
        _camera.enabled = false;
    }

    public void RT(RenderTexture  rt)
    {
        Graphics.Blit(rt, renderTexture);
        _camera.targetTexture = rt;
    }

    public void Init()
    {
        uiTexture = showObject.GetComponent<UITexture>();
        renderTexture = new RenderTexture(2048, 2048, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.sRGB);
        renderTexture.isPowerOfTwo = false;
        uiTexture.mainTexture = renderTexture;
        uiTexture.pivot = UIWidget.Pivot.Center;
        uiTexture.width = uiTexture.height;
        heroModel = NGUITools.AddChild(gameObject, effect);
        heroModel.transform.localPosition = new UnityEngine.Vector3(0, -1.8f, 5);
        heroModel.transform.localScale = new UnityEngine.Vector3(0.6f, 0.6f, 1);

        if (_camera == null)
        {
            CreateCamera();
        }
        else
        {
            _camera.orthographic = true;
            _camera.depth = 4;
            _camera.backgroundColor = new Color(0f, 0f, 0f, 0f);
            _camera.targetTexture = renderTexture;
            _camera.clearFlags = CameraClearFlags.SolidColor;

        }

    }

    public void ChangShare(GameObject go)
    {
        //SkinnedMeshRenderer[] smrs = go.GetComponentsInChildren<SkinnedMeshRenderer>();

//         for (int i = 0; i < smrs.Length; ++i)
//         {
//             smrs[i].sharedMaterial = SpritesDefault;
//             isChange = true;
//         }
        SpriteRenderer[] srs = go.GetComponentsInChildren<SpriteRenderer>();
        for (int i = 0; i < srs.Length; ++i)
        {
            srs[i].material = SpritesDefault;
            isChange = true;
        }
    }
}
