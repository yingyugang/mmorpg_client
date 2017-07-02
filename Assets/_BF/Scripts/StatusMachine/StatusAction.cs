using UnityEngine;
using System.Collections;

namespace StatusMachines
{
	[System.Serializable]
	public class StatusAction
	{
		public GameObject GO;
		public StatusMachine statusMachine;

		public virtual void OnAwake ()
		{
			
		}

		public virtual void OnEnter ()
		{
		
		}

		public virtual void OnUpdate ()
		{

		}

		public virtual void OnExit ()
		{

		}
	}
}
