using UnityEngine;
using System.Collections;
using System;
using UnityEditor;
using UnityEditorInternal;
using System.Reflection;

[CustomEditor(typeof(SortingLayerWithChildren))]
public class SortingLayerWithChildrenEditor : Editor {

	string[] sortingLayerNames;

	SortingLayerWithChildren mTarget;
	Renderer[] renderers;

	void OnEnable()		
	{
		sortingLayerNames = GetSortingLayerNames();
		mTarget = target as SortingLayerWithChildren;
		popupMenuIndex = mTarget.currentLayer;
		renderers = mTarget.GetComponentsInChildren<Renderer>(true);
	}

    public void SetSortingOrder(int order)
    {
        foreach(Renderer r in renderers)
        {
            r.sortingOrder = order;
            r.gameObject.GetOrAddComponent<Puppet2D_SortingLayer>();
            EditorUtility.SetDirty(r.gameObject);
        } 
    }

	public void SetSortingLayer(string sortingLayerName)       
	{   
		Debug.Log(sortingLayerName);
		foreach(Renderer r in renderers)
		{
			Debug.Log(r);
			Undo.RecordObject(r.gameObject, "Edit Sorting Layer Name");
			r.sortingLayerName = sortingLayerName;
			r.sortingOrder = 10;
//			if(r.gameObject.GetComponent<Puppet2D_SortingLayer>()!=null)
//				DestroyImmediate(r.gameObject.GetComponent<Puppet2D_SortingLayer>());
			r.gameObject.GetOrAddComponent<Puppet2D_SortingLayer>();
			EditorUtility.SetDirty(r.gameObject);
		} 
	}

	public int previousPopupMenuIndex;
	public int popupMenuIndex;
	public int orderInLayer;
    public int previousOrderInLayer;
	public override void OnInspectorGUI()
	{
		popupMenuIndex = EditorGUILayout.Popup("Sorting Layer", popupMenuIndex, sortingLayerNames);
		if(popupMenuIndex != previousPopupMenuIndex)
		{
			SetSortingLayer(sortingLayerNames[popupMenuIndex]);
			previousPopupMenuIndex = popupMenuIndex;
			mTarget.currentLayer = popupMenuIndex;
		}
        orderInLayer = EditorGUILayout.IntField("Sorting Order",orderInLayer);
        if(orderInLayer != previousOrderInLayer)
        {
            SetSortingOrder(orderInLayer);
            mTarget.currentOrder = orderInLayer;
            previousOrderInLayer = orderInLayer;
        }
	}

	public string[] GetSortingLayerNames()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
}
