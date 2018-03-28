using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class MeleeSkill : BaseSkill
	{

		public override bool Play ()
		{
			return base.Play ();
		}

		protected override void OnActive ()
		{
			base.OnActive ();
			//DO somthing.
			Melee();
		}

		void Melee(){
		
		}

	}
}
