using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using WWWNetwork;

namespace MMO
{
	public class LoginManager : MonoBehaviour
	{
		public ServerListPanel serverListPanel;
		public SigninAPI signin;

		void Awake ()
		{
			serverListPanel.onBtnIpClick = ()=>{
				SignIn();
			};
		}

		void Update(){
			if(Input.GetKeyDown(KeyCode.H)){
				SignIn ();
			}
		}

		void SignIn ()
		{
			string ip = ServerListPanel.targetIp;
			PathConstant.SERVER_DOWNLOAD_PATH = string.Format("http://{0}/kingofhero/",ip.Trim());
			//TODO change to mmorpg
//			PathConstant.SERVER_PATH = string.Format("http://{0}/kingofhero/",ip.Trim());
//		
//			signin.Send ((WWW www)=>{
			SceneManager.LoadDownload ();
//			});
		}
	}

}
