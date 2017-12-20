using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class SkillDefault : SkillBase
	{

		public override bool Play ()
		{
			return base.Play ();
		}

		protected override void OnActive ()
		{
			base.OnActive ();
			//DO somthing.
		}

	}
}
