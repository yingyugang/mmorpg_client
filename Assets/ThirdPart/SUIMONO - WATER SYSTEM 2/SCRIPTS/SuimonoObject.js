#pragma strict

@script ExecuteInEditMode()
import System.IO;


//preset variables
var renderTex : RenderTexture;
//var renderTex : Texture2D;
var presetIndex : int;
var presetUseIndex : int;
var presetOptions : String[];

var refractSpeed : float = 1.0;
var refractScale : float = 1.0;
var blurSpread : float = 0.1;

var presetTransIndexFrm : int = 0;
var presetTransIndexTo : int = 0;
var presetStartTransition : boolean = false;
var presetTransitionTime : float = 3.0;
var presetTransitionCurrent : float = 1.0;
var presetSaveName : String = "my custom preset";
var presetToggleSave : boolean = false;
var presetDataArray : String[];
var presetDataString : String;
var baseDir : String = "SUIMONO - WATER SYSTEM 2";
var suimonoModuleObject : GameObject;
var suimonoModuleLibrary : SuimonoModuleLib;
var mirrorTexture : RenderTexture;

var hFoamHeight : float = 5.5;
var hFoamAmt : float = 1.0;
var hFoamSpread : float = 2.0;

var reflectionObject : GameObject;
var shorelineObject : GameObject;
var waveObject : GameObject;
//var reflectionCompo : Component;


var enableUnderDebris : boolean = true;

var waterForce : Vector2 = Vector2(0.0,0.0);
var flowForce : Vector2 = Vector2(0.0,0.0);

//general variables
var presetFile : String = "/RESOURCES/_PRESETS.txt";
var typeIndex : int;
var typeUseIndex : int;
//var typeOptions : String[];
//var setOptions : String =  "Ocean,Plane,River";
//var typeOptions : String[] = setOptions.Split(","[0]);
var typeOptions = new Array("3D Ocean","Flat Plane");
var enableCustomTextures : boolean = false;

//editor variables
var showGeneral : boolean = false;
var showWaves : boolean = false;
var showSurface : boolean = false;
var showFoam : boolean = false;
var showUnderwater : boolean = false;
var showEffects : boolean = false;
var showPresets : boolean = false;
var showTess : boolean = false;


//underwater settings
var enableUnderwaterFX : boolean = true;
var enableUnderRefraction : boolean = true;
var underRefractionAmount : float = 1.0;
var enableUnderBlur : boolean = true;
var underBlurAmount : float = 0.01;
var enableUnderAutoFog : boolean = true;
var underwaterColor : Color = Color(0.2,0.2,1.0,1.0);
var enableUnderEthereal : boolean = true;
var etherealShift : float = 0.1;
var underwaterFogDist : float = 1.0;
var underwaterFogSpread : float = 1.0;

//wave settings
var waveHeight : float = 0.0;
var waveShoreHeight : float = 0.0;
var waveScale : float = 0.0;
var shoreInfluence : float = 0.0;

var overallScale : float = 1.0;
var lightAbsorb : float = 0.125;
var lightRefract : float = 1.0;
var foamScale : float = 0.5;
var foamAmt : float = 0.3;
var foamColor : Color = Color(0.9,0.9,0.9,0.9);
var useFoamColor : Color = Color(0.9,0.9,0.9,0.9);
var edgeSpread : float = 0.3;
var edgeColor : Color = Color(0.9,0.9,0.9,0.9);
var useEdgeColor : Color = Color(0.9,0.9,0.9,0.9);
var reflectDist : float = 0.2;
var reflectDistUnderAmt : float = 0.2;
var reflectSpread : float = 0.05;
var colorDynReflect : Color = Color(1.0,1.0,1.0,0.4);
var reflectionOffset : float = 0.35;
var reflectPlaneObject : GameObject;

var colorSurfHigh : Color = Color(0.25,0.5,1.0,0.75);
var colorSurfLow : Color = Color(0,0,0,0);
var colorHeight : float = 6.0;
var colorHeightSpread : float = 2.5;
var surfaceSmooth : float = 1.0;

//color variables
var depthColor : Color;
var depthColorR : Color;
var depthColorG : Color;
var depthColorB : Color;

var specColorH : Color;
var specColorL : Color;
var specScatterWidth : float;
var specScatterAmt : float;

//tessellation settings
var waveTessAmt : float = 4.0;
var waveTessMin : float = -180.0;
var waveTessSpread : float = 200.0;

var waveFac : float = 1.0;


//flowmap
var inheritColor : boolean = true;
var wave_speed : Vector2 = Vector2(0.0015,0.0015);
var foam_speed : Vector2 = Vector2(-0.02,-0.02);

//tide
var tideColor : Color;
var tideAmount : float;
var tideSpread : float;
               
               
               

private var useDepthColor : Color; 

//splash & collision variables
var splashIsOn : boolean = true;
var UpdateSpeed : float = 0.5;
var rippleSensitivity : float = 0.0;
var splashSensitivity :float = 0.2;
private var isinwater : boolean = false;
private var isUnderwater : boolean = false;
private var atDepth : float = 0.0;
private var setvolume = 0.65;
private var ringsTime = 0.0;

//private var objectRingsTime = new Array();
private var objectRingsTime : float[];
private var objectRingsTimes = new Array();

//private var CurrentColliders = new Array();
//private var AllColliders : Collider[];
private var CurrentColliders : Collider[];
private var CurrentCollider = new Array();
private var moduleSplashObject : SuimonoModule;


private var shoreAmt : float = 0.85;
private var shoreAmtDk : float = 1.0;
private var shoreTideTimer : float = 0.0;

//wave texture animation variables
var flowSpeed : float = 0.1;
var setflowSpeed : float = 0.1;
var waveSpeed : float = 0.1;
var foamSpeed : float = 0.1;
var flow_dir : Vector2 = Vector2(0.0015,0.0015);
var flow_dir_degrees : float = 0.0;

var wave_dir : Vector2 = Vector2(0.0015,0.0015);
var foam_dir : Vector2 = Vector2(-0.02,-0.02);
var water_dir : Vector2 = Vector2(0.0,0.0);
private var animationSpeed : float = 1.0;
//private var m_fFlowMapOffset0 : float = 0.0f;
//private var m_fFlowMapOffset1 : float = 0.0f;
//private var m_fFlowSpeed : float = 0.05f;
//private var m_fCycle : float = 1.0f;
//private var m_fWaveMapScale : float = 2.0f;

private var timex : float = 0.0;
private var timey : float = 0.0;
//private var overShader : Shader;
//private var //underShader : Shader;

//flowmap
private var m_animationSpeed : float = 1.0;
private var systemSpeed : float = 1.0;
private var m_fFlowMapOffset0 : float = 0.0f;
private var m_fFlowMapOffset1 : float = 0.0f;
private var m_fFlowSpeed : float = 0.05f;
private var m_fCycle : float = 1.0f;
private var m_fWaveMapScale : float = 2.0f;
	
	



function Start () {
	
	//REFERENCE MAIN MODULE
	suimonoModuleObject = GameObject.Find("SUIMONO_Module").gameObject;
	reflectionObject = this.gameObject.Find("Suimono_reflectionObject");
	shorelineObject = this.gameObject.Find("Suimono_shorelineObject");
	waveObject = this.gameObject.Find("Suimono_waveObject");
	
	//reflectionObject = this.gameObject.Find(this.gameObject.name+"/Suimono_reflectionObject");
	//reflectionCompo = reflectionObject.gameObject.GetComponent(suimono_reflectionMirror);
	
	renderTex = new RenderTexture(512,512,16,RenderTextureFormat.ARGB32);
	//renderTex = new Texture2D(512,512);
	//underShader = Shader.Find("Suimono2/water_under5");

	//SPLASH & COLLISION SETUP
	objectRingsTime = objectRingsTimes.ToBuiltin(float) as float[];
	
	if (suimonoModuleObject != null){
		moduleSplashObject = suimonoModuleObject.GetComponent(SuimonoModule);
		suimonoModuleLibrary = suimonoModuleObject.GetComponent(SuimonoModuleLib);
	} else {
		Debug.Log("SUIMONO: Warning... SUIMONO_Module game object cannot be found!");
	}
	

	CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];

	//PresetGetData();
	//if (presetIndex != presetUseIndex) PresetLoad();
	PresetLoad();
	
}









function Update(){
	
	
	// get all scene colliders
	//AllColliders = gameObject.FindObjectsOfType(Collider);
	
	//SET SHADER DEFAULTS
	//avoids incompatible shader assignments in various unity/target versions
	//#if UNITY_ANDROID
	//	renderer.sharedMaterial.shader = Shader.Find("Suimono2/water_mobile");
	//#endif
	
	
	//UPDATE PRESETS
	//get the current preset data.
	PresetGetData();
	if (presetIndex != presetUseIndex) PresetLoad();
	if (presetToggleSave) PresetSave("");
	if (presetStartTransition) PresetDoTransition();
	if (!presetStartTransition) presetTransitionCurrent = 0.0;
	
	
	//CONVERT DEGREES to FLOW DIRECTION
	//convert 0-360 degree setting to useable Vector2 data.
	//used for the normal and foam flow/uv scrolling.
	var convertAngle:Vector3;
	if (flow_dir_degrees <= 180.0){
		convertAngle = Vector3.Slerp(Vector3.forward,-Vector3.forward,(flow_dir_degrees)/180.0);
		flow_dir = Vector2(convertAngle.x,convertAngle.z);
	}
	if (flow_dir_degrees > 180.0){
		convertAngle = Vector3.Slerp(-Vector3.forward,Vector3.forward,(flow_dir_degrees-180.0)/180.0);
		flow_dir = Vector2(-convertAngle.x,convertAngle.z);
	}
	
	
	
	
	
	
	//FLOW MAP HANDLING
	m_animationSpeed = 1.0;//= renderer.material.GetFloat("_AnimSpeed");
	m_animationSpeed = Mathf.Clamp(m_animationSpeed,0.0,1.0);
	
	//set speed limits
	setflowSpeed = flowSpeed;//*(this.transform.localScale.x/10.0);
	wave_speed.x = -flow_dir.x*(setflowSpeed*2.0);
	wave_speed.y = -flow_dir.y*(setflowSpeed*2.0);
	//wave_speed.x = Mathf.Clamp(wave_speed.x,-0.5,0.5);
	//wave_speed.y = Mathf.Clamp(wave_speed.y,-0.5,0.5);
	//foam_speed.x = Mathf.Clamp(foam_speed.x,-0.5,0.5);
	//foam_speed.y = Mathf.Clamp(foam_speed.y,-0.5,0.5);
	
	//assign speed to shader
	//renderer.material.SetTextureOffset("_HeightMap",Vector2(wave_speed.x*Time.time*animationSpeed,wave_speed.y*Time.time*animationSpeed));
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_WaveTex",Vector2(wave_speed.x*Time.time*m_animationSpeed,wave_speed.y*Time.time*m_animationSpeed));
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_WaveTex",Vector2(wave_speed.x*Time.time*m_animationSpeed,wave_speed.y*Time.time*m_animationSpeed));
	
	GetComponent.<Renderer>().sharedMaterial.SetFloat("flowOffX",GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_WaveTex").x);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("flowOffY",GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_WaveTex").y);
	
	//renderer.material.SetTextureOffset("_FoamTex",Vector2(foam_speed.x*Time.time*animationSpeed,foam_speed.y*Time.time*animationSpeed));
	//renderer.material.SetTextureOffset("_FoamTex",Vector2(foam_speed.x*Time.time*animationSpeed,foam_speed.y*Time.time*animationSpeed));
	
	//AssignWaveColor
	//if (inheritColor){
		//var wCol : Color = transform.parent.renderer.material.GetColor("_SurfaceColor") * 1.2;
		//wCol.a = 1.0;
		//renderer.material.SetColor("_WaveColor",wCol);
	//}
	
	
	
	
	//FILL MAIN TEXTURES BY DEFAULT
	//fills default texture slots from the Module object
	if (enableCustomTextures == false){
		if (suimonoModuleLibrary){
		if (suimonoModuleLibrary.texDisplace) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveLargeTex",suimonoModuleLibrary.texDisplace);
		if (suimonoModuleLibrary.texHeight1) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_Surface1",suimonoModuleLibrary.texHeight1);
		if (suimonoModuleLibrary.texHeight2) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_Surface2",suimonoModuleLibrary.texHeight2);
		if (suimonoModuleLibrary.texFoam1) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_FoamOverlay",suimonoModuleLibrary.texFoam1);
		if (suimonoModuleLibrary.texFoam2) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_FoamTex",suimonoModuleLibrary.texFoam2);
		if (suimonoModuleLibrary.texRampWave) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveRamp",suimonoModuleLibrary.texRampWave);
		if (suimonoModuleLibrary.texRampDepth) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_DepthRamp",suimonoModuleLibrary.texRampDepth);
		if (suimonoModuleLibrary.texRampBlur) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_BlurRamp",suimonoModuleLibrary.texRampBlur);
		if (suimonoModuleLibrary.texRampFoam) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_FoamRamp",suimonoModuleLibrary.texRampFoam);
		if (suimonoModuleLibrary.texCube1) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_CubeTex",suimonoModuleLibrary.texCube1);
		if (suimonoModuleLibrary.texCube2) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_CubeMobile",suimonoModuleLibrary.texCube2);
		if (suimonoModuleLibrary.texWave) this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveTex",suimonoModuleLibrary.texWave);
		
		
		if (!Application.isPlaying){
			//if (suimonoModuleLibrary.texBlank) this.renderer.sharedMaterial.SetTexture("_ReflectionTex",suimonoModuleLibrary.texBlank);
		}
		
		}
	}
	
	//GET Flowmap Texture
	//this.renderer.sharedMaterial.SetTexture("_WaveMaskTex",gameObject.Find("Suimono_shorelineObject").gameObject.GetComponent(Suimono_shoreGenerator).shoreMapTex);
	
	
	
	//SET TEXTURE SCALE BASED ON SURFACE SIZE
	this.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_Surface1",Vector2(this.transform.localScale.x,this.transform.localScale.z));
	this.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_Surface2",Vector2(this.transform.localScale.x,this.transform.localScale.z));
	
	
	
	//SET SHADER TIME and SCALE
	//var time = Time.time;//Application.isEditor?(float)EditorApplication.timeSinceStartup:Time.time;
    this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_Phase", Time.time );
    this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_dScaleX", this.GetComponent.<Renderer>().sharedMaterial.GetTextureScale("_Surface1").x);
	this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_dScaleY", this.GetComponent.<Renderer>().sharedMaterial.GetTextureScale("_Surface1").y);
	
	//this.renderer.sharedMaterial.SetFloat("_timeX", renderer.sharedMaterial.GetTextureOffset("_Surface1").x);
	


	//TESSELLATION SETTINGS
	this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_Tess", waveTessAmt);
	var setTessStart : float = Mathf.Lerp(-180.0,0.0,waveTessMin);
	this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_minDist", setTessStart);
	var setTessSpread : float = Mathf.Lerp(20.0,500.0,waveTessSpread);
	this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_maxDist", setTessSpread);
	this.GetComponent.<Renderer>().sharedMaterial.SetFloat("_Displacement", 1.0);
	


	//EDITOR MODE TWEAKS
	//certain calculations rely on depth buffer generation which the scene camera
	//won't calculate while in editor mode. The below temporarily addresses these
	//issues so the water surface doesn't look whack in editor mode.  this shouldn't
	//effect the in-game modes at all.
	useFoamColor = foamColor;
	useDepthColor = depthColor;
	useEdgeColor = edgeColor;

	if (!Application.isPlaying){
		useFoamColor.a = 0.0;
		useDepthColor = GetComponent.<Renderer>().sharedMaterial.GetColor("_DepthColorB");
		useDepthColor.a = 0.35;
		useEdgeColor.a = 0.0;
	}
	

	//SPLASH AND COLLISION EFFECTS
	//advance fx timer.
	ringsTime += Time.deltaTime;

	//populate & advance custom ringsTimer
	//for (var cx = 0; cx < CurrentColliders.length; cx++){
		//objectRingsTime[cx] += Time.deltaTime;
	//}
	if (CurrentColliders.length > 0){
		for (var cx = 0; cx < CurrentColliders.length; cx++){
			objectRingsTime[cx] += Time.deltaTime;
		}
	}

	
	
	//Read the collision function
	if (splashIsOn){
		CallCollisionFunction();
	}
	
	
	

	//#######  MANAGE LOCAL REFLECTION TEXTURE  #######
	var renderTex : boolean = true;
	
	#if UNITY_IPHONE
		renderTex = false;
	#endif
	#if UNITY_ANDROID
		renderTex = false;
	#endif
	#if UNITY_EDITOR
	if (!UnityEditor.PlayerSettings.advancedLicense){
		renderTex = false;
	}
	#endif
	if (!renderTex){
		#if !UNITY_3_5
			if (reflectionObject != null) reflectionObject.SetActive(false);
	   	#else
			if (reflectionObject != null) reflectionObject.active = false;
	  	#endif

	   	
	   		
	} else {
	
		if (reflectionObject != null && renderTex){
			//enable reflection based on distance
			var reflDist : float;
			if (moduleSplashObject.setCamera) reflDist = Vector3.Distance(transform.localPosition,moduleSplashObject.setCamera.localPosition);
			if (reflectionObject != null){
			#if !UNITY_3_5
				//unity 4+ compilation
				if (reflDist <= (60.0*transform.localScale.x)) reflectionObject.SetActive(true);
				if (reflDist > (60.0*transform.localScale.x)) reflectionObject.SetActive(false);
	    	#else
	    		//unity 3.5 compilation
				if (reflDist <= (60.0*transform.localScale.x)) reflectionObject.active = true;
				if (reflDist > (60.0*transform.localScale.x)) reflectionObject.active = false;
	   		#endif
			}
							
			//check for underwater
			isUnderwater = false;
			//if (moduleSplashObject.setCamera && moduleSplashObject.setCamera.position.y <= this.transform.position.y) isUnderwater = true;
			var waterLevel : float = moduleSplashObject.GetHeight(moduleSplashObject.setCamera,"wave height");
			if (waterLevel >= 0.0){//this.transform.position.y){// isUnderwater = true;
			
			//if (isUnderwater == true){
				//swap reflection coordinates underwater
				if (reflectionObject != null) reflectionObject.transform.eulerAngles = Vector3(0.0,0.0,180.0);
			} else {
			//if (isUnderwater == false){
				//reset reflection coordinates
				if (reflectionObject != null) reflectionObject.transform.eulerAngles = Vector3(0.0,0.0,0.0);
			}
		}
	}
	
	
	
	if (shorelineObject != null){
			#if !UNITY_3_5
				//unity 4+ compilation
				if (reflDist <= (60.0*transform.localScale.x)) shorelineObject.SetActive(true);
				if (reflDist > (60.0*transform.localScale.x)) shorelineObject.SetActive(false);
	    	#else
	    		//unity 3.5 compilation
				if (reflDist <= (60.0*transform.localScale.x)) shorelineObject.active = true;
				if (reflDist > (60.0*transform.localScale.x)) shorelineObject.active = false;
	   		#endif
	}
	
	if (waveObject != null){
			#if !UNITY_3_5
				//unity 4+ compilation
				if (reflDist <= (60.0*transform.localScale.x)) waveObject.SetActive(true);
				if (reflDist > (60.0*transform.localScale.x)) waveObject.SetActive(false);
	    	#else
	    		//unity 3.5 compilation
				if (reflDist <= (60.0*transform.localScale.x)) waveObject.active = true;
				if (reflDist > (60.0*transform.localScale.x)) waveObject.active = false;
	   		#endif
	}




	// ########## ASSIGN GENERAL ATTRIBUTES ############
	//Set Master Scale
	var useScale : float = (overallScale * 0.2);

	GetComponent.<Renderer>().sharedMaterial.SetFloat("_MasterScale",useScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveAmt",useScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_NormalAmt",useScale*10.0);
	
	
	//Calculate Shore & Wave FX
	var shoreWaveStretch : float = 4.5;
	var shoreWaveStretch2 : float = 0.0;
	shoreAmt = ((1.0-shoreWaveStretch)+Mathf.Sin(Time.time*0.75)*shoreWaveStretch);

	tideAmount = ((0.3)+Mathf.Sin(Time.time*0.45)*0.2);

	
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_ShoreAmt",shoreAmt);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_TideAmount",tideAmount);
	
	
	
	//Calculate Waves
	#if UNITY_EDITOR
	if (EditorApplication.isPlaying){
		GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveHeight",waveHeight*waveFac);
		GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveShoreHeight",waveShoreHeight*waveFac);
	} else {
		GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveHeight",0.0);
		GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveShoreHeight",0.0);	
	}
	#endif
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_WaveScale",waveScale);
	var setFlowScale : float = Mathf.Lerp(0.1,10,waveScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_FlowScale",setFlowScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_MaskAmt",8.0-shoreInfluence);

	GetComponent.<Renderer>().sharedMaterial.SetFloat("_TimeX",GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").x*waveScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_TimeY",GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").y*waveScale);
	timex += Time.deltaTime * waveSpeed;
	timey += Time.deltaTime * waveSpeed;
	//renderer.sharedMaterial.SetFloat("_TimeX",timex);
	//renderer.sharedMaterial.SetFloat("_TimeY",timey);
	
	
	
	
	//Calculate Light Absorption
	var absorbAmt : float = Mathf.Lerp(0.0,25.0,lightAbsorb);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_DepthAmt",absorbAmt);
	
	//Calculate Refraction
	var setSCL :float = transform.localScale.x;
	var refractAmt : float = Mathf.Lerp(0.0,(500.0/setSCL),lightRefract);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_RefrStrength",refractAmt);
	var refractSpd : float = Mathf.Lerp(0.0,(4.0),refractSpeed);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_RefrSpeed",refractSpd);
	var refractScl : float = Mathf.Lerp(0.0,(2.25),refractScale);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_RefrScale",refractScl);
	
	
	//Calculate Reflections
	//if (reflectionObject == null){
	//	renderer.sharedMaterial.SetFloat("useReflection",0.0);
	//} else {
	//	renderer.sharedMaterial.SetFloat("useReflection",1.0);
	//}
	//Calculate Reflections
	if (GetComponent.<Renderer>().sharedMaterial.GetTexture("_ReflectionTex") == null){
		GetComponent.<Renderer>().sharedMaterial.SetFloat("useReflection",0.0);
	} else {
		GetComponent.<Renderer>().sharedMaterial.SetFloat("useReflection",1.0);
	}
	
	
	var reflectDistAmt : float = Mathf.Lerp(-200,200, reflectDist);
	var reflectSpreadAmt : float = Mathf.Lerp(0.015,0.001,reflectSpread);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_ReflDist",reflectDistAmt);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_ReflBlend",reflectSpreadAmt);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_DynReflColor",colorDynReflect);
	var reflectAmt : float = Mathf.Lerp(0.0,40.0,reflectionOffset);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_ReflectStrength",reflectAmt);
	
	
	//Calculate Underwater Reflections
	var reflectUnderDist : float = Mathf.Lerp(-30,0,reflectDistUnderAmt);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_UnderReflDist",reflectUnderDist);

	
	//Calculate Blur
	var blurSprd : float = Mathf.Lerp(0.0,0.125,blurSpread);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_BlurSpread",blurSprd);
	//surface smoothness
	var surfaceSmoothAmt : float = Mathf.Lerp(0.0,1.0,surfaceSmooth);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_BumpStrength",surfaceSmoothAmt);
	
	
	//colors
	//renderer.sharedMaterial.SetFloat("_CenterHeight",transform.position.y+colorHeight);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_CenterHeight",colorHeight);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_MaxVariance",50.0-colorHeightSpread);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_HighColor",colorSurfHigh);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_LowColor",colorSurfLow);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColor",useDepthColor);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorR",depthColorR);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorG",depthColorG);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorB",depthColorB);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_UnderColor",underwaterColor);
	
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_UnderFogDist",underwaterFogDist);
	
	//specular
	GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorH",specColorH);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorL",specColorL);
	var _SpecHotAmt : float = Mathf.Lerp(0.01,0.3,specScatterWidth);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_SpecScatterWidth",_SpecHotAmt);
	var _SpecAmt : float = Mathf.Lerp(0.001,0.2,specScatterAmt);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_SpecScatterAmt",_SpecAmt);
	
	//tide
	GetComponent.<Renderer>().sharedMaterial.SetColor("_TideColor",tideColor);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_TideAmount",tideAmount);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_TideSpread",tideSpread);

	
	// SET FOAM SCALING
	var setFoamScale : float = Mathf.Lerp(0.0001,10.0,foamScale);
	var useFoamScale : float = this.transform.localScale.x*setFoamScale;
	GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_FoamTex",Vector2(useFoamScale,useFoamScale));
	GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_FoamOverlay",Vector2(useFoamScale,useFoamScale));
	
	
	
	var foamSpread : float = (foamAmt) + (Mathf.Sin(Time.time*1.0)*(0.05 * (1.0+foamAmt)));

	//var useFoamHt : float = transform.position.y + hFoamHeight;
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_FoamHeight", hFoamHeight);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_HeightFoamAmount", hFoamAmt);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_HeightFoamSpread", 50.0-hFoamSpread);
	
	var setFoamSpread : float = Mathf.Lerp(0.02,1.0,foamSpread);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_FoamSpread",setFoamSpread);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_FoamColor",useFoamColor);
	
	var setEdgeSpread : float = Mathf.Lerp(0.02,1.0*transform.localScale.x,edgeSpread);
	GetComponent.<Renderer>().sharedMaterial.SetFloat("_EdgeSpread",setEdgeSpread);
	GetComponent.<Renderer>().sharedMaterial.SetColor("_EdgeColor",useEdgeColor);


	
	//WAVE TEXTURE ANIMATION
	animationSpeed = 1.0/(transform.localScale.x/20.0);
	
	
	//set speed limits
	flow_dir.x = Mathf.Clamp(flow_dir.x,-1.0,1.0);
	flow_dir.y = Mathf.Clamp(flow_dir.y,-1.0,1.0);
	
	wave_dir.x = flow_dir.x;
	wave_dir.y = flow_dir.y;

	foam_dir.x = flow_dir.x;
	foam_dir.y = flow_dir.y;
	
	//assign speed to shader
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_FoamTex",Vector2(foam_dir.x*Time.time*animationSpeed * foamSpeed,foam_dir.y*Time.time*animationSpeed * foamSpeed));
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_FoamOverlay",Vector2(-foam_dir.x*Time.time*animationSpeed * foamSpeed *0.5,-foam_dir.y*Time.time*animationSpeed * foamSpeed *0.5));
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_Surface1",Vector2(flow_dir.x*Time.time*animationSpeed*setflowSpeed,flow_dir.y*Time.time*animationSpeed * setflowSpeed));
	GetComponent.<Renderer>().sharedMaterial.SetTextureOffset("_Surface2",Vector2(flow_dir.x*Time.time*-animationSpeed*setflowSpeed*0.5,flow_dir.y*Time.time*animationSpeed * -setflowSpeed));
	
	//underwater waves
	//EffectRefractObject.renderer.sharedMaterial.SetTextureOffset("_Surface2",Vector2(-flow_dir.x*Time.time*(animationSpeed * setflowSpeed *0.05),flow_dir.y*Time.time*(animationSpeed * setflowSpeed * 0.25)));
	
	
	


}















// ###################################################################
// ##### START CUSTOM FUNCTIONS ######################################
// ###################################################################









// ########## SPLASH / COLLISION FUNCTIONS ##########

// Note this only works by objects intersecting the actual collision
// mesh which is not deformed along with the waves! 
function OnTriggerEnter(other : Collider) {
	CurrentCollider.Add(other);
	objectRingsTimes.Add(0.0);
	objectRingsTime = objectRingsTimes.ToBuiltin(float) as float[];
	CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];										
}

function OnTriggerStay  (other : Collider) {
	isinwater = true;
	atDepth = this.gameObject.transform.position.y - other.gameObject.transform.position.y ;
}

function OnTriggerExit  (other : Collider) {
	CurrentCollider.Remove(other);
	CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];
}



//function checkColliders(){
	//for (coll as Collider in AllColliders){ 
		//check bounds against wavespace point
		//add or remove colliders from intersection system
	//}
//}




function CallCollisionFunction(){

	//var hitVeloc : float = 0.0;// : Vector3 = Vector3(0.0,0.0,0.0);
	
	//for (var cx = 0; cx < CurrentColliders.length; cx++){
		//var ckSpeed = UpdateSpeed;
		//if (CurrentColliders[cx] == null){
			//CurrentColliders.RemoveAt(cx);
	for (var cx : int = 0; cx < CurrentColliders.length; cx++){
		var ckSpeed = UpdateSpeed;
		if (CurrentColliders[cx] == null){
			CurrentCollider.RemoveAt(cx);
			CurrentColliders = CurrentCollider.ToBuiltin(Collider) as Collider[];
		} else {
		
			//get optional parameters
			var alwaysEmit : boolean = false;
			var addSize : float = 0.17;
			var addRot : float = Random.Range(0.0,359.0);
			
			
			var hitVeloc = Vector3(1.0,1.0,1.0);
			
			if (CurrentColliders[cx].gameObject.GetComponent.<Rigidbody>()){
				hitVeloc = CurrentColliders[cx].gameObject.GetComponent.<Rigidbody>().velocity;
			}
			
			//calculate rotation
			if (Mathf.Abs(hitVeloc.x) >= rippleSensitivity || Mathf.Abs(hitVeloc.z) >= rippleSensitivity){
				var tempPointer : GameObject;
				var tempDetector : GameObject;
				tempPointer = new GameObject ("tempPointer");
				tempDetector = new GameObject ("tempDetector");
				
				tempPointer.transform.position = CurrentColliders[cx].transform.position + (hitVeloc*10.0);
				tempDetector.transform.position = CurrentColliders[cx].transform.position;
				tempDetector.transform.LookAt(tempPointer.transform.position);
				addRot = tempDetector.transform.eulerAngles.y+40.0;
				if (Mathf.Abs(hitVeloc.x) > 2.4 || Mathf.Abs(hitVeloc.z) > 2.4){
					//ckSpeed = UpdateSpeed*0.5;
				}
				
				gameObject.Destroy(tempPointer);
				gameObject.Destroy(tempDetector);
			}
				
				
			if (CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects) != null){
				ckSpeed = CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects).setSplashRingsTimer;
				addSize = CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects).splashRingsSize;
				if (CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects).splashRingsRotation != 0.0){
					addRot = CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects).splashRingsRotation;
				}
				if (CurrentColliders[cx].gameObject.GetComponent(fx_splashEffects).alwaysEmitRipples || moduleSplashObject.alwaysEmitRipples){
					alwaysEmit = true;
				}
				
			}
			if (objectRingsTime[cx] > ckSpeed){
				objectRingsTime[cx] = 0.0;
				var checkColl : Collider = CurrentColliders[cx];
				var collsetpos : Vector3 = checkColl.transform.position;
				
				//calculate Y-Height
				collsetpos.y = this.transform.position.y+0.01;
				var layer : int = 4;
				var layermask : int = 1 << layer;
				var hit : RaycastHit;
				var testpos : Vector3 = Vector3(collsetpos.x,collsetpos.y+5000.0,collsetpos.z);
				if (Physics.Raycast (testpos, -Vector3.up, hit,10000,layermask)) {
					if (hit.transform.gameObject.layer==4){
						if (hit.transform == transform){
							collsetpos.y = hit.point.y+0.01;
						}
					}
				}
				

				
				var sizeScale : float = CurrentColliders[cx].gameObject.transform.localScale.x;
				

				//init splash ring effect
				if (Mathf.Abs(hitVeloc.x) >= rippleSensitivity || Mathf.Abs(hitVeloc.z ) >= rippleSensitivity){
					moduleSplashObject.AddEffect("rings",collsetpos,1,addSize*sizeScale,addRot);
				} else {
					if (alwaysEmit){
					moduleSplashObject.AddEffect("rings",collsetpos,1,addSize*sizeScale,addRot);
					}
				}
				
				//check for movement and init splash effects
				if (Mathf.Abs(hitVeloc.x) >= splashSensitivity || Mathf.Abs(hitVeloc.z) >= splashSensitivity){
					moduleSplashObject.AddEffect("ringfoam",collsetpos,1,addSize*(sizeScale*0.5),addRot);
					moduleSplashObject.AddEffect("splash",collsetpos,10,addSize*sizeScale,addRot);
					moduleSplashObject.AddEffect("splashDrop",collsetpos,40,addSize,0.0);
				
				//add sound
					moduleSplashObject.AddSound("splash",collsetpos,hitVeloc);
				}

			}
		}
	}
	
}












// ########## PRESET FUNCTIONS ##########

function PresetLoad(){
	presetUseIndex = presetIndex;

	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetUseIndex) break;
	}
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set colors
	var sK : int = 21;
	depthColor = Color(float.Parse(workData.Substring((sK*1)+1,4)),float.Parse(workData.Substring((sK*1)+6,4)),float.Parse(workData.Substring((sK*1)+11,4)),float.Parse(workData.Substring((sK*1)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColor",depthColor);
	colorSurfHigh = Color(float.Parse(workData.Substring((sK*2)+1,4)),float.Parse(workData.Substring((sK*2)+6,4)),float.Parse(workData.Substring((sK*2)+11,4)),float.Parse(workData.Substring((sK*2)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_HighColor",colorSurfHigh);
	colorSurfLow = Color(float.Parse(workData.Substring((sK*3)+1,4)),float.Parse(workData.Substring((sK*3)+6,4)),float.Parse(workData.Substring((sK*3)+11,4)),float.Parse(workData.Substring((sK*3)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_LowColor",colorSurfLow);
	depthColorR = Color(float.Parse(workData.Substring((sK*4)+1,4)),float.Parse(workData.Substring((sK*4)+6,4)),float.Parse(workData.Substring((sK*4)+11,4)),float.Parse(workData.Substring((sK*4)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorR",depthColorR);
	depthColorG = Color(float.Parse(workData.Substring((sK*5)+1,4)),float.Parse(workData.Substring((sK*5)+6,4)),float.Parse(workData.Substring((sK*5)+11,4)),float.Parse(workData.Substring((sK*5)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorG",depthColorG);
	depthColorB = Color(float.Parse(workData.Substring((sK*6)+1,4)),float.Parse(workData.Substring((sK*6)+6,4)),float.Parse(workData.Substring((sK*6)+11,4)),float.Parse(workData.Substring((sK*6)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorB",depthColorB);		
	specColorH = Color(float.Parse(workData.Substring((sK*7)+1,4)),float.Parse(workData.Substring((sK*7)+6,4)),float.Parse(workData.Substring((sK*7)+11,4)),float.Parse(workData.Substring((sK*7)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorH",specColorH);
	specColorL = Color(float.Parse(workData.Substring((sK*8)+1,4)),float.Parse(workData.Substring((sK*8)+6,4)),float.Parse(workData.Substring((sK*8)+11,4)),float.Parse(workData.Substring((sK*8)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorL",specColorL);
	colorDynReflect = Color(float.Parse(workData.Substring((sK*9)+1,4)),float.Parse(workData.Substring((sK*9)+6,4)),float.Parse(workData.Substring((sK*9)+11,4)),float.Parse(workData.Substring((sK*9)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DynReflColor",colorDynReflect);
	foamColor = Color(float.Parse(workData.Substring((sK*10)+1,4)),float.Parse(workData.Substring((sK*10)+6,4)),float.Parse(workData.Substring((sK*10)+11,4)),float.Parse(workData.Substring((sK*10)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_FoamColor",useFoamColor);
	edgeColor = Color(float.Parse(workData.Substring((sK*11)+1,4)),float.Parse(workData.Substring((sK*11)+6,4)),float.Parse(workData.Substring((sK*11)+11,4)),float.Parse(workData.Substring((sK*11)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_EdgeColor",useEdgeColor);			
	underwaterColor = Color(float.Parse(workData.Substring((sK*12)+1,4)),float.Parse(workData.Substring((sK*12)+6,4)),float.Parse(workData.Substring((sK*12)+11,4)),float.Parse(workData.Substring((sK*12)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_UnderColor",underwaterColor);	
	tideColor = Color(float.Parse(workData.Substring((sK*13)+1,4)),float.Parse(workData.Substring((sK*13)+6,4)),float.Parse(workData.Substring((sK*13)+11,4)),float.Parse(workData.Substring((sK*13)+16,4)));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_TideColor",tideColor);	
	
	//set attributes
	//overallScale = float.Parse(workData.Substring((sK*13)+1,6));
	lightAbsorb = float.Parse(workData.Substring((sK*14)+(8*1)+1,6));
	lightRefract = float.Parse(workData.Substring((sK*14)+(8*2)+1,6));
	refractSpeed = float.Parse(workData.Substring((sK*14)+(8*3)+1,6));
	refractScale = float.Parse(workData.Substring((sK*14)+(8*4)+1,6));

	blurSpread = float.Parse(workData.Substring((sK*14)+(8*5)+1,6));
	surfaceSmooth = float.Parse(workData.Substring((sK*14)+(8*6)+1,6));
	reflectDist = float.Parse(workData.Substring((sK*14)+(8*7)+1,6));
	reflectSpread = float.Parse(workData.Substring((sK*14)+(8*8)+1,6));
	reflectionOffset = float.Parse(workData.Substring((sK*14)+(8*9)+1,6));
	colorHeight = float.Parse(workData.Substring((sK*14)+(8*10)+1,6));
	colorHeightSpread = float.Parse(workData.Substring((sK*14)+(8*11)+1,6));
	specScatterAmt = float.Parse(workData.Substring((sK*14)+(8*12)+1,6));
	specScatterWidth = float.Parse(workData.Substring((sK*14)+(8*13)+1,6));
	hFoamHeight = float.Parse(workData.Substring((sK*14)+(8*14)+1,6));
	hFoamAmt = float.Parse(workData.Substring((sK*14)+(8*15)+1,6));
	hFoamSpread = float.Parse(workData.Substring((sK*14)+(8*16)+1,6));
	foamAmt = float.Parse(workData.Substring((sK*14)+(8*17)+1,6));
	foamScale = float.Parse(workData.Substring((sK*14)+(8*18)+1,6));
	edgeSpread = float.Parse(workData.Substring((sK*14)+(8*19)+1,6));
	flowSpeed = float.Parse(workData.Substring((sK*14)+(8*20)+1,6));
	foamSpeed = float.Parse(workData.Substring((sK*14)+(8*21)+1,6));
	UpdateSpeed = float.Parse(workData.Substring((sK*14)+(8*22)+1,6));
	rippleSensitivity = float.Parse(workData.Substring((sK*14)+(8*23)+1,6));
	splashSensitivity = float.Parse(workData.Substring((sK*14)+(8*24)+1,6));
	reflectDistUnderAmt = float.Parse(workData.Substring((sK*14)+(8*25)+1,6));
	underRefractionAmount = float.Parse(workData.Substring((sK*14)+(8*26)+1,6));
	underBlurAmount = float.Parse(workData.Substring((sK*14)+(8*27)+1,6));
	etherealShift = float.Parse(workData.Substring((sK*14)+(8*28)+1,6));

	underwaterFogDist = float.Parse(workData.Substring((sK*14)+(8*29)+1,6));	
	underwaterFogSpread = float.Parse(workData.Substring((sK*14)+(8*30)+1,6));

	waveHeight = float.Parse(workData.Substring((sK*14)+(8*31)+1,6));
	waveShoreHeight = float.Parse(workData.Substring((sK*14)+(8*32)+1,6));
	waveScale = float.Parse(workData.Substring((sK*14)+(8*33)+1,6));		
											
	waveTessAmt = float.Parse(workData.Substring((sK*14)+(8*34)+1,6));	
	waveTessMin = float.Parse(workData.Substring((sK*14)+(8*35)+1,6));	
	waveTessSpread = float.Parse(workData.Substring((sK*14)+(8*36)+1,6));	
        
	tideAmount = float.Parse(workData.Substring((sK*14)+(8*37)+1,6));	
    tideSpread = float.Parse(workData.Substring((sK*14)+(8*38)+1,6));	
        




}




/*
function PresetGetComponent( presetCheck : int, presetKey : String){
	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetCheck) break;
	}
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set colors
	var sK : int = 21;
	if (presetKey == "_DepthColor") return Color(float.Parse(workData.Substring(sK+1,4)),float.Parse(workData.Substring(sK+6,4)),float.Parse(workData.Substring(sK+11,4)),float.Parse(workData.Substring(sK+16,4)));
	if (presetKey == "_HighColor") return Color(float.Parse(workData.Substring((sK*2),4)),float.Parse(workData.Substring((sK*2)+6,4)),float.Parse(workData.Substring((sK*2)+11,4)),float.Parse(workData.Substring((sK*2)+16,4)));
	if (presetKey == "_LowColor") return Color(float.Parse(workData.Substring((sK*3),4)),float.Parse(workData.Substring((sK*3)+6,4)),float.Parse(workData.Substring((sK*3)+11,4)),float.Parse(workData.Substring((sK*3)+16,4)));
	if (presetKey == "_DepthColorR") return Color(float.Parse(workData.Substring((sK*4),4)),float.Parse(workData.Substring((sK*4)+6,4)),float.Parse(workData.Substring((sK*4)+11,4)),float.Parse(workData.Substring((sK*4)+16,4)));
	if (presetKey == "_DepthColorG") return Color(float.Parse(workData.Substring((sK*5),4)),float.Parse(workData.Substring((sK*5)+6,4)),float.Parse(workData.Substring((sK*5)+11,4)),float.Parse(workData.Substring((sK*5)+16,4)));
	if (presetKey == "_DepthColorB") return Color(float.Parse(workData.Substring((sK*6),4)),float.Parse(workData.Substring((sK*6)+6,4)),float.Parse(workData.Substring((sK*6)+11,4)),float.Parse(workData.Substring((sK*6)+16,4)));
	if (presetKey == "_SpecColorH") return Color(float.Parse(workData.Substring((sK*7),4)),float.Parse(workData.Substring((sK*7)+6,4)),float.Parse(workData.Substring((sK*7)+11,4)),float.Parse(workData.Substring((sK*7)+16,4)));
	if (presetKey == "_SpecColorL") return Color(float.Parse(workData.Substring((sK*8),4)),float.Parse(workData.Substring((sK*8)+6,4)),float.Parse(workData.Substring((sK*8)+11,4)),float.Parse(workData.Substring((sK*8)+16,4)));
	if (presetKey == "_DynReflColor") return Color(float.Parse(workData.Substring((sK*9),4)),float.Parse(workData.Substring((sK*9)+6,4)),float.Parse(workData.Substring((sK*9)+11,4)),float.Parse(workData.Substring((sK*9)+16,4)));
	if (presetKey == "_FoamColor") return Color(float.Parse(workData.Substring((sK*10),4)),float.Parse(workData.Substring((sK*10)+6,4)),float.Parse(workData.Substring((sK*10)+11,4)),float.Parse(workData.Substring((sK*10)+16,4)));
	if (presetKey == "_EdgeColor") return Color(float.Parse(workData.Substring((sK*11),4)),float.Parse(workData.Substring((sK*11)+6,4)),float.Parse(workData.Substring((sK*11)+11,4)),float.Parse(workData.Substring((sK*11)+16,4)));
	if (presetKey == "_UnderwaterColor") return Color(float.Parse(workData.Substring((sK*12),4)),float.Parse(workData.Substring((sK*12)+6,4)),float.Parse(workData.Substring((sK*12)+11,4)),float.Parse(workData.Substring((sK*12)+16,4)));

	//set attributes
	if (presetKey == "_MasterScale") return float.Parse(workData.Substring((sK*13)+1,6));
	if (presetKey == "_LightAbsorb") return float.Parse(workData.Substring((sK*13)+(8*1),6));
	if (presetKey == "_LightRefract") return float.Parse(workData.Substring((sK*13)+(8*2),6));
	if (presetKey == "_RefractSpeed") return float.Parse(workData.Substring((sK*13)+(8*3),6));
	if (presetKey == "_RefractScale") return float.Parse(workData.Substring((sK*13)+(8*4),6));
	if (presetKey == "_BlurSpread") return float.Parse(workData.Substring((sK*13)+(8*5),6));
	if (presetKey == "_SurfaceSmooth") return float.Parse(workData.Substring((sK*13)+(8*6),6));
	if (presetKey == "_ReflectDist") return float.Parse(workData.Substring((sK*13)+(8*7),6));
	if (presetKey == "_ReflectSpread") return float.Parse(workData.Substring((sK*13)+(8*8),6));
	if (presetKey == "_ReflectionOffset") return float.Parse(workData.Substring((sK*13)+(8*9),6));
	if (presetKey == "_ColorHeight") return float.Parse(workData.Substring((sK*13)+(8*10),6));
	if (presetKey == "_ColorHeightSpread") return float.Parse(workData.Substring((sK*13)+(8*11),6));
	if (presetKey == "_SpecScatterAmt") return float.Parse(workData.Substring((sK*13)+(8*12),6));
	if (presetKey == "_SpecScatterWidth") return float.Parse(workData.Substring((sK*13)+(8*13),6));
	if (presetKey == "_HFoamHeight") return float.Parse(workData.Substring((sK*13)+(8*14),6));
	if (presetKey == "_HFoamAmt") return float.Parse(workData.Substring((sK*13)+(8*15),6));
	if (presetKey == "_HFoamSpread") return float.Parse(workData.Substring((sK*13)+(8*16),6));
	if (presetKey == "_FoamAmt") return float.Parse(workData.Substring((sK*13)+(8*17),6));
	if (presetKey == "_FoamScale") return float.Parse(workData.Substring((sK*13)+(8*18),6));
	if (presetKey == "_EdgeSpread") return float.Parse(workData.Substring((sK*13)+(8*19),6));
	if (presetKey == "_WaveSpeed") return float.Parse(workData.Substring((sK*13)+(8*20),6));
	if (presetKey == "_FoamSpeed") return float.Parse(workData.Substring((sK*13)+(8*21),6));
	if (presetKey == "_UpdateSpeed") return float.Parse(workData.Substring((sK*13)+(8*22),6));
	if (presetKey == "_RippleSensitivity") return float.Parse(workData.Substring((sK*13)+(8*23),6));
	if (presetKey == "_SplashSensitivity") return float.Parse(workData.Substring((sK*13)+(8*24),6));
	if (presetKey == "_ReflectDistUnderAmt") return float.Parse(workData.Substring((sK*13)+(8*25),6));
	if (presetKey == "_UnderRefractionAmount") return float.Parse(workData.Substring((sK*13)+(8*26),6));
	if (presetKey == "_UnderBlurAmount") return float.Parse(workData.Substring((sK*13)+(8*27),6));
	if (presetKey == "_EtherealShift") return float.Parse(workData.Substring((sK*13)+(8*28),6));
	
	if (presetKey == "_UnderwaterFogDist") return float.Parse(workData.Substring((sK*13)+(8*29),6));
	if (presetKey == "_UnderwaterFogSpread") return float.Parse(workData.Substring((sK*13)+(8*30),6));

	if (presetKey == "_WaveHeight") return float.Parse(workData.Substring((sK*13)+(8*31),6));
	if (presetKey == "_WaveShoreHeight") return float.Parse(workData.Substring((sK*13)+(8*32),6));
	if (presetKey == "_WaveScale") return float.Parse(workData.Substring((sK*13)+(8*33),6));

	if (presetKey == "_WaveTessAmt") return float.Parse(workData.Substring((sK*13)+(8*34),6));
	if (presetKey == "_WaveTessMin") return float.Parse(workData.Substring((sK*13)+(8*35),6));
	if (presetKey == "_WaveTessSpread") return float.Parse(workData.Substring((sK*13)+(8*36),6));

	
	
}
*/







function PresetGetColor( presetCheck : int, presetKey : String) : Color {
	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetCheck) break;
	}
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set colors
	var sK : int = 21;
	if (presetKey == "_DepthColor") return Color(float.Parse(workData.Substring(sK+1,4)),float.Parse(workData.Substring(sK+6,4)),float.Parse(workData.Substring(sK+11,4)),float.Parse(workData.Substring(sK+16,4)));
	if (presetKey == "_HighColor") return Color(float.Parse(workData.Substring((sK*2),4)),float.Parse(workData.Substring((sK*2)+6,4)),float.Parse(workData.Substring((sK*2)+11,4)),float.Parse(workData.Substring((sK*2)+16,4)));
	if (presetKey == "_LowColor") return Color(float.Parse(workData.Substring((sK*3),4)),float.Parse(workData.Substring((sK*3)+6,4)),float.Parse(workData.Substring((sK*3)+11,4)),float.Parse(workData.Substring((sK*3)+16,4)));
	if (presetKey == "_DepthColorR") return Color(float.Parse(workData.Substring((sK*4),4)),float.Parse(workData.Substring((sK*4)+6,4)),float.Parse(workData.Substring((sK*4)+11,4)),float.Parse(workData.Substring((sK*4)+16,4)));
	if (presetKey == "_DepthColorG") return Color(float.Parse(workData.Substring((sK*5),4)),float.Parse(workData.Substring((sK*5)+6,4)),float.Parse(workData.Substring((sK*5)+11,4)),float.Parse(workData.Substring((sK*5)+16,4)));
	if (presetKey == "_DepthColorB") return Color(float.Parse(workData.Substring((sK*6),4)),float.Parse(workData.Substring((sK*6)+6,4)),float.Parse(workData.Substring((sK*6)+11,4)),float.Parse(workData.Substring((sK*6)+16,4)));
	if (presetKey == "_SpecColorH") return Color(float.Parse(workData.Substring((sK*7),4)),float.Parse(workData.Substring((sK*7)+6,4)),float.Parse(workData.Substring((sK*7)+11,4)),float.Parse(workData.Substring((sK*7)+16,4)));
	if (presetKey == "_SpecColorL") return Color(float.Parse(workData.Substring((sK*8),4)),float.Parse(workData.Substring((sK*8)+6,4)),float.Parse(workData.Substring((sK*8)+11,4)),float.Parse(workData.Substring((sK*8)+16,4)));
	if (presetKey == "_DynReflColor") return Color(float.Parse(workData.Substring((sK*9),4)),float.Parse(workData.Substring((sK*9)+6,4)),float.Parse(workData.Substring((sK*9)+11,4)),float.Parse(workData.Substring((sK*9)+16,4)));
	if (presetKey == "_FoamColor") return Color(float.Parse(workData.Substring((sK*10),4)),float.Parse(workData.Substring((sK*10)+6,4)),float.Parse(workData.Substring((sK*10)+11,4)),float.Parse(workData.Substring((sK*10)+16,4)));
	if (presetKey == "_EdgeColor") return Color(float.Parse(workData.Substring((sK*11),4)),float.Parse(workData.Substring((sK*11)+6,4)),float.Parse(workData.Substring((sK*11)+11,4)),float.Parse(workData.Substring((sK*11)+16,4)));
	if (presetKey == "_UnderwaterColor") return Color(float.Parse(workData.Substring((sK*12),4)),float.Parse(workData.Substring((sK*12)+6,4)),float.Parse(workData.Substring((sK*12)+11,4)),float.Parse(workData.Substring((sK*12)+16,4)));
	if (presetKey == "_TideColor") return Color(float.Parse(workData.Substring((sK*13),4)),float.Parse(workData.Substring((sK*13)+6,4)),float.Parse(workData.Substring((sK*13)+11,4)),float.Parse(workData.Substring((sK*13)+16,4)));



	
    
}




function PresetGetFloat( presetCheck : int, presetKey : String) : float {
	var workData : String;
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		if (px == presetCheck) break;
	}
	
	//set data
	var pName : String = workData.Substring(0,20);

	//set attributes
	var sK : int = 21;
	if (presetKey == "_MasterScale") return float.Parse(workData.Substring((sK*14)+1,6));
	if (presetKey == "_LightAbsorb") return float.Parse(workData.Substring((sK*14)+(8*1),6));
	if (presetKey == "_LightRefract") return float.Parse(workData.Substring((sK*14)+(8*2),6));
	if (presetKey == "_RefractSpeed") return float.Parse(workData.Substring((sK*14)+(8*3),6));
	if (presetKey == "_RefractScale") return float.Parse(workData.Substring((sK*14)+(8*4),6));
	if (presetKey == "_BlurSpread") return float.Parse(workData.Substring((sK*14)+(8*5),6));
	if (presetKey == "_SurfaceSmooth") return float.Parse(workData.Substring((sK*14)+(8*6),6));
	if (presetKey == "_ReflectDist") return float.Parse(workData.Substring((sK*14)+(8*7),6));
	if (presetKey == "_ReflectSpread") return float.Parse(workData.Substring((sK*14)+(8*8),6));
	if (presetKey == "_ReflectionOffset") return float.Parse(workData.Substring((sK*14)+(8*9),6));
	if (presetKey == "_ColorHeight") return float.Parse(workData.Substring((sK*14)+(8*10),6));
	if (presetKey == "_ColorHeightSpread") return float.Parse(workData.Substring((sK*14)+(8*11),6));
	if (presetKey == "_SpecScatterAmt") return float.Parse(workData.Substring((sK*14)+(8*12),6));
	if (presetKey == "_SpecScatterWidth") return float.Parse(workData.Substring((sK*14)+(8*13),6));
	if (presetKey == "_HFoamHeight") return float.Parse(workData.Substring((sK*14)+(8*14),6));
	if (presetKey == "_HFoamAmt") return float.Parse(workData.Substring((sK*14)+(8*15),6));
	if (presetKey == "_HFoamSpread") return float.Parse(workData.Substring((sK*14)+(8*16),6));
	if (presetKey == "_FoamAmt") return float.Parse(workData.Substring((sK*14)+(8*17),6));
	if (presetKey == "_FoamScale") return float.Parse(workData.Substring((sK*14)+(8*18),6));
	if (presetKey == "_EdgeSpread") return float.Parse(workData.Substring((sK*14)+(8*19),6));
	if (presetKey == "_WaveSpeed") return float.Parse(workData.Substring((sK*14)+(8*20),6));
	if (presetKey == "_FoamSpeed") return float.Parse(workData.Substring((sK*14)+(8*21),6));
	if (presetKey == "_UpdateSpeed") return float.Parse(workData.Substring((sK*14)+(8*22),6));
	if (presetKey == "_RippleSensitivity") return float.Parse(workData.Substring((sK*14)+(8*23),6));
	if (presetKey == "_SplashSensitivity") return float.Parse(workData.Substring((sK*14)+(8*24),6));
	if (presetKey == "_ReflectDistUnderAmt") return float.Parse(workData.Substring((sK*14)+(8*25),6));
	if (presetKey == "_UnderRefractionAmount") return float.Parse(workData.Substring((sK*14)+(8*26),6));
	if (presetKey == "_UnderBlurAmount") return float.Parse(workData.Substring((sK*14)+(8*27),6));
	if (presetKey == "_EtherealShift") return float.Parse(workData.Substring((sK*14)+(8*28),6));
	
	if (presetKey == "_UnderwaterFogDist") return float.Parse(workData.Substring((sK*14)+(8*29),6));
	if (presetKey == "_UnderwaterFogSpread") return float.Parse(workData.Substring((sK*14)+(8*30),6));

	if (presetKey == "_WaveHeight") return float.Parse(workData.Substring((sK*14)+(8*31),6));
	if (presetKey == "_WaveShoreHeight") return float.Parse(workData.Substring((sK*14)+(8*32),6));
	if (presetKey == "_WaveScale") return float.Parse(workData.Substring((sK*14)+(8*33),6));

	if (presetKey == "_WaveTessAmt") return float.Parse(workData.Substring((sK*14)+(8*34),6));
	if (presetKey == "_WaveTessMin") return float.Parse(workData.Substring((sK*14)+(8*35),6));
	if (presetKey == "_WaveTessSpread") return float.Parse(workData.Substring((sK*14)+(8*36),6));

	if (presetKey == "_TideAmount") return float.Parse(workData.Substring((sK*14)+(8*37),6));
	if (presetKey == "_TideSpread") return float.Parse(workData.Substring((sK*14)+(8*38),6));

	
}


	
	

function PresetSave( useName : String ){
	
	var pName : String;
	pName = useName;
	presetToggleSave = false;
	var workCol : Color;
	//var pName : String = presetSaveName;
	var pL : int = pName.Length;
	
	PresetGetData();
	
	//check name
	if (pName == "") pName = "my custom preset"+(presetDataArray.length+1);
	if (pL < 20) pName = pName.PadRight(20);
	if (pL > 20) pName = pName.Substring(0,20);

	//SET COLORS
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_DepthColor");
	var useDepthCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_HighColor");
	var useHighCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_LowColor");
	var useLowCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_DepthColorR");
	var useDepthColR : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_DepthColorG");
	var useDepthColG : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_DepthColorB");
	var useDepthColB : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";	
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_SpecColorH");
	var useSpecColorH : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_SpecColorL");
	var useSpecColorL : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_DynReflColor");
	var useDynRefCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_FoamColor");
	var useFoamCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = this.GetComponent.<Renderer>().sharedMaterial.GetColor("_EdgeColor");
	var useEdgeCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = underwaterColor;
	var useUnderCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	workCol = tideColor;
	var useTideCol : String = "("+workCol.r.ToString("0.00")+","+workCol.g.ToString("0.00")+","+workCol.b.ToString("0.00")+","+workCol.a.ToString("0.00")+")";
	
	
	
	//SET ATTRIBUTES
	var useMScale : String = "("+overallScale.ToString("00.000")+")";
	var useAbsorb : String = "("+lightAbsorb.ToString("00.000")+")";
	var useRefractAmt : String = "("+lightRefract.ToString("00.000")+")";
	var userefractSpeed : String = "("+refractSpeed.ToString("00.000")+")";
	var userefractScale : String = "("+refractScale.ToString("00.000")+")";
	var useblurSpread : String = "("+blurSpread.ToString("00.000")+")";
	var usesurfaceSmooth : String = "("+surfaceSmooth.ToString("00.000")+")";
	var usereflectDist : String = "("+reflectDist.ToString("00.000")+")";
	var usereflectSpread : String = "("+reflectSpread.ToString("00.000")+")";
	var usereflectionOffset : String = "("+reflectionOffset.ToString("00.000")+")";
	var usecolorHeight : String = "("+colorHeight.ToString("00.000")+")";
	var usecolorHeightSpread : String = "("+colorHeightSpread.ToString("00.000")+")";
	var usespecScatterAmt : String = "("+specScatterAmt.ToString("00.000")+")";
	var usespecScatterWidth : String = "("+specScatterWidth.ToString("00.000")+")";
	var usehFoamHeight : String = "("+hFoamHeight.ToString("00.000")+")";
	var usehFoamAmt : String = "("+hFoamAmt.ToString("00.000")+")";
	var usehFoamSpread : String = "("+hFoamSpread.ToString("00.000")+")";
	var usefoamAmt : String = "("+foamAmt.ToString("00.000")+")";
	var usefoamScale : String = "("+foamScale.ToString("00.000")+")";
	var useedge : String = "("+edgeSpread.ToString("00.000")+")";
	var useflowSpeed : String = "("+flowSpeed.ToString("00.000")+")";
	var usefoamSpeed : String = "("+foamSpeed.ToString("00.000")+")";
	var useUpdateSpeed : String = "("+UpdateSpeed.ToString("00.000")+")";
	var userippleSensitivity : String = "("+rippleSensitivity.ToString("00.000")+")";
	var usesplashSensitivity : String = "("+splashSensitivity.ToString("00.000")+")";
	var usereflectDistUnderAmt : String = "("+reflectDistUnderAmt.ToString("00.000")+")";
	var useunderRefractionAmount : String = "("+underRefractionAmount.ToString("00.000")+")";
	var useunderBlurAmount : String = "("+underBlurAmount.ToString("00.000")+")";
	var useetherealShift : String = "("+etherealShift.ToString("00.000")+")";
	var useunderwaterFogDist : String = "("+underwaterFogDist.ToString("00.000")+")";
	var useunderwaterFogSpread : String = "("+underwaterFogSpread.ToString("00.000")+")";																																						

	var usewaveHeight : String = "("+waveHeight.ToString("00.000")+")";	
	var usewaveShoreHeight : String = "("+waveShoreHeight.ToString("00.000")+")";	
	var usewaveScale : String = "("+waveScale.ToString("00.000")+")";	

	var usewaveTessAmt : String = "("+waveTessAmt.ToString("00.000")+")";
	var usewaveTessMin : String = "("+waveTessMin.ToString("00.000")+")";
	var usewaveTessSpread : String = "("+waveTessSpread.ToString("00.000")+")";

	var useTideAmount : String = "("+tideAmount.ToString("00.000")+")";
	var useTideSpread : String = "("+tideSpread.ToString("00.000")+")";




	//SAVE DATA																																																																																																																									
	var saveData : String = pName+" "+useDepthCol+useHighCol+useLowCol+useDepthColR+useDepthColG+useDepthColB+useSpecColorH+useSpecColorL+useDynRefCol+useFoamCol+useEdgeCol+useUnderCol+useTideCol;
	saveData += useMScale+useAbsorb+useRefractAmt+userefractSpeed+userefractScale+useblurSpread+usesurfaceSmooth+usereflectDist+usereflectSpread+usereflectionOffset;
	saveData += usecolorHeight+usecolorHeightSpread+usespecScatterAmt+usespecScatterWidth+usehFoamHeight+usehFoamAmt+usehFoamSpread+usefoamAmt+usefoamScale+useedge;
	saveData += useflowSpeed+usefoamSpeed+useUpdateSpeed+userippleSensitivity+usesplashSensitivity+usereflectDistUnderAmt+useunderRefractionAmount+useunderBlurAmount;
	saveData += useetherealShift+useunderwaterFogDist+useunderwaterFogSpread+usewaveHeight+usewaveShoreHeight+usewaveScale;
	saveData += usewaveTessAmt+usewaveTessMin+usewaveTessSpread+useTideAmount+useTideSpread;
	
	//add padding for future variables
	saveData += "(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)(00.000)";
	
	//check for already existing preset match and insert data
	var ckNme : boolean = false;
	var workData : String;
	var rName : String;
	var rL : int;
	for (var cx = 0; cx < (presetDataArray.length); cx++){
		workData = presetDataArray[cx];
		rName = workData.Substring(0,20);
		rL = rName.Length;
		if (rL < 20) rName = rName.PadRight(20);
		if (rL > 20) rName = rName.Substring(0,20);
		if (rName == pName){
			ckNme = true;
			presetDataArray[cx] = saveData; //overwrite existing record
			break;
		}
	}
	
	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	for (var px = 0; px < (presetDataArray.length); px++){
		sw.Write(presetDataArray[px]);
		if (px != presetDataArray.length-1) sw.Write("\n");
	}
	if (ckNme == false){
		sw.Write("\n"+saveData); //add new record
	}
    sw.Close();
	Debug.Log("Preset '"+presetSaveName+"' has been saved!"); 

}









function PresetRename( oldName : String, newName : String ){
	
	var oName : String;
	oName = oldName;
	var nName : String;
	nName = newName;

	PresetGetData();
	
	//check name
	if (oName.Length < 20) oName = oName.PadRight(20);
	if (oName.Length > 20) oName = oName.Substring(0,20);
	if (nName.Length < 20) nName = nName.PadRight(20);
	if (nName.Length > 20) nName = nName.Substring(0,20);
	
	//check for already existing preset match and insert data
	//var ckNme : boolean = false;
	var workData : String;
	var rName : String;
	//var rL : int;
	for (var cx = 0; cx < (presetDataArray.length); cx++){
		workData = presetDataArray[cx];
		rName = workData.Substring(0,20);
		if (rName == oName){
			//ckNme = true;
			var repString : String = presetDataArray[cx];
			//repString.Replace(rName,nName);
			repString = nName + workData.Substring(20,(workData.length-20));
			presetDataArray[cx] = repString; //overwrite existing record
		}
	}
	
	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	for (var px = 0; px < (presetDataArray.length); px++){
		sw.Write(presetDataArray[px]);
		if (px != presetDataArray.length-1) sw.Write("\n");
	}
    sw.Close();
	Debug.Log("Preset '"+oldName+"' has been renamed to "+newName+"!"); 

}











function PresetDelete( preName : String ){
	
	var oName : String;
	oName = preName;

	PresetGetData();
	
	//check name
	if (oName.Length < 20) oName = oName.PadRight(20);
	if (oName.Length > 20) oName = oName.Substring(0,20);

	var workData : String;
	var rName : String;

	//save to file
	var fileName = baseDir+presetFile;
	var sw = new StreamWriter(Application.dataPath + "/" + fileName);
	sw.AutoFlush = true;
	//remove line
	for (var px = 0; px < (presetDataArray.length); px++){
		workData = presetDataArray[px];
		rName = workData.Substring(0,20);
		if (rName != oName){
			sw.Write(presetDataArray[px]);
			if (px != presetDataArray.length-1) sw.Write("\n");
		}
	}
    sw.Close();
    
    //remove trailing CR if applicable
    //PresetGetData();
    //var xw = new StreamWriter(Application.dataPath + "/" + fileName);
	//xw.AutoFlush = true;
	//for (px = 0; px < (presetDataArray.length); px++){
	//	if (px == presetDataArray.length-1) presetDataArray[px].Replace("\n","");
	//	xw.Write(presetDataArray[px]);
	//}
    //xw.Close();
    
	Debug.Log("Preset '"+preName+"' has been deleted!"); 

}










function PresetDoTransition(){
	
	//waterStartState = currentState;
	presetTransitionCurrent += (Time.deltaTime/presetTransitionTime);
	
	//transition
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColor"),PresetGetColor(presetTransIndexTo,"_DepthColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_HighColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_HighColor"),PresetGetColor(presetTransIndexTo,"_HighColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_LowColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_LowColor"),PresetGetColor(presetTransIndexTo,"_LowColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorR",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorR"),PresetGetColor(presetTransIndexTo,"_DepthColorR"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorG",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorG"),PresetGetColor(presetTransIndexTo,"_DepthColorG"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DepthColorB",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DepthColorB"),PresetGetColor(presetTransIndexTo,"_DepthColorB"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorH",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_SpecColorH"),PresetGetColor(presetTransIndexTo,"_SpecColorH"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_SpecColorL",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_SpecColorL"),PresetGetColor(presetTransIndexTo,"_SpecColorL"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_DynReflColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_DynReflColor"),PresetGetColor(presetTransIndexTo,"_DynReflColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_FoamColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_FoamColor"),PresetGetColor(presetTransIndexTo,"_FoamColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_EdgeColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_EdgeColor"),PresetGetColor(presetTransIndexTo,"_EdgeColor"),presetTransitionCurrent));
	//underwaterColor = Color.Lerp(PresetGetColor(presetTransIndexFrm,"_UnderwaterColor"),PresetGetColor(presetTransIndexTo,"_UnderwaterColor"),presetTransitionCurrent);
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_UnderColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_UnderwaterColor"),PresetGetColor(presetTransIndexTo,"_UnderwaterColor"),presetTransitionCurrent));
	this.GetComponent.<Renderer>().sharedMaterial.SetColor("_TideColor",Color.Lerp(PresetGetColor(presetTransIndexFrm,"_TideColor"),PresetGetColor(presetTransIndexTo,"_TideColor"),presetTransitionCurrent));
	
	//overallScale = Mathf.Lerp(PresetGetComponent(presetTransIndexFrm,"_MasterScale"),PresetGetComponent(presetTransIndexTo,"_MasterScale"),presetTransitionCurrent);
	lightAbsorb = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_LightAbsorb"),PresetGetFloat(presetTransIndexTo,"_LightAbsorb"),presetTransitionCurrent);
	lightRefract = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_LightRefract"),PresetGetFloat(presetTransIndexTo,"_LightRefract"),presetTransitionCurrent);
	refractSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_RefractSpeed"),PresetGetFloat(presetTransIndexTo,"_RefractSpeed"),presetTransitionCurrent);
	refractScale = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_RefractScale"),PresetGetFloat(presetTransIndexTo,"_RefractScale"),presetTransitionCurrent);
	blurSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_BlurSpread"),PresetGetFloat(presetTransIndexTo,"_BlurSpread"),presetTransitionCurrent);
	surfaceSmooth = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SurfaceSmooth"),PresetGetFloat(presetTransIndexTo,"_SurfaceSmooth"),presetTransitionCurrent);
	reflectDist = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectDist"),PresetGetFloat(presetTransIndexTo,"_ReflectDist"),presetTransitionCurrent);
	reflectSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectSpread"),PresetGetFloat(presetTransIndexTo,"_ReflectSpread"),presetTransitionCurrent);
	reflectionOffset = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectionOffset"),PresetGetFloat(presetTransIndexTo,"_ReflectionOffset"),presetTransitionCurrent);
	colorHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ColorHeight"),PresetGetFloat(presetTransIndexTo,"_ColorHeight"),presetTransitionCurrent);
	colorHeightSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ColorHeightSpread"),PresetGetFloat(presetTransIndexTo,"_ColorHeightSpread"),presetTransitionCurrent);
	specScatterAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SpecScatterAmt"),PresetGetFloat(presetTransIndexTo,"_SpecScatterAmt"),presetTransitionCurrent);
	specScatterWidth = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SpecScatterWidth"),PresetGetFloat(presetTransIndexTo,"_SpecScatterWidth"),presetTransitionCurrent);
	hFoamHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamHeight"),PresetGetFloat(presetTransIndexTo,"_HFoamHeight"),presetTransitionCurrent);
	hFoamAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamAmt"),PresetGetFloat(presetTransIndexTo,"_HFoamAmt"),presetTransitionCurrent);
	hFoamSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_HFoamSpread"),PresetGetFloat(presetTransIndexTo,"_HFoamSpread"),presetTransitionCurrent);
	foamAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_FoamAmt"),PresetGetFloat(presetTransIndexTo,"_FoamAmt"),presetTransitionCurrent);
	foamScale = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_FoamScale"),PresetGetFloat(presetTransIndexTo,"_FoamScale"),presetTransitionCurrent);
	edgeSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_EdgeSpread"),PresetGetFloat(presetTransIndexTo,"_EdgeSpread"),presetTransitionCurrent);
	flowSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveSpeed"),PresetGetFloat(presetTransIndexTo,"_WaveSpeed"),presetTransitionCurrent);
	foamSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_FoamSpeed"),PresetGetFloat(presetTransIndexTo,"_FoamSpeed"),presetTransitionCurrent);
	UpdateSpeed = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UpdateSpeed"),PresetGetFloat(presetTransIndexTo,"_UpdateSpeed"),presetTransitionCurrent);
	rippleSensitivity = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_RippleSensitivity"),PresetGetFloat(presetTransIndexTo,"_RippleSensitivity"),presetTransitionCurrent);
	splashSensitivity = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_SplashSensitivity"),PresetGetFloat(presetTransIndexTo,"_SplashSensitivity"),presetTransitionCurrent);
	reflectDistUnderAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_ReflectDistUnderAmt"),PresetGetFloat(presetTransIndexTo,"_ReflectDistUnderAmt"),presetTransitionCurrent);
	underRefractionAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderRefractionAmount"),PresetGetFloat(presetTransIndexTo,"_UnderRefractionAmount"),presetTransitionCurrent);
	underBlurAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderBlurAmount"),PresetGetFloat(presetTransIndexTo,"_UnderBlurAmount"),presetTransitionCurrent);
	etherealShift = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_EtherealShift"),PresetGetFloat(presetTransIndexTo,"_EtherealShift"),presetTransitionCurrent);
	underwaterFogDist = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderwaterFogDist"),PresetGetFloat(presetTransIndexTo,"_UnderwaterFogDist"),presetTransitionCurrent);
	underwaterFogSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_UnderwaterFogSpread"),PresetGetFloat(presetTransIndexTo,"_UnderwaterFogSpread"),presetTransitionCurrent);

	waveHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveHeight"),PresetGetFloat(presetTransIndexTo,"_WaveHeight"),presetTransitionCurrent);
	waveShoreHeight = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveShoreHeight"),PresetGetFloat(presetTransIndexTo,"_WaveShoreHeight"),presetTransitionCurrent);
	waveScale = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveScale"),PresetGetFloat(presetTransIndexTo,"_WaveScale"),presetTransitionCurrent);

	waveTessAmt = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveTessAmt"),PresetGetFloat(presetTransIndexTo,"_WaveTessAmt"),presetTransitionCurrent);
	waveTessMin = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveTessMin"),PresetGetFloat(presetTransIndexTo,"_WaveTessMin"),presetTransitionCurrent);
	waveTessSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_WaveTessSpread"),PresetGetFloat(presetTransIndexTo,"_WaveTessSpread"),presetTransitionCurrent);

	tideAmount = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_TideAmount"),PresetGetFloat(presetTransIndexTo,"_TideAmount"),presetTransitionCurrent);
	tideSpread = Mathf.Lerp(PresetGetFloat(presetTransIndexFrm,"_TideSpread"),PresetGetFloat(presetTransIndexTo,"_TideSpread"),presetTransitionCurrent);







	
	//set final
	if (presetTransitionCurrent >= 1.0){
		//set collider at end of transition
		//collider.isTrigger = GetPreset(presetTransIndex,"collider");
		//renderer.sharedMaterial.SetFloat("_RefrSpeed",GetPreset(waterTargetState,"_RefrSpeed"));
		//renderer.sharedMaterial.SetFloat("_AnimSpeed",GetPreset(waterTargetState,"_AnimSpeed"));
	
		//reset
		presetIndex = presetTransIndexTo;
		presetStartTransition = false;
		presetTransitionCurrent = 0.0;
	}	

}




function PresetGetData(){

	var fileName = baseDir+presetFile;
	var sr = new StreamReader(Application.dataPath + "/" + fileName);
    presetDataString = sr.ReadToEnd();
    sr.Close();

    presetDataArray = presetDataString.Split("\n"[0]);
	var workOptions = presetDataString.Split("\n"[0]);
	presetOptions = workOptions;
	
	for (var ax = 0; ax < (presetOptions.length); ax++){
		presetOptions[ax] = workOptions[ax].Substring(0,20);
		presetOptions[ax] = presetOptions[ax].Trim();
	}

}


