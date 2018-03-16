using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace YuRiYuRi
{
	public class DataCenter:SingleMonoBehaviour<DataCenter>
	{

//		public string playerName;
//		public int itemCount;
//		public Dictionary<int,CormJson.CormData> mCormDic;
//		public Dictionary<int,SeedJson.SeedData> mSeedDic;
//		public Dictionary<int,SeedingJson.SeedingData> mSeedingDic;
//		public Dictionary<int,FlowerJson.FlowerData> mFlowerDic;
//
//		public List<int> mOwnFlowerIds;
//
//		public Dictionary<int,VoiceJson.VoiceData> mVoiceDic;
//		public Dictionary<int,IllustrationJson.IllustrationData> mIllustrationDic;
//		public Dictionary<int,AnimationJson.AnimationData> mAnimationDic;
//
//		public const int ITEM_CCNEKTLGREEN_ID = 2;
////CCNEKTL青い
//		public const int ITEM_CCNEKTLBLUE_ID = 3;
////CCNEKTL
//		public const int ITEM_CCNEKTLRED_ID = 1;
////CCNEKTL赤い
//		public const int ITEM_CCNEKTLBLACK_ID = 4;
////CCNEKTL黒い
//		public const int ITEM_CCNEKTLWHITE_ID = 5;
////CCNEKTL
//
//		public const int ITEM_KOKOYU_ID = 6;
////ココ湯
//		public const int ITEM_KOKOTAN_ID = 7;
////ココたん
//		public const int ITEM_HEART_ID = 8;
////ハートグミ
//		public const int ITEM_MUCHIPOI_ID = 9;
////ムシポイ
//		public const int ITEM_AKURA_ID = 10;
////快眠マクラ
//		public const int ITEM_EIYOU_ID = 11;
////栄養剤
//
//		public Dictionary<int,ItemJson.ItemData> mItems;
//		public  Dictionary<int,ItemJson.ItemData> mCCNEKTLs;
//
//		protected override void Awake ()
//		{
//			base.Awake ();
//			//Init ();
//		}
//
//		public void Init ()
//		{
//			//获取本地基本数据
//			InitBaseData ();
//			//用户数据
//			InitUserData ();
//			//item数据
//			InitItemData ();
//		}
//
//		//基础数据
//		void InitBaseData ()
//		{
//			mCormDic = new Dictionary<int, CormJson.CormData> ();
//			foreach (CormJson.CormData corm in JsonManager.GetInstance ().cormJson.data) {
//				if (!mCormDic.ContainsKey (corm.id))
//					mCormDic.Add (corm.id, corm);
//			}
//			mSeedDic = new Dictionary<int, SeedJson.SeedData> ();
//			foreach (SeedJson.SeedData seed in JsonManager.GetInstance ().seedJson.data) {
//				if (!mSeedDic.ContainsKey (seed.id))
//					mSeedDic.Add (seed.id, seed);
//			}
//			mSeedingDic = new Dictionary<int, SeedingJson.SeedingData> ();
//			foreach (SeedingJson.SeedingData seeding in JsonManager.GetInstance ().seedingJson.data) {
//				if (!mSeedingDic.ContainsKey (seeding.id))
//					mSeedingDic.Add (seeding.id, seeding);
//			}
//			mFlowerDic = new Dictionary<int, FlowerJson.FlowerData> ();
//			foreach (FlowerJson.FlowerData flower in JsonManager.GetInstance ().flowerJson.data) {
//				if (!mFlowerDic.ContainsKey (flower.id))
//					mFlowerDic.Add (flower.id, flower);
//			}
//
//			mVoiceDic = new Dictionary<int, VoiceJson.VoiceData> ();
//			foreach (VoiceJson.VoiceData data in JsonManager.GetInstance ().voiceJson.data) {
//				if (mVoiceDic.ContainsKey (data.id)) {
//					Debug.LogError ("mGameVoiceDic already has VoiceData , id : " + data.id);
//					continue;
//				}
//				if (mFlowerDic.ContainsKey (data.m_flower_id)) {
//					if (mFlowerDic [data.m_flower_id].voiceDic == null)
//						mFlowerDic [data.m_flower_id].voiceDic = new Dictionary<int, VoiceJson.VoiceData> ();
//					mFlowerDic [data.m_flower_id].voiceDic.Add (data.id, data);
//				}
//				mVoiceDic.Add (data.id, data);
//			}
//
//			mIllustrationDic = new Dictionary<int, IllustrationJson.IllustrationData> ();
//			foreach (IllustrationJson.IllustrationData data in JsonManager.GetInstance ().illustrationJson.data) {
//				if (mIllustrationDic.ContainsKey (data.id)) {
//					Debug.LogError ("mGameIllustrationDic already has IllustrationData , id : " + data.id);
//					continue;
//				}
//				if (mFlowerDic.ContainsKey (data.m_flower_id)) {
//					if (mFlowerDic [data.m_flower_id].illustrationDic == null)
//						mFlowerDic [data.m_flower_id].illustrationDic = new Dictionary<int, IllustrationJson.IllustrationData> ();
//					mFlowerDic [data.m_flower_id].illustrationDic.Add (data.id, data);
//				}
//				mIllustrationDic.Add (data.id, data);
//			}
//
//			mAnimationDic = new Dictionary<int, AnimationJson.AnimationData> ();
//			foreach (AnimationJson.AnimationData data in JsonManager.GetInstance ().animationJson.data) {
//				if (mAnimationDic.ContainsKey (data.id)) {
//					Debug.LogError ("mGameAnimationDic already has AnimationData , id : " + data.id);
//					continue;
//				}
//				if (mFlowerDic.ContainsKey (data.m_flower_id)) {
//					if (mFlowerDic [data.m_flower_id].animationDic == null)
//						mFlowerDic [data.m_flower_id].animationDic = new Dictionary<int, AnimationJson.AnimationData> ();
//					mFlowerDic [data.m_flower_id].animationDic.Add (data.id, data);
//				}
//				mAnimationDic.Add (data.id, data);
//			}
//		}
//
//		void InitItemData ()
//		{
//			Dictionary<int,ItemJson.ItemData> dic = JsonManager.GetInstance ().GetDictionary<ItemJson.ItemData> (JsonManager.GetInstance ().itemJson.data);
//			mItems = new Dictionary<int, ItemJson.ItemData> ();
//			if (dic.ContainsKey (ITEM_KOKOYU_ID))
//				mItems.Add (dic [ITEM_KOKOYU_ID].id, dic [ITEM_KOKOYU_ID]);
//			if (dic.ContainsKey (ITEM_KOKOTAN_ID))
//				mItems.Add (dic [ITEM_KOKOTAN_ID].id, dic [ITEM_KOKOTAN_ID]);
//			if (dic.ContainsKey (ITEM_HEART_ID))
//				mItems.Add (dic [ITEM_HEART_ID].id, dic [ITEM_HEART_ID]);
//			if (dic.ContainsKey (ITEM_MUCHIPOI_ID))
//				mItems.Add (dic [ITEM_MUCHIPOI_ID].id, dic [ITEM_MUCHIPOI_ID]);
//			if (dic.ContainsKey (ITEM_AKURA_ID))
//				mItems.Add (dic [ITEM_AKURA_ID].id, dic [ITEM_AKURA_ID]);
//			if (dic.ContainsKey (ITEM_EIYOU_ID))
//				mItems.Add (dic [ITEM_EIYOU_ID].id, dic [ITEM_EIYOU_ID]);
//
//
//			mCCNEKTLs = new Dictionary<int, ItemJson.ItemData> ();
//			if (dic.ContainsKey (ITEM_CCNEKTLGREEN_ID))
//				mCCNEKTLs.Add (dic [ITEM_CCNEKTLGREEN_ID].id, dic [ITEM_CCNEKTLGREEN_ID]);
//			if (dic.ContainsKey (ITEM_CCNEKTLBLUE_ID))
//				mCCNEKTLs.Add (dic [ITEM_CCNEKTLBLUE_ID].id, dic [ITEM_CCNEKTLBLUE_ID]);
//			if (dic.ContainsKey (ITEM_CCNEKTLRED_ID))
//				mCCNEKTLs.Add (dic [ITEM_CCNEKTLRED_ID].id, dic [ITEM_CCNEKTLRED_ID]);
//			if (dic.ContainsKey (ITEM_CCNEKTLBLACK_ID))
//				mCCNEKTLs.Add (dic [ITEM_CCNEKTLBLACK_ID].id, dic [ITEM_CCNEKTLBLACK_ID]);
//			if (dic.ContainsKey (ITEM_CCNEKTLWHITE_ID))
//				mCCNEKTLs.Add (dic [ITEM_CCNEKTLWHITE_ID].id, dic [ITEM_CCNEKTLWHITE_ID]);
//
//			//TODO
//			foreach (ItemJson.ItemData data in mItems.Values) {
//				data.num = 66;
//			}
//
//			//TODO
//			foreach (ItemJson.ItemData data in mCCNEKTLs.Values) {
//				data.num = 66;
//			}
//		}
//
//		//用户数据
//		void InitUserData ()
//		{
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.seed_list) {
//				if (mSeedDic.ContainsKey (data.id))
//					mSeedDic [data.id].get_at = data.get_at;
//			}
//
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.seeding_list) {
//				if (mSeedingDic.ContainsKey (data.id))
//					mSeedingDic [data.id].get_at = data.get_at;
//			}
//
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.flower_list) {
//				if (mFlowerDic.ContainsKey (data.id)) {
//					mFlowerDic [data.id].get_at = data.get_at;
//					mFlowerDic [data.id].num = data.num;
//					if (mOwnFlowerIds == null)
//						mOwnFlowerIds = new List<int> ();
//					mOwnFlowerIds.Add (data.id);
//				}
//			}
//
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.voice_list) {
//				if (mVoiceDic.ContainsKey (data.id)) {
//					mVoiceDic [data.id].get_at = data.get_at;
//					if (mFlowerDic.ContainsKey (mVoiceDic [data.id].m_flower_id)) {
//						if (mFlowerDic [mVoiceDic [data.id].m_flower_id].ownVoiceDic == null) {
//							mFlowerDic [mVoiceDic [data.id].m_flower_id].ownVoiceDic = new Dictionary<int, VoiceJson.VoiceData> ();
//						}
//						mFlowerDic [mVoiceDic [data.id].m_flower_id].ownVoiceDic.Add (data.id, mVoiceDic [data.id]);
//					}
//				}
//			}
//
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.illust_list) {
//				if (mIllustrationDic.ContainsKey (data.id)) {
//					mIllustrationDic [data.id].get_at = data.get_at;
//					if (mFlowerDic.ContainsKey (mIllustrationDic [data.id].m_flower_id)) {
//						if (mFlowerDic [mIllustrationDic [data.id].m_flower_id].ownIllustrationDic == null) {
//							mFlowerDic [mIllustrationDic [data.id].m_flower_id].ownIllustrationDic = new Dictionary<int, IllustrationJson.IllustrationData> ();
//						}
//						mFlowerDic [mIllustrationDic [data.id].m_flower_id].ownIllustrationDic.Add (data.id, mIllustrationDic [data.id]);
//					}
//				}
//			}
//
//			foreach (UserJson.UserData data in JsonManager.GetInstance().userJson.animation_list) {
//				if (mAnimationDic.ContainsKey (data.id)) {
//					mAnimationDic [data.id].get_at = data.get_at;
//					if (mFlowerDic.ContainsKey (mAnimationDic [data.id].m_flower_id)) {
//						if (mFlowerDic [mAnimationDic [data.id].m_flower_id].ownAnimationDic == null) {
//							mFlowerDic [mAnimationDic [data.id].m_flower_id].ownAnimationDic = new Dictionary<int, AnimationJson.AnimationData> ();
//						}
//						mFlowerDic [mAnimationDic [data.id].m_flower_id].ownAnimationDic.Add (data.id, mAnimationDic [data.id]);
//					}
//				}
//			}
//		}
//
//		//ココ湯
//		public ItemJson.ItemData item_kokoyu {
//			get {
//				if (mItems.ContainsKey (ITEM_KOKOYU_ID)) {
//					return mItems [ITEM_KOKOYU_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		//ココたん
//		public ItemJson.ItemData item_kokotan {
//			get {
//				if (mItems.ContainsKey (ITEM_KOKOTAN_ID)) {
//					return mItems [ITEM_KOKOTAN_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		//ハートグミ
//		public ItemJson.ItemData item_heart {
//			get {
//				if (mItems.ContainsKey (ITEM_HEART_ID)) {
//					return mItems [ITEM_HEART_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		//ムシポイ
//		public ItemJson.ItemData item_muchipoi {
//			get {
//				if (mItems.ContainsKey (ITEM_MUCHIPOI_ID)) {
//					return mItems [ITEM_MUCHIPOI_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		//快眠マクラ
//		public ItemJson.ItemData item_akura {
//			get {
//				if (mItems.ContainsKey (ITEM_AKURA_ID)) {
//					return mItems [ITEM_AKURA_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//		//栄養剤
//		public ItemJson.ItemData item_eiyou {
//			get {
//				if (mItems.ContainsKey (ITEM_EIYOU_ID)) {
//					return mItems [ITEM_EIYOU_ID];
//				} else {
//					return null;
//				}
//			}
//		}
//
//		public void AddCorm (int id, int num, int get_at)
//		{
//		
//		}
//
//		public void RemoveCorm (int id, int num)
//		{
//
//		}
//
//		public CormJson.CormData GetPlayerCorm (int id)
//		{
//			return null;
//		}
//
//		public void AddSeed (int id, int num)
//		{
//	
//		}
//
//		public void RemoveSeed (int id, int num)
//		{
//	
//		}
//
//		public SeedJson.SeedData GetPlayerSeed (int id)
//		{
//			return null;
//		}
//
//		public void AddSeeding (int id, int num)
//		{
//	
//		}
//
//		public void RemoveSeeding (int id, int num)
//		{
//	
//		}
//
//		public SeedingJson.SeedingData GetPlayerSeeding (int id)
//		{
//			return null;
//		}
//
//		public void AddFlower (int id, int num)
//		{
//	
//		}
//
//		public void RemoveFlower (int id, int num)
//		{
//	
//		}
//
//		public FlowerJson.FlowerData GetFlower (int id)
//		{
//			return null;
//		}

	}
}
