using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(HeroPrefabAndIdMap))]
public class HeroPrefabAndIdMapEditor : Editor {

	HeroPrefabAndIdMap mTarget;
	
	public void OnEnable()
	{
		mTarget = (HeroPrefabAndIdMap)target;
	}

	HeroPrefabAndIdMapping delHm;
	public override void OnInspectorGUI()
	{
		serializedObject.Update ();
		Undo.RecordObject(mTarget,"HeroPrefabAndIdMap");
		if(GUILayout.Button("Sort",GUILayout.Width(80)))
		{
			mTarget.heroPrefabs.Sort(
				delegate(HeroPrefabAndIdMapping p1, HeroPrefabAndIdMapping p2)
				{
					int compareDate = p1.heroPrefabId.CompareTo(p2.heroPrefabId);
					if (compareDate == 0)
					{
						return p1.heroPrefabName.CompareTo(p2.heroPrefabName);
					}
					return compareDate;
				}
			);
		}

		NGUIEditorTools.BeginContents();
		foreach(HeroPrefabAndIdMapping hm in mTarget.heroPrefabs)
		{
			EditorGUILayout.BeginHorizontal();
			hm.heroPrefabId = EditorGUILayout.IntField(hm.heroPrefabId,GUILayout.Width(80));
			hm.heroPrefabName = EditorGUILayout.TextField(hm.heroPrefabName,GUILayout.Width(80));
			if(GUILayout.Button("Del",GUILayout.Width(50)))
			{
				if(EditorUtility.DisplayDialog("Warning!","Are you sure to delete " + hm.heroPrefabId,"Yes","No"))
				{
					delHm = hm;
				}
			}
			EditorGUILayout.EndHorizontal();
		}
		if(delHm!=null)
		{
			mTarget.heroPrefabs.Remove(delHm);
			delHm = null;
		}
		NGUIEditorTools.EndContents();
		serializedObject.ApplyModifiedProperties ();
	}


}
