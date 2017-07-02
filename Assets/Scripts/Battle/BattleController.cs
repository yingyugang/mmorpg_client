using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleController : MonoBehaviour {

	public BattleStatus status = BattleStatus.Begin;
	public BattlePerspective defaultPerspective = BattlePerspective.StayBehind;

	public SpawnManager spawnManager;
	public AudioManager audioManager;
	public CameraController cameraContoller;

	public PlayerController playerController;
	public EnemyController enemyController;

	public BaseAttribute playerAttribute;
	public BaseAttribute enemyAttribute;

	public bool showEnemy = true;
	public Transform playerSpawnPoint;
	public Transform enemySpawnPoint;
	public Vector3 playerSpawnPos;

	public Vector3 enemySpawnPos;
	public Vector3 enemySpawnEuler;

	public List<Collection> collections;
	public List<Gate> gates;

	public AnimationClip[] enemyDeathCameraAnimClips;
	public float[] enemyDeathAnimNormalizes;
	public AnimationClip[] playerDeathCameraAnimClips;
	public float[] playerDeathAnimNormalizes;
	public AnimationClip playerComingAnimClip;

	static BattleController instance;
	public static BattleController SingleTon()
	{
		return instance;
	}

	void Awake()
	{
		if (instance == null)
		{
			instance = this;		
		}

//		GameObject battleCamera = Resources.Load<GameObject> ("BattleCamera");
//		Instantiate (battleCamera);

		if (spawnManager == null)
		{
			spawnManager = gameObject.GetOrAddComponent<SpawnManager>();
		}
		if(audioManager == null)
		{
			audioManager = gameObject.GetOrAddComponent<AudioManager>();
		}
		if (cameraContoller == null) 
		{
			cameraContoller = gameObject.GetOrAddComponent<CameraController>();
		}


		if (playerSpawnPoint == null)
		{
			playerSpawnPoint = new GameObject().transform;
			playerSpawnPoint.position = playerSpawnPos;
		}

		if(enemySpawnPoint == null)
		{
			enemySpawnPoint = new GameObject().transform;
			enemySpawnPoint.position = enemySpawnPos;
			enemySpawnPoint.localEulerAngles = enemySpawnEuler;
		}

		if(playerController == null)
		{
			playerController = spawnManager.InitPlayer(playerSpawnPoint.position,playerSpawnPoint.rotation).GetComponent<PlayerController>();
			if(playerController!=null)playerAttribute = playerController.GetComponent<BaseAttribute>();
		}

		if(enemyController == null)
		{
			enemyController = spawnManager.InitEnemy(enemySpawnPoint.position,enemySpawnPoint.rotation).GetComponent<EnemyController>();
			if(enemyController!=null)enemyAttribute = enemyController.GetComponent<BaseAttribute>();
		}

		enemyController.playerAttr = playerAttribute;
		playerController.enemy = enemyController.gameObject;
		collections.Clear();
		collections.AddRange(GameObject.FindObjectsOfType<Collection> ());
		foreach(Collection col in collections)
		{
			col.playerAttr = playerAttribute;
		}
		gates.AddRange (GameObject.FindObjectsOfType<Gate>());
		if(Gate.currentNextGate!=null && gates.Count > 0)
		{
			Gate currentGate = gates[0];
			foreach(Gate gate in gates)
			{
				if(gate.gateName == Gate.currentNextGate)
				{
					currentGate = gate;
					break;
				}
			}
			playerController.transform.position = currentGate.spawnPoint.position;
			playerController.transform.rotation = currentGate.spawnPoint.rotation;
		}

		if (!showEnemy) 
		{
			enemyController.gameObject.SetActive(false);
		}
#if UNITY_STANDALONE_WIN
		Texture2D cursorTex = TextureUtility.CreateTexture2D (6,6,Color.green);
		Cursor.SetCursor (cursorTex,new Vector2(3,3),CursorMode.Auto);
#endif
	}

	void Start()
	{
		cameraContoller.cameraFollow.player = playerController.transform;
		cameraContoller.cameraFollow.target = enemyController.transform;
		cameraContoller.fixedCamera.player = playerController.transform;
		cameraContoller.mobileSimpleRpgCamera.target = playerController.transform;
		cameraContoller.TogglePerspective(defaultPerspective);
		StartCoroutine("_BossEnter");
	}

	public float maxClickDelay = 0.1f;
	float lastClickTime;
	Vector2 lastClickPos;
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.K))
		{
			StartCoroutine("_BossEnter");
		}
	}

	IEnumerator _BossEnter()
	{
		yield return new WaitForSeconds (1);
		cameraContoller.PlayBossComingAnimation (playerComingAnimClip,enemyController.transform,"Base Layer.005bellow");
	}

	public void EnemyDead()
	{
		if (enemyAttribute != null && enemyAttribute.gameObject.activeInHierarchy) {
			enemyAttribute.gameObject.SetActive (false);	
		}			
	}


	int mHOffset = 0;
	void OnGUI()
	{
//		if (status == BattleStatus.Win) 
//		{
//			GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 50, 50), "You Win!");
//		}
//		else if(status == BattleStatus.Fail)
//		{
//			GUI.Label (new Rect (Screen.width / 2, Screen.height / 2, 50, 50), "You Lost!");
//		}
//		if(status == BattleStatus.Win || status == BattleStatus.Fail)
//		{
//			if(GUI.Button(new Rect(10,10,100,30),"Restart!"))
//			{
//				Application.LoadLevel ("iphone5 wide");
//			}
//		}

		if(enemyController!=null && enemyController.pm.Fsm.ActiveStateName == "Dead")
		{
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/10/2,Screen.height/2,Screen.width/10,Screen.height/10),"Restart"))
			{
				Application.LoadLevel(0);
			}
		}

		if(playerController!=null && playerController.pm.Fsm.ActiveStateName == "Dead")
		{
			if(GUI.Button(new Rect(Screen.width/2 - Screen.width/10/2,Screen.height/2,Screen.width/10,Screen.height/10),"Restart"))
			{
				UnityEngine.SceneManagement.SceneManager.LoadScene("Login");
			}
		}
//		mHOffset = 1;
//		if(GUI.Button(new Rect(10,Screen.height - mHOffset * (Screen.height/10 + 10) ,Screen.width/10,Screen.height/10),"A"))
//		{
//			Application.LoadLevel("BattleA BackGround");
//		}
//		mHOffset ++;
//		if(GUI.Button(new Rect(10,Screen.height - mHOffset * (Screen.height/10 + 10),Screen.width/10,Screen.height/10),"B"))
//		{
//			Application.LoadLevel("BattleB BackGround");
//		}
//		mHOffset ++;
//		if(GUI.Button(new Rect(10,Screen.height - mHOffset * (Screen.height/10 + 10),Screen.width/10,Screen.height/10),"C"))
//		{
//			Application.LoadLevel("BattleC BackGround");
//		}
//		mHOffset ++;
//		if(GUI.Button(new Rect(10,Screen.height - mHOffset * (Screen.height/10 + 10),Screen.width/10,Screen.height/10),"E"))
//		{
//			Application.LoadLevel("BattleE BackGround");
//		}
//		mHOffset ++;
//		if(GUI.Button(new Rect(10,10,30,50),"Camera"))
//		{
//			CameraController.SingleTon().TogglePerspective();
//		}
	}

	void OnDrawGizmos()
	{
		if(playerSpawnPoint!=null)
		{
			Gizmos.color = Color.red;
			Gizmos.DrawSphere(playerSpawnPoint.position,0.5f);
		}
		if(enemySpawnPoint!=null)
		{
			Gizmos.color = Color.green;
			Gizmos.DrawSphere(enemySpawnPoint.position,0.5f);
		}
	}

	public void PlayEnemyDeathAnim()
	{
		cameraContoller.PlayDeathAnimation (enemyDeathCameraAnimClips,enemyDeathAnimNormalizes,enemyController.transform,"Base Layer.125dead");
	}

	public void PlayPlayerDeathAnim()
	{
		cameraContoller.PlayDeathAnimation (playerDeathCameraAnimClips,playerDeathAnimNormalizes,playerController.transform,"Base Layer.Dead");
	}

}
