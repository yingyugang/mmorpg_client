#define CONSOLESELF_OPEN

using UnityEngine;
using System.Collections.Generic;

public class ConsoleSelf :
#if CONSOLESELF_OPEN
 SingletonMonoBehaviour<ConsoleSelf>
#else
    Singleton<ConsoleSelf>
#endif
{
    public void addText(System.String text, params object[] args)
    {
#if CONSOLESELF_OPEN
        addText(System.String.Format(text, args));
#endif
    }

    public void addText(System.String text)
    {
#if CONSOLESELF_OPEN
        GUIText guitext = getGUIText();
        guitext.text = text;
        resize();
#endif
    }

    GUIText getGUIText()
    {
#if CONSOLESELF_OPEN
        GUIText guitext = null;
        if (guiTextList.Count >= maxLine)
        {
            guitext = guiTextList[0];
            for (int i = 1; i < guiTextList.Count; ++i)
                guiTextList[i - 1] = guiTextList[i];
            guiTextList[guiTextList.Count - 1] = guitext;
        }
        else
        {
            GameObject t = new GameObject("guitext-" + guiTextList.Count);
            guitext = t.AddComponent<GUIText>();
            guitext.color = Color.red;
            guitext.fontSize = 30;
            guiTextList.Add(guitext);

            t.transform.parent = gameObject.transform;
            t.transform.localPosition = new Vector3(0.1f, 0.8f, 0f);
        }

        return guitext;
#else
        return null;
#endif
    }

    void resize()
    {
        Vector2 position = new Vector2(0, 0);
        foreach (GUIText gui in guiTextList)
        {
            gui.pixelOffset = position;
            position.y -= 17;
        }
    }

    void OnDestroy()
    {
        if (Application.isPlaying)
        {
            foreach (GUIText text in guiTextList)
                Destroy(text.gameObject);
        }
        else
        {
            foreach (GUIText text in guiTextList)
                DestroyImmediate(text.gameObject);
        }

        guiTextList.Clear();
    }

    public int maxLine = 20;
    public List<GUIText> guiTextList = new List<GUIText>();
}