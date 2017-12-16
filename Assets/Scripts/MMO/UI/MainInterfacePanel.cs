using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class MainInterfacePanel : PanelBase
	{

		public Image img_head;
		public Image img_exp;
		public Slider slider_health;
		public Text txt_health;
		public Text txt_name;
		public Text txt_level;

		protected override void Awake ()
		{
			base.Awake ();
		}

		void Update ()
		{
			UpdatePlayerInfo ();
		}

		void UpdatePlayerInfo(){
			if (MMOController.Instance.playerInfo == null)
				return;
			//			txt_level.text = MMOController.Instance.playerInfo.level.ToString ();
			txt_level.text = MMOController.Instance.playerInfo.attribute.level.ToString();
			txt_name.text = MMOController.Instance.playerInfo.attribute.unitName;
			txt_health.text = string.Format ("{0} / {1}", MMOController.Instance.playerInfo.attribute.currentHP, MMOController.Instance.playerInfo.attribute.maxHP);
			if(MMOController.Instance.playerInfo.attribute.maxHP>0)
				slider_health.value = MMOController.Instance.playerInfo.attribute.currentHP / (float)MMOController.Instance.playerInfo.attribute.maxHP;
		}


	}
}