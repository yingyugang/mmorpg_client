//Define variables
var alwaysEmitRipples : boolean = false;
var splashCoolDownTime : float = 30.0;
var splashRingsTimer : float = 0.1;
var splashRingsSize : float = 0.17;
var splashRingsRotation : float = 0.0;
var followObjectRotation : boolean = false;

static var setSplashRingsTimer : float = 0.5;

//Define private variables
private var isMoving : boolean = false;
private var oldPosition : Vector3;
private var addTimer : float = 0.0;


function Start () {

}


function Update(){


	//calculate movement and ring time
	var posDiffx = Mathf.Abs(Mathf.Abs(this.transform.position.x) - Mathf.Abs(oldPosition.x));
	var posDiffz = Mathf.Abs(Mathf.Abs(this.transform.position.z) - Mathf.Abs(oldPosition.z));
	if (posDiffx >= 0.01 || posDiffz >= 0.01){
		isMoving = true;
	} else {
		isMoving = false;
	}
	oldPosition = this.transform.position;
	
	
	if (splashCoolDownTime > 0.0){
		if (isMoving == false){
			addTimer += (Time.deltaTime/splashCoolDownTime);
			if (addTimer > 1.0) addTimer = 1.0;
			setSplashRingsTimer = Mathf.Lerp(splashRingsTimer,10.0,addTimer);
		} else if (isMoving == true){
			setSplashRingsTimer = splashRingsTimer;
			addTimer = 0.0;
		}
	} else {
		setSplashRingsTimer = splashRingsTimer;
	}


	if (followObjectRotation){

		//calculate rotation
		splashRingsRotation = this.transform.eulerAngles.y+40.0;
		if (isMoving == true){
			addTimer = 0.0;
			setSplashRingsTimer = splashRingsTimer * 0.25;
		}
	}


}


