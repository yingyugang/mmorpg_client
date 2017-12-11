using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class MMOUIController : SingleMonoBehaviour<MMOUIController>
	{

		public const string TARGET_IP = "TARGET_IP";
		public const string TARGET_PORT = "TARGET_PORT";

		public InputField input_ip;
		public InputField input_port;
		public Button btn_connect;
		public GameObject uiRoot;

		protected override void Awake ()
		{
			base.Awake ();
			input_ip.text = PlayerPrefs.GetString (TARGET_IP);
			input_port.text = PlayerPrefs.GetString (TARGET_PORT);
			btn_connect.onClick.AddListener (()=>{
				MMOController.Instance.Connect(input_ip.text.Trim(),int.Parse(input_port.text));
				PlayerPrefs.SetString(TARGET_IP,input_ip.text);
				PlayerPrefs.SetString(TARGET_PORT,input_port.text);
				PlayerPrefs.Save();
				uiRoot.SetActive(false);
			});
		}

	}
}
