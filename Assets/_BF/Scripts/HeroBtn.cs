using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Hero button.the controller for player.
/// </summary>
[ExecuteInEditMode]
public class HeroBtn : MonoBehaviour {

	//[HideInInspector]
	public Hero hero;
	static public string[] HPBarSprites = new string[]{"FightUIHeroHpGreen","FightUIHeroHpYellow","FightUIHeroHpRed"};

	public UISlider HealthProgressBar;
	public UISlider EnergyProgressBar;
	public UISprite BtnHead;
	public UISprite BtnMask;
	public UISprite BtnThunderMask;
	public UILabel BtnText;
	public UILabel BtnHealthText;
	public UILabel BtnHeroName;
	public UISprite BtnFinger;
	public UISprite BtnType;
	public UISprite BtnHintUp;
	public UISprite BtnHintDown;
	public UISprite BtnStar;
	public UISprite BtnDeath;
	public Collider2D BtnCollider2D;
	public GameObject linerEffect;
	public bool Load;

	public BattleController Controller;
	static List<HeroBtn> mHeroBtns = new List<HeroBtn>();
	static List<HeroBtn> mHoveredHeroBtns = new List<HeroBtn>();
	static HeroBtn mCurrent;
	static int mFrame;

	Transform mTrans;
	public bool RequireUpdate;
	private GameObject energy;
	public GameObject skillFrame;
	public SummonMonsterItemController summonItemController;
	public int summonIndex;


	void Awake()
	{
		mHeroBtns.Add(this);
		mTrans = transform;
	}

	static public void Clear()
	{
		if(mHeroBtns!=null)mHeroBtns.Clear();
		if(mHoveredHeroBtns!=null)mHoveredHeroBtns.Clear();
		if(mCurrent!=null)mCurrent=null;
	}

	void Start()
	{
		GetController();
	}

	public void ShowLinerEffect()
	{
		if(linerEffect!=null)
		{
			linerEffect.SetActive(false);
			linerEffect.SetActive(true);
		}
	}

	BattleController GetController()
	{
		if(Controller==null)
			Controller = BattleController.SingleTon();
		return Controller;
	}

	static public void InActiveAll()
	{
		for(int i = 0;i < mHeroBtns.Count;i ++)
		{
			mHeroBtns[i].InActiveButton();
		}
	}

	static public void ActiveAll()
	{
		Debug.Log("ActiveAll");
		for(int i = 0;i < mHeroBtns.Count;i ++)
		{
			if(mHeroBtns[i].hero != null)
			{
				mHeroBtns[i].ActiveButton();
			}
		}
	}

	float mPressedDuration;
	float SkillHintStrikeDuration = 0.5f;
	void Update()
	{
#if UNITY_EDITOR
		if(Load && !Application.isPlaying)
		{
			LoadSerializeField();
		}
#endif

		if(mIsPress && BattleUtility.IsAttackable() && hero.heroAttribute.currentEnergy >= hero.heroAttribute.maxEnergy)
		{
			mPressedDuration += Time.deltaTime;
			if(mPressedDuration > SkillHintStrikeDuration)
			{
//				Controller.ShowSkillHint(mTrans.position,true);
			}
		}else{
			mPressedDuration = 0;
		}

		if(mFrame!=Time.frameCount)
		{
			mFrame = Time.frameCount;
			for(int i = 0 ; i < mHeroBtns.Count ; i ++)
			{
				if(mHeroBtns[i].RequireUpdate)
				{
					mHeroBtns[i].UpdateSelf();
				}
			}
		}
	}

	public void ShakeHead()
	{
		iTween.ShakePosition(BtnHead.gameObject,BattleUtility.BtnShakeOffset,0.5f);
	}

	public void OnBtnClick()
	{

		//if (hero.gameObject.activeSelf == false)
		//if (hero.isSummonMonster)

		if (summonItemController && summonItemController.selectedSummon == -1)
		{
			//hero.gameObject.SetActive(true);
			//hero.HeroBody.gameObject.SetActive(true);

			//StartCoroutine(BattleController.SingleTon().ExtraEffectForHeroEnter(hero,BattleController.SingleTon().RightPoints[hero.Index].position));
			//hero.Btn = this;

			BattleController.SingleTon().isSummoning = true;
			_HeroStatus status = hero.Status;
			hero.Status = _HeroStatus.BeforeTurn;
			hero.gameObject.transform.localPosition = new Vector3(40,-10f,hero.gameObject.transform.localPosition.z);
			hero.gameObject.transform.localScale = new Vector3(1f,1f,hero.gameObject.transform.localScale.z);
			hero.heroRes.CenterOffset = new Vector3(-7,9,0);
			hero.gameObject.SetActive(true);
			BattleController.SingleTon().RightHeroes.Add(hero);
			BattleController.SingleTon().RightHeroes[BattleController.SingleTon().RightHeroes.Count -1].isSummonMonster = true;

			/*for (int i = 0; i < BattleController.SingleTon().RightHeroes.Count; i ++)
			{
				//先添加召唤兽，再修改状态，以免战斗状态切换为“敌人攻击”
				if (BattleController.SingleTon().RightHeroes[i].isSummonMonster == false)
					BattleController.SingleTon().RightHeroes[i].gameObject.SetActive(false);
			}*/

			if (summonItemController != null)
				summonItemController.setItemPosition(summonIndex);
			return;
		}

		if (BattleController.SingleTon().isSummoning)
			return;

		Debug.Log("OnBtnClick................................................................" + this.gameObject.name);
		if(Controller.IsPotioning)
		{
			Potion.Current.Apply(hero);
		}
		else if(BattleUtility.IsAttackable())
		{
			if(AudioManager.me!=null)
				AudioManager.me.PlayBtnActionClip();
			if(hero!=null)
			{
				hero.heroAttack.Attack(0);
			}
		}
	}

	Vector3 mStartMousePos;
	bool mIsPress;
	public void OnBtnPress()
	{
//		Debug.Log("OnBtnPress");
		if (hero.gameObject.activeSelf == false)
			return;
		if(!BattleUtility.IsAttackable())return;
		mIsPress = true;
		mStartMousePos = Input.mousePosition;
		mCurrent = this;
	}

	public void OnBtnHoverOver()
	{
//		Debug.Log("OnBtnHoverOver" + mCurrent);
		if(mCurrent != null)
		{
			if(!mHoveredHeroBtns.Contains(this))
				mHoveredHeroBtns.Add(this);
		}
	}
//
//	IEnumerator _LineAttack()
//	{
//		for(int i =0; i < mHoveredHeroBtns.Count;i++)
//		{
//			mHoveredHeroBtns[i].hero.heroAttack.Attack();
//			yield return new WaitForSeconds(1);
//		}
//	}
//	IEnumerator _LineAttack(List<HeroBtn> heroBtns)
//	{
//		foreach(HeroBtn heroBtn in heroBtns)
//		{
//			heroBtn.hero.heroAttack.Attack();
//			yield return null;
//		}
//	}

	public void OnBtnRelease()
	{	
		if (hero.gameObject.activeSelf == false || BattleController.SingleTon().isGlobalSkill)
			return;
		if(!BattleUtility.IsAttackable())return;
		Vector3 endMousePos = Input.mousePosition;
		float xOffset = Mathf.Abs(endMousePos.x - mStartMousePos.x);
		float yOffset = Mathf.Abs(endMousePos.y - mStartMousePos.y);
		Vector2 startPos = Controller.UICamera.ScreenToWorldPoint(mStartMousePos);
		Vector2 endPos = Controller.UICamera.ScreenToWorldPoint(endMousePos);
		RaycastHit2D[] hits = Physics2D.LinecastAll(startPos,endPos);
		Debug.Log(hits.Length);
		HeroBtn btn;
		foreach(RaycastHit2D hit in hits)
		{
			if(hit.transform.parent!=null)
			{
				btn = hit.transform.parent.GetComponent<HeroBtn>();
				if(btn!=null && !mHoveredHeroBtns.Contains(btn))
				{
					mHoveredHeroBtns.Add(btn);
				}
			}
		}

		if(xOffset > yOffset)
		{
			if(mHoveredHeroBtns.Count>1)
			{
//				bool skillAble = endMousePos.x - mStartMousePos.x > 0 ? false : true;
//				StartCoroutine(AttackQueue(mHoveredHeroBtns,skillAble));
			}
		}
		else
		{
			yOffset = endMousePos.y - mStartMousePos.y;
			if(yOffset > 10)
			{
				//勇气值达到最大，且没有被诅咒
				if(hero.heroAttribute.currentEnergy >= hero.heroAttribute.maxEnergy && hero.heroAttribute.isCurse == false)
				{
					StartCoroutine(SkillAttack());
				}
				else
				{
					hero.heroAttack.Attack();
				}
			}
			else if(yOffset<-10)
			{
				if(!hero.heroAttack.isDefense)
				{
					hero.heroAttack.Defense();
				}
			}
		}
//		Controller.ShowSkillHint(transform.position,false);
		mHoveredHeroBtns.Clear();
		mIsPress = false;
	}

	IEnumerator SkillAttack()
	{
		List<Hero> heros = new List<Hero>();
		heros.Add(hero);

		Controller.GlobalSkill.Play(heros);

		while(Time.timeScale==0)
		{
			yield return null;
		}
		hero.heroAttack.Attack(1);
	}

	public void InitData(Hero h)
	{
		hero = h;
		if(BtnHeroName!=null)BtnHeroName.text = hero.HeroName;
//		Debug.Log("BtnType:" + BtnType);
//		Debug.Log("Controller.TypeSprite:" + GetController().TypeSprite);
		if(BtnType!=null)BtnType.spriteName = BattleUtility.GetElementBigSpriteName(hero.heroAttribute.elementType);
		if(BtnStar!=null)BtnStar.spriteName = BattleUtility.GetStarSpriteName(hero.heroAttribute.star);
		//BtnHead.atlas = UI.PanelTools.GetUIAtlas("heroTemp");;
		if(BtnHead!=null)
		{
			if(BtnHead.atlas.GetSprite(hero.heroAttribute.spriteName) == null)
			{
				BtnHead.spriteName = AssetBundleMgr.defaultPrefab;
			}
			else
			{
				BtnHead.spriteName = hero.heroAttribute.spriteName;
			}
			BtnHead.gameObject.SetActive(true);
		}
		RequireUpdate = true;
	}

	public void UpdateSelf()
	{
		if(BtnText!=null && !BtnText.gameObject.activeInHierarchy && hero.heroAttribute.currentHP==0)
		{
			BtnText.gameObject.SetActive(true);
			if (skillFrame)
				skillFrame.SetActive(false);
		}
		if(BtnHealthText != null)
			BtnHealthText.text = hero.heroAttribute.currentHP > hero.heroAttribute.maxHP? hero.heroAttribute.maxHP.ToString(): hero.heroAttribute.currentHP.ToString();
		if(Controller!=null && Controller.IsPotioning && hero.heroAttribute.currentHP >= hero.heroAttribute.maxHP && BtnFinger.gameObject.activeInHierarchy)
		{
			HideFinger();//移动到Hero里面。解耦
		}
		AdjustEnergyBar();
		AdjustHealthBar();
		RequireUpdate = false;
	}
	
	void AdjustEnergyBar()
	{
		float rate = (float)hero.heroAttribute.currentEnergy / (float)hero.heroAttribute.maxEnergy;
		if(EnergyProgressBar!=null)
			EnergyProgressBar.value = rate;

		if (rate >= 1 && hero.heroAttribute.currentHP > 0)
		{
			/*(if (!energy)
			{
				GameObject energyEffectPrefab =  Resources.Load("Effect/Effect_HeroBG_lightting") as GameObject;
				energy = Instantiate(energyEffectPrefab,Vector3.zero,Quaternion.identity) as GameObject;
				energy.transform.parent = transform;
			}*/
			if (skillFrame)
				skillFrame.SetActive(true);
		}
		else
		{
			//energy.SetActive(false);
			if (skillFrame)
				skillFrame.SetActive(false);
		}
		
	}

	void AdjustHealthBar()
	{
		float rate = (float)hero.heroAttribute.currentHP / (float)hero.heroAttribute.maxHP;
		if(HealthProgressBar!=null)
		{
			HealthProgressBar.value = rate;
//			if(HealthProgressBar.value<0.5f)
//			{
//				HealthProgressBar.foregroundWidget.GetComponent<UISprite>().spriteName = HPBarSprites[2];
//			}
//			else if(HealthProgressBar.value<1.0f)
//			{
//				HealthProgressBar.foregroundWidget.GetComponent<UISprite>().spriteName = HPBarSprites[1];
//			}
//			else
//			{
//				HealthProgressBar.foregroundWidget.GetComponent<UISprite>().spriteName = HPBarSprites[0];
//			}
		}
	}

	public void InActiveButton()
	{
		if(BtnCollider2D!=null)BtnCollider2D.enabled = false;
		if(GetComponent<Collider>()!=null)GetComponent<Collider>().enabled = false;
		if(BtnMask!=null)BtnMask.gameObject.SetActive(true);
	}
	
	public void ActiveButton()
	{
		if(BtnCollider2D!=null)BtnCollider2D.enabled = true;
		if(GetComponent<Collider>()!=null)GetComponent<Collider>().enabled = true;
		if(BtnMask!=null)BtnMask.gameObject.SetActive(false);
	}

	public void ShowFinger()
	{
		if(!BtnFinger.gameObject.activeInHierarchy)BtnFinger.gameObject.SetActive(true);
	}
	
	public void HideFinger()
	{
		if(BtnFinger.gameObject.activeInHierarchy)BtnFinger.gameObject.SetActive(false);
	}

	public void ShowHintDown()
	{
		BtnHintUp.gameObject.SetActive(false);
		BtnHintDown.gameObject.SetActive(true);
	}

	public void HideHintDown()
	{
		BtnHintDown.gameObject.SetActive(false);
	}

	public void ShowHintUp()
	{
		BtnHintUp.gameObject.SetActive(true);
		BtnHintDown.gameObject.SetActive(false);
	}

	public void HideHintUp()
	{
		BtnHintUp.gameObject.SetActive(false);
	}

	public void ShowDeathMask()
	{
		if(BtnDeath!=null)
		{
			BtnDeath.gameObject.SetActive(true);
			UITweener tw = BtnDeath.GetComponent<UITweener>();
			tw.ResetToBeginning();
			tw.PlayForward();
		}
	}

	void LoadSerializeField()
	{
		UIEventTrigger evTrigger = GetComponent<UIEventTrigger>();
		EventDelegate characterEvent;

		characterEvent = new EventDelegate(OnBtnClick);
		evTrigger.onClick.Clear();
		evTrigger.onClick.Add(characterEvent);

		evTrigger.onPress.Clear();
		characterEvent = new EventDelegate(OnBtnPress);
		evTrigger.onPress.Add(characterEvent);

		evTrigger.onDragOver.Clear();
		characterEvent = new EventDelegate(OnBtnHoverOver);
		evTrigger.onDragOver.Add(characterEvent);

		evTrigger.onRelease.Clear();
		characterEvent = new EventDelegate(OnBtnRelease);
		evTrigger.onRelease.Add(characterEvent);

		HealthProgressBar = transform.FindChild("BarHealth").GetComponent<UISlider>();
		EnergyProgressBar = transform.FindChild("BarEnergy").GetComponent<UISlider>();
		BtnHead = transform.FindChild("Head").GetComponent<UISprite>();
		BtnMask = transform.FindChild("Mask").GetComponent<UISprite>();
		BtnHeroName = transform.FindChild("Name").GetComponent<UILabel>();
		BtnText = transform.FindChild("Dead").GetComponent<UILabel>();
		BtnHealthText = transform.FindChild("HealthText").GetComponent<UILabel>();
		BtnThunderMask = transform.FindChild("Thunder").GetComponent<UISprite>();
		BtnFinger = transform.FindChild("TouchFinger").GetComponent<UISprite>();
		BtnType = transform.FindChild("TypeIcon").GetComponent<UISprite>();
		BtnHintUp = transform.FindChild("SpriteRefRainUp").GetComponent<UISprite>();
		BtnHintDown =  transform.FindChild("SpriteRefRainDown").GetComponent<UISprite>();

		Load = false;
	}

}
