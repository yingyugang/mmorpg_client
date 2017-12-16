using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace MMO
{
	public class ChatPanel : PanelBase
	{
		public Button btn_send;
		public InputField input_send;
		public Text txt_recieve;
		CanvasGroup mCanvasGroup;

		protected override void Awake ()
		{
			base.Awake ();
			btn_send.onClick.AddListener (()=>{
				Send();
			});
			MMOController.Instance.onChat = OnRecieve;
			mCanvasGroup = txt_recieve.GetComponent<CanvasGroup> ();
		}

		void LateUpdate(){
			if(Input.GetKeyDown(KeyCode.Return)){
				if (EventSystem.current.currentSelectedGameObject != input_send.gameObject) {
					EventSystem.current.SetSelectedGameObject (input_send.gameObject);
				}
				else {
					Send ();
				}
			}
			//TODO
			if (EventSystem.current.currentSelectedGameObject != input_send.gameObject) {
				MMOController.Instance.simpleRpgPlayerController.enabled = true;
				mCanvasGroup.alpha = mAlpha;
				mAlpha -= Time.deltaTime;
			}
			else {
				MMOController.Instance.simpleRpgPlayerController.enabled = false;
			}
		}

		void Send(){
			if (!string.IsNullOrEmpty (input_send.text)) {
				MMOController.Instance.SendChat (input_send.text);
				input_send.text = "";
			}
		}


		float mAlpha = 3;
		void OnRecieve(string msg){
			if(txt_recieve.cachedTextGenerator.lineCount > 8){
				txt_recieve.text = txt_recieve.text.Remove (0, txt_recieve.text.IndexOf ("\r\n") + 2);
			}
			txt_recieve.text = txt_recieve.text + string.Format("{0}\r\n",msg);
			mAlpha = 3; 
		}
	}
}
