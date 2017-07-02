using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;

//using LuaInterface;
using CSV;
using System.IO;

public class CSVManager : SingleMonoBehaviour<CSVManager>
{

	public const string CSV_PATH = @"Assets/Resources/Configs/dict/";
	public const string CSV_UNIT_SKILL = "m_unit_skill";

	public const string CSV_SKILL = "m_skill";
	public const string CSV_SKILL_BASE = "m_skill_base";
	public const string CSV_SKILL_EFFECT = "m_skill_effect";
	public const string CSV_SKILL_EFFECT_BASE = "m_skill_effect_base";
	public const string CSV_SKILL_CAST = "m_skill_cast";
	public const string CSV_SKILL_CAST_BASE = "m_skill_cast_base";

	CsvContext mCsvContext;
	public List<SkillCSVStructure> skillList;
	public Dictionary<int,SkillCSVStructure> skillDic;

	public List<UnitSkillCSVStructure> unitSkillList;
	public Dictionary<int,UnitSkillCSVStructure> unitSkillDic;
	public Dictionary<int,List<UnitSkillCSVStructure>> unitSkillGroupDic;

	public List<SkillEffectBaseCSVStructure> skillEffectBaseList;
	public Dictionary<int,SkillEffectBaseCSVStructure> skillEffectBaseDic;

	public List<SkillEffectCSVStructure> skillEffectList;
	public Dictionary<int,SkillEffectCSVStructure> skillEffectDic;


	protected override void Awake ()
	{
		base.Awake ();
		if (!isInited) {
			LoadCSVs ();
			isInited = true;
		}

	}

	void Start ()
	{
		/*
		LuaState lua = new LuaState ();
		TextAsset text = Resources.Load<TextAsset> ("Lua/Main.lua");
		lua.Start ();
		LuaBinder.Bind(lua);
		lua.DoString (text.text,"CSVManager.cs");
		LuaFunction func = lua.GetFunction("Main");
		func.Call ();
		lua.Collect ();
		*/
	}

	void LoadCSVs ()
	{
		mCsvContext = new CsvContext ();
		TextAsset textAsset = Resources.Load<TextAsset> ("Configs/dict/" + CSV_SKILL_EFFECT_BASE);
		MemoryStream stream = new MemoryStream (textAsset.bytes);
		StreamReader reader = new StreamReader (stream); 
		LoadCSV<SkillEffectBaseCSVStructure> (mCsvContext.Read<SkillEffectBaseCSVStructure> (reader), out skillEffectBaseList, out skillEffectBaseDic);

		textAsset = Resources.Load<TextAsset> ("Configs/dict/" + CSV_SKILL_EFFECT);
		stream = new MemoryStream (textAsset.bytes);
		reader = new StreamReader (stream); 
		LoadCSV<SkillEffectCSVStructure> (mCsvContext.Read<SkillEffectCSVStructure> (reader), out skillEffectList, out skillEffectDic);

		textAsset = Resources.Load<TextAsset> ("Configs/dict/" + CSV_SKILL);
		stream = new MemoryStream (textAsset.bytes);
		reader = new StreamReader (stream); 
		LoadCSV<SkillCSVStructure> (mCsvContext.Read<SkillCSVStructure> (reader), out skillList, out skillDic);

		textAsset = Resources.Load<TextAsset> ("Configs/dict/" + CSV_UNIT_SKILL);
		stream = new MemoryStream (textAsset.bytes);
		reader = new StreamReader (stream); 
		LoadCSV<UnitSkillCSVStructure> (mCsvContext.Read<UnitSkillCSVStructure> (reader), out unitSkillList, out unitSkillDic);

		//从镜像技能合并
		for (int i = 0; i < skillList.Count; i++) {
			SkillCSVStructure baseSkill = skillList [i];
			if (skillDic.ContainsKey (baseSkill.mirror_id)) {
				SkillCSVStructure mirrorSkill = skillDic [baseSkill.mirror_id];
				baseSkill.CopyFromMirror (mirrorSkill);
			}
		}

		//初始化Unit技能组
		unitSkillGroupDic = new Dictionary<int, List<UnitSkillCSVStructure>> ();
		for (int i = 0; i < unitSkillList.Count; i++) {
			if (!unitSkillGroupDic.ContainsKey (unitSkillList [i].unit_id)) {
				unitSkillGroupDic.Add (unitSkillList [i].unit_id, new List<UnitSkillCSVStructure> ());
			}
			unitSkillGroupDic [unitSkillList [i].unit_id].Add (unitSkillList [i]);
		}

	}

	public void LoadCSV<T> (IEnumerable<T> datas, out List<T> listData, out Dictionary<int,T> dicData) where T : BaseCSVStructure
	{
		listData = new List<T> ();
		listData.AddRange (datas);
		dicData = ListToDictionary<T> (listData);
	}

	public Dictionary<int,T> ListToDictionary<T> (List<T> list) where T : BaseCSVStructure
	{
		Dictionary<int,T> dic = new Dictionary<int, T> ();
		foreach (T t in list) {
			if (!dic.ContainsKey (t.id))
				dic.Add (t.id, t);
		}
		return dic;
	}

}
