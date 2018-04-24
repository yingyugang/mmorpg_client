using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class UIUnitGroupGrid : MonoBehaviour
	{
		public GameObject uiUintFramePrefab;

		Dictionary<int,UIUnitFrame> mUIUnitFrames;

		void Awake(){
			mUIUnitFrames = new Dictionary<int, UIUnitFrame> ();
		}

		public void AddUnitFrame(PlayerInfo playerInfo){
		
		}

	}
}
