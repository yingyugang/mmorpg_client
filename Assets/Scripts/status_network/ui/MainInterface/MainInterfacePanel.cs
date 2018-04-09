using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace MMO
{
	public class MainInterfacePanel : PanelBase
	{

		public Image img_head;
		public Image img_health;
		public Image img_mana;
		public Text txt_name;
		public Text txt_level;
		public Slider slider_skill;
		public Image img_bigmap_mask;

		const int SKILL_ICON_COUNT = 20;
		public GridLayoutGroup skillGrid;
		public GameObject skillItemPrefab;
		public List<Button> skillButtonList;
		public Button btn_normal_attack;
		public Button btn_voice;
		bool isRecording;
		public MobileSkillButtonGroup mobileSkillButtonGroup;

		Dictionary<Button,SkillBase> mSkillButtonDic;
		MMOUnitSkill mUnitSkill;
		Button mSelectButton;
		bool mIsIconInited = false;
		float mTimeToCloseMobileSkillButtonGroup;
		const float DurationToCloseMobileSkillButtonGroup = 5f;

		protected override void Awake ()
		{
			base.Awake ();
			mSkillButtonDic = new Dictionary<Button, SkillBase> ();
			InitIconItems ();
			skillGrid.gameObject.SetActive(false);
			btn_voice.onClick.AddListener (()=>{
				if(!isRecording){
					MicrophoneManager.Instance.StartMicrophone();
					isRecording = true;
				}else{
					float[] datas = MicrophoneManager.Instance.EndMicrophone();
					MMOController.Instance.SendVoice(datas);
					isRecording = false;
				}
			});
		}

		protected override void Start(){
			base.Start ();
		}

		void InitIconItems ()
		{
			if (mIsIconInited)
				return;
			skillButtonList = new List<Button> ();
			for (int i = 0; i < SKILL_ICON_COUNT; i++) {
				GameObject item = Instantiate (skillItemPrefab);
				item.transform.SetParent (skillGrid.transform);
				item.transform.localScale = Vector3.one;
				item.transform.localPosition = Vector3.zero;
				item.SetActive (true);
				skillButtonList.Add (item.GetComponentInChildren<Button>(true));
			}
			mIsIconInited = true;
		}

		public void SetSkillDatas (MMOUnitSkill unitSkill)
		{
			InitIconItems ();
			ResetSkillIcons ();
			mUnitSkill = unitSkill;
			this.mobileSkillButtonGroup.Init (unitSkill);
			List<SkillBase> skills = unitSkill.skillList;
			for (int i = 0; i < skills.Count; i++) {
//				SkillBase sb = skills [i];
				Button btnSkill = skillButtonList [i];
				btnSkill.onClick.AddListener (()=>{
					if(mSelectButton!=null){
						UnSelectSkillButton(mSelectButton);
					}
					mSelectButton = btnSkill;
					SelectSkillButton(mSelectButton);
					ShowSkillSilder(GlobalConstant.DEFAULT_SKILL_READ_DURATION,unitSkill,skills[i]);
				});
				Image imgIcon = btnSkill.GetComponent<Image>();
				if (imgIcon != null && skills [i].mSkill != null) {
					imgIcon.sprite = ResourcesManager.Instance.GetSkillIcon (skills [i].mSkill.id);// skillIconList[sb.skillId % skillIconList.Count];
					imgIcon.gameObject.SetActive (true);
				}
			}
		}

		void UnSelectSkillButton(Button skillBtn){
			if(skillBtn!=null){
				Transform activeTrans = skillBtn.transform.parent.Find ("img_active");
				activeTrans.gameObject.SetActive (false);
				skillBtn = null;
			}
		}

		void SelectSkillButton(Button skillBtn){
			if(skillBtn!=null){
				Transform activeTrans = skillBtn.transform.parent.Find ("img_active");
				activeTrans.gameObject.SetActive (true);
			}
		}

		void UpdateCooldowns(){
			if (mUnitSkill == null || mUnitSkill.skillList == null)
				return;
			List<SkillBase> skills = mUnitSkill.skillList;
			for (int i = 0; i < skills.Count; i++) {
				SkillBase sb = skills [i];
				Button btnSkill = skillButtonList [i];
				SkillBase skillBase = skills [i];
				Image imgCooldown = btnSkill.transform.parent.Find ("img_cooldown").GetComponent<Image> ();
				imgCooldown.fillAmount = Mathf.Max(0,skillBase.GetCooldown());
				if (imgCooldown.fillAmount == 0) {
					imgCooldown.gameObject.SetActive (false);
				} else {
					imgCooldown.gameObject.SetActive (true);
				}
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
			UpdateCooldowns ();
		
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

		Coroutine mCoroutine;
		public void ShowSkillSilder(float duration,MMOUnitSkill unitSkill,SkillBase skillBase){
			//TODO to check the animation clip name;
			StartCoroutine (_PlayAnimation("cast",2));
			if (mCoroutine != null) {
				StopCoroutine (mCoroutine);
			}
			mCoroutine = StartCoroutine (_ShowSkillSilder(duration,unitSkill,skillBase));
		}

		IEnumerator _PlayAnimation(string clip,float length){
			this.mUnitSkill.mmoUnit.SetAnimation(clip,1f);
			yield return new WaitForSeconds (length);
//			this.mUnitSkill.mmoUnit.SetAnimation ("idle",1f);
		}

		IEnumerator _ShowSkillSilder(float duration,MMOUnitSkill unitSkill,SkillBase skillBase){
			slider_skill.GetComponent<CanvasGroup> ().DOFade (1,0.1f);
			float t = 0;
			slider_skill.value = 0;
			while(t < 1){
				t += Time.deltaTime / duration;
				slider_skill.value = t;
				yield return null;
			}
			unitSkill.PlayClientSkill(skillBase);
			ShopSkill ();
		}

		void ShopSkill(){
			UnSelectSkillButton (mSelectButton);
			slider_skill.GetComponent<CanvasGroup> ().DOFade (0,0.1f);
		}
	}
}