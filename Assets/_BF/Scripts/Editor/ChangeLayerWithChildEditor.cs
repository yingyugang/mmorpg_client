using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections;
using System;
using System.Reflection;

[CustomEditor(typeof(ChangeLayerWithChild))]
public class ChangeLayerWithChildEditor : Editor {

	ChangeLayerWithChild mTarget;
	public void OnEnable()
	{
		mTarget = (ChangeLayerWithChild)target;
		layerNames = GetSortingLayerNames();
	}

	string[] layerNames ;
	int mCurrentLayerIndex;

	// these methods taken from here: http://answers.unity3d.com/questions/585108/how-do-you-access-sorting-layers-via-scripting.html
	// Get the sorting layer names
	public static string[] GetSortingLayerNames()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayersProperty = internalEditorUtilityType.GetProperty("sortingLayerNames", BindingFlags.Static | BindingFlags.NonPublic);
		return (string[])sortingLayersProperty.GetValue(null, new object[0]);
	}
	
	// Get the unique sorting layer IDs
	public static int[] GetSortingLayerUniqueIDs()
	{
		Type internalEditorUtilityType = typeof(InternalEditorUtility);
		PropertyInfo sortingLayerUniqueIDsProperty = internalEditorUtilityType.GetProperty("sortingLayerUniqueIDs", BindingFlags.Static | BindingFlags.NonPublic);
		return (int[])sortingLayerUniqueIDsProperty.GetValue(null, new object[0]);
	}

	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		EditorGUILayout.BeginHorizontal();

		mCurrentLayerIndex = EditorGUILayout.Popup(mCurrentLayerIndex,layerNames);
		EditorGUILayout.EndHorizontal();
		EditorGUILayout.BeginHorizontal();
		if(GUILayout.Button("Change",GUILayout.Width(100)))
		{
			SpriteRenderer[] srs = mTarget.GetComponentsInChildren<SpriteRenderer>();
			SkinnedMeshRenderer[] smrs = mTarget.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach(SpriteRenderer sr in srs)
			{
				sr.sortingLayerName = "UnitLayer0";
			}
			foreach(SkinnedMeshRenderer smr in smrs)
			{
				smr.sortingLayerName = "UnitLayer0";
			}
		}
		EditorGUILayout.EndHorizontal();
		EditorUtility.SetDirty (mTarget);
		serializedObject.ApplyModifiedProperties ();
	}
}
