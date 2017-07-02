//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2013 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Very simple sprite animation. Attach to a sprite and specify a common prefix such as "idle" and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Skill Animation")]
public class UISkillAnimation : MonoBehaviour
{
    public int mFPS = 30;
	public string mPrefix = "";
	public bool mLoop = false;

	UISprite mSprite;
    float mPlayTime;
	float mDelta = 0f;
	int mIndex = 0;
	bool mActive = false;
    bool mDestroy = false;
    bool mReversePlay = false;
    public float mScale = 1.0f;

	List<string> mSpriteNames = new List<string>();

	/// <summary>
	/// Number of frames in the animation.
	/// </summary>

	public int frames { get { return mSpriteNames.Count; } }

	/// <summary>
	/// Animation framerate.
	/// </summary>

	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>

    public bool isPlaying { get { return mActive; } set { mActive = value; } }

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>
    /// 

    /// <summary>
    /// 设置缩放比例
    /// </summary>
    /// 
    public float scale { get { return mScale; } set { mScale = value; } }

    /// <summary>
    /// 播放完毕删除
    /// </summary>
    public bool isDestroy { get { return mDestroy; } set { mDestroy = value; } }

    /// <summary>
    /// 是否逆向播放
    /// </summary>
    public bool isReverse { get { return mReversePlay; } set { mReversePlay = value; } }

    /// <summary>
    /// 自定义帧播放
    /// </summary>
    public List<string> FrameList { get { return mSpriteNames; } set { mSpriteNames = value; } }

    /// <summary>
    /// 获取播放一次动画所需时间
    /// </summary>
    public float GetPlayTime()
    {
        RebuildSpriteList();
        float rate = 1f / mFPS;
        mPlayTime = rate * mSpriteNames.Count;
        return mPlayTime;
    }

    //是否初始化
    bool IsInit = false;

    void Start()
    {
        if (!IsInit)
            RebuildSpriteList();
        //ControlFight.Log(mSprite.transform.localScale);
        //mSprite.transform.localScale = mSprite.transform.localScale * mScale;
    }


	void Update ()
	{
		if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0f)
		{
			mDelta += Time.deltaTime;
			float rate = 1f / mFPS;

			if (rate < mDelta)
			{
				
				mDelta = (rate > 0f) ? mDelta - rate : 0f;
				if (++mIndex >= mSpriteNames.Count)
				{
                    if (mDestroy)
                    {
                        DestroyObject(gameObject);
                    }
					mIndex = 0;
					mActive = loop;
				}

				if (mActive)
				{
                    SetScale(mSprite,mSpriteNames[mIndex]);
				}
			}
		}
	}

	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>

	void RebuildSpriteList ()
	{
        if (mSprite == null) mSprite = GetComponent<UISprite>();
        if (FrameList.Count <= 0)
        {
            mSpriteNames.Clear();

            if (mSprite != null && mSprite.atlas != null)
            {
                List<UISpriteData> sprites = mSprite.atlas.spriteList;

                for (int i = 0, imax = sprites.Count; i < imax; ++i)
                {
                    UISpriteData sprite = sprites[i];

                    if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
                    {
                        mSpriteNames.Add(sprite.name);
                    }
                }
                
                if (mReversePlay)
                {
                    mSpriteNames.Sort((x, y) => y.CompareTo(x));
                }
                else
                {
                    mSpriteNames.Sort();
                }
            }
        }
        SetScale(mSprite, mSpriteNames[0]);
        mActive = true;
	}
	
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>
	
	public void Reset()
	{
		mActive = true;
		mIndex = 0;

		if (mSprite != null && mSpriteNames.Count > 0)
		{
            SetScale(mSprite, mSpriteNames[mIndex]);
		}
	}

    public void SetScale(UISprite Sprite,string name)
    {
        Sprite.spriteName = name;
        Sprite.MakePixelPerfect();
        if (mScale != 1.0f)
        {
            Sprite.transform.localScale = Sprite.transform.localScale * mScale;
        }
    }
}
