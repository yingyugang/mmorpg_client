using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public class CharacterSelectManager : MonoBehaviour
	{

		public void LoadMain(){
			SceneManager.LoadMain ();
		}

		public void LoadCharacterCreate(){
			SceneManager.LoadCharacterCreate ();
		}

	}
}
