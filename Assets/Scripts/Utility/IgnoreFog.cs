using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class IgnoreFog : MonoBehaviour
	{

		private bool mRevertFogState = false;

		void OnPreRender ()
		{
			mRevertFogState = RenderSettings.fog;
			RenderSettings.fog = false;
		}

		void OnPostRender ()
		{
			RenderSettings.fog = mRevertFogState;
		}
	}
}
