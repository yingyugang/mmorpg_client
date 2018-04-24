using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DuloGames.UI;

namespace MMO
{
	public class MainInterface : SingleMonoBehaviour<MainInterface>
	{
		public UIUnitFrame uiMainUnitFrame;

		public UIUnitGroupGrid uiUnitGroupGrid;
		//TODO バトル優先、後で作る。
		public void AddGroupPlayer(PlayerInfo playerInfo){
		
		}
		//TODO バトル優先、後で作る。
		public void RemoveGroupPlayer(int playerId){
		
		}

		public DuloGames.UI.UIWindow inventory;

		public DuloGames.UI.UIWindow character;

		public DuloGames.UI.UIWindow spellBook;

		public DuloGames.UI.UIWindow dialog;

		public DuloGames.UI.UIWindow questLog;

		public DuloGames.UI.UIWindow vendor;

		public DuloGames.UI.UIWindow options;

		public DuloGames.UI.UIWindow gameMenu;

		public GameObject notification;

		public Demo_Chat chat;

		void Update(){
			if(Input.GetKeyDown(KeyCode.Tab)){
				inventory.Toggle ();
			}
			if(Input.GetKeyDown(KeyCode.T)){
				if(!chat.gameObject.activeInHierarchy)
					chat.gameObject.SetActive (true);
				else
					chat.gameObject.SetActive (false);
			}
		}


	}
}