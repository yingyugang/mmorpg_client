using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class SZUIRenderQueue : MonoBehaviour {
    public int renderQueue = 4000;
    public bool runOnlyOnce = false;

    void Start() 
    { 
        Update();
    } 
    void Update() 
    { 
        if (GetComponent<Renderer>() != null && GetComponent<Renderer>().sharedMaterial != null) 
        { 
            GetComponent<Renderer>().sharedMaterial.renderQueue = renderQueue; 
        }
        if (runOnlyOnce && Application.isPlaying) 
        {
            this.enabled = false; 
        }
    } 
}