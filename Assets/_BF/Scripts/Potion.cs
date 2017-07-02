using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//[ExecuteInEditMode]
public class Potion : MonoBehaviour {

	public PotionAttr PotAttr;
	public UILabel PotionName;
	public UILabel PotionNumber;
	public UISprite PotionIcon;
	public UISprite PotionMask;
	public UIEventTrigger Trigger;

	static public Potion Current; 
	static public List<Potion> Potions = new List<Potion>();
	static public List<Hero> PotionAbleHeros = new List<Hero>();

	void Awake()
	{
		this.gameObject.SetActive(false);
		Potions.Add(this);
	}

	static public void Clear()
	{
		if(PotionAbleHeros.Count>0)PotionAbleHeros.Clear();
		if(Potions.Count>0)Potions.Clear();
		if(Current!=null)Current = null;
	}

	public void SetPotionAttr(PotionAttr attr)
	{
		if(attr == null || attr.Count == 0)
		{
			PotAttr = null;
			PotionName.text = "";
			PotionNumber.text = "";
			PotionIcon.spriteName = "";
			DisablePotion();
		}
		else
		{
			this.gameObject.SetActive(true);
			PotAttr = attr;
			PotionName.text = attr.PosName;
			PotionNumber.text = "X " + attr.Count;
			PotionIcon.spriteName = attr.Icon;
		}
	}

	public void Apply(Hero hero)
	{
		if(Potion.PotionAbleHeros.Contains(hero))
		{
			if(AudioManager.me!=null)AudioManager.me.PlayHCClip();
			int count = PotAttr.ImpactNum;
			_Apply(hero);
			count --;
			List<Hero> tmp = new List<Hero>();
			tmp.AddRange(PotionAbleHeros);
			for(int i = 0 ; i < tmp.Count ;i ++)
			{
				if(count<=0)
				{
					break;
				}
				count -- ;
				_Apply(tmp[i]);
			}
			AdjustCount(-1);
		}
	}

	void _Apply(Hero hero)
	{
		if(PotAttr.Type == _PotionType.Health)
		{
			Heal(hero);
		}
		else if(PotAttr.Type == _PotionType.Enhance)
		{
			Enhance(hero);
		}
		else if(PotAttr.Type == _PotionType.Rebirth)
		{
			Rebirth(hero);
		}

	}

//	void Heals(Hero hero)
//	{
//		AudioManager.SingleTon().PlayHCClip();
//		int count = PotAttr.ImpactNum;
//		Heal(hero);
//		count --;
//		foreach(Hero h in PotionAbleHeros)
//		{
//			Debug.Log(h);
//			if(count<=0)
//			{
//				break;
//			}
//			if(hero!=h)
//			{
//				Heal(h);
//			}
//		}
//		AdjustCount(-1);
//	}

	void Rebirth(Hero hero)
	{
		Heal(hero);
		BattleController.SingleTon().RightHeroes.Add(hero);
		BattleController.SingleTon().DeadHeroes.Remove(hero);
		hero.Btn.ActiveButton();
		hero.Btn.BtnText.gameObject.SetActive(false);
		BattleController.SingleTon().CurrentAttackers.Add(hero);
		hero.gameObject.SetActive(true);
		hero.Rebirth();

	}

	void Heal(Hero hero)
	{
		hero.OnHeal(BattleUtility.GetHealPotionPoint(hero,PotAttr));
		if(hero.heroAttribute.currentHP >= hero.heroAttribute.maxHP)
		{
			hero.Btn.HideFinger();
			PotionAbleHeros.Remove(hero);
		}
		BattleController.SingleTon().ShowBuffEffect(hero,Color.green);
	}

	void Enhance(Hero hero)
	{
		if(PotAttr.SubType == _SubPotionType.Attack)
		{
			hero.IsElementATKEnhanced = true;
			hero.ElementATKEnhancePercent = PotAttr.Power;
		}
		else if(PotAttr.SubType == _SubPotionType.Defence)
		{
			hero.IsElementDEFEnhanced = true;
			hero.ElementDEFEnhancePercent = PotAttr.Power;
		}
		if(PotAttr.ElementType == _ElementType.Fire)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,Color.red);
		}
		else if(PotAttr.ElementType == _ElementType.Water)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,Color.blue);
		}
		else if(PotAttr.ElementType == _ElementType.Wood)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,Color.green);
		}
		else if(PotAttr.ElementType == _ElementType.Wind)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,Color.yellow);
		}
		else if(PotAttr.ElementType == _ElementType.Holy)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,Color.white);
		}
		else if(PotAttr.ElementType == _ElementType.Evil)
		{
			BattleController.SingleTon().ShowBuffEffect(hero,new Color(0.5f,0,1));
		}
		hero.Btn.HideFinger();
		PotionAbleHeros.Remove(hero);
		hero.HeroBuff.AddSprite(PotAttr.BuffSpriteName);
	}

	static public bool IsEnable = true;
	static public void InActiveAll()
	{
//		Debug.Log("DisablePotions");
		if(IsEnable && Potions!=null)
		{
			foreach(Potion pot in Potions)
			{
				pot.DisablePotion();
			}
			IsEnable = false;
		}
	}

	static public void ActiveAll()
	{
		if(!IsEnable && Potions!=null)
		{
			foreach(Potion pot in Potions)
			{
				if(pot.PotAttr!= null && pot.PotAttr.Count > 0)
				{
					pot.EnablePotion();
				}
			}
			IsEnable = true;
		}
	}

	public void DisablePotion()
	{
		GetComponent<Collider>().enabled = false;
		PotionMask.gameObject.SetActive(true);
	}

	public void EnablePotion()
	{
		GetComponent<Collider>().enabled = true;
		PotionMask.gameObject.SetActive(false);
	}

	public void AdjustCount(int num)
	{
		PotAttr.Count += num;
		PotionNumber.text = "X" + PotAttr.Count;
		if(PotAttr.Count == 0)
		{
			DisablePotion();
			UnPotionSelect();
		}
	}

	void OnClick()
	{
		if(!BattleController.SingleTon().IsPotioning)
		{
			PotionAbleHeros = GetPotionAbleHeros();
			if(PotionAbleHeros.Count > 0)
			{
				Current = this;
				OnPotionSelect(this);
				ShowHints();
				AudioManager.me.PlayBtnActionClip();
			}
			else
			{
				AudioManager.me.PlayBtnCancelClip();
			}
		}
		else
		{
			if(Current == this)
			{
				UnPotionSelect();
			}
			AudioManager.me.PlayBtnActionClip();
			BattleController.SingleTon().IsPotioning = false;
			Current = null;
			HideHints();
		}
	}
	
	public Potion SelectedPotion;
	public void OnPotionSelect(Potion potion)
	{
		BattleController.SingleTon().IsPotioning = true;
		SelectedPotion = potion;
		foreach(Potion po in Potions)
		{
			if(po!=potion)
				po.DisablePotion();
		}
	}
	
	public void UnPotionSelect()
	{
		BattleController.SingleTon().IsPotioning = false;
		SelectedPotion = null;
		foreach(Potion po in Potions)
		{
			if(po.PotAttr!=null && po.PotAttr.Count > 0)
			{
				po.EnablePotion();
			}
		}
		foreach(Hero hero in BattleController.SingleTon().RightHeroes)
		{
			hero.Btn.HideFinger();
		}
	}

	List<Hero> GetPotionAbleHeros()
	{
		List<Hero> heros = new List<Hero>();
		List<Hero> checkList = null;
		if(PotAttr.Type == _PotionType.Rebirth)
		{
			checkList = BattleController.SingleTon().DeadHeroes;
		}
		else
		{
			checkList = BattleController.SingleTon().RightHeroes;
		}
		foreach(Hero hero in checkList)
		{
			if(!IsPotionAble(PotAttr,hero) )
			{
				continue;
			}
			heros.Add(hero);
		}
		return heros;
	}

	bool IsPotionAble(PotionAttr potionAttr,Hero hero)
	{
		bool potionAble = false;
		if(potionAttr.Type == _PotionType.Health)
		{
			if(hero.heroAttribute.currentHP < hero.heroAttribute.maxHP)
			{
				potionAble = true;
			}
		}
		else if(potionAttr.Type == _PotionType.Enhance)
		{
			if(potionAttr.SubType == _SubPotionType.Attack)
			{
				if(hero.heroAttribute.elementType == potionAttr.ElementType && !hero.IsElementATKEnhanced)
				{
					potionAble = true;
				}
			}
			else if(potionAttr.SubType == _SubPotionType.Defence)
			{
				if(hero.heroAttribute.elementType == potionAttr.ElementType && !hero.IsElementDEFEnhanced)
				{
					potionAble = true;
				}
			}
		}
		else if(potionAttr.Type == _PotionType.Remove)
		{
			if(potionAttr.SubType == _SubPotionType.Curse && hero.IsCursed)
			{
				potionAble = true;
			}
			else if(potionAttr.SubType == _SubPotionType.Poison && hero.IsPoisoned)
			{
				potionAble = true;
			}
		}
		else if(potionAttr.Type == _PotionType.Rebirth)
		{
			if(hero.IsDead)
			{
				potionAble = true;
			}
		}
		return potionAble;
	}

	void HideHints()
	{
//		List<GameObject> playerHeros = BattleController.SingleTon().RightHeroes;
		BattleController.SingleTon().IsPotioning = false;
		foreach(Hero hero in PotionAbleHeros)
		{
//			Hero hero = go.GetComponent<Hero>();
			hero.Btn.HideFinger();
			if(hero.heroAttribute.currentHP <= 0)
			{
				hero.Btn.InActiveButton();
			}
		}
	}

	void ShowHints()
	{
//		List<GameObject> playerHeros = BattleController.SingleTon().RightHeroes;
		BattleController.SingleTon().IsPotioning = true;
		foreach(Hero hero in PotionAbleHeros)
		{
			hero.Btn.ShowFinger();
			if(PotAttr.Type == _PotionType.Rebirth)
			{
				hero.Btn.ActiveButton();
			}

//			Hero hero = go.GetComponent<Hero>();
//			if(hero.heroAttribute.currentHP < hero.heroAttribute.maxHP)
//			{
//			}
		}
	}


	//	void Update()
	//	{
	//		GetParter();
	//	}

//	public bool IsGet;
//	void GetParter()
//	{
//		if(IsGet)
//		{
//			Trigger = GetComponent<UIEventTrigger>();
//			foreach(Transform trans in transform)
//			{
//				switch(trans.name)
//				{
//					case "LabelName":PotionName = trans.GetComponent<UILabel>();break;
//					case "LabelNumber":PotionNumber = trans.GetComponent<UILabel>();break;
//					case "SpriteIcon":PotionIcon = trans.GetComponent<UISprite>();break;
//					case "SpriteMask":PotionMask = trans.GetComponent<UISprite>();break;
//					default:break;
//				}
//			}
//		}
//		IsGet = false;
//	}

}
