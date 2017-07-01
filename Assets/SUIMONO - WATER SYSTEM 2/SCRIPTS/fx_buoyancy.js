#pragma strict

//PUBLIC VARIABLES
var applyToParent : boolean = false;
var engageBuoyancy : boolean = false;
var buoyancyStrength : float = 1.0;
var buoyancyOffset : float = 0.0;


// PRIVATE VARIABLES
private var surfaceLevel : float = 0.0;
private var underwaterLevel : float = 0.0;

private var isUnderwater : boolean = false;
private var isOverWater : boolean = false;

private var physTarget : Transform;
private var moduleObject : SuimonoModule;
private var suimonoObject : SuimonoObject;

private var height : float = -1;
private var getScale : float = 1.0;
private var waveHeight : float = 0.0;

function Start () {

	moduleObject = GameObject.Find("SUIMONO_Module").gameObject.GetComponent(SuimonoModule);
	
}



function Update () {

	//set debug visibility
	if (moduleObject != null){
	if (moduleObject.showDebug){
		GetComponent.<Renderer>().enabled = true;
		//renderer.material.SetColor("_TintColor",labelColor);
	}
	
	if (!moduleObject.showDebug){
		GetComponent.<Renderer>().enabled = false;
	}
	
}


	//set physics target
	if (applyToParent){
		physTarget = this.transform.parent.transform;
	} else {
		physTarget  = this.transform;
	}
	
	
	//Creset values
	isUnderwater = false;
	isOverWater = false;
	height = -1;
	surfaceLevel = -1;
	suimonoObject = null;
	underwaterLevel = 0.0;

	//get wave height at position
	height = GetHeight();
	
	
	//calculate scaling
	var testObjectHeight = (transform.position.y+buoyancyOffset);
	
	if (suimonoObject != null){
		getScale = suimonoObject.waveHeight;
		waveHeight = surfaceLevel+(height*(getScale*1.25));
		if (testObjectHeight < waveHeight){
			isUnderwater = true;
		}
		underwaterLevel =  waveHeight-testObjectHeight;
	}
	
	
	//set buoyancy
	if (engageBuoyancy){
	if (physTarget.GetComponent.<Rigidbody>() && !physTarget.GetComponent.<Rigidbody>().isKinematic){
		if (underwaterLevel > 0.1){
			
			var buoyancyFactor : float = 10.0;//(underwaterLevel * underwaterLevel);
			if (buoyancyFactor < 2.0) buoyancyFactor = 2.0;
			if (buoyancyFactor > 10.0) buoyancyFactor = 10.0;
			
			physTarget.GetComponent.<Rigidbody>().AddForceAtPosition(Vector3(0,1,0) * buoyancyFactor * buoyancyStrength, transform.position);
		
		} else if (underwaterLevel < 0.1 && underwaterLevel > 0.0){
			//
		} else if (underwaterLevel < 0.0){// && underwaterLevel > -0.1){
				
			//physTarget.rigidbody.velocity = Vector3(0,0,0);
			physTarget.GetComponent.<Rigidbody>().AddForceAtPosition(Vector3(0,-1,0) * 1.0, transform.position);		
			
		//} else if (underwaterLevel < -0.1){
			//
		}
	}
	}
	

	
	
	
}




function GetHeight() : float {

	var getheight : float = 0.0;
	var layer : int = 4;
	var layermask : int = 1 << layer;
	var hit : RaycastHit;
	var testpos : Vector3 = Vector3(this.transform.position.x,this.transform.position.y+5000,this.transform.position.z);
	if (Physics.Raycast (testpos, -Vector3.up, hit,10000,layermask)) {
		if (hit.transform.gameObject.layer==4){ //hits object on water layer
			

			if (hit.transform.gameObject.GetComponent(SuimonoObject) != null){
				suimonoObject = hit.transform.gameObject.GetComponent(SuimonoObject);	
				isOverWater = true;
				surfaceLevel = hit.point.y;
				
				if (hit.collider.GetComponent.<Renderer>().material.GetTexture("_WaveMap") != null){
					var checktex = hit.collider.GetComponent.<Renderer>().material.GetTexture("_WaveMap") as Texture2D;
		   			var pixelUV : Vector2 = hit.textureCoord;
		    		pixelUV.x *= checktex.width;
		    		pixelUV.y *= checktex.height;
		    		getheight = checktex.GetPixel(pixelUV.x, pixelUV.y).r;
	    		}
	    		

			}
		}
	}

	return getheight;

}


