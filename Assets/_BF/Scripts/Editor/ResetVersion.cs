// Builds an asset bundle from the selected objects in the project view,
// and changes the texture format using an AssetPostprocessor.

using UnityEngine;
using UnityEditor;

public class ResetVersion {
	
	// Store current texture format for the TextureProcessor.
	public static TextureImporterFormat textureFormat;
	
	[MenuItem("Assets/Reset Version")]
	static void ExportResourceRGB2 () {
		textureFormat = TextureImporterFormat.PVRTC_RGB2;
		Reset();       
	}   

	static void Reset()
	{
		PlayerPrefs.SetInt(AssetBundleMgr.bfVersionSubName,0);
	}

}
