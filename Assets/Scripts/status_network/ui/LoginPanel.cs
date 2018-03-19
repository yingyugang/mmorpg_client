using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class LoginPanel : PanelBase
	{
	
		public const string TARGET_IP = "TARGET_IP";
		public const string TARGET_PORT = "TARGET_PORT";
		public const string TARGET_NAME = "TARGET_NAME";
		public InputField input_ip;
		public InputField input_port;
		public InputField input_name;
		public Button btn_connect;
		public GameObject uiRoot;

		protected override void Awake ()
		{
			base.Awake ();
			btn_connect.onClick.AddListener (()=>{
				MMOController.Instance.playerName = input_name.text.Trim();
				MMOController.Instance.Connect(input_ip.text.Trim(),int.Parse(input_port.text));
				PlayerPrefs.SetString(TARGET_IP,input_ip.text);
				PlayerPrefs.SetString(TARGET_PORT,input_port.text);
				PlayerPrefs.SetString(TARGET_NAME,input_name.text);
				PlayerPrefs.Save();
				PlatformController.Instance.ShowJoystick();
				uiRoot.SetActive(false);
			});
		}

		void Update(){
			input_ip.text = ServerListPanel.targetIp;
		}

	}
}
