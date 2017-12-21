using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class MainInterfacePanel : PanelBase
	{

		public Image img_head;
		public Image img_health;
		public Image img_mana;
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

		void Start(){
			MMOUnitSkill unitSkill = MMOController.Instance.player.GetComponent<MMOUnitSkill>();
			SetSkillDatas (unitSkill);
		}

		void InitIconItems ()
		{
			for (int i = 0; i < SKILL_ICON_COUNT; i++) {
				GameObject item = Instantiate (skillItemPrefab);
				item.transform.SetParent (skillGrid.transform);
				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.zero;
				item.SetActive (true);
				Image imgIcon = item.GetComponent<Image>();
				imgIcon.sprite = skillIconList[i % skillIconList.Count];
				skillButtonList.Add (item.GetComponentInChildren<Button>(true));
			}
		}

		void SetSkillDatas (MMOUnitSkill unitSkill)
		{
			ResetSkillIcons ();
			List<SkillBase> skills = unitSkill.skillList;
			for (int i = 0; i < skills.Count; i++) {
				SkillBase sb = skills [i];
				Button btnSkill = skillButtonList [i];
				SkillBase skillBase = skills [i];
				btnSkill.onClick.AddListener (()=>{
					int skillId = skillBase.skillId;
					MMOController.Instance.SendUseSkill(skillId);
				});
				Image imgIcon = btnSkill.GetComponent<Image>();
				imgIcon.sprite = skillIconList[sb.skillId % skillIconList.Count];
				imgIcon.gameObject.SetActive (true);
			}
		}

		void ResetSkillIcons ()
		{
			for (int i = 0; i < skillButtonList.Count; i++) {
				Button btn = skillButtonList [i];
				Image imgIcon = btn.GetComponent<Image>();
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
			txt_level.text = MMOController.Instance.playerInfo.unitInfo.attribute.level.ToString ();
			txt_name.text = MMOController.Instance.playerInfo.unitInfo.attribute.unitName;
			if (MMOController.Instance.playerInfo.unitInfo.attribute.maxHP > 0)
				img_health.fillAmount = MMOController.Instance.playerInfo.unitInfo.attribute.currentHP / (float)MMOController.Instance.playerInfo.unitInfo.attribute.maxHP;
		}


	}
}