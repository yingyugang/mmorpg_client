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
		float mDropIpDelay = 10;

		float mCheckIpInterval = 2;
		float mNextCheckTime;
		public static string targetIp;

		protected override void Awake ()
		{
			base.Awake ();
			mServerBtns = new Dictionary<string, GameObject> ();
		}

		void Update ()
		{
			if (mNextCheckTime < Time.time) {
				mNextCheckTime = Time.time + mCheckIpInterval;
				foreach (string ip in MessageReciever.ips.Keys) {
					if (!mServerBtns.ContainsKey (ip)) {
						GameObject go = Instantiate (itemPrefab);
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
	}
}
