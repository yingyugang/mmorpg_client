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
			txt_level.text = MMOController.Instance.playerInfo.unitInfo.attribute.level.ToString();
			txt_name.text = MMOController.Instance.playerInfo.unitInfo.attribute.unitName;
			txt_health.text = string.Format ("{0} / {1}", MMOController.Instance.playerInfo.unitInfo.attribute.currentHP, MMOController.Instance.playerInfo.unitInfo.attribute.maxHP);
			if(MMOController.Instance.playerInfo.unitInfo.attribute.maxHP>0)
				slider_health.value = MMOController.Instance.playerInfo.unitInfo.attribute.currentHP / (float)MMOController.Instance.playerInfo.unitInfo.attribute.maxHP;
		}


	}
}