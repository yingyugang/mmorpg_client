// ### PUBLIC VARIABLES ####


var PlayerObject : Transform;


//camera variables
var camHeight = 6.0;
var camDistance = 9.5;
var camLookHeight = 5.0;
var camLat = 0.0;
var camRotation = 0.0;
var rotationDelay=1.0;
var zoomFactor = 0.0;
var forcePerspective = true;

//movement variables
var walkSpeed = 8.0;//4.0; 
var trotSpeed = 5.0; 
var runSpeed = 12.0; 
var rotSpeed = 5.0;


static var gamemode : String = "game";


// ### PRIVATE VARIABLES ####
private var target_distance = 0;
private var maxcamHeight = 16;
private var maxcamDistance = 20;
private var maxcamLookHeight = 4.5;
private var maxcamRotation = -74;
private var maxrotationDelay = 1.0;

//private var GameMode = "play";
private var savegamemode = "play";
private var Movement_Mode: String = "still";
private var Command_Mode: String = "active";

private var camCloseDistance = 0.0;
private var camSetDistance = 0.0;
private var camSpeed = 3.0;
private var camBlur = 0.0;
private var camTrgt = 0.0;

private var isMap = false;
private var isRagdoll = false;
private var isFreeLook = false;
private var isMoving = false;
private var isMovingBack = false;
private var isRun = false;
private var isSprinting = false;
private var isCrouching = false;
private var isWalking = false;
private var isSideWalking = false;
private var isJumping = false;
private var isJumpModifier = false;
private var isControllable = true;
private var isLooking = false;
private var isGrounded = true;

private var isWeap_Drawing = false;
private var isWeap_Sheathing = false;


private var controlUseKey = false;

private var isUnderwater = false;
private var isInWater = false;
private var waterDepth = 0.0;

private var forwardDistance = 0.0;
private var setforwardDistance = 0.0;
private var sidewaysDistance = 0.0;
private var MouseRotationDistance = 0.0;
private var MouseVerticalDistance = 0.0;

private var startPoint = Vector3;
private var startTime = 0.0;

private var setDistance = 0.0;
private var setRotation = 0.0;

private var look: Vector3;
private var distMod : float = 0.0;
private var rotmod : float = 1.0;
private var leanmod : float =0.0;

private var camoffset : Vector3 = Vector3(0,0,0);
private var recKey = false;

private var lookTargetHeight = 4.5;
private var rsThreshhold = 11;
private var playrotate = 1.0;
private var setangle : float = 180.0;
private var defcamLat = 2.0;
private var lastJumpButtonTime : float = 0.0;
private var jumpfactor = 0.5;

private var playerHeight = 0.0;


private var weapIsDrawn : boolean = false;

private var verticalSpeed = 0.0;
private var prevVerticalSpeed = 0.0;
private var verticalFallTime = 0.0;


var camfovbase :float = 19.36957;
var camfov : float = camfovbase;
var camfovspeed : float = 2.0;








function Update() {


var cameraTransform = transform;



// ## GET INPUTS ##
isControllable = true;
controlUseKey = false;
recKey = false;
Movement_Mode = "still";
Command_Mode = "passive";
isRun=false;
isSprinting=false;
isWalking=false;
isCrouching = false;
isSideWalking=false;
isMoving=false;
isLooking=false;

isWeap_Drawing = false;

forwardDistance=0.0;
sidewaysDistance=0.0;



isGrounded=true;



//calculate vertical Speed
if (!isGrounded){
verticalFallTime += Time.deltaTime;
verticalSpeed = (PlayerObject.transform.position.y - prevVerticalSpeed);
prevVerticalSpeed = PlayerObject.transform.position.y;
} else {
verticalFallTime = 0.0;
}





if (gamemode =="menu"){
	isControllable = false;
	controlUseKey = true;
	Screen.lockCursor = false;
	defcamLat = 2.0;
	
} else if (gamemode == "charactercreation"){
	isControllable = false;
	controlUseKey = true;
	Screen.lockCursor = false;
	lookTargetHeight = 3.0;
	defcamLat = 0.0;

} else {
	Screen.lockCursor = false;
	lookTargetHeight = 4.5;
	defcamLat = 2.0;
}




if (Input.GetButtonUp("Fire1")){
	if (gamemode !="menu"){
		savegamemode = gamemode;
		gamemode = "menu";
	} else {
		gamemode = savegamemode;
	}
}
		
		

if (isControllable){

		if (Input.GetButton ("Vertical")){ //if (Input.GetButton ("Move Forward")){
				if (isMovingBack) {
					rotmod=4.0;
				} else {
					rotmod = 1.0;
				}
				Movement_Mode = "walk";
				isWalking=true;
				isMovingBack=false;
				forwardDistance = Input.GetAxisRaw("Vertical");
				
				//if(forwardDistance < 0.0){
					//Movement_Mode = "walk";
					//isWalking=true;
					//isMovingBack=true;
				//}
				
		}
		if (Input.GetButton ("Horizontal")){ //if (Input.GetButton ("Move Sideways")){
				if (isMovingBack) {
					rotmod=4.0;
				} else {
					rotmod = 1.0;
				}
				Movement_Mode = "walk";
				isWalking=true;
				//isRun = false;
				isSideWalking = true;
				//isMovingBack=false;
				sidewaysDistance = Input.GetAxisRaw("Horizontal");
				forwardDistance = Mathf.Abs(sidewaysDistance);
		}
		
		//if (Input.GetButton ("Move Backward")){
				//if (!isMovingBack){
				//	rotmod=4.0;
				//} else {
				//	rotmod = 1.0;
				//}
				//Movement_Mode = "walk";
				//isWalking=true;
				//isMovingBack=true;
				//forwardDistance = 0-Input.GetAxisRaw("Move Backward");
		//}
		
		

}
		//zoomFactor += Input.GetAxisRaw("Mouse ScrollWheel");
		//if (zoomFactor < 0.0 ) zoomFactor = 0.0;
		//if (zoomFactor > 4.5 ) zoomFactor = 4.5;

		//if (Input.GetButton ("Active")){
		//	Command_Mode = "active";
			//recKey = true;
		//	if (controlUseKey){
		//		recKey = true;
		//	}

		//}




		//if (Movement_Mode=="walk" && Command_Mode=="active"){
			//isSprinting = true;
		//	Movement_Mode = "run";
		//	isRun=true;
		//	isWalking = false;
		//}





	
	
	isCrouching = false;
	

	if (forwardDistance != 0.0){
		isMoving = true;
		forwardDistance = forwardDistance / 12.0;
		if (isRun){
			//if (isJumpModifier){
			//	isSprinting = true;
			//	isJumping = false;
			//}
			//if (isSprinting){
			//	forwardDistance = forwardDistance*40.0;	
			//} else {
			//	forwardDistance = forwardDistance*12.0;
			//}
		} else if (isWalking) {
			forwardDistance = forwardDistance*6.2;
			//if (isJumpModifier){
			//	isCrouching = true;
				//isJumping = false;
			//}
			//if (isCrouching){
			//	forwardDistance = forwardDistance/2.0;
			//}
		}
	}




	isUnderwater = false;
	isInWater = false;
	if (waterDepth > 0.0) {
		var waterspeedMod = 1.0;
		
		if (waterDepth >= 0.25) waterspeedMod = 0.85;
		if (waterDepth >= 1.0) waterspeedMod = 0.75;
		if (waterDepth >= 2.0) waterspeedMod = 0.65;
		if (waterDepth >= 3.0)waterspeedMod = 0.5;

		if (waterDepth >= 0.25) isInWater = true;

		if (waterDepth >= 4.0){
			waterspeedMod = 0.5;
			isUnderwater = true;
			//if (this.transform.position.y >=-11.2) this.transform.position.y=-11.2;
		}
		
		forwardDistance = forwardDistance * waterspeedMod;
		
		//Debug.Log(waterDepth+" , "+camHeight);
		//if (waterDepth >= 1.0) waterspeedMod = 0.5;
		
	}
	

//}


if (isControllable){

	MouseRotationDistance = Input.GetAxisRaw("Mouse X");
	MouseRotationDistance = MouseRotationDistance*rotationDelay;
	MouseVerticalDistance = -Input.GetAxisRaw("Mouse Y");
	MouseVerticalDistance = MouseVerticalDistance*rotationDelay;
		
} else {
	if (recKey){
		MouseRotationDistance = Input.GetAxisRaw("Mouse X");
		MouseVerticalDistance = Input.GetAxisRaw("Mouse Y");
		//MouseVerticalDistance = MouseVerticalDistance;
	} else {
		MouseRotationDistance = 0.0;
		MouseVerticalDistance = 0.0;
		MouseVerticalDistance = 0.0;
	}
} 


//} else {

	//MouseRotationDistance = 0.0;
	//MouseVerticalDistance = 0.0;
	//MouseVerticalDistance = 0.0;

//}

















//### SET MOVEMENT ###
if (Movement_Mode=="still" || isRagdoll) setforwardDistance = 0.0;

if (Movement_Mode!="still" || isLooking ){
	var savex : float = PlayerObject.rotation.x;
	var savez : float = PlayerObject.rotation.z;
	

	var setrot : Vector3 = PlayerObject.eulerAngles;
	var targrot : Vector3 = cameraTransform.eulerAngles;
	var setangles : Vector3;
	var angdist : float = 0.0;
	var setmove : float = 0.0;
	
	
	//rotation
	if (isMovingBack) targrot.y = targrot.y-180.0;
	angdist = targrot.y-setrot.y;
	if (Mathf.Abs(angdist) <=300 ){
		setangles = Vector3.Lerp(setrot,targrot,(rotSpeed*rotmod)*Time.smoothDeltaTime); //rotSpeed*rotmod
	} else {
		setangles = targrot;
	}
	
	setangles.x = savex;
	setangles.z = savez;
	
	setangles.y = cameraTransform.eulerAngles.y+camLat;
	
	
	if (isSideWalking){
		if (Input.GetAxisRaw("Vertical") !=0.0 ){
			setangles.y += (sidewaysDistance*3.0);
			//cameraTransform.eulerAngles.y += (sidewaysDistance*3.0);
			forwardDistance = (forwardDistance*0.7);
		} else {
			setangles.y += (sidewaysDistance*7.0);
			//cameraTransform.eulerAngles.y += (sidewaysDistance*3.0);
			forwardDistance = (forwardDistance*0.5);
		}
	}
	
	
	var rotvalue = 0.0;
	if (isMovingBack) rotvalue = 180.0;
		
	//if (sidewaysDistance !=0.0){
	if (isSideWalking){
	var rangle : float = 35.0;
	
	
		if (sidewaysDistance >0.0){
			playrotate = Mathf.Lerp(playrotate,(rangle+(forwardDistance*camLat*8)),3.5*Time.smoothDeltaTime); //5
			//setangles.y += playrotate;
			//setangles.y += 45+(forwardDistance*camLat*12);
		} else {
			//playrotate = (rangle+(forwardDistance*camLat*12));
			playrotate = Mathf.Lerp(playrotate,rotvalue-(rangle+(forwardDistance*camLat*6)),3.5*Time.smoothDeltaTime); //5
			//setangles.y -= playrotate;
			//setangles.y -= 45+(forwardDistance*(camLat*24));
		}
	}else {
		//playrotate = 0.0;
		playrotate = Mathf.Lerp(playrotate,rotvalue,6.0*Time.smoothDeltaTime);
		//setangles.y = Mathf.Lerp(setangles.y,0.0,2.0*Time.smoothDeltaTime);
	}

	setangles.y += playrotate;
	//setangles.y = playrotate;
	
	//lean
	if (isRun){
		var leanamt : float = angdist;
		
		if (leanamt>16) leanamt = 16.0;
		if (leanamt<-16) leanamt = -16.0;


		if (sidewaysDistance!=0.0){
			if (leanamt>6) leanamt = 6.0;
			if (leanamt<-6) leanamt = -6.0;
			leanmod = Mathf.Lerp(leanmod,rotvalue+leanamt,4.0*Time.smoothDeltaTime);
		} else {
			leanmod = Mathf.Lerp(leanmod,rotvalue-leanamt,4.0*Time.smoothDeltaTime);
		}

	} else {
		leanmod = 0.0;
	}
	


	forwardDistance = (forwardDistance*50)*Time.smoothDeltaTime;
	
	if (!isGrounded){
		jumpfactor = Mathf.Lerp(jumpfactor,-0.5,5.0*Time.deltaTime); //set gravity force to pull caracter down
	} else {
		jumpfactor = Mathf.Lerp(jumpfactor,0.0,2.0*Time.deltaTime);
	}
	
	
	if (isJumping && isGrounded && !isSprinting && !isCrouching){

	} else {

	}
	
	

	if (isMovingBack){
		cameraTransform.eulerAngles.x = 0.0;
		PlayerObject.eulerAngles = setangles;
		forwardDistance = 0.0 - forwardDistance;
		PlayerObject.Translate(Vector3(sidewaysDistance*0.5,jumpfactor,-1) * forwardDistance, cameraTransform);

	} else {
		setforwardDistance = Mathf.Lerp(setforwardDistance, forwardDistance, 2.5*Time.smoothDeltaTime);
		cameraTransform.eulerAngles.x = 0.0;
		PlayerObject.eulerAngles = setangles;
		PlayerObject.Translate(Vector3(sidewaysDistance*0.5,jumpfactor,1) * setforwardDistance, cameraTransform);
	}

}


if (isJumping){
	isMoving = true;
	if (isGrounded){
		var jumpamt = 40;
		if (isWalking) jumpamt = 65;
		if (isRun) jumpamt = 150;
		PlayerObject.GetComponent(Rigidbody).AddForce (0, jumpamt, 0);
		isGrounded = false;
	} else {

	}
}




















//### CAMERA ###

camSetDistance = 30.0; // default camera distance
camSpeed = 3.0;
camfov = camfovbase;
camfovspeed = 2.0;
maxcamLookHeight = 4.5;



        
if (isWalking){
	camSetDistance = 28.0;
	camSpeed = 6.0;
	if (isCrouching){
		camSpeed = 6.0;
	}
}

if (isRun){
	if (isSprinting){
		camSetDistance = 24.0;
		camSpeed = 10.0;
		camfov = 35.0;
		camfovspeed = 5.0;
	} else {
		camSetDistance = 26.0;
		camSpeed = 10.0;
		camfov = 25.0;
		camfovspeed = 2.0;
	}
}

if (isLooking){
		zoomFactor = 5.0;

} else {

}


camLat  = Mathf.Clamp(camLat,-25.0,25.0);
camLat = Mathf.SmoothStep(camLat,	defcamLat,7.0*Time.smoothDeltaTime);



camSetDistance  = camSetDistance / (1+zoomFactor);


if (camDistance<=rsThreshhold){
	if (camLookHeight < 5.5) camLookHeight = Mathf.SmoothStep(camLookHeight,5.5,(rotSpeed*rotmod)*Time.smoothDeltaTime);
} else {
	camLookHeight = Mathf.SmoothStep(camLookHeight,lookTargetHeight,(rotSpeed*rotmod*0.5)*Time.smoothDeltaTime);
}






camHeight += (MouseVerticalDistance*0.25);
camRotation += MouseRotationDistance;
camCloseDistance = 0.0;




if (camHeight<=0.05){
//turned off temporarily
	camCloseDistance = camHeight;
	camHeight=0.05;

if (camDistance>=4) camDistance += camCloseDistance;
} else {


if (target_distance > 0) camSetDistance = target_distance;
	camDistance = Mathf.SmoothStep(camDistance,camSetDistance,camSpeed*Time.smoothDeltaTime*1.5);

}



maxcamHeight = 24.0;
if (camHeight>maxcamHeight) camHeight = maxcamHeight;


if (camLookHeight>maxcamLookHeight) camLookHeight = maxcamLookHeight;



var newPlayerObject:Vector3 = PlayerObject.transform.position;
newPlayerObject.y -= 3.0;

if (isLooking){
	look = Vector3(newPlayerObject.x+0.3,newPlayerObject.y+5.0,newPlayerObject.z+0.3);
} else {
	look = Vector3(newPlayerObject.x,newPlayerObject.y+camLookHeight,newPlayerObject.z);
}

cameraTransform.position = Vector3(newPlayerObject.x,newPlayerObject.y+camHeight,newPlayerObject.z-camDistance-distMod);






//camera water limits
var layer : int = 4;
var layermask : int = 1 << layer;
var hit : RaycastHit;
var testpos : Vector3 = Vector3(PlayerObject.transform.position.x,PlayerObject.transform.position.y+5000.0,PlayerObject.transform.position.z);
if (Physics.Raycast (testpos, -Vector3.up, hit,10000,layermask)) {
	if (hit.transform.gameObject.layer==4){ //hits object on water layer
		
		if (isUnderwater){
			if (cameraTransform.position.y > (hit.transform.position.y-1.0)){
			}
		}

		
	}
}






cameraTransform.RotateAround (look, Vector3.up, camRotation);




if (forcePerspective){
cameraTransform.GetComponent.<Camera>().fieldOfView = Mathf.Lerp(cameraTransform.GetComponent.<Camera>().fieldOfView,camfov,camfovspeed*Time.smoothDeltaTime);
}

if (Movement_Mode!="still"){
	distMod = 0.0;

} else {
	distMod = 0.0;
}



cameraTransform.LookAt(look);

cameraTransform.eulerAngles.y += camLat;






































}












