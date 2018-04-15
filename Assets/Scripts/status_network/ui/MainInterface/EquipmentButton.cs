using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.Events;

namespace MMO
{
	public class EquipmentButton : MonoBehaviour
	{

		public Button btn_equip;
		public Image img_equip;
		public Text txt_cooldown;
		public Text txt_num;
		float mCooldown;
		const float EQUIPMENT_COOLDOWN  = 3f;
		SkillBase mSkillBase;
		UnityAction<SkillBase> onSkillButtonClick;

		void Awake(){
			btn_equip.onClick.AddListener (()=>{
				OnEquipmentButtonClick();
			});
		}

		void OnEquipmentButtonClick(){
			StartCoroutine (_Cooldown());
		}

		IEnumerator _Cooldown(){
			img_equip.fillAmount = 0;
			float t = 0;
			while(t < 1){
				t += Time.deltaTime / EQUIPMENT_COOLDOWN;
				txt_cooldown.text = string.Format ("{0:N}", (1 - img_equip.fillAmount) * EQUIPMENT_COOLDOWN);// (1 - img_equip.fillAmount) * EQUIPMENT_COOLDOWN ;
				yield return null;
			}
			img_equip.fillAmount = 1;
			img_equip.raycastTarget = true;
		}

		public void InitEquipIcon(){
			
		}

	}

}
