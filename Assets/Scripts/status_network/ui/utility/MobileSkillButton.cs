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
		int mUnitSkillId;
		UnityAction<int> onSkillButtonClick;

		void Awake(){
			btn_skill.onClick.AddListener (OnClickSkillButton);
			btn_skill = transform.Find ("").GetComponent<Button>();
			img_skill = btn_skill.GetComponent<Image> ();
			txt_cooldown = transform.Find ("").GetComponent<Text> ();
			img_cooldown_mask = transform.Find ("").GetComponent<Image> ();
		}
	
		void OnClickSkillButton(){
			img_cooldown_mask.fillAmount = 1;
			img_cooldown_mask.enabled = true;
			img_cooldown_mask.DOFillAmount (0,mCooldown).OnComplete(()=>{
				img_cooldown_mask.enabled = false;
			});
			StartCoroutine (_Cooldown());
			if (onSkillButtonClick != null)
				onSkillButtonClick (mUnitSkillId);
		}

		IEnumerator _Cooldown(){
			img_cooldown_mask.fillAmount = 1;
			img_cooldown_mask.enabled = true;
			float t = 0;
			while(t < 1){
				img_cooldown_mask.fillAmount = 1 - t;
				txt_cooldown.text = Mathf.CeilToInt (mCooldown * img_cooldown_mask.fillAmount).ToString();
				yield return null;
			}
			img_cooldown_mask.enabled = false;
		}

		public void InitSkillButton(Sprite skillIcon,float cooldown,int unitSkillId,UnityAction<int> onClick){
			this.img_skill.sprite = skillIcon;
			this.mCooldown = cooldown;
			this.mUnitSkillId = unitSkillId;
			this.onSkillButtonClick = onClick;
		}

	}
}
