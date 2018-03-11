using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

namespace MMO
{
	public class CommonDialogPanel : PanelBase
	{
		public Button btn_ok;
		public Text txt_title;
		public Text txt_msg;
		public Text txt_ok;
		public GameObject uiRoot;
		UnityAction mOnOk;

		protected override void Awake ()
		{
			base.Awake ();
			btn_ok.onClick.AddListener (()=>{
				gameObject.SetActive(false);
				if(mOnOk!=null)
					mOnOk();
			});
		}

		public void ShowCommonDialog(string title,string msg,string ok,UnityAction onOk){
			mOnOk = onOk;
			txt_title.text = title;
			txt_msg.text = msg;
			txt_ok.text = ok;
			gameObject.SetActive(true);
		}
	}
}