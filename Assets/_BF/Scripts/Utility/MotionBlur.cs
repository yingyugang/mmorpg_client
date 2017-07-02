using UnityEngine;
using System.Collections;

public class MotionBlur : MonoBehaviour {

	public Camera motionCamera;
	public Shader motionBlur;
	[Range(0,1)]
	public float accPercent = 0.5f;
	Material mMotionBlurMat;
	RenderTexture mAccRenderTexture;

	// Use this for initialization
	void Start () {
		motionBlur = Shader.Find("Custom/MotionBlur");
		mMotionBlurMat = new Material(motionBlur);
		mMotionBlurMat.SetFloat("accPercent",accPercent);
		motionCamera = GetComponent<Camera>();
		mPreviousMotionBlur = accPercent;
	}

	float mPreviousMotionBlur;
	void Update()
	{
		if(mPreviousMotionBlur != accPercent)
		{
			mPreviousMotionBlur = accPercent;
			mMotionBlurMat.SetFloat("accPercent",accPercent);
		}
	}

	float mDelta = 0;
	void OnRenderImage(RenderTexture src,RenderTexture dest)
	{
		if(mAccRenderTexture==null)
		{
			mAccRenderTexture = new RenderTexture(Screen.width,Screen.height,0);
			mMotionBlurMat.SetTexture("_AccTex",mAccRenderTexture);
			Graphics.Blit(src,mAccRenderTexture);
		}
		Graphics.Blit(src,mAccRenderTexture,mMotionBlurMat);//Blend the textures
		Graphics.Blit(mAccRenderTexture,dest);
	}
}