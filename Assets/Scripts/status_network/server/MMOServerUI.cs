using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class MMOServerUI : SingleMonoBehaviour<MMOServerUI>
	{
		public Text txt_ip;
		public Text txt_port;

		protected override void Awake ()
		{
			base.Awake ();
		}

		void Start ()
		{
			txt_port.text = NetConstant.LISTENE_PORT.ToString ();
			txt_ip.text = Network.player.ipAddress;
		}

	}
}
