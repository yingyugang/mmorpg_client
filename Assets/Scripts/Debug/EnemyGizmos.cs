using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;

public class EnemyGizmos : MonoBehaviour {

	public EnemyController enemyController;
	List<float> mRadios;
	List<Color> mColors;

	List<FsmFloat> mMaxCooldowns;
	List<FsmFloat> mCurrentCooldowns;

	// Use this for initialization
	void Awake () {
		enemyController = GetComponent<EnemyController>();
	}

	public void Add(float radius,Color c){
		if(mRadios==null)mRadios = new List<float> ();
		if(mColors==null)mColors = new List<Color> ();
		mRadios.Add (radius);
		mColors.Add (c);
	}

	public void AddCooldown(FsmFloat currentCooldown,FsmFloat cooldown)
	{
		if (mCurrentCooldowns == null)mCurrentCooldowns = new List<FsmFloat> ();
		if (mMaxCooldowns == null)mMaxCooldowns = new List<FsmFloat> ();
		mCurrentCooldowns.Add (currentCooldown);
		mMaxCooldowns.Add (cooldown);
	}

	void OnDrawGizmos()
	{
		if (mRadios != null)
		{
			for(int i=0;i < mRadios.Count;i++)
			{
				Gizmos.color = mColors[i];
				Gizmos.DrawWireSphere(new Vector3(transform.position.x,transform.position.y + 0.05f,transform.position.z),mRadios[i]);
			}	
		}
	}

	int mOffsetY;
	void OnGUI()
	{
		mOffsetY = 1;
		if(mCurrentCooldowns != null)
		{
			GUI.color = Color.blue;
			for(int i=0;i < mCurrentCooldowns.Count;i++)
			{
				GUI.Label(new Rect(10,20 + mOffsetY * 50,100,50),mCurrentCooldowns[i].Name + ":");
				GUI.Label(new Rect(10 + 100,20 + mOffsetY * 50,100,50),Mathf.Min(mCurrentCooldowns[i].Value,mMaxCooldowns[i].Value).ToString() + "/");
				GUI.Label(new Rect(10 + 200,20 + mOffsetY * 50,100,50),mMaxCooldowns[i].Value.ToString());
				mOffsetY ++;
			}
			GUI.color = Color.white;
		}
	}
}
