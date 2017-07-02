using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SpriteToUni2DMesh : MonoBehaviour {

// 	SpriteRenderer[] srs;
// 	public bool IsPlaying;
// 
// 	void Update () {
// 		if(IsPlaying)
// 		{
// 			srs = FindObjectsOfType<SpriteRenderer>();
// 			foreach(SpriteRenderer sr in srs)
// 			{
// 				Vector3 v0 = sr.transform.position;
// 				Texture2D a_rTexture = sr.sprite.texture;
// 				GameObject go = Uni2DEditorSpriteBuilderUtils.GenerateSpriteFromSettings( new Uni2DEditorSpriteSettings( a_rTexture, false ) );
// 				go.transform.position = v0;
// 			}
// 			IsPlaying = false;
// 		}
// 	}
    public bool IsPlaying;

    void Update ()
    {
#if UNITY_EDITOR
        if (IsPlaying)
        {
            GameObject root = new GameObject("Sprite_" + gameObject.name);
            SpriteRenderer[] srs = gameObject.GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in srs)
            {
                if (sr.gameObject.GetComponentsInChildren<PolygonCollider2D>().Length > 0)
                    continue;
                Vector3 v0 = sr.transform.position;
                Texture2D a_rTexture = sr.sprite.texture;
                GameObject go = Uni2DEditorSpriteBuilderUtils.GenerateSpriteFromSettings(new Uni2DEditorSpriteSettings(a_rTexture, false));
                go.transform.position = v0;
                go.transform.parent = root.transform;
            }
            IsPlaying = false;
        }
#endif
    }
}
