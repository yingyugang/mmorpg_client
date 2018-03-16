using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MMO
{
	public static class SceneManager
	{

		public static void LoadLogin(){
			UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_LOGIN);
		}

		public static void LoadDownload(){
			UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_DOWNLOAD);
		}

		public static void LoadMain(){
			UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_MAIN);
		}

		public static void LoadBattlefield(){
			UnityEngine.SceneManagement.SceneManager.LoadScene (SceneConstant.SCENE_BATTLE);
		}

	}
}
