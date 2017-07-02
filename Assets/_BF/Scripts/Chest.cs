using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DataCenter;

public class Chest : MonoBehaviour {

	static public string dropClipName = "Box_Drop";
	static public string dropMonsterClipName = "Box_DropGhost";
	static public string attackClipname = "Box_Attack";

	static public List<Chest> chests = new List<Chest>();
	public int location;
	public bool isOpened;
	public bool isOpening;
	public GameObject chestObj;
	public Transform chestCenter;
	public GameObject fingerGo;
	public Hero monster;
	public Animation anim;
	public BattleDropChest dropChest;
	public string orderLayerName;
	public bool isMonster;

	void Awake()
	{
		if(chests==null)
			chests = new List<Chest>();
		chestObj = gameObject;
		anim = GetComponentInChildren<Animation>();
	}

	static int lastFrame;
	void Update()
	{
		if(lastFrame!=Time.frameCount)
		{
			if(Input.GetMouseButtonDown(0))
			{
				if(BattleController.SingleTon().CurrentStatus == _AttackTurn.Waving)
				{
					Vector2 pos = BattleController.SingleTon().BattleCamera.ScreenToWorldPoint(Input.mousePosition);
					Collider2D coll = Physics2D.OverlapPoint(pos);
					if(coll!=null && coll.GetComponent<Chest>()!=null)
					{
						coll.GetComponent<Chest>().Open();
					}
				}
			}
			lastFrame = Time.frameCount;
		}
	}

	public bool IsFalling()
	{
		if(anim!=null && anim.IsPlaying(dropClipName) && anim[dropClipName].time < anim[dropClipName].length)
		{
			return true;
		}
		return false;
	}

	public void MonsterEnter()
	{
		if(monster!=null)
		{
			monster.gameObject.SetActive(true);
			monster.transform.position = transform.position;
		}
		else
		{
			if(dropChest!=null && dropChest.dropMonster!=null)
			{
				Debug.Log("(int)dropChest.dropMonster.typeid:" + (int)dropChest.dropMonster.typeid);
				//TODO temp code,need amend;
				bool contain = false;
				if((int)dropChest.dropMonster.typeid == 10062)
				{
					contain = true;
				}
				else if((int)dropChest.dropMonster.typeid == 10063)
				{
					contain = true;
				}
				else if((int)dropChest.dropMonster.typeid == 10064)
				{
					contain = true;
				}
				if(!contain)
				{
					Debug.LogError("dropChest.dropMonster.typeid:" + (int)dropChest.dropMonster.typeid + " not existing!");
				}
				int realHeroId = contain ? (int)dropChest.dropMonster.typeid : 10064;
				
				monster = SpawnUtility.InitTmpHero(_Side.Enemy,realHeroId,SpawnManager.SingleTon().battleHeroPrefab,location);
				if(monster!=null)
					monster.gameObject.SetActive(true);
			}
			else
			{
				Debug.LogError("dropChest:" + dropChest);
			}
		}

		if(monster!=null)
			BattleController.SingleTon().LeftHeroes.Add(monster);

		chests.Remove(this);
		gameObject.SetActive(false);
	}

	public void Drop()
	{
		Debug.Log("Drop items~!");
		GameObject prefab = null;
		int dropCount = 0;
		_DropType dropType = _DropType.Coin;
		switch(dropChest.type)
		{
			case CHEST_TYPE.CHEST_COIN:
				prefab = BattleController.SingleTon().prefabCoin;
				dropCount = 10;
				dropType = _DropType.Coin;
				break;
			case CHEST_TYPE.CHEST_SOUL:
				prefab = BattleController.SingleTon().prefabSoul;
				dropCount = 10;
				dropType = _DropType.Soul;
				break;
			case CHEST_TYPE.CHEST_ITEM:
				prefab=BattleController.SingleTon().prefabMaterial;
				dropCount = 1;
				dropType = _DropType.BattleMaterial;
				break;
			default:break;
		}
		Vector3 centerPos;
		Vector3 offset;
		Debug.Log("Drop prefab:" + prefab + ";dropCount:" + dropCount);
		for(int i = 0;i < dropCount;i ++)
		{
			centerPos = chestCenter == null ? transform.position : chestCenter.position;
			offset =  new Vector3(Random.Range(-2.0f,2.0f),Random.Range(-2.0f,0),0);
			Drop drop = PoolManager.SingleTon().Spawn(prefab,centerPos,Quaternion.identity).GetComponent<Drop>();
			drop.type = dropType;
			if(drop!=null)
				drop.ChangeOrderLayer(orderLayerName,(int)(transform.position.y * -100));
			drop.Val = (int)dropChest.value / dropCount;
			drop.OnSpawn(transform.position,offset);
			BattleController.SingleTon().Drops.Add(drop);
		}
	}

	public void Open()
	{
		if(!isOpened && !isOpening)
		{
			isOpening = true;
			BattleController.SingleTon().HideChestFinger(this);
			if(dropChest.type == CHEST_TYPE.CHEST_MOSTER)
			{
				StartCoroutine(_MonsterEnter());
			}
			else
			{
				StartCoroutine(_Drop());
			}
		}
	}

	IEnumerator _Drop()
	{
		float length = 0.1f;
		if(anim!=null && anim.GetClip(dropClipName)!=null)
		{
			anim.Play(dropClipName);
			length = anim[dropClipName].length;
		}
		yield return new WaitForSeconds(length);
		Drop();
		isOpened=true;
	}

	IEnumerator _MonsterEnter()
	{
		float length = 0.1f;
		if(anim!=null && anim.GetClip(dropMonsterClipName)!=null)
		{
			anim.Play(dropMonsterClipName);
			length = anim[dropMonsterClipName].length;
		}
		yield return new WaitForSeconds(length);
		MonsterEnter();
		isOpened=true;
	}

}

public class DropAttribute
{
	public _DropType type;
	public int total;
	public int count;
	public List<int> battleMaterialIds = new List<int>();
	public List<int> battleMaterialNums = new List<int>();
}


