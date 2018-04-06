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
		SkillBase mSkillBase;
		UnityAction<SkillBase> onSkillButtonClick;

		void Awake(){
			btn_skill = GetComponentInChildren<Button>();
			btn_skill.onClick.AddListener (OnClickSkillButton);
//			img_skill = btn_skill.GetComponent<Image> ();
			txt_cooldown = transform.Find ("txt_cooldown").GetComponent<Text> ();
			txt_cooldown.color = new Color (1,1,1,0);
			img_cooldown_mask = btn_skill.GetComponent<Image> ();
		}
	
		void OnClickSkillButton(){
//			img_cooldown_mask.fillAmount = 1;
//			img_cooldown_mask.enabled = true;
//			img_cooldown_mask.DOFillAmount (0,mCooldown).OnComplete(()=>{
//				img_cooldown_mask.enabled = false;
//			});
			StartCoroutine (_Cooldown());
			if (onSkillButtonClick != null)
				onSkillButtonClick (mSkillBase);
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

		public void InitSkillButton(Sprite skillIcon,float cooldown,SkillBase baseSkill,UnityAction<SkillBase> onClick){
//			this.img_skill.sprite = skillIcon;
			this.mCooldown = cooldown;
			this.mSkillBase = baseSkill;
			this.onSkillButtonClick = onClick;
		}

	}
}
