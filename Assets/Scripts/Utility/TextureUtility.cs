using UnityEngine;
using System.Collections;

public static class TextureUtility {

	public static Texture2D CreateTexture2D(int width,int height,Color color)
	{
		Texture2D texture = new Texture2D (width,height);
		for(int i = 0 ;i < width ;i++)
		{
			for(int j = 0;j < height ;j++)
			{
				texture.SetPixel(i,j,color);
			}
		}
		texture.Apply ();
		return texture;
	}


}
