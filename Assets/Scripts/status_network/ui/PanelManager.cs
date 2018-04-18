using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


namespace MMO
{
	//TODO 臨時使用します。
	public class PanelManager : SingleMonoBehaviour<PanelManager>
	{

		public LoginPanel loginPanel;
		public MainInterfacePanel mainInterfacePanel;
		public ChatPanel chatPanel;
		public ServerListPanel serverListPanel;
		//TODO
		public CommonDialogPanel commonDialogPanel;

		protected override void Awake ()
		{
			base.Awake ();
		}

		void Start(){
			if (serverListPanel != null) {
				serverListPanel.gameObject.SetActive (true);
				serverListPanel.onBtnIpClick = () => {
					loginPanel.gameObject.SetActive (true);
				};
			}
		}

		public void InitSkillIcons(MMOUnitSkill mmoUnitSkill){
			mainInterfacePanel.SetSkillDatas (mmoUnitSkill);
		}

		public void ShowBigMapMask(){
			if(!mainInterfacePanel.img_bigmap_mask.gameObject.activeInHierarchy)
				mainInterfacePanel.img_bigmap_mask.gameObject.SetActive (true);
		}

		public void HideBigMapMask(){
			if(mainInterfacePanel.img_bigmap_mask.gameObject.activeInHierarchy)
				mainInterfacePanel.img_bigmap_mask.gameObject.SetActive (false);
		}

		public void HideCommonDialog(){
			if (commonDialogPanel.gameObject.activeInHierarchy)
				commonDialogPanel.gameObject.SetActive (false);
		}

		public void ShowCommonDialog(string title,string msg,string ok,UnityAction onOk){
			commonDialogPanel.ShowCommonDialog (title,msg,ok,onOk);
		}


	}
}
