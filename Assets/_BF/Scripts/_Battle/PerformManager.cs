using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//演出
public class PerformManager : SingleMonoBehaviour<PerformManager> {

	public GameObject globalEffect;
	public GlobalSkillEffect globleSkillEffect;
	public SpriteRenderer globalEffectMask;
	public Camera battleCamera;

	protected override void Awake(){
		base.Awake ();
		defaultBattleCameraPos = battleCamera.transform.position;
	}

	void Update(){
		if(Input.GetKeyDown(KeyCode.H)){
			PlayTeamSkillEffect ();
		}
	}

	public void ShowCritAndRelationDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		BattleUIManager.GetInstance ().ShowCritAndRelationDamageBeat ( shakeRadius,  damage,  pos,  scale);
	}

	public void ShowRelationDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		BattleUIManager.GetInstance ().ShowRelationDamageBeat ( shakeRadius,  damage,  pos,  scale);
	}

	public void ShowCritDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		BattleUIManager.GetInstance ().ShowCritDamageBeat ( shakeRadius,  damage,  pos,  scale);
	}

	public void ShowDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		BattleUIManager.GetInstance ().ShowDamageBeat ( shakeRadius,  damage,  pos,  scale);
	}

	public void ShowHealthBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		BattleUIManager.GetInstance ().ShowHealthBeat ( shakeRadius,  damage,  pos,  scale);
	}

	public void ShowDodge(Unit unit){
		BattleUIManager.GetInstance ().ShowDodge (unit);
	}

	public void ShowDefence(Unit attackTarget){
		Debug.Log ("ShowDefence");
	}

	public void ShowCrit(Hero attackTarget){
		Vector3 hitPos = BattleUtility.GetHitPos(attackTarget);
		Vector3 offset = new Vector3(Random.Range(-0.5f,0.5f),Random.Range(1.5f,3.4f),0);
		BattleUtility.ShowCritEffect(hitPos + offset);
		BattleController.SingleTon ().ShakeCamera (0.5f,0);
	}

	public void ShowCombo(Hero attackTarget){
		Vector3 hitPos = BattleUtility.GetHitPos(attackTarget);
		Vector3 offset = new Vector3(Random.Range(-1.5f,1.5f),Random.Range(2.5f,3.5f),0);
		SpawnManager.SingleTon().comboCount++;
		BattleUtility.ShowMultiEffect(hitPos + offset);
		BattleController.SingleTon ().ShakeCamera (0.5f,0);
	}

	public void ShowAttributeEnhance(Unit unit,string str,Color color){
		BattleUIManager.GetInstance ().ShowBuff (unit,str,color);
	}

	public void ShowElementHitEffect(Unit hero,Vector3 pos)
	{
		GameObject prefab = GetHitEffectPrefab(hero);
		GameObject go = PoolManager.SingleTon().Spawn(prefab,pos,Quaternion.identity);
		PoolManager.SingleTon ().UnSpawn (2,go);
	}

	public void ShowDefenseHitEffect(Hero hero,Vector3 pos)
	{
		PoolManager.SingleTon().Spawn(CommonEffectManager.GetInstance().prefabDefenseHit,pos,Quaternion.identity,0,2);
	}

	GameObject GetHitEffectPrefab(Unit hero)
	{
		if(CommonEffectManager.GetInstance() !=null)
		{
			if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Water) {
				return CommonEffectManager.GetInstance ().prefabHitCommanWater;
			} else if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Wind) {
				return CommonEffectManager.GetInstance ().prefabHitCommanWind;
			} else if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Fire) {
				return CommonEffectManager.GetInstance ().prefabHitCommanFire;
			} else if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Wood) {
				return CommonEffectManager.GetInstance ().prefabHitCommanWood;
			} else if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Evil) {
				return CommonEffectManager.GetInstance ().prefabHitCommanMagic;
			} else if (hero.GetAttribute (EffectType.Element) == (int)_ElementType.Holy) {
				return CommonEffectManager.GetInstance ().prefabHitCommanLight;
			} else {
				return CommonEffectManager.GetInstance ().prefabHitCommon;
			}
		}
		return null;
	}

	public void PlayTeamSkillEffect(){
//		BattleController.SingleTon ().AutoMask.SetActive (true);

		GameObject go = Instantiate (globalEffect) as GameObject;
		go.transform.position = BattleUtility.GetLeftSkillTargetPos ();
		ShakeCamera (4,0.5f);
		StartCoroutine (_ToggleMask());
	}

	public void PlayUnitSkillEffect(Unit unit){
		List<Hero> heros = new List<Hero> ();
		heros.Add (unit.GetComponent<Hero>());

		globleSkillEffect.Play (heros);
	}

	Vector3 defaultBattleCameraPos;
	public void ShakeCamera (float dur, float delay)
	{
		StartCoroutine (_ShakeCamera (dur, delay));
	}

	IEnumerator _ShakeCamera (float dur, float delay)
	{
		for(int i=0;i<BattleSpawnManager.GetInstance().scrolls.Count;i++){
			BattleSpawnManager.GetInstance ().scrolls [i].isRunning = false;
		}
		yield return new WaitForSeconds (delay);
		iTween.ShakePosition (battleCamera.gameObject, Vector3.one * 0.5f, dur);
		yield return new WaitForSeconds (dur);
		battleCamera.transform.position = defaultBattleCameraPos;
		for(int i=0;i<BattleSpawnManager.GetInstance().scrolls.Count;i++){
			BattleSpawnManager.GetInstance ().scrolls [i].isRunning = true;
		}
	}

	IEnumerator _ToggleMask(){
		globalEffectMask.gameObject.SetActive (true);
		globalEffectMask.sortingLayerName = "UI";
		globalEffectMask.color = new Color (0, 0, 0, 0);
		float t = 0;
		float duration = 0.5f;
		while(t<1){
			t += Time.deltaTime / duration;
			globalEffectMask.color = new Color (0,0,0,t);
			yield return null;
		}
		Time.timeScale = 0.4f;
		yield return new WaitForSeconds (3);
		Time.timeScale = 1;
		duration = 0.5f;
		while(t>0){
			t -= Time.deltaTime / duration;
			globalEffectMask.color = new Color (0,0,0,t);
			yield return null;
		}
		globalEffectMask.sortingLayerName = "SupperLayer1";
		globalEffectMask.gameObject.SetActive (false);
	}
}
