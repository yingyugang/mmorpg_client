#pragma strict

@script ExecuteInEditMode()

//Underwater Effects variables
var isUnityPro : boolean = true;
var unityVersion : String = "---";
var setCamera : Transform;
var setTrack : Transform;
var enableUnderwaterPhysics : boolean = true;
var enableUnderwaterFX : boolean = true;
var enableInteraction : boolean = true;


var etherealScroll : float = 0.1;

var enableRefraction : boolean = true;
var enableBlur : boolean = true;
var underwaterColor : Color = Color(0.58,0.61,0.61,0.0);
var enableTransition : boolean = true;
var blurAmount : float = 0.005;
var refractionAmount : float = 20.0;
var cameraPlane_offset : float = 3.5;

var showDebug : boolean = false;

var manageShaders : boolean = true;

private var camRendering : RenderingPath;

private var shaderSurface : Shader;
private var shaderUnderwater : Shader;
private var shaderUnderwaterFX : Shader;
private var shaderDropletsFX : Shader;
private var debrisShaderFX : Shader;

private var underwaterLevel = 0.0;
private var underwaterRefractPlane : GameObject;
private var waterTransitionPlane : GameObject;
private var waterTransitionPlane2 : GameObject;
private var underwaterDebris : ParticleSystem;

private var underFogDist : float = 0.0;
private var underFogSpread : float = 0.0;
private var reflectColor : Color;
private var causticsColor : Color;
private var causticsSizing : float;
private var hitAmt : float = 1.0;
private var origDepthAmt : float = 1.0;
private var origReflColr : Color;

private var refractAmt : float = 0.0;

private var targetSurface : Transform;
private var targetObject : SuimonoObject;

static var isUnderwater : boolean = false;
static var doWaterTransition : boolean = false;




//splash effects variables
var alwaysEmitRipples : boolean = false;
var maxEmission = 5000;
var playSounds : boolean = true;
var maxVolume = 1.0;
var maxSounds = 10;
var defaultSplashSound : AudioClip[];
var soundObject : Transform;

private var isinwater : boolean = false;
private var atDepth : float = 0.0;

private var splash_rings : Transform;
private var splash_small : Transform;
private var splash_med : Transform;
private var splash_dirt : Transform;
private var splash_drops : Transform;

private var isPlayingTimer = 0.0;

private var setvolumetarget = 0.65;
private var setvolume = 0.65;

private var ringsSystem : ParticleSystem;
private var ringsParticles : ParticleSystem.Particle[];

private var ringFoamSystem : ParticleSystem;
private var ringFoamParticles : ParticleSystem.Particle[];
private var ringFoamParticlesNum : int = 1;

private var splashSystem : ParticleSystem;
private var splashParticles : ParticleSystem.Particle[];
private var splashParticlesNum : int = 1;

private var splashDropSystem : ParticleSystem;
private var splashDropParticles : ParticleSystem.Particle[];
private var splashDropParticlesNum : int = 1;

private var sndparentobj : fx_soundModule;
private var sndObject = new Array();
private var sndObjects : Transform[];
private var currentSound = 0;



function Start () {

	//get unity version
	unityVersion = Application.unityVersion.ToString().Substring(0,1);
	
	//INITIATE DEFAULT SHADERS
	shaderSurface = Shader.Find("Suimono2/water_pro");
	shaderUnderwater = Shader.Find("Suimono2/water_under_pro");
	shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane");
	shaderDropletsFX = Shader.Find("Suimono2/effect_refraction");
	debrisShaderFX = Shader.Find("Suimono2/particle_Alpha");


	//INITIATE OBJECTS
    underwaterRefractPlane = GameObject.Find("effect_refract_plane");
    waterTransitionPlane = GameObject.Find("effect_dropletsParticle");
    waterTransitionPlane2 = GameObject.Find("effect_water_fade");
    
    #if !UNITY_3_5
    	underwaterRefractPlane.SetActive(true);
    #else
    	underwaterRefractPlane.active = true;
    #endif
    
    underwaterDebris = GameObject.Find("effect_underwater_debris").gameObject.GetComponent(ParticleSystem);
    
    //splash effects init
	ringsSystem = GameObject.Find("splash_rings_normal_prefab").gameObject.GetComponent(ParticleSystem);
	ringFoamSystem = GameObject.Find("splash_ringsFoam_prefab").gameObject.GetComponent(ParticleSystem);
	splashSystem = GameObject.Find("splash_prefab").gameObject.GetComponent(ParticleSystem);
	splashDropSystem = GameObject.Find("splash_droplets_prefab").gameObject.GetComponent(ParticleSystem);
	sndparentobj = GameObject.Find("_sound_effects").gameObject.GetComponent(fx_soundModule);
#if UNITY_EDITOR
	if (EditorApplication.isPlaying){
	if (soundObject != null && sndparentobj != null){
		maxSounds = sndparentobj.maxSounds;
		sndObjects = new Transform[maxSounds];
		for (var sx=0; sx < (maxSounds); sx++){
			var soundObjectPrefab = Instantiate(soundObject, transform.position, transform.rotation);
			soundObjectPrefab.transform.parent = sndparentobj.transform;
			sndObjects[sx] = (soundObjectPrefab);
		}
	}
	}
#endif
	//
	
	
	//set camera
	if (setCamera == null){
		if (Camera.main != null){
			setCamera = Camera.main.transform;
			camRendering = setCamera.GetComponent.<Camera>().renderingPath;
		} else {
			Debug.Log("SUIMONO Error: no camera has been defined!  Suimono requires a camera to either be explicitly set in the Suimono_Module object, OR to be tagged with the 'MainCamera' GameObject tag."); 
		}
	} else {
		setCamera.GetComponent.<Camera>().depthTextureMode = DepthTextureMode.Depth;
		camRendering = setCamera.GetComponent.<Camera>().renderingPath;
	}
	
	//set track object
	if (setTrack == null){
		setTrack = setCamera.transform;
	}


}




function Update () {
	
	
	
	if (manageShaders){
	
		//DX11 SPECIFIC
		#if !UNITY_3_5 
			debrisShaderFX = Shader.Find("Suimono2/particle_Alpha_4");
			#if !UNITY_STANDALONE_OSX
			#if UNITY_EDITOR
			if (PlayerSettings.useDirect3D11){
			shaderSurface = Shader.Find("Suimono2/water_pro_dx11");
			shaderUnderwater = Shader.Find("Suimono2/water_under_pro_dx11");
			}
			#endif
			#endif
		#endif
		
		
		//UNITY BASIC VERSION SPECIFIC
		#if UNITY_EDITOR
		if (!PlayerSettings.advancedLicense){
			shaderSurface = Shader.Find("Suimono2/water_mobile");
			shaderUnderwater = Shader.Find("Suimono2/water_under_mobile");
			shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_mobile");
			shaderDropletsFX = Shader.Find("Suimono2/effect_refraction_mobile");
		}
		#endif
		#if UNITY_IPHONE
			shaderSurface = Shader.Find("Suimono2/water_mobile");
			shaderUnderwater = Shader.Find("Suimono2/water_under_mobile");
			shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_mobile");
			shaderDropletsFX = Shader.Find("Suimono2/effect_refraction_mobile");	
		#endif
		
		#if UNITY_ANDROID
			shaderSurface = Shader.Find("Suimono2/water_mobile");
			shaderUnderwater = Shader.Find("Suimono2/water_under_mobile");
			shaderUnderwaterFX = Shader.Find("Suimono2/effect_refractPlane_mobile");
			shaderDropletsFX = Shader.Find("Suimono2/effect_refraction_mobile");
		#endif
	
	}
	
	
	#if UNITY_EDITOR
	//reset sound objects
	if (!EditorApplication.isPlaying){
		sndObjects = null;
	}
	#endif
	//set camera
	if (setCamera == null){
		if (Camera.main != null){
			setCamera = Camera.main.transform;
			camRendering = setCamera.GetComponent.<Camera>().renderingPath;
		} else {
			Debug.Log("SUIMONO Error: no camera has been defined!  Suimono requires a camera to either be explicitly set in the Suimono_Module object, OR to be tagged with the 'MainCamera' GameObject tag."); 
		}
	} else {
		setCamera.GetComponent.<Camera>().depthTextureMode = DepthTextureMode.Depth;
		camRendering = setCamera.GetComponent.<Camera>().renderingPath;
	}
	
	//set track object
	if (setTrack == null){
		setTrack = setCamera.transform;
	}
	
	
	checkUnderwaterEffects();
	checkWaterTransition();

}








//#############################
//	CUSTOM FUNCTIONS
//#############################



function AddEffect(addMode : String, effectPos : Vector3, addRate : int, addSize : float, addRot : float){
if (enableInteraction){
	var px = 0;
	

	if (addMode == "rings"){
		if (ringsSystem != null){
		if (manageShaders) ringsSystem.GetComponent.<Renderer>().sharedMaterial.shader = shaderDropletsFX;
		ringsSystem.Emit(addRate);
		//get particles
		ringsParticles = new ParticleSystem.Particle[ringsSystem.particleCount];
		ringsSystem.GetParticles(ringsParticles);
		//set particles
		for (px = (ringsSystem.particleCount-addRate); px < ringsSystem.particleCount; px++){
				//set position
				ringsParticles[px].position.x = effectPos.x;
				ringsParticles[px].position.y = effectPos.y;
				ringsParticles[px].position.z = effectPos.z;
				//set variables
				ringsParticles[px].size = addSize;
				ringsParticles[px].rotation = addRot;
		}
		ringsSystem.SetParticles(ringsParticles,ringsParticles.length);
		ringsSystem.Play();
	}
	}
	
	
	if (addMode == "ringfoam"){
		if (ringFoamSystem != null){
		ringFoamSystem.Emit(addRate);
		//get particles
		ringFoamParticles = new ParticleSystem.Particle[ringFoamSystem.particleCount];
		ringFoamSystem.GetParticles(ringFoamParticles);
		//set particles
		for (px = (ringFoamSystem.particleCount-addRate); px < ringFoamSystem.particleCount; px++){
				//set position
				ringFoamParticles[px].position.x = effectPos.x;
				ringFoamParticles[px].position.y = effectPos.y;
				ringFoamParticles[px].position.z = effectPos.z;
				//set variables
				ringFoamParticles[px].size = addSize;
				ringFoamParticles[px].rotation = addRot;
		}
		ringFoamSystem.SetParticles(ringFoamParticles,ringFoamParticles.length);
		ringFoamSystem.Play();
	}
	}
	
	
	if (addMode == "splash"){
		if (splashSystem != null){
		splashSystem.Emit(addRate);
		//get particles
		splashParticles = new ParticleSystem.Particle[splashSystem.particleCount];
		splashSystem.GetParticles(splashParticles);
		//set particles
		for (px = (splashSystem.particleCount-addRate); px < splashSystem.particleCount; px++){
				//set position
				splashParticles[px].position.x = effectPos.x;
				splashParticles[px].position.y = effectPos.y;
				splashParticles[px].position.z = effectPos.z;
		}
		splashSystem.SetParticles(splashParticles,splashParticles.length);
		splashSystem.Play();
	}
	}
	
	
	
	if (addMode == "splashDrop"){
		if (splashDropSystem != null){
		splashDropSystem.Emit(addRate);
		//get particles
		splashDropParticles = new ParticleSystem.Particle[splashDropSystem.particleCount];
		splashDropSystem.GetParticles(splashDropParticles);
		//set particles
		for (px = (splashDropSystem.particleCount-addRate); px < splashDropSystem.particleCount; px++){
				//set position
				splashDropParticles[px].position.x = effectPos.x;
				splashDropParticles[px].position.y = effectPos.y;
				splashDropParticles[px].position.z = effectPos.z;
		}
		splashDropSystem.SetParticles(splashDropParticles,splashDropParticles.length);
		splashDropSystem.Play();
	}
	}

}
}











function AddSound(sndMode : String, soundPos : Vector3, sndVelocity:Vector3){
if (enableInteraction){
	var setstep : AudioClip;
	var setpitch = 1.0;
	var waitTime = 0.4;
	var setvolume = 1.0;
	
	
	
	if (playSounds && sndparentobj.defaultSplashSound.length >= 1 ){
		setstep = sndparentobj.defaultSplashSound[Random.Range(0,sndparentobj.defaultSplashSound.length-1)];
		waitTime = 0.4;
		setpitch = Random.Range(1.0,1.5);
		setvolume = 0.2;
		if (Mathf.Abs(sndVelocity.z) >= setvolume) setvolume = Mathf.Abs(sndVelocity.z);
		if (setvolume > maxVolume) setvolume = maxVolume;
		setvolume = maxVolume;
		
		//check depth and morph sounds if underwater
		if (GetHeight(setCamera.transform,"object depth") > 0.0){
			setpitch *=0.25;
			setvolume *=0.5;
		}
		
		if (!sndObjects[currentSound].GetComponent(AudioSource).isPlaying){
			sndObjects[currentSound].GetComponent(AudioSource).transform.position = soundPos;
			sndObjects[currentSound].GetComponent(AudioSource).volume = setvolume;
			sndObjects[currentSound].GetComponent(AudioSource).pitch = setpitch;
			sndObjects[currentSound].GetComponent(AudioSource).minDistance = 4.0;
			sndObjects[currentSound].GetComponent(AudioSource).maxDistance = 20.0;
			sndObjects[currentSound].GetComponent(AudioSource).clip = setstep;
			sndObjects[currentSound].GetComponent(AudioSource).loop = false;
			sndObjects[currentSound].GetComponent(AudioSource).Play();
		}
		currentSound += 1;
		if (currentSound >= (maxSounds-1)) currentSound = 0;
	}

}
}













function checkUnderwaterEffects(){
#if UNITY_EDITOR
if (EditorApplication.isPlaying){
	//check for underwater
	
		
	//set blur
	if (enableBlur){
		underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_BlurSpread",blurAmount);
	} else {
		underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_BlurSpread",0.0);
	}


	var camHeight : float = GetHeight(setCamera.transform,"object depth");
		
	
	if (camHeight > 0.0){
	
		//swap camera rendering to deferred
		setCamera.GetComponent.<Camera>().renderingPath = RenderingPath.DeferredLighting;

		if (enableUnderwaterFX){
		
			//reposition refract plane
			if (manageShaders) underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.shader = shaderUnderwaterFX;
			underwaterRefractPlane.transform.parent = setCamera.transform;
			underwaterRefractPlane.transform.localPosition = Vector3(0.0,0.0,(setCamera.GetComponent.<Camera>().nearClipPlane+cameraPlane_offset));
			underwaterRefractPlane.transform.localEulerAngles = Vector3(270.0,0.0,0.0);
	   		underwaterRefractPlane.GetComponent.<Renderer>().enabled = true;

		}
		
	} else {
		
		//swap camera rendering to back to default
		setCamera.GetComponent.<Camera>().renderingPath = camRendering;
   		underwaterRefractPlane.GetComponent.<Renderer>().enabled = false;

	}
	
	
}
#endif
}








function checkWaterTransition () {
#if UNITY_EDITOR
if (EditorApplication.isPlaying){
	
		
		
		//SET COLORS
		reflectColor = Color(0.827,0.941,1.0,1.0);


		if (enableUnderwaterFX){


		var blurmod = 0.0;
		
		var camHeight : float = GetHeight(setCamera.transform,"object depth");
		if (camHeight > 0.0){
		
			var layer : int = 4;
			var suimonoObject : SuimonoObject;
			var layermask : int = 1 << layer;
			var hit : RaycastHit;
			var testpos : Vector3 = Vector3(setCamera.transform.position.x,setCamera.transform.position.y+5000,setCamera.transform.position.z);
			if (Physics.Raycast (testpos, -Vector3.up, hit,10000,layermask)) {
				if (hit.transform.gameObject.layer==4){ //hits object on water layer
					
					if (hit.transform.gameObject.GetComponent(SuimonoObject) != null){
						suimonoObject = hit.transform.gameObject.GetComponent(SuimonoObject);	
					}
				}
			}
		
		
		
			if (suimonoObject != null){
				targetSurface = hit.transform;
				targetObject = targetSurface.gameObject.GetComponent(SuimonoObject);
				if (targetObject != null){
				if (hit.transform.GetComponent.<Renderer>().sharedMaterial.GetFloat("_DepthAmt") != 0.0){
					hitAmt = hit.transform.GetComponent.<Renderer>().sharedMaterial.GetFloat("_DepthAmt");
					origDepthAmt = hit.transform.GetComponent.<Renderer>().sharedMaterial.GetFloat("_DepthAmt");
				}
				}
			}
		
		
		
			doWaterTransition = true;
			
	       	//if you want to add underwater blur effect, simply uncomment the below block
	       	//and add a blur camera effect to your main camera.
	      	//if (enableBlur){
	      		//if (setCamera.GetComponent(BlurEffect) != null){
		      		//setCamera.GetComponent(BlurEffect).enabled = true;
			        //setCamera.GetComponent(BlurEffect).iterations = 4;
			        //camBlur = Mathf.Lerp(camBlur,camTrgt,Time.deltaTime);
			       	// setCamera.GetComponent(BlurEffect).blurSpread = underwaterBlur;
		        //}
	        //}
	        
	       	//set audio low pass filter (for underwater effect)
	      	//if (setCamera.GetComponent(AudioLowPassFilter) != null){
	       		//setCamera.GetComponent(AudioLowPassFilter).enabled = true;
	       		//setCamera.GetComponent(AudioLowPassFilter).cutoffFrequency = 270;
	       		//setCamera.GetComponent(AudioLowPassFilter).lowpassResonaceQ = 2.21;
	       	//}
	
	       	//set underwater debris
	       	if (targetObject != null){
		       	if (targetObject.enableUnderDebris){
		       		if (manageShaders) underwaterDebris.GetComponent.<Renderer>().sharedMaterial.shader = debrisShaderFX;
			       	underwaterDebris.transform.position = setCamera.transform.position;
			       	underwaterDebris.transform.rotation = setCamera.transform.rotation;
			       	underwaterDebris.transform.Translate(Vector3.forward * 40.0);
					underwaterDebris.GetComponent.<Renderer>().enabled=true;
					underwaterDebris.enableEmission=true;
					underwaterDebris.Play();
				}
				
	
			   
				//get attributes from surface
				underwaterColor = targetObject.underwaterColor;
				refractionAmount = targetObject.underRefractionAmount;
		       	blurAmount = targetObject.underBlurAmount;
		       	underFogSpread = targetObject.underwaterFogSpread;
		       	underFogDist = targetObject.underwaterFogDist;
		       	refractAmt = targetObject.underRefractionAmount;

	       	
	       	//set attributes to shader
	       	underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_RefrStrength",refractAmt);
	       	underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_underFogStart",underFogDist);
	       	underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_underFogStretch",underFogSpread);
	       	underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_BlurSpread",blurAmount);
			underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorB",underwaterColor);
			underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_DepthAmt",0.001+(hitAmt*0.2));
			underwaterRefractPlane.GetComponent.<Renderer>().sharedMaterial.SetFloat("_Strength",refractionAmount);
			underwaterRefractPlane.transform.parent = setCamera.transform;
			underwaterRefractPlane.transform.localPosition = Vector3(0.0,0.0,(setCamera.GetComponent.<Camera>().nearClipPlane+0.02));
			underwaterRefractPlane.transform.localEulerAngles = Vector3(270.0,0.0,0.0);
		    underwaterRefractPlane.GetComponent.<Renderer>().enabled = true;
  
	       	} else {
	       		underwaterRefractPlane.GetComponent.<Renderer>().enabled = false;
	       	}
	       	
	       	
	       	//hide water transition
	     	waterTransitionPlane.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = false;
	     	waterTransitionPlane.GetComponent(ParticleSystem).Clear();
	     	
			waterTransitionPlane2.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = false;
	        waterTransitionPlane2.GetComponent(ParticleSystem).Clear();
	       	//Switch Shaders
	       	if (manageShaders){
	       	if (targetSurface){
	       		if (targetSurface.GetComponent.<Renderer>().sharedMaterial.shader != shaderUnderwater){
					targetSurface.GetComponent.<Renderer>().sharedMaterial.shader = shaderUnderwater;
				}
			}
			}
			
		
			
	    } else {
	    
			//if you want to add underwater blur effect, simply uncomment the below block
	       	//and add a blur camera effect to your main camera.
			//if (setCamera.GetComponent(BlurEffect) != null){
	       	//	setCamera.GetComponent(BlurEffect).enabled = false;
	        //}

	
	        //reset audio low pass filter (for underwater effect)
	        //if (setCamera.GetComponent(AudioLowPassFilter) != null){
	       	//	setCamera.GetComponent(AudioLowPassFilter).enabled = false;
	       	//}
	       	
	       	
	        //reset underwater debris
	       	underwaterDebris.GetComponent.<Renderer>().enabled=false;
	       	

	       	//turn off water refraction plane
	     	underwaterRefractPlane.GetComponent.<Renderer>().enabled = false;
	     	
	     	//show water transition
	     	if (enableTransition){
	     	if (doWaterTransition){
	     	
	     		//sets and emits random water "screen" droplets
	     		if (manageShaders) waterTransitionPlane.GetComponent.<Renderer>().sharedMaterial.shader = shaderDropletsFX;
	     		waterTransitionPlane.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = true;
	     		waterTransitionPlane.transform.parent = setCamera.transform;
	     		//cameraPlane_offset = 3.7;
	       		waterTransitionPlane.transform.localPosition = Vector3(0.0,0.0,setCamera.GetComponent.<Camera>().nearClipPlane+cameraPlane_offset+0.02);
	       		waterTransitionPlane.transform.localEulerAngles = Vector3(270.0,262.9,0.0);
	      		waterTransitionPlane.GetComponent(ParticleSystem).Play();
	      		waterTransitionPlane.GetComponent(ParticleSystem).Emit(Random.Range(60,120));
	      		
	      		//sets and plays water transition "screen" effect
	      		if (manageShaders) waterTransitionPlane2.GetComponent.<Renderer>().sharedMaterial.shader = shaderDropletsFX;
	      		waterTransitionPlane2.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = true;
	     		waterTransitionPlane2.transform.parent = setCamera.transform;
	       		waterTransitionPlane2.transform.localPosition = Vector3(0.0,0.0,setCamera.GetComponent.<Camera>().nearClipPlane+cameraPlane_offset+0.01);
	       		waterTransitionPlane2.transform.localEulerAngles = Vector3(270.0,262.9,0.0);
	      		waterTransitionPlane2.GetComponent(ParticleSystem).Emit(1);
	      		
	       		doWaterTransition = false;
	       		
	     	} else {
	     		
	     		if (manageShaders) waterTransitionPlane.GetComponent.<Renderer>().sharedMaterial.shader = shaderDropletsFX;
	     		waterTransitionPlane.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = true;
	     		waterTransitionPlane.GetComponent(ParticleSystem).Stop();
	     		
	     		if (manageShaders) waterTransitionPlane2.GetComponent.<Renderer>().sharedMaterial.shader = shaderDropletsFX;
	     		waterTransitionPlane2.GetComponent(ParticleSystem).GetComponent.<Renderer>().enabled = true;
	     		
	     	}
	       	}
	       	
	       	//Switch Shaders
	       	if (manageShaders){
	       	if (targetSurface){
	       		if (targetSurface.GetComponent.<Renderer>().sharedMaterial.shader != shaderSurface){
					targetSurface.GetComponent.<Renderer>().sharedMaterial.shader = shaderSurface;
				}
			}
			}
			
	     	
	    }
    }
    
    
    if (!enableUnderwaterFX){
    	//reset underwater FX and Shaders
    	underwaterRefractPlane.transform.parent = this.transform;
   		underwaterRefractPlane.GetComponent.<Renderer>().enabled = false;
   		
		underwaterDebris.GetComponent.<Renderer>().enabled=false;
		if (manageShaders){
		if (targetSurface){
			if (targetSurface.GetComponent.<Renderer>().sharedMaterial.shader != shaderSurface){
				targetSurface.GetComponent.<Renderer>().sharedMaterial.shader = shaderSurface;
			}
		}
		}
    }
    
}
#endif   
}










function GetHeight(testObject : Transform, returnMode : String) : float {

	var baseheight : float = 0.0;
	var getheight : float = 0.0;
	var getpixelvalue : float = 0.0;
	var getScale : float = 0.0;
	var layer : int = 4;
	var suimonoObject : SuimonoObject;
	var layermask : int = 1 << layer;
	var hit : RaycastHit;
	var testpos : Vector3 = Vector3(testObject.position.x,testObject.position.y+5000,testObject.position.z);
	if (Physics.Raycast (testpos, -Vector3.up, hit,10000,layermask)) {
		if (hit.transform.gameObject.layer==4){ //hits object on water layer
			

			if (hit.transform.gameObject.GetComponent(SuimonoObject) != null){
				suimonoObject = hit.transform.gameObject.GetComponent(SuimonoObject);	
				baseheight = hit.point.y;
				
				if (hit.collider.GetComponent.<Renderer>().sharedMaterial.GetTexture("_WaveMap") != null){
					var checktex = hit.collider.GetComponent.<Renderer>().material.GetTexture("_WaveMap") as Texture2D;
		   			var pixelUV : Vector2 = hit.textureCoord;
		    		pixelUV.x *= checktex.width;
		    		pixelUV.y *= checktex.height;
		    		getpixelvalue = checktex.GetPixel(pixelUV.x, pixelUV.y).r;
	    		}
	    		

			}
		}
	}
	
	//calculate wave Height
	if (suimonoObject != null){
		getScale = suimonoObject.waveHeight;
		//getheight = baseheight+(getpixelvalue*(getScale*1.25));
		getheight = baseheight+(getpixelvalue*(getScale*3.0));
	}




	//RETURN VALUES
	var returnValue : float = 0.0;
	if (returnMode == "object depth"){
		returnValue = getheight - testObject.position.y;
	}
	if (returnMode == "wave height"){
		returnValue = getheight - testObject.position.y;
	}
	
	return returnValue;

}

