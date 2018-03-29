﻿using System.Collections;
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

		void Awake ()
		{
			mDefaultPosList = new List<Vector3> ();
			for (int i = 0; i < btn_skills.Count; i++) {
				mDefaultPosList.Add (btn_skills[i].transform.localPosition);
				btn_skills [i].GetComponent<Image> ().enabled = false;
				btn_skills [i].transform.localPosition = Vector3.zero;
			}
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
				Sprite iconSprite = ResourcesManager.Instance.GetMobileSkillIcon (sb.skillId);
				MobileSkillButton mobileSkillButton = btnSkill.gameObject.GetOrAddComponent<MobileSkillButton> ();
				mobileSkillButton.InitSkillButton (iconSprite,3,i,OnSkill);
			}
		}

		//TODO 需要使用实际的技能id
		void OnSkill(int skillIndex){
			SkillBase skillBase = mMMOUnitSkill.skillList [skillIndex];
			Debug.Log (string.Format("skill id : {0}",skillBase.skillId));
		}
	}
}