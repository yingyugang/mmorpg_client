using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MMO
{
	public class MobileSkillButtonGroup : MonoBehaviour
	{

		public Button btn_normal_skill;
		public List<Button> btn_skills;
		public bool isShow;
		List<Vector3> mDefaultPosList;
		float duration = 0.6f;
		float mTimeToCloseMobileSkillButtonGroup;
		const float DurationToCloseMobileSkillButtonGroup = 5f;
		MMOUnitSkill mUnitSkill;

		void Awake ()
		{
			mDefaultPosList = new List<Vector3> ();
			btn_normal_skill.onClick.AddListener (OnNormalAttack);
			btn_normal_skill.gameObject.SetActive(true);
			for (int i = 0; i < btn_skills.Count; i++) {
				mDefaultPosList.Add (btn_skills[i].transform.localPosition);
				btn_skills [i].GetComponent<Image> ().enabled = false;
				btn_skills [i].transform.localPosition = Vector3.zero;
			}
		}

		void Update(){
			//TODO to update the time on sub skill button.
			if(mTimeToCloseMobileSkillButtonGroup < Time.time && isShow){
				HideSkills ();
			}
		}

		public void Init(MMOUnitSkill unitSkill){
			this.mUnitSkill = unitSkill;
		}

		public void ShowSkills ()
		{
			for (int i = 0; i < btn_skills.Count; i++) {
				btn_skills [i].transform.DORotate (new Vector3 (0, 0, 180f), duration);
				btn_skills [i].transform.DOLocalMove (mDefaultPosList[i],duration);
				Image image = btn_skills [i].GetComponent<Image> ();
				image.enabled = true;
				image.raycastTarget = false;
				image.DOFade (1f, duration).OnComplete(()=>{
					image.raycastTarget = true;
				});
			}
			isShow = true;
		}

		public void HideSkills ()
		{
			for (int i = 0; i < btn_skills.Count; i++) {
				btn_skills [i].transform.DORotate (new Vector3 (0, 0, 0), duration);
				btn_skills [i].transform.DOLocalMove (new Vector3 (0, 0, 0),duration);
				Image image = btn_skills [i].GetComponent<Image> ();//.enabled = false;
				image.DOFade (0f, duration).OnComplete(()=>{
					image.raycastTarget = false;
				});
			}
			isShow = false;
		}

		MMOUnitSkill mMMOUnitSkill;
		public void SetSkillDatas (MMOUnitSkill unitSkill)
		{
			//TODO get normal attack
			//TODO get skill
			this.mMMOUnitSkill = unitSkill;
			List<SkillBase> skills = unitSkill.skillList;
			for(int i=0;i<skills.Count;i++){
				SkillBase sb = skills [i];
				Button btnSkill = btn_skills [i];
				Sprite iconSprite = ResourcesManager.Instance.GetMobileSkillIcon (sb.mSkill.id);
				MobileSkillButton mobileSkillButton = btnSkill.gameObject.GetOrAddComponent<MobileSkillButton> ();
				mobileSkillButton.InitSkillButton (iconSprite,3,i,OnSkill);
			}
		}

		//TODO 需要使用实际的技能id
		void OnSkill(int skillIndex){
			SkillBase skillBase = mMMOUnitSkill.skillList [skillIndex];
		}

		void OnNormalAttack(){
			mTimeToCloseMobileSkillButtonGroup = Time.time + DurationToCloseMobileSkillButtonGroup;
			if (!isShow) {
				ShowSkills ();
				return;
			}
			//need a area to place the config at user handled skill.
			//this is not in default mmorpg.
			if (mUnitSkill.mmoUnit.IsInState ("attack3") ) {
				MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.CAST,mUnitSkill.skillList[0].mUnitSkill.id);
				mUnitSkill.mmoUnit.SetTrigger ("attack4");
			} else if (mUnitSkill.mmoUnit.IsInState ("attack2")) {
				MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.CAST,mUnitSkill.skillList[1].mUnitSkill.id);
				mUnitSkill.mmoUnit.SetTrigger ("attack3");
			} else if (mUnitSkill.mmoUnit.IsInState ("attack1") ) {
				MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.CAST,mUnitSkill.skillList[2].mUnitSkill.id);
				mUnitSkill.mmoUnit.SetTrigger ("attack2");
			} else {
				MMOController.Instance.SendPlayerAction (BattleConst.UnitMachineStatus.CAST,mUnitSkill.skillList[3].mUnitSkill.id);
				mUnitSkill.mmoUnit.SetTrigger ("attack1");
			}
		}
	}
}