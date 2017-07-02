using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

	public Button btn_blood;
	public Button btn_fog;
	public Button btn_thunder;
	public Button btn_moon;
	public Button btn_sand;

	public const string SCENE_BLOOD = "BattleA BackGround";
	public const string SCENE_FOG = "BattleB BackGround";
	public const string SCENE_THUNDER = "BattleC BackGround";
	public const string SCENE_MOON = "BattleD BackGround";
	public const string SCENE_SAND = "BattleE BackGround";

	void Awake(){
		btn_blood.onClick.AddListener (()=>{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_BLOOD);
		});
		btn_fog.onClick.AddListener (()=>{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_FOG);
		});
		btn_thunder.onClick.AddListener (()=>{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_THUNDER);
		});
		btn_moon.onClick.AddListener (()=>{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_MOON);
		});
		btn_sand.onClick.AddListener (()=>{
			UnityEngine.SceneManagement.SceneManager.LoadScene(SCENE_SAND);
		});
	}


}
