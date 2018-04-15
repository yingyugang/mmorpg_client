using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MMO
{
	public class EquipmentButtonGroup : MonoBehaviour
	{

		public Button btn_arrow;
		public GridLayoutGroup grid_equipment;
		public List<Button> btn_items;
		const float moveDuration = 0.2f;
		const float startY = -57;
		const float endY = 57;

		void Awake(){
			btn_arrow.onClick.AddListener (()=>{
				if(!mIsShowed)
					ShowEquipments();
				else
					HideEquipments();
			});
			for(int i=0;i<btn_items.Count;i++){
				btn_items [i].onClick.AddListener (()=>{
					
				});
			}
		}

		bool mIsShowed;
		void ShowEquipments(){
				mIsShowed = true;
			btn_arrow.transform.localEulerAngles = new Vector3 (0,0,180);
				grid_equipment.GetComponent<RectTransform> ().DOAnchorPosY (endY,moveDuration);
		}

		void HideEquipments(){
				mIsShowed = false;
				btn_arrow.transform.localEulerAngles = new Vector3 (0,0,0);
				grid_equipment.GetComponent<RectTransform> ().DOAnchorPosY (startY,moveDuration);
		}

	}
}