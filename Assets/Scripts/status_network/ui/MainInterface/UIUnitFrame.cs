using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuloGames.UI;
using UnityEngine.UI;

namespace MMO
{
	public class UIUnitFrame : MonoBehaviour
	{

		public UIProgressBar mainBar;

		public UIProgressBar secondryBar;

		public Text txtMainBar;

		public Text txtSecondaryBar;

		PlayerInfo mPlayerInfo;

		void Awake(){
			if (mainBar == null) {
				mainBar = transform.Find ("Main Bar").GetComponent<UIProgressBar>();
				txtMainBar = mainBar.transform.Find ("Text").GetComponent<Text> ();
			}
			if (secondryBar == null) {
				secondryBar= transform.Find ("Secondary Bar").GetComponent<UIProgressBar>();
				txtSecondaryBar = secondryBar.transform.Find ("Text").GetComponent<Text> ();
			}
		}

		public void SetPlayerInfo(PlayerInfo playerInfo){
			this.mPlayerInfo = playerInfo;
		}

		void Update(){
			if(mPlayerInfo!=null){
				if(mainBar!=null){
					if (mPlayerInfo.unitInfo.attribute.maxHP == 0)
						mainBar.fillAmount = 1;
					else
						mainBar.fillAmount = mPlayerInfo.unitInfo.attribute.currentHP / (float)mPlayerInfo.unitInfo.attribute.maxHP;
					txtMainBar.text = string.Format ("{0:P0}",mainBar.fillAmount);
				}
				if(secondryBar!=null){
//					secondryBar.fillAmount = mPlayerInfo.unitInfo.attribute.currentHP / (float)mPlayerInfo.unitInfo.attribute.
				}
			}
		}

	}
}
