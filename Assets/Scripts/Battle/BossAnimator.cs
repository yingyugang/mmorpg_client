using UnityEngine;
using System.Collections;

public class BossAnimator : MonoBehaviour
{

	//private Animator mAnimator;

	// Use this for initialization
	void Start ()
	{

		//mAnimator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//每次攻击后先用射线找下角色是否在面前，不在的话随机选择一个方向转身然后移动。
		//在攻击范围内触发对应的攻击。是否可以在头尾分别作判断呢。比如在背后设置一个范围控制转身攻击。

	}
}
