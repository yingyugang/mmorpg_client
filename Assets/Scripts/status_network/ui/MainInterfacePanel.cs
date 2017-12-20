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
		const int SKILL_ICON_COUNT = 20;

		public List<Sprite> skillIconList;
		public GridLayoutGroup skillGrid;
		public GameObject skillItemPrefab;
		public List<Button> skillButtonList;

		protected override void Awake ()
		{
			base.Awake ();
			InitIconItems ();
		}

		void InitIconItems ()
		{
			for (int i = 0; i < SKILL_ICON_COUNT; i++) {
				GameObject item = Instantiate (skillItemPrefab);
				item.transform.SetParent (skillGrid.transform);
				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.zero;
				item.SetActive (true);
				skillButtonList.Add (item.GetComponent<Button>());
			}
		}

		void SetSkillDatas (MMOUnitSkill unitSkill)
		{
			ResetSkillIcons ();
			List<SkillBase> skills = unitSkill.skillList;
			for (int i = 0; i < skills.Count; i++) {
				SkillBase sb = skills [i];
				Button btnSkill = skillButtonList [i];
				Image imgIcon = btnSkill.transform.Find ("img_icon").GetComponent<Image>();
				imgIcon.sprite = skillIconList[sb.skillId % skillIconList.Count];
				imgIcon.gameObject.SetActive (true);
			}
		}

		void ResetSkillIcons ()
		{
			for (int i = 0; i < skillButtonList.Count; i++) {
				Button btn = skillButtonList [i];
				Image imgIcon = btn.transform.Find ("img_icon").GetComponent<Image>();
				imgIcon.gameObject.SetActive (false);
			}
		}

		void Update ()
		{
			UpdatePlayerInfo ();
		}

		void UpdatePlayerInfo ()
		{
			if (MMOController.Instance.playerInfo == null)
				return;
			//			txt_level.text = MMOController.Instance.playerInfo.level.ToString ();
			txt_level.text = MMOController.Instance.playerInfo.unitInfo.attribute.level.ToString ();
			txt_name.text = MMOController.Instance.playerInfo.unitInfo.attribute.unitName;
			txt_health.text = string.Format ("{0} / {1}", MMOController.Instance.playerInfo.unitInfo.attribute.currentHP, MMOController.Instance.playerInfo.unitInfo.attribute.maxHP);
			if (MMOController.Instance.playerInfo.unitInfo.attribute.maxHP > 0)
				slider_health.value = MMOController.Instance.playerInfo.unitInfo.attribute.currentHP / (float)MMOController.Instance.playerInfo.unitInfo.attribute.maxHP;
		}


	}
}