using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BattleUIManager : SingleMonoBehaviour<BattleUIManager>
{

	public Button btn_battle_start;
	public GameObject prefab_enhance;
	public GameObject prefab_damage;
	public GameObject prefab_health;
	public Button btn_global_skill;
	public Image img_energy;
	public Canvas canvas;
	public Camera mainCamera;
	public Camera uiCamera;

	protected override void Awake ()
	{
		base.Awake ();
		btn_battle_start.gameObject.SetActive (false);
		Init ();
	}

	void Init ()
	{
		btn_battle_start.onClick.AddListener (() => {
			BattleManager.GetInstance ().sm.ChangeStatus (_BattleMachineStatus.Attack.ToString ());
			btn_battle_start.gameObject.SetActive (false);
			btn_global_skill.gameObject.SetActive (true);
			btn_global_skill.enabled = false;
			btn_global_skill.GetComponent<Image> ().color = Color.gray;
		});
		btn_global_skill.onClick.AddListener (() => {
			BattleManager.GetInstance ().PlayTeamSkill ();
			btn_global_skill.enabled = false;
		});
		if (prefab_damage != null)
			PoolManager.SingleTon ().AddPool (prefab_damage, 30,canvas.transform);
		if (prefab_health != null)
			PoolManager.SingleTon ().AddPool (prefab_health, 10,canvas.transform);
	}

	Vector3 WorldCameraPosToUIPosition(Vector3 worldPos){
		Vector3 pos = mainCamera.WorldToScreenPoint(worldPos);
		pos = uiCamera.ScreenToWorldPoint(pos);
		return pos;
	}

	void ShowUnitActiveTextGO (Unit unit, string str, Color color)
	{
		GameObject go = Instantiate (prefab_enhance) as GameObject;
		Vector3 pos = this.WorldCameraPosToUIPosition (unit.unitRes.GetCenterPos());
		go.SetActive (true);
		go.transform.SetParent (canvas.transform);
		go.transform.localScale = Vector3.one;
		go.transform.position = pos;
		Text text = go.GetComponentInChildren<Text> (true);
		text.color = color;
		text.text = str;
	}

	public void ShowCritAndRelationDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		string text = "Crit && Relation ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, new Color (0.8f, 0, 0.2f), text);
	}

	public void ShowRelationDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		string text = "Relation ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, Color.yellow, text);
	}

	public void ShowCritDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		string text = "Crit ";
		ShowDamageBeat (shakeRadius, damage, pos, scale, Color.red, text);
	}

	public void ShowDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		ShowDamageBeat (shakeRadius, damage, pos, scale, Color.white, "");
	}

	public void ShowHealthBeat (float shakeRadius, int damage, Vector3 pos, float scale)
	{
		ShowHealthBeat (shakeRadius, damage, pos, scale, Color.green, "");
	}

	void ShowHealthBeat (float shakeRadius, int damage, Vector3 pos, float scale, Color color, string str)
	{
		Vector3 cameraPos =  WorldCameraPosToUIPosition (pos);
		GameObject go = PoolManager.SingleTon ().Spawn (this.prefab_health, cameraPos, Quaternion.identity);
		go.transform.localScale = Vector3.one;
		Text text = go.GetComponentInChildren<Text> (true);
		text.text = str + (Mathf.Abs (damage)).ToString ();
		text.color = color;
	}

	void ShowDamageBeat (float shakeRadius, int damage, Vector3 pos, float scale, Color color, string str)
	{
		Vector3 cameraPos =  WorldCameraPosToUIPosition (pos);
		GameObject go = PoolManager.SingleTon ().Spawn (this.prefab_damage, cameraPos, Quaternion.identity);
		go.transform.localScale = Vector3.one;
		Text text = go.GetComponentInChildren<Text> (true);
		text.text = str + (Mathf.Abs (damage)).ToString ();
		text.color = color;
	}

	public void ShowDodge (Unit unit)
	{
		ShowUnitActiveTextGO (unit, "Dodge", Color.blue);
	}

	public void ShowBuff (Unit unit, string str, Color color)
	{
		ShowUnitActiveTextGO (unit, str, color);
	}

	public void SetEnergy (float value)
	{
		img_energy.fillAmount = value;
		if (value >= 1) {
			btn_global_skill.enabled = true;
			btn_global_skill.GetComponent<Image> ().color = Color.white;
		}
	}

	IEnumerator _ToggleTextColor (Text txt, Color color)
	{
		float t = 2;
		while (t > 0) {
			t -= Time.deltaTime;
			txt.color = new Color (color.r, color.g, color.b, t);
			txt.transform.position += new Vector3 (0, 10 * Time.deltaTime, 0);
			yield return null;
		}
		Destroy (txt.gameObject);
	}

	IEnumerator _ToggleColor (Image img, Color color)
	{
		float t = 1;
		while (t > 0) {
			t -= Time.deltaTime;
			img.color = new Color (color.r, color.g, color.b, t);
			yield return null;
		}
		Destroy (img.gameObject);
	}
}
