using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class ServerListPanel : PanelBase
	{

		public GameObject itemPrefab;
		public GameObject txt_search;
		public Transform listParent;
		Dictionary<string,GameObject> mServerBtns;

		float mCheckIpInterval = 2;
		float mNextCheckTime;
		string[] mServerNames;
		public static string targetIp;

		protected override void Awake ()
		{
			base.Awake ();
			mServerBtns = new Dictionary<string, GameObject> ();
			mServerNames = GetServerNames ();
		}

		void Update ()
		{
			if (mNextCheckTime < Time.time) {
				mNextCheckTime = Time.time + mCheckIpInterval;
				foreach (string ip in MessageReciever.ips.Keys) {
					if (!mServerBtns.ContainsKey (ip)) {
						GameObject go = Instantiate (itemPrefab);
						Text text = go.GetComponentInChildren<Text> (true);
						string serverName = mServerNames[Random.Range(0,mServerNames.Length)];
						text.text = serverName;
						go.SetActive (true);
						go.transform.SetParent (listParent);
						mServerBtns.Add (ip, go);
						txt_search.SetActive (false);
						go.GetComponent<Button> ().onClick.AddListener (()=>{
							targetIp = ip;
							gameObject.SetActive(false);
							PanelManager.Instance.loginPanel.gameObject.SetActive(true);
						});
					}
				}

			}
		}

		string[] GetServerNames(){
			string[] serverNames = new string[]{
				"费尔威泽(Felwithe)",
				"自由港(Freeport)",
				"克勒辛(Kelethin)",
				"奎诺斯(Qeynos)",
				"大河谷(Rivervale)",
				"费尔威泽(Felwithe)",
				"自由港(Freeport)",
				"奎诺斯(Qeynos)",
				"卡比利斯(Cabilis)",
				"奥格克(Oggok)",
				"格洛波(Grobb)",
				"克勒辛(Kelethin)",
				"哈勒斯(Halas)",
				"尼瑞克(Neriak)",
				"卡拉丁(Kaladim)",
				"艾露丁(Erudin)",
				"阿克农(Ak'Anon)"};
			return serverNames;
		}
	}
}
