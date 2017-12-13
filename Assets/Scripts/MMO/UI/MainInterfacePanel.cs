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
			txt_level.text = MMOController.Instance.playerData.level.ToString ();
			txt_name.text = MMOController.Instance.playerData.playerName;
			txt_health.text = string.Format ("{0} / {1}", MMOController.Instance.playerData.currentHP, MMOController.Instance.playerData.maxHP);
			slider_health.value = MMOController.Instance.playerData.currentHP / (float)MMOController.Instance.playerData.maxHP;
			img_exp.fillAmount = MMOController.Instance.playerData.currentExp / (float)MMOController.Instance.playerData.maxExp;
//			img_head.sprite = ????/TODO head sprite package.
		}

	}
}