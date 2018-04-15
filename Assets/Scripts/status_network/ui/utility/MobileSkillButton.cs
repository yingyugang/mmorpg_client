using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace MMO
{
	public class MobileSkillButton : MonoBehaviour
	{

		Button btn_skill;
		Image img_skill;
		Text txt_cooldown;
		Image img_cooldown_mask;
		float mCooldown;
		public SkillBase skillBase;
		UnityAction<SkillBase> onSkillButtonClick;

		Vector3 mDefaultButtonPos ;
		Vector3 mDefaultTextPos;
		MobileSkillButtonGroup mMobileSkillButtonGroup;

		void Awake(){
			btn_skill = GetComponentInChildren<Button>();
			btn_skill.onClick.AddListener (OnClickSkillButton);
			img_skill = btn_skill.GetComponent<Image> ();
			txt_cooldown = transform.Find ("txt_cooldown").GetComponent<Text> ();
			txt_cooldown.color = new Color (1,1,1,0);
			img_cooldown_mask = btn_skill.GetComponent<Image> ();
			txt_cooldown.raycastTarget = false;
			mDefaultButtonPos = img_skill.transform.position;
			mDefaultTextPos = txt_cooldown.transform.position;
		}
	
		public void Show(){
			btn_skill.transform.DORotate (new Vector3 (0, 0, 180f), BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			btn_skill.transform.DOLocalMove (mDefaultButtonPos, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
//			txt_cooldown.transform.DORotate (new Vector3 (0, 0, 180f), BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			txt_cooldown.transform.DOLocalMove (mDefaultTextPos, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			img_skill.enabled = true;
			img_skill.raycastTarget = false;
			img_skill.DOFade (1f, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION).OnComplete (() => {
				img_skill.raycastTarget = true;
			});
			if(img_cooldown_mask.fillAmount<1){
				txt_cooldown.DOFade (1f,BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			}
		}

		public void Hide(){
			btn_skill.transform.DORotate (new Vector3 (0, 0, 0), BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			btn_skill.transform.DOLocalMove (mDefaultButtonPos, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
//			txt_cooldown.transform.DORotate (new Vector3 (0, 0, 0), BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			txt_cooldown.transform.DOLocalMove (mDefaultTextPos, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
			img_skill.enabled = true;
			img_skill.raycastTarget = false;
			img_skill.DOFade (0, BattleConst.MOBILE_BUTTON_TOGGLE_DURATION).OnComplete (() => {
				img_skill.raycastTarget = true;
			});
			txt_cooldown.DOFade (0,BattleConst.MOBILE_BUTTON_TOGGLE_DURATION);
		}

		public void OnClickSkillButton(){
			this.mMobileSkillButtonGroup.sorting_skills.Remove (this);
			if(skillBase.IsUseAble()){
				if (this.mMobileSkillButtonGroup.mmoUnitSkill.mmoUnit.unitAnimator.IsIdle () || this.mMobileSkillButtonGroup.mmoUnitSkill.mmoUnit.unitAnimator.IsRun ()) {
					this.mMobileSkillButtonGroup.minNextCheckTime = Time.time + skillBase.mUnitSkill.anim_length;
					if(MMOController.Instance.player.GetComponent<BasePlayerController>()!=null)
						MMOController.Instance.player.GetComponent<BasePlayerController>().enabled =true;

					StartCoroutine (_Cooldown());
					if (onSkillButtonClick != null)
						onSkillButtonClick (skillBase);
					btn_skill.enabled = true;
					img_skill.DOKill ();
					img_skill.color = Color.white;
				} else {
					AddToSort ();
				}
			}
		}

		void AddToSort(){
			this.mMobileSkillButtonGroup.sorting_skills.Add (this);
			btn_skill.enabled = false;
			img_skill.DOColor (Color.red,0.25f).SetLoops(-1,LoopType.Yoyo).SetEase(DG.Tweening.Ease.Linear);
//			iTween.MoveTo(gameObject, {“x”:2, “time”:3, “loopType”:”pingPong”, “delay”:1));
		}

		public void ClearSort(){
			for(int i=0;i<this.mMobileSkillButtonGroup.sorting_skills.Count;i++){
				this.mMobileSkillButtonGroup.sorting_skills [i].btn_skill.enabled = true;
			}
		}

		IEnumerator _Cooldown(){
			img_cooldown_mask.fillAmount = 0;
			img_cooldown_mask.raycastTarget = false;
			txt_cooldown.DOFade (1,0.5f);
			float t = 0;
			while(t < 1){
				img_cooldown_mask.fillAmount = t;
				t += Time.deltaTime / mCooldown;
				txt_cooldown.text = string.Format ("{0:N}", mCooldown * (1 - img_cooldown_mask.fillAmount));
				yield return null;
			}
			img_cooldown_mask.fillAmount = 1;
			img_cooldown_mask.raycastTarget = true;
			txt_cooldown.DOFade (0,0.5f);
		}

		public void InitSkillButton(Sprite skillIcon,float cooldown,SkillBase baseSkill,MobileSkillButtonGroup mobileSkillButtonGroup,UnityAction<SkillBase> onClick){
//			this.img_skill.sprite = skillIcon;
			this.mMobileSkillButtonGroup = mobileSkillButtonGroup;
			this.mCooldown = cooldown;
			this.skillBase = baseSkill;
			this.onSkillButtonClick = onClick;
		}

	}
}
