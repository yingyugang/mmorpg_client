using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class ConfirmDialogPanel : PanelBase
	{
		public Button btn_ok;
		public Button btn_cancal;
		public GameObject uiRoot;

		protected override void Awake ()
		{
			base.Awake ();
		}
	}
}
