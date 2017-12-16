using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MMO
{
	public class MMOUnit : MonoBehaviour
	{
		public Transform headTrans;
		public Text txt_chat;
		public MMOAttribute attribute;
		public Camera uiCamera;
		public Camera mainCamera;

		public void SetChat (string chat)
		{
			txt_chat.text = chat;
		}

	}
}
