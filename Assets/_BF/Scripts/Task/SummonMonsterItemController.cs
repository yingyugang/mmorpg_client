using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;
using BaseLib;

public class SummonMonsterItemController : MonoBehaviour {

	public List<GameObject> itemList;
	//public GameObject BtnLeft;
	//public GameObject BtnRight;

	//public GameObject BgUpdate;
	private bool init = false;
	private bool pressState;
	//private GameObject unOpen;
	private List<Vector3> posList;
	private List<int> posIndex;
	private UIPlayTween playTween;
	private TweenPosition tweenPos;
	private TweenScale tweenScale;
	private UISprite itemSprite;
	private UILabel itemLevel;
	private int selectedIndex;
	private GameObject currentItem;
	private bool press;
	private float mTime;
	private GameObject pressGameObject;
	private GameObject grid;
	private UIGrid summonGrid;

	private Hero summonMonster;
	private Hero selectedSummonMonster;

	public GameObject summonMonsterBtnPrefabs;

	private GameObject preSummonItem;
	private GameObject summonMonsterControllBtn;

	private bool Finished;
	private float offsetx;
	private List<float> itemPosList;
	private int currentIndex;
	public int selectedSummon;
	public string selectSummonName;
	//public GameObject summonMask;

	private GameObject battleBg;
	private UISprite battleBgSprite;

	private GameObject summonPanel;
	private GameObject Summon_Ready;
	private GameObject controllBtns;


	private GameObject LeviathanEffectA;
	private GameObject LeviathanEffectB;
	private GameObject LeviathanEffectC;
	private GameObject LeviathanEffectC2;
	private GameObject Leviathan_skill_abording;
	private GameObject SummonIceImpactGenerated;
	private GameObject SummonLeaving_Skill;
	private GameObject SummonLeaving_SkillRelease;
	private GameObject SummonScreenMask;
	private SpriteRenderer SummonScreensprite;

	void Start () {
		if(init)
			return;
		Finished = true;

		posIndex = new List<int>();
		posIndex.Add(0);
		posIndex.Add(1);
		posIndex.Add(2);
		posIndex.Add(3);
		posIndex.Add(4);

		posList = new List<Vector3>();
		posList.Add(new Vector3(-200,0,0));
		posList.Add(new Vector3(-100,0,0));
		posList.Add(new Vector3(0,0,0));
		posList.Add(new Vector3(100,0,0));
		posList.Add(new Vector3(200,0,0));
		
		UIEventListener.Get(this.gameObject).onDrag = onItemDrag;
		//UIEventListener.Get(this.gameObject).onPress = onItemPress;
		//UIEventListener.Get(this.gameObject).onClick = onSummonMonster;
		//unOpen = GameObject.Find ("Bg/BgUpdate/unOpen");
		init = true;
		selectedIndex = 2;
		offsetx = 0;
		itemPosList = new List<float>();

		selectedSummon = -1;
		summonMonsterControllBtn = UI.PanelStack.me.FindPanel("BattleUI/summonMonsterControllBtn");

		battleBg = UI.PanelStack.me.FindPanel("BattleUI/Bg");
		battleBgSprite = battleBg.GetComponent<UISprite>();

		for (int i = 0; i < itemList.Count; i ++)
		{
			/*TweenPosition tp = itemList[i].GetComponent<TweenPosition>();
			if (tp)
			{
				EventDelegate finish = new EventDelegate(onFinish);
				tp.onFinished.Add(finish);
			}*/

			//UISprite sprite = itemList[i].GetComponent<UISprite>();
			//BoxCollider boxcollider = itemList[i].AddComponent<BoxCollider>();
			//boxcollider.size = new Vector3(sprite.width,sprite.height,1);
			//boxcollider.enabled = false;
			//UIEventListener.Get(itemList[i]).onClick = onSummonMonster;
			//UIEventListener.Get(itemList[i]).onPress = onItemPress;
			if (i == 2)
				currentItem = itemList[i];
		}


		/*UI.PanelStack.me.root = this.gameObject;
		grid = UI.PanelStack.me.FindPanel("scrowView/summonGrid") as GameObject;
		summonGrid = grid.GetComponent<UIGrid>();*/

		//onGetSummonMonsterList();
	}

	void OnEnable()
	{
		//Start ();
	}

	void Update () {
		if (press)
		{
			mTime += Time.deltaTime;
			if (mTime > 0.5f)
			{
				onGetPressGameObject();
			}
		}
	}


	void setLeftPosition(int i)
	{
	
		//Debug.Log("setLeftPosition");
		//UIEventListener.Get(itemList[i]).onClick = null;
		//BoxCollider boxcollider = itemList[i].GetComponent<BoxCollider>();
		//boxcollider.enabled = false;
		tweenPos = itemList[i].GetComponent<TweenPosition>();
		playTween = itemList[i].GetComponent<UIPlayTween>();
		itemSprite = itemList[i].GetComponent<UISprite>();
		itemLevel = itemList[i].transform.Find("Level").GetComponent<UILabel>();
		tweenPos.duration = 0.5f;
		tweenPos.from = tweenPos.transform.localPosition;
		int currindex = posIndex[i];
		if (currindex == 0)
		{
			posIndex[i] = itemList.Count -1;
			tweenPos.to = posList[itemList.Count -1];
			itemSprite.depth = 12;
			itemLevel.depth = 12;
		}
		else
		{
			posIndex[i] = currindex - 1;
			tweenPos.to = posList[currindex - 1];
			itemSprite.depth = 13;
			itemLevel.depth = 14;
		}

		tweenScale = itemList[i].GetComponent<TweenScale>();
		tweenScale.duration = 0.5f;
		if (posIndex[i] == 0)
		{
			tweenScale.from = new Vector3(0.9f,0.9f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);			
		}
		else if (posIndex[i] == 1)
		{
			tweenScale.from = new Vector3(1,1,1);
			tweenScale.to = new Vector3(0.9f,0.9f,1);
			
		}
		else if (posIndex[i] == 2)
		{
			tweenScale.from = new Vector3(0.9f,0.9f,1);
			tweenScale.to = new Vector3(1,1,1);
			selectedIndex = i;
			itemSprite.depth = 20;

			//boxcollider.enabled = true;

			currentItem = itemList[i];

		}
		else if (posIndex[i] == 3)
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.9f,0.9f,1);
			
		}
		else if (posIndex[i] == 4)
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);
			
		}
		else
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);
		}
		
		playTween.Play(true);
	}

	void setRightPosition(int i)
	{

		//BoxCollider boxcollider = itemList[i].GetComponent<BoxCollider>();
		//boxcollider.enabled = false;
		tweenPos = itemList[i].GetComponent<TweenPosition>();
		playTween = itemList[i].GetComponent<UIPlayTween>();
		itemSprite = itemList[i].GetComponent<UISprite>();
		itemLevel = itemList[i].transform.Find("Level").GetComponent<UILabel>();
		tweenPos.from = tweenPos.transform.localPosition;
		tweenPos.duration = 0.5f;
		int currindex = posIndex[i];
		if (currindex == itemList.Count -1)
		{
			posIndex[i] = 0;
			tweenPos.to = posList[0];
			itemSprite.depth = 12;
			itemLevel.depth = 12;
		}
		else
		{
			posIndex[i] = currindex + 1;
			tweenPos.to = posList[currindex + 1];
			itemSprite.depth = 13;
			itemLevel.depth = 14;
		}
		
		tweenScale = itemList[i].GetComponent<TweenScale>();
		tweenScale.duration = 0.5f;

		if (posIndex[i] == 0)
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);
			
		}
		else if (posIndex[i] == 1)
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.9f,0.9f,1);
			
		}
		else if (posIndex[i] == 2)
		{
			tweenScale.from = new Vector3(0.9f,0.9f,1);
			tweenScale.to = new Vector3(1,1,1);
			selectedIndex = i;
			itemSprite.depth = 20;
			currentItem = itemList[i];

			//boxcollider.enabled = true;
		}
		else if (posIndex[i] == 3)
		{
			tweenScale.from = new Vector3(1,1,1);
			tweenScale.to = new Vector3(0.9f,0.9f,1);
			
		}
		else if (posIndex[i] == 4)
		{
			tweenScale.from = new Vector3(0.9f,0.9f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);
			
		}
		else
		{
			tweenScale.from = new Vector3(0.8f,0.8f,1);
			tweenScale.to = new Vector3(0.8f,0.8f,1);
		}

		playTween.Play(true);
	}

	void onItemDrag(GameObject go ,Vector2 delta)
	{
		if (delta.x > 20)
			onRightClick(go);
		else if (delta.x < -20)
			onLeftClick(go);
		
		//Debug.Log(delta);
	}

	void onLeftClick(GameObject go)
	{
		if (!Finished) 
			return;
		
		Finished = false;
		for (int i = 0; i < itemList.Count; i ++)
		{
			setLeftPosition(i);
		}
	}
	
	void onRightClick(GameObject go)
	{
		if (!Finished) 
			return;
		
		Finished = false;
		for (int i = itemList.Count -1; i >= 0 ; i --)
		{
			setRightPosition(i);
		}
	}

	void onItemPress(GameObject go,bool state)
	{
		Debug.Log("onItemPress.................................." + state);
		press = state;
		pressGameObject = go;
		//Debug.Log(state);
	}

	void onFinish()
	{
		Finished = true;
	}

	void onSummonMonster(GameObject go)
	{
		//if (go == currentItem)
		//{
			//SpawnManager.SingleTon().SpawnForSummonMonster();
			//this.gameObject.SetActive(false);
			//summonMonsterControllBtn.SetActive(true);		

		for (int i = 0; i < itemList.Count; i ++)
		{
			if (itemList[i] != go)
				itemList[i].SetActive(false);
		}
		

		//}
	}

	void onGetPressGameObject()
	{
		if (pressGameObject == currentItem)
			Debug.Log("onSummonMonster.........................." + pressGameObject.name);
	}

		
	public void onGetSummonMonsterList()
	{
		if (battleBgSprite == null)
		{
			battleBg = UI.PanelStack.me.FindPanel("BattleUI/Bg");
			battleBgSprite = battleBg.GetComponent<UISprite>();
			battleBgSprite.spriteName = "FgtSummonBarBg1";
		}

		if (summonPanel == null)
		{
			summonPanel = UI.PanelStack.me.FindPanel("BattleUI/Bg/summonPanel");
		}
		summonPanel.SetActive(false);

		if (Summon_Ready == null)
		{
			Summon_Ready = UI.PanelStack.me.FindPanel("BattleUI/Bg/summonBtn/Summon_Ready");
		}
		Summon_Ready.SetActive(true);

		
		BattleController.SingleTon().summonMask.transform.localPosition = new Vector3(0,0,0);
		setAutoFightBtnState(false);
		//BattleController.SingleTon().summonMask.SetActive(true);
		
	
		int j = 0;
		itemList.Clear();
		preSummonItem = null;
		for (int i = 0; i < SpawnManager.SingleTon().summonMonsters.Count; i ++)
		{

			//if (BattleController.SingleTon().RightHeroes[i].isSummonMonster == true)
			//{
				summonMonster = SpawnManager.SingleTon().summonMonsters[i];

				//summonMonster.HeroBody.gameObject.SetActive(true);
				/*if (j < itemList.Count && itemList[j] != null)
				{
					itemList[j].GetComponent<HeroBtn>().hero = summonMonster;
					summonMonster.SetControllBtn(itemList[j].GetComponent<HeroBtn>());
				}
				j += 1;*/




				GameObject item = getSummonItem();
				item.GetComponent<HeroBtn>().hero = summonMonster;
				item.GetComponent<HeroBtn>().summonItemController = this;
				item.GetComponent<HeroBtn>().summonIndex = i;
				summonMonster.SetControllBtn(item.GetComponent<HeroBtn>());

				TweenAlpha tp = item.GetComponent<TweenAlpha>();
				EventDelegate.Add(tp.onFinished,onItemFinished);
				itemList.Add(item);	
		

			//}
		}


		if (controllBtns == null)
			controllBtns = UI.PanelStack.me.FindPanel("BattleUI/ControllBtns");

		TweenPosition controlltp = controllBtns.GetComponent<TweenPosition>();
		controlltp.from = new Vector3(0,controlltp.from.y,controlltp.from.z);
		controlltp.to = new Vector3(-1,controlltp.from.y,controlltp.from.z);
		controlltp.ResetToBeginning();
		controlltp.PlayForward();


		TweenPosition summontp = this.gameObject.GetComponent<TweenPosition>();
		summontp.from = new Vector3(1,summontp.from.y,summontp.from.z);
		summontp.to = new Vector3(0,summontp.from.y,summontp.from.z);
		summontp.ResetToBeginning();
		summontp.PlayForward();
		//summontp.PlayForward();


		loadSummonEnterEffect();

	}

	GameObject getSummonItem()
	{
		GameObject item = Instantiate(summonMonsterBtnPrefabs) as GameObject;
		item.transform.parent = this.transform;
		item.transform.localScale = new Vector3(0.009f,0.0024f,0.007f);
		if (preSummonItem == null)
		{
			item.transform.localPosition = new Vector3(-0.32f,-0.37f,0);
		}
		else
		{
			item.transform.localPosition = new Vector3(preSummonItem.transform.localPosition.x + 0.32f,-0.37f,0);
		}
		preSummonItem = item;
		//UIEventListener.Get(item).onClick = onSummonMonsterItemClick;
		return item;
	}


	public void scrollSummonItem(int offset)
	{
		if ((itemList.Count - this.transform.localPosition.x / 0.32f) < 2)
			return;


		if (itemList.Count > 3 && Mathf.Abs(offset) > 200)
		{
			Vector3 pos = this.transform.localPosition;

			if (offset < 1)
			{			
				pos = new Vector3(this.transform.localPosition.x - 0.32f * offset / 20,this.transform.localPosition.y,this.transform.localPosition.z);
			}
			else
				pos = new Vector3(this.transform.localPosition.x + 0.32f * offset / 20,this.transform.localPosition.y,this.transform.localPosition.z);

			StartCoroutine(scrollSummonList(pos));

		}
	}

	IEnumerator scrollSummonList(Vector3 pos)
	{
		float t = 0;
		while (t < 0.5f)
		{
			t += Time.deltaTime;
			this.transform.localPosition = Vector3.Lerp(this.transform.localPosition,pos,t);
			yield return null;	
		}
	}

	void onSummonMonsterItemClick(GameObject go)
	{
		onItemFinished();
	}

	public void setItemPosition(int index)
	{
		offsetx = 0;
		if (index > 1)
			offsetx = -0.32f;
		else 
		if (index < 1)
			offsetx = 0.32f;
						
		currentIndex = index;
		selectedSummon = currentIndex;

		itemPosList.Clear();

		for (int i = 0; i < itemList.Count; i ++)
		{
			if (i == index)
			{
				selectedSummonMonster = SpawnManager.SingleTon().summonMonsters[currentIndex];
				selectSummonName = selectedSummonMonster.name;

				UISprite[] sprites = itemList[i].transform.GetComponentsInChildren<UISprite>();
				for (int j = 0; j < sprites.Length; j ++)
				{
					sprites[j].depth += 10;
				}

				GameObject CardClickPrefab = Resources.Load("Effect/UI_Effect/CardClick") as GameObject;
				if (CardClickPrefab != null)
				{
					GameObject CardClick = GameObject.Instantiate(CardClickPrefab) as GameObject;
					CardClick.transform.parent = itemList[i].transform;
					CardClick.transform.localPosition = Vector3.zero;
					CardClick.transform.localScale = new Vector3(10,47,10);					
					CardClick.SetActive(true);
				}
			

				TweenPosition tp = itemList[i].GetComponent<TweenPosition>();
				tp.from = itemList[i].transform.localPosition;
				tp.to = new Vector3(itemList[i].transform.localPosition.x + offsetx,itemList[i].transform.localPosition.y,itemList[i].transform.localPosition.z);
				tp.Play();	

				TweenScale ts = itemList[i].GetComponent<TweenScale>();
				ts.AddOnFinished(onItemScaleFinished);
				StartCoroutine(setSummonScaleEffect(ts));

				//TweenRotation tr = itemList[i].GetComponent<TweenRotation>();
				//StartCoroutine(setSummonRotationEffect(tr));

			}
		}

		if (Summon_Ready == null)
		{
			Summon_Ready = UI.PanelStack.me.FindPanel("BattleUI/Bg/summonBtn/Summon_Ready");
		}
		Summon_Ready.SetActive(false);


		if (summonPanel == null)
		{
			summonPanel = UI.PanelStack.me.FindPanel("BattleUI/Bg/summonPanel");
		}
		summonPanel.SetActive(true);
				
		//setAutoFightBtnState(true);

	}

	IEnumerator setSummonScaleEffect(TweenScale ts)
	{
		ts.PlayForward();	
		yield return new WaitForSeconds(0.5f);
		
		for (int i = 0; i < itemList.Count; i ++)
		{
			if (i != currentIndex)
			{		
				//itemList[i].SetActive(false);
				DestroyObject(itemList[i]);
				GameObject CardDisppearPrefab = Resources.Load("Effect/UI_Effect/CardDisppear") as GameObject;
				if (CardDisppearPrefab != null)
				{
					GameObject CardDisppear = GameObject.Instantiate(CardDisppearPrefab) as GameObject;
					CardDisppear.transform.parent = itemList[i].transform.parent;
					CardDisppear.transform.localPosition = itemList[i].transform.localPosition;
					CardDisppear.transform.localScale = new Vector3(0.06f,0.06f,10f);					
					CardDisppear.SetActive(true);
				}			
			}
		}
		
		yield return new WaitForSeconds(1f);
		ts.PlayReverse();

		StartCoroutine(BattleController.SingleTon().PlayersEnterOrExit(false));
		yield return new WaitForSeconds(2f);


		for (int i = 0; i < BattleController.SingleTon().RightHeroes.Count; i ++)
		{
			//先添加召唤兽，再修改状态，以免战斗状态切换为“敌人攻击”
			if (BattleController.SingleTon().RightHeroes[i].isSummonMonster == false)
				BattleController.SingleTon().RightHeroes[i].gameObject.SetActive(false);
		}

		//yield return new WaitForSeconds(1.5f);
		float t = 0;
		//GameObject SummonScreenMask = UI.PanelStack.me.FindPanel("BattleUI/SummonScreenMask");
		//SpriteRenderer sprite = SummonScreenMask.GetComponent<SpriteRenderer>();
		Color color = SummonScreensprite.color;
		color.a = 0;
		SummonScreensprite.color = color;
		SummonScreenMask.SetActive(true);
		while (t < 1)
		{
			t += Time.deltaTime * 3;
			color.a = t;
			SummonScreensprite.color = color;
			//sprite.alpha = t;
			yield return null;
		}
		SummonScreenMask.SetActive(false);

		//setSummonScreenMask();


		StartCoroutine(playSummonEnterEffect());
		
	}

	public IEnumerator setSummonScreenMask()
	{
		float t = 0;
		Color color = SummonScreensprite.color;
		color.a = 0;
		SummonScreensprite.color = color;
		SummonScreenMask.transform.localPosition = Vector3.zero;
		SummonScreenMask.SetActive(true);
		while (t < 1)
		{
			t += Time.deltaTime * 3;
			color.a = t;
			SummonScreensprite.color = color;
			//sprite.alpha = t;
			yield return null;
		}
		SummonScreenMask.SetActive(false);
	}

	IEnumerator setSummonRotationEffect(TweenRotation tr)
	{
		tr.PlayForward();
		yield return new WaitForSeconds(0.5f);
		tr.PlayReverse();
	}

	void onItemScaleFinished()
	{
		GameObject CardDropdownPrefab = Resources.Load("Effect/UI_Effect/CardDropdown") as GameObject;
		if (CardDropdownPrefab != null)
		{
			GameObject CardDropdown = GameObject.Instantiate(CardDropdownPrefab) as GameObject;
			CardDropdown.transform.parent = itemList[currentIndex].transform;
			CardDropdown.transform.localPosition = Vector3.zero;
			CardDropdown.transform.localScale = new Vector3(15,25,10);					
			CardDropdown.SetActive(true);
		}
	}
	
	public void DestorySummonItem()
	{
		for (int i = itemList.Count-1; i >= 0; i--)
		{
			if (currentIndex != i)
				Destroy(itemList[i]);
		}
	}

	public void setAutoFightBtnState(bool state)
	{
		BattleController.SingleTon().DisableAutoBtn.gameObject.SetActive(state);
		BattleController.SingleTon().EnableAutoBtn.gameObject.SetActive(state);
	}


	void onItemFinished()
	{
		//summonMask.SetActive(true);
		if (TweenPosition.current.gameObject != null)
		{		
			if (TweenPosition.current.gameObject.GetComponent<HeroBtn>().summonIndex != currentIndex)
			{
				TweenPosition.current.gameObject.SetActive(false);
			}
		}
		
	}

	void loadSummonEnterEffect()
	{
		LeviathanEffectA = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/A"));
		LeviathanEffectA.transform.position = new Vector3(100,100,0);
		LeviathanEffectA.SetActive(false);

		LeviathanEffectB = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/B"));
		LeviathanEffectB.transform.position = new Vector3(200,100,0);
		LeviathanEffectB.SetActive(false);

		LeviathanEffectC = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/C"));
		LeviathanEffectC.transform.position = new Vector3(300,100,0);
		LeviathanEffectC.SetActive(false);

		LeviathanEffectC2 = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/C2"));
		LeviathanEffectC2.transform.position = new Vector3(400,100,0);
		LeviathanEffectC2.SetActive(false);

		Leviathan_skill_abording = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/Leviathan_skill_abording"));
		Leviathan_skill_abording.SetActive(false);
		Leviathan_skill_abording.transform.localScale = Vector3.one;
		Leviathan_skill_abording.transform.localPosition = new Vector3(15.430f,8.104f,10);


		SummonIceImpactGenerated = (GameObject)Instantiate(Resources.Load("Effect/Hero_Effect2/Leviathan/SummonIceImpactGenerated"));
		SummonIceImpactGenerated.SetActive(false);
		SummonIceImpactGenerated.transform.localScale = Vector3.one;
		SummonIceImpactGenerated.transform.localPosition = new Vector3(-7.097f,-3.685f,10);


		SummonLeaving_Skill = (GameObject)Instantiate(Resources.Load("Effect/Common_Effect2/SummonLeaving_Skill"));
		SummonLeaving_Skill.SetActive(false);
		SummonLeaving_Skill.transform.localScale = Vector3.one;
		SummonLeaving_Skill.transform.localPosition = new Vector3(6.65f,-0.79f,10);

		SummonLeaving_SkillRelease = (GameObject)Instantiate(Resources.Load("Effect/Common_Effect2/SummonLeaving_SkillRelease"));
		SummonLeaving_SkillRelease.SetActive(false);
		SummonLeaving_SkillRelease.transform.localScale = Vector3.one;
		SummonLeaving_SkillRelease.transform.localPosition = new Vector3(6.65f,-0.79f,10);


		SummonScreenMask = UI.PanelStack.me.FindPanel("BattleUI/SummonScreenMask");
		SummonScreensprite = SummonScreenMask.GetComponent<SpriteRenderer>();

	}

	IEnumerator playSummonEnterEffect()
	{

		//入场表现
		LeviathanEffectA.SetActive(true);
		yield return new WaitForSeconds(4.9f);
		LeviathanEffectB.SetActive(true);
		yield return new WaitForSeconds(0.2f);
		LeviathanEffectA.SetActive(false);
		

		yield return new WaitForSeconds(0.85f);
		LeviathanEffectC.SetActive(true);
		yield return new WaitForSeconds(0.4f);
		LeviathanEffectB.SetActive(false);		

		//yield return new WaitForSeconds(1.4f);
		BattleController.SingleTon().summonMask.transform.localPosition = new Vector3(100,0,0);
		BattleController.SingleTon().SummonBG.transform.localPosition = new Vector3(0,0,0);		
		
		//LeviathanEffectC2.SetActive(true);
		yield return new WaitForSeconds(0.5f);		
		LeviathanEffectC2.SetActive(true);
		yield return new WaitForSeconds(0.6f);		
		LeviathanEffectC.SetActive(false);
		
		yield return new WaitForSeconds(1.4f);
		LeviathanEffectC2.SetActive(false);

		yield return new WaitForSeconds(0.2f);
		SummonIceImpactGenerated.SetActive(true);
		yield return new WaitForSeconds(0.1f);
		Leviathan_skill_abording.SetActive(true);
		//yield return new WaitForSeconds(0.1f);
		//yield return new WaitForSeconds(1f);

	

		EnterSkillEffect();
		Transform players = GameObject.Find("Players").transform;
		for (int i = players.childCount -1; i >=0; i--)
		{
			//if (players.GetChild(i).GetComponent<Hero>().isSummonMonster && i != currentIndex && players.GetChild(i).GetComponent<Hero>().name != selectSummonName)
			if (players.GetChild(i).GetComponent<Hero>().isSummonMonster && players.GetChild(i).GetComponent<Hero>().name != selectSummonName)
				DestroyObject(players.GetChild(i).gameObject);
				
		}
				

		//yield return new WaitForSeconds(0.5f);
		float t = 0;
		Vector3 to = new Vector3(11f,-6f,selectedSummonMonster.gameObject.transform.localPosition.z);
		while (t < 1)
		{
			t += Time.deltaTime / 40;
			//summonMonster.gameObject.transform.localPosition = Vector3.Lerp(15f,summonMonster.gameObject.transform.localPosition.y,summonMonster.gameObject.transform.localPosition.z);
			if (selectedSummonMonster != null && selectedSummonMonster.gameObject != null)
				selectedSummonMonster.gameObject.transform.localPosition = Vector3.Lerp(selectedSummonMonster.gameObject.transform.localPosition,to,t);
			else
				t = 1;

			yield return null;
		}


	}


	void EnterSkillEffect()
	{
		//入场伤害
		List<AttackAttribute> attackAttributes = selectedSummonMonster.heroAttack.GetAttackAttributes();
		if (attackAttributes.Count > 0)
		{
			selectedSummonMonster.heroAttack.GetHitTargets(attackAttributes[0]);
			for(int j =0;j < attackAttributes[0].attackTargets.Count;j++)
			{
				attackAttributes[0].attackTargets[j].heroAttribute.Hit(selectedSummonMonster,attackAttributes[0],0);
			}
		}

		BattleController.SingleTon().isSummoning = false;
		setAutoFightBtnState(true);

	}

	public IEnumerator ExitSkillEffect()
	{
		SummonLeaving_Skill.SetActive(true);	
		yield return null;
	}

	public IEnumerator ExitSkillEffect2()
	{	
		yield return new WaitForSeconds(1f);
		SummonLeaving_Skill.SetActive(false);	
		SummonLeaving_SkillRelease.SetActive(true);
		yield return new WaitForSeconds(0.5f);
		foreach(Hero hero in BattleController.SingleTon().RightHeroes)
		{
			GameObject HealingEffect1 = (GameObject)Instantiate(Resources.Load("Effect/Common_Effect2/HealingEffect1"));
			HealingEffect1.transform.parent = hero.gameObject.transform;
			HealingEffect1.transform.localScale = Vector3.one;
			HealingEffect1.transform.localPosition = Vector3.zero;
			HealingEffect1.SetActive(true);		
		}


		foreach(Hero hero in BattleController.SingleTon().RightHeroes)
		{
			hero.heroAttribute.currentHP = Mathf.FloorToInt(hero.heroAttribute.currentHP +  hero.heroAttribute.currentHP * 0.1f);
			int value = Mathf.FloorToInt(100);
			hero.Btn.RequireUpdate = true;			
			BattleUtility.ShowEnhaceEffect(hero,value);
		}

		yield return null;
	}


}
