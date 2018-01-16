using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace izvr
{
	public class TagNameMaker : MonoBehaviour {

		public static void addTag(string tagname) {
			#if UNITY_EDITOR
			UnityEngine.Object[] asset = AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/TagManager.asset");
			if ((asset != null) && (asset.Length > 0)) {
				SerializedObject so = new SerializedObject(asset[0]);
				SerializedProperty tags = so.FindProperty("tags");

				for (int i = 0; i < tags.arraySize; ++i) {
					if (tags.GetArrayElementAtIndex(i).stringValue == tagname) {
						return;
					}
				}
				int index = tags.arraySize;
				tags.InsertArrayElementAtIndex(index);
				tags.GetArrayElementAtIndex(index).stringValue = tagname;
				so.ApplyModifiedProperties();
				so.Update();
			}
			#endif
		}
	}
}
