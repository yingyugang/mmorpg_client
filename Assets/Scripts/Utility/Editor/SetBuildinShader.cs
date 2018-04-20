using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;
using UnityEditor.Rendering;

public class SetBuildinShader {


	[MenuItem("Tools/included shader", false, 11)]
	public static void TestIncludedShader()
	{
		string[] myShaders = new string[4]{
			"Nature/Terrain/Diffuse","Nature/Tree Creator Leaves Fast","Hidden/Nature/Tree Creator Bark Optimized","Standard"
		};

		SerializedObject graphicsSettings = new SerializedObject (AssetDatabase.LoadAllAssetsAtPath ("ProjectSettings/GraphicsSettings.asset") [0]);
		SerializedProperty it = graphicsSettings.GetIterator ();
		SerializedProperty dataPoint;
		HashSet<Object> includedShaderSet = new HashSet<Object> ();
		while (it.NextVisible(true)) {
			if (it.name == "m_AlwaysIncludedShaders") {
				//it.ClearArray();

				for(int j=0;j<it.arraySize;j++){
					if(!includedShaderSet.Contains(it.GetArrayElementAtIndex(j).objectReferenceValue)){
						includedShaderSet.Add (it.GetArrayElementAtIndex(j).objectReferenceValue);
					}
				}
				for (int i = 0; i < myShaders.Length; i++) { 
					Shader shader = Shader.Find (myShaders[i]);
					if (!includedShaderSet.Contains (shader)) {
						includedShaderSet.Add (shader);
						int index = it.arraySize;
						it.InsertArrayElementAtIndex(index);
						dataPoint = it.GetArrayElementAtIndex (index);
						dataPoint.objectReferenceValue = shader;
						index++;
					}

				}
				graphicsSettings.ApplyModifiedProperties ();
			}
		}
	}


}
