using UnityEngine;
using System.Collections;

public class GUIBar : MonoBehaviour {

	public enum BarSide{Left,Right};

	public float healthRadiu;
	private Color mHealthColor = Color.green;
	public static Texture2D healthTex;

	public float powerRadiu;
	private Color mPowerColor = Color.yellow;
	public static Texture2D powerTex;

	private Color mCollectColor = Color.blue;
	public static Texture2D collectTex;

	public BarSide side = BarSide.Left;
	public static int defaultWidth = 960;
	public static int defaultHeight = 640;
	static int offsetTop = 10;
	static int healthBarHeight = 10;
	static int healthBarWidth = defaultWidth / 2 - 10;
	static int powerBarHeight = 5;
	static int powerBarWidth = defaultWidth / 2 - 10;

	private float mWidthRadiu;
	private float mHeightRadiu;

	// Use this for initialization
	void Start () {
		if(healthTex==null)
			healthTex = TextureUtility.CreateTexture2D (10,10,mHealthColor);
		if(powerTex==null)
			powerTex = TextureUtility.CreateTexture2D (10,10,mPowerColor);
		if (collectTex == null)
			collectTex = TextureUtility.CreateTexture2D (10,10,mCollectColor);
	}
	
	void OnGUI()
	{
		mWidthRadiu = (float)Screen.width / defaultWidth;
		mHeightRadiu = (float)Screen.height / defaultHeight;
		if (side == BarSide.Left)
		{
			GUI.DrawTexture (new Rect(10 * mWidthRadiu,offsetTop *mHeightRadiu,healthBarWidth * healthRadiu *mWidthRadiu, healthBarHeight * mHeightRadiu),healthTex);
			GUI.DrawTexture (new Rect(10 * mWidthRadiu,(offsetTop + healthBarHeight + 3) * mHeightRadiu,powerBarWidth * powerRadiu *mWidthRadiu, powerBarHeight * mHeightRadiu),powerTex);
		}
		else 
		{
			GUI.DrawTexture (new Rect((defaultWidth / 2 + 10 + (defaultWidth/2 - 20) * (1-healthRadiu)) * mWidthRadiu,offsetTop *mHeightRadiu ,(defaultWidth/2 - 20)*healthRadiu*mWidthRadiu,healthBarHeight * mHeightRadiu),healthTex);
			GUI.DrawTexture (new Rect((defaultWidth / 2 + 10 + (defaultWidth/2 - 20) * (1-powerRadiu)) * mWidthRadiu,(offsetTop + healthBarHeight + 3) *mHeightRadiu ,(defaultWidth/2 - 20)*powerRadiu*mWidthRadiu,powerBarHeight * mHeightRadiu),powerTex);
		}
	}

}
