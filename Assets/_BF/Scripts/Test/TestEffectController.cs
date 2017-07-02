using UnityEngine;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode]
public class TestEffectController : MonoBehaviour {

	public Transform playerPos;
	public GameObject mGo;
	public HeroRes heroRes;
	public HeroResEffect heroResEffect;
	public HeroAnimation heroAnimation;
	public HeroEffect heroEffect;

	public Transform leftEffectPos;
	public Transform rightEffectPos;
	public List<GameObject> enemyList;
	public List<GameObject> testObjects;

	static TestEffectController controller;
	public static TestEffectController SingleTon()
	{
		return controller;
	}

	void Awake()
	{
		if(controller==null)
		{
			controller = this;
		}
	}

	public void Init(GameObject go){
		mGo = go;
		mGo.transform.position = playerPos.position;
		mGo.transform.rotation = Quaternion.identity;
		heroRes = go.GetComponent<HeroRes>();
		heroResEffect = go.GetComponent<HeroResEffect>();
		heroAnimation = gameObject.GetOrAddComponent<HeroAnimation>();
		heroAnimation.heroRes = heroRes;
		heroAnimation.heroResEffect = heroResEffect;
		heroEffect = gameObject.GetOrAddComponent<HeroEffect>();
		heroEffect.heroResEffect = heroResEffect;
	}

	void OnGUI()
	{
		if(mGo!=null)
		{
			List<AnimMapping> anims =  heroRes.bodyAnims;
			float yOffset = 10;
			float xOffect = 10;
			int index = 0;
			foreach(AnimMapping am in anims)
			{
				if(GUI.Button(new Rect(xOffect,yOffset + index * 40,100,30),am.clipName))
				{
					switch(am.clipName)
					{
						case "Attack":{
							heroEffect.PlayEffects(_AnimType.Attack,enemyList);
							heroAnimation.PlayByFrags("Attack");
							break;
						}
						case "Skill1":{
							heroEffect.PlayEffects(_AnimType.Skill1,enemyList);
							heroAnimation.PlayByFrags("Skill1");
							break;
						}
						case "StandBy":{
							heroEffect.PlayEffects(_AnimType.StandBy,enemyList);
							heroAnimation.PlayByFrags("StandBy");
							break;
						}
						case "Death" :{
							heroEffect.PlayEffects(_AnimType.Death);
							heroAnimation.PlayByFrags("Death");
							break;
						}
						case "Hit" :{
							heroEffect.PlayEffects(_AnimType.Hit);
							heroAnimation.PlayByFrags("Hit");
							break;
						}
						case "Run":{
							heroEffect.PlayEffects(_AnimType.Run,enemyList);
							heroAnimation.PlayByFrags("Run");
							break;
						}
						case "Cheer" :{
							heroEffect.PlayEffects(_AnimType.Cheer,enemyList);
							heroAnimation.PlayByFrags("Cheer");
							break;
						}
						case "Power" :{
							heroEffect.PlayEffects(_AnimType.Power,enemyList);
							heroAnimation.PlayByFrags("Power");
							break;
						}
						case "Sprint":{
							heroEffect.PlayEffects(_AnimType.Sprint,enemyList);
							heroAnimation.PlayByFrags("Sprint");
							break;
						}
					}
				}
				index ++;
			}
			SampleAnim();
		}
	}

	float animPoint = 0;
	float tmpAnimPoint = 0;
	AnimationState currentState = null;
	void SampleAnim()
	{
		AnimMapping am = heroRes.CurrentAm;
		if(am!=null && am.anim!=null && am.clip!=null)
		{
			GUI.Label(new Rect(10,Screen.height-80,300,30),"Current State:" + am.clipName + ";Length:" + am.clip.length + "s" );
			animPoint = GUI.HorizontalSlider(new Rect(10,Screen.height-50,300,30),animPoint,0,am.anim[am.clip.name].length);
			GUI.Label(new Rect(310,Screen.height-50,120,30), animPoint + "s" );
			if(tmpAnimPoint!=animPoint)
			{
				AnimationState state = am.anim[am.clip.name];
				state.time = animPoint;
				am.anim.Play(state.name);
				state.speed = 0;
				state.time = animPoint;
				am.anim.Sample();
				tmpAnimPoint = animPoint;
			}
		}
	}

//	void EditorPlay()  
//	{  
//		string sceneName; 
//		Debug.Log("PlayingSave!!!!!!");  
//		sceneName = EditorApplication.currentScene;  
//		string[] path = sceneName.Split(char.Parse("/"));  
//		
//		path[path.Length -1] = "Temp_" + path[path.Length-1];  
//		string tempScene = string.Join("/",path);  
//		EditorApplication.SaveScene(tempScene);  
//		
//		FileUtil.DeleteFileOrDirectory(sceneName);  
//		FileUtil.MoveFileOrDirectory(tempScene, sceneName);  
//		FileUtil.DeleteFileOrDirectory(tempScene);  
//		
//		EditorApplication.SaveScene(sceneName);  
//		EditorApplication.OpenScene(sceneName);  
//	}  



}
