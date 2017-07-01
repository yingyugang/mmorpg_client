#pragma strict
#pragma implicit
#pragma downcast

//PUBLIC VARIABLES
var enableCaustics : boolean = true;
var causticRange : float = 50.0;
var maxCausticEffects : int = 30;
var causticObject : Transform;

var useTheseLayers : LayerMask = 0;
var causticFPS : int = 32;
var animationSpeed : float = 1.0;

var step = 40.0;
var causticFrames : Texture2D[];


//PUBLIC VARIABLES
public var useTex : Texture2D;


//PRIVATE VARIABLES
private var followObject : Transform;
private var useObject : Transform;
private var moduleObject : SuimonoModule;
private var frameIndex : int = 0;
private var currentSpeed : float = 1.0;
private var causticObjects : Transform[];

var savedPosition : Vector3;


function Start () {
	
	//get master objects
	moduleObject = GameObject.Find("SUIMONO_Module").GetComponent(SuimonoModule);
	
	//instantiate caustic object pool
	if (causticObject != null){
		causticObjects = new Transform[maxCausticEffects];
		for (var cx=0; cx < maxCausticEffects; cx++){
			var setPos : Vector3 = transform.position;
			setPos.y = -500.0;
			var causticObjectPrefab = Instantiate(causticObject, setPos, transform.rotation);
			causticObjectPrefab.transform.parent = this.transform;
			causticObjects[cx] = (causticObjectPrefab);
		}
	}
	
	//get the cureent follow object from module
	//if (moduleObject.setCamera != null){
		//useObject = moduleObject.setCamera;
	//} else {
		//useObject = Camera.mainCamera.transform;
	//}
	//if (useObject != null) savedPosition = useObject.transform.position;
	
	
	//set animation scheduler
	InvokeRepeating("CausticEffectUpdate", (1.0 / causticFPS), (1.0 / causticFPS)); 
	
}



function Update () {

	followObject = moduleObject.setTrack;

	animationSpeed = Mathf.Clamp(animationSpeed,0.001,3.0);

	//reset invoke
	if (currentSpeed != animationSpeed){
		CancelInvoke();
		InvokeRepeating("CausticEffectUpdate", 0.0, (1.0 / (causticFPS*animationSpeed)));
		currentSpeed=animationSpeed;
	}
	
	//get the cureent follow object from module
	if (followObject != null){
		useObject = followObject;
	} else {
		useObject = Camera.main.transform;
	}
	
	
	SetGridSpace();
	
}








function SetGridSpace(){	

	step = Mathf.Sqrt(causticRange) * (causticRange/maxCausticEffects);
	
	

	//var collsetpos : Vector3 = collsetpos = moduleSplashObject.setCamera.transform.position;
	//var layer : int = 4;
	//var layermask : int = 1 << layer;

			
	//reposition caustic objects from ppol
	if (useObject != null){
		var checkPos : Vector3 = Vector3(useObject.transform.position.x,0.0,useObject.transform.position.z);
		if (Vector3.Distance(savedPosition,checkPos) >= 5.0){
		
			savedPosition = checkPos;//Vector3(useObject.transform.position.x,0.0,useObject.transform.position.z);
			
			//-move lights into new positions
			//var row : int = 0;
			for (var xP : int = (savedPosition.x - causticRange); xP <= (savedPosition.x + causticRange); xP += step){
			for (var yP : int = (savedPosition.z - causticRange); yP <= (savedPosition.z + causticRange); yP += step){
			
			
			
				for (var lx : int = 0; lx < causticObjects.length; lx++){
				
				
				var lightPos : Vector3 = Vector3(causticObjects[lx].transform.position.x,0.0,causticObjects[lx].transform.position.z);
				var lightDist : float = Vector3.Distance(lightPos,checkPos);
				
				//var hasHit : boolean = true;
				/*
				var hits : RaycastHit[];
				var testpos : Vector3 = Vector3(lightPos.x,lightPos.y+5000.0,lightPos.z);
				
				hits = Physics.RaycastAll(testpos, -Vector3.up, 10000.0, layermask);
				for (var i = 0;i < hits.Length; i++) {
					var ht : RaycastHit = hits[i];
					if (ht.transform.gameObject.layer==4) hasHit = true;
				}
				*/
				
				
				//if (hasHit){

					if (lightDist > (causticRange*0.5)){
						causticObjects[lx].transform.localEulerAngles = Vector3(90.0,0.0,0.0);
						causticObjects[lx].gameObject.GetComponent(fx_causticObject).shiftTime = 3.0 + Random.Range(0.0,12.0);
						
						//check positions
						var posPass : boolean = true;
						var setPX = (Mathf.Round(xP/step))*step;
						var setPY = (Mathf.Round(yP/step))*step;
						for (var ly : int = 0; ly < causticObjects.length; ly++){
							if (causticObjects[ly].transform.position.x == setPX){
							if (causticObjects[ly].transform.position.z == setPY){
								posPass = false;
							}
							}
						}
						
						//set new position
						if (posPass){
							causticObjects[lx].transform.position.x = setPX;
							causticObjects[lx].transform.position.z = setPY;
							causticObjects[lx].GetComponent.<Light>().intensity = 0.0;
						}
						
						//row += 1;
						//xP += step;//10.0;
						//if (row >= 8){
							//row = 0;
							//xP = savedPosition.x - 50;
							//yP += step;//10.0;
						//}
						
					//} else {
					//if (lightDist >= 100.0){
						//causticObjects[lx].transform.position = Vector3(0,0,0);
					//}
					
					}
				
				
				//} else {
				//	causticObjects[lx].transform.position = Vector3(0,0,0);
				//	causticObjects[lx].light.intensity = 0.0;
				//}
				//}
				
				
				}
			
			}
			}
	
		}
	}
	
}












function CausticEffectUpdate() {
	
	if (this.enabled){
	if (animationSpeed > 0.0){
		
  		useTex = causticFrames[frameIndex];

		frameIndex += 1;
    	if (frameIndex == causticFrames.length) frameIndex = 0;

    }
    }
    
}



