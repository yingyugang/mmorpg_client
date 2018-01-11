using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BattleFramework.Data;
using CSV;
using System.IO;

namespace MMO
{
	public class CSVManager : SingleMonoBehaviour<CSVManager>
	{
		//	public const string CSV_PATH = @"Assets/CSV";//
		private const string CSV_UNIT = "m_unit";
		private const string CSV_SKILL = "m_skill";
		private const string CSV_SKILL_EFFECT_BASE = "m_skill_effect_base";
		private const string CSV_UNIT_SKILL = "m_unit_skill";

		private CsvContext mCsvContext;
		List<MUnit> mUnitList;
		public Dictionary<int,MUnit> mUnitDic;
		List<MSkillEffectBaseCSVStructure> mSkillEffectBaseList;
		public Dictionary<int,MSkillEffectBaseCSVStructure> skillEffectBaseDic;
		List<MSkill> mSkillList;
		public Dictionary<int,MSkill> skillDic;
		List<MUnitSkill> mUnitSkillList;
		public Dictionary<int,MUnitSkill> unitSkillDic;

		protected override void Awake ()
		{
			base.Awake ();
			StartLoading ();
		}

		byte[] GetCSV (string fileName)
		{
			//#if UNITY_EDITOR
			//TODO 因为时间关系暂时用Resources，放到固定的文件夹下面，可以编辑最佳。
			return Resources.Load<TextAsset> ("CSV/" + fileName).bytes;
			//#else
			//return ResourcesManager.Ins.GetCSV (fileName);
			//#endif
		}

		void StartLoading ()
		{
			mCsvContext = new CsvContext ();
			LoadUnitTable ();
			LoadSkillEffectBaseTable ();
			LoadSkillTable ();
			ListUnitSkillTable ();
		}

		public MUnit GetUnit (int unitId)
		{
			if (mUnitDic.ContainsKey (unitId))
				return mUnitDic [unitId];
			else {
				return mUnitList [0];
			}
		}

		void LoadUnitTable ()
		{
			mUnitList = CreateCSVList<MUnit> (CSV_UNIT);
			mUnitDic = GetDictionary<MUnit> (mUnitList);
		}

		void LoadSkillEffectBaseTable ()
		{
			mSkillEffectBaseList = CreateCSVList<MSkillEffectBaseCSVStructure> (CSV_SKILL_EFFECT_BASE);
			skillEffectBaseDic = GetDictionary<MSkillEffectBaseCSVStructure> (mSkillEffectBaseList);
		}

		void LoadSkillTable ()
		{
			mSkillList = CreateCSVList<MSkill> (CSV_SKILL);
			skillDic = GetDictionary<MSkill> (mSkillList);
		}

		void ListUnitSkillTable ()
		{
			mUnitSkillList = CreateCSVList<MUnitSkill> (CSV_UNIT_SKILL);
			unitSkillDic = GetDictionary<MUnitSkill> (mUnitSkillList);
			for (int i = 0; i < mUnitSkillList.Count; i++) {
				MUnitSkill mUnitSkill = mUnitSkillList [i];
				if(mUnitDic.ContainsKey(mUnitSkill.unit_id)){
					MUnit mUnit = mUnitDic[mUnitSkill.unit_id];
					if (mUnit.unitSkillList == null) {
						mUnit.unitSkillList = new List<MUnitSkill> ();
					}
					if(mUnit.skillIdList == null){
						mUnit.skillIdList = new List<int> ();
					}
					mUnit.skillIdList.Add(mUnitSkill.skill_id);
					mUnit.unitSkillList.Add (mUnitSkill);
				}
			}
			for(int i=0;i<mUnitList.Count;i++){
				if(mUnitList [i].skillIdList!=null)
					mUnitList [i].skillIds = mUnitList [i].skillIdList.ToArray ();
				else
					mUnitList [i].skillIds = new int[0];
			}
		}

		List<T> CreateCSVList<T> (string csvname) where T:BaseCSVStructure, new()
		{
			var stream = new MemoryStream (GetCSV (csvname));
			var reader = new StreamReader (stream);
			IEnumerable<T> list = mCsvContext.Read<T> (reader);
			return new List<T> (list);
		}

		Dictionary<int,T> GetDictionary<T> (List<T> list) where T : BaseCSVStructure
		{
			Dictionary<int,T> dic = new Dictionary<int, T> ();
			foreach (T t in list) {
				if (!dic.ContainsKey (t.id))
					dic.Add (t.id, t);
			}
			return dic;
		}
	}
}
