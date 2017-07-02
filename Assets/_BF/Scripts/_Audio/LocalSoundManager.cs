using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace BoomBeach{
    public class LocalSoundManager : MonoBehaviour {

	    public float commonSpatialBlend = 1;
	    public int commonMaxDistance = 50;

		Dictionary<string,AudioClipInterval> mSoundsDic;
	   
	    public static LocalSoundManager SingleTon(){
		    return instance;
	    }
	    static LocalSoundManager instance;

	    void Awake()
	    {
		    instance = this;
		    playingSounds = new Dictionary<string, float> ();
			InitSounds ();
	    }

		void InitSounds(){
			mSoundsDic = new Dictionary<string, AudioClipInterval> ();
			mSoundsDic.Add("heavy_fire_01",InitSound("heavy_fire_01",0.1f)); //机枪兵射击声;
			mSoundsDic.Add("heavy_bullet_hit_01",InitSound("heavy_bullet_hit_01",0.1f)); //机枪兵子弹击中声;
			mSoundsDic.Add("coins_collect_02",InitSound("coins_collect_02",0.1f)); //采集金币;
			mSoundsDic.Add("gold_ding_01",InitSound("gold_ding_01",0.05f));	//金币到达;
			mSoundsDic.Add("wood_collect_06",InitSound("wood_collect_06",0.1f)); //木材采集;
			mSoundsDic.Add("wood_ding_01",InitSound("wood_ding_01",0.1f)); //木材到达;
			mSoundsDic.Add("stone_collect_02",InitSound("stone_collect_02",0.1f)); //石头采集;
			mSoundsDic.Add("rock_ding_01",InitSound("rock_ding_01",0.1f)); //石头到达;
			mSoundsDic.Add("metal_collect_05",InitSound("metal_collect_05",0.1f)); //金属采集;
			mSoundsDic.Add("metal_ding_01",InitSound("metal_ding_01",0.1f)); //金属到达;
			mSoundsDic.Add("collect_diamonds_02",InitSound("collect_diamonds_02",0.1f)); //宝石收集;
			mSoundsDic.Add("gem_ding_01",InitSound("gem_ding_01",0.1f)); //宝石到达;
			mSoundsDic.Add("loot_fly_in_01",InitSound("loot_fly_in_01",0.1f)); //战斗结束结果项飞行音效;
			mSoundsDic.Add("ammo_counter_01",InitSound("ammo_counter_01",0.1f));  //获得弹药;
			mSoundsDic.Add("building_destroyed_01",InitSound("building_destroyed_01",0.1f));//建筑爆炸;
			mSoundsDic.Add("heavy_die_05",InitSound("heavy_die_05",0.1f));//以下是机枪兵死亡;
			mSoundsDic.Add("tank_die_01",InitSound("tank_die_01",0.1f)); //坦克死亡;
			mSoundsDic.Add("tank_fire_01",InitSound("tank_fire_01",0.1f)); //坦克开火;
			mSoundsDic.Add("tank_hit_01",InitSound("tank_hit_01",0.1f)); //坦克击中;
			mSoundsDic.Add("assault_troop_shoot_01",InitSound("assault_troop_shoot_01",0.1f)); //步枪射击;
			mSoundsDic.Add("assault_troop_bullet_hit_01",InitSound("assault_troop_bullet_hit_01",0.1f)); //步枪兵击中;
			mSoundsDic.Add("assault_troop_die_04",InitSound("assault_troop_die_04",0.1f)); //步枪兵死亡;
			mSoundsDic.Add("bazooka_troop_fire_01",InitSound("bazooka_troop_fire_01",0.1f)); //导弹兵射击;
			mSoundsDic.Add("bazooka_hit_01",InitSound("bazooka_hit_01",0.1f));  //导弹兵击中;
			mSoundsDic.Add("bazooka_die_04",InitSound("bazooka_die_04",0.1f)); //导弹兵死亡;
			mSoundsDic.Add("native_attack_04",InitSound("native_attack_04",0.1f)); //战士攻击;
			mSoundsDic.Add("native_die_04",InitSound("native_die_04",0.1f)); //战士死亡;
			mSoundsDic.Add("missile_hit_01",InitSound("missile_hit_01",0.1f)); //导弹击中地面;
			mSoundsDic.Add("artillery_02",InitSound("artillery_02",0.1f));  //12发导弹的发射音效;
			mSoundsDic.Add("cannon",InitSound("cannon",0.1f)); //火炮与药水发射声;
			mSoundsDic.Add("machinegun_attack_01",InitSound("machinegun_attack_01",0.1f)); //机枪声;
			mSoundsDic.Add("rocket_launcher_fire_01",InitSound("rocket_launcher_fire_01",0.1f)); //火箭;
			mSoundsDic.Add("flame_thrower_01",InitSound("flame_thrower_01",0.1f)); //火焰喷射;
		}

		AudioClipInterval InitSound(string clipName,float interval){
			AudioClipInterval audioClip = new AudioClipInterval ();
			AudioClip clip = Resources.Load("Audio/sfx/"+clipName) as AudioClip;
			audioClip.clip = clip;
			if(clip==null){
				Debug.LogError (clipName);
			}
			audioClip.minInterval = interval;
			return audioClip;
		}

		public AudioClipInterval GetSound(string name){
			if(!mSoundsDic.ContainsKey(name)){
				return null;
			}
			return this.mSoundsDic [name];
		}


		//防止声音叠加
		Dictionary<string,float> playingSounds;
	    public float soundInterval = 0.1f;
	    public bool IsPlayable(string clipName)
	    {
			float soundInterval = this.soundInterval;
			if (mSoundsDic.ContainsKey(clipName)) {
				soundInterval = mSoundsDic [clipName].minInterval;		
			}
			if (playingSounds.ContainsKey (clipName)) {
				if (playingSounds [clipName] < Time.time) {
					playingSounds [clipName] = Time.time + soundInterval;
				    return true;
			    } else {
				    return false;
			    }
		    } else {
				playingSounds.Add(clipName,Time.time + soundInterval);
			    return true;
		    }
	    }
    }

    public class AudioClipInterval
    {
	    public AudioClip clip;
	    public float minInterval;
    }
}

