#pragma strict
#pragma implicit
#pragma downcast


//PUBLIC VARIABLES MESH
var debugShowMesh : boolean = false;
var generateMap : boolean = false;
var waveFPS : float = 5.0;
var waveMapTex : Texture2D;
var physMapTex : Texture2D;
var maskMapTex : Texture2D;
var parentMesh : Mesh;


//PUBLIC VARIABLE MORPH
private var autoTimer : float = 0.0;
private var beaufortscale: float = 0.0; //turned off for now

//wake variables
//var wakeFoamSpread : float = 1.0;
//var wakeFoamFade : float = 1.0;
//var showWakeDebug : boolean = false;





//PRIVATE VARIABLES 
private var waveLarge : Texture2D;
private var waveMedium : Texture2D;

private var waveDirection : Vector2 = Vector2(1,1);
private var suimonoModuleObject : GameObject;
private var moduleSplashObject : SuimonoModule;
private var moduleSurfaceObject : SuimonoObject;






function Start(){
	
	//GET OBJECTS
	suimonoModuleObject = GameObject.Find("SUIMONO_Module").gameObject;
	if (suimonoModuleObject != null) moduleSplashObject = suimonoModuleObject.GetComponent(SuimonoModule);
	moduleSurfaceObject = transform.parent.gameObject.GetComponent(SuimonoObject);

	waveLarge = transform.parent.GetComponent.<Renderer>().sharedMaterial.GetTexture("_Surface1");
	waveMedium = transform.parent.GetComponent.<Renderer>().sharedMaterial.GetTexture("_Surface2");
	
	//mesh generation test
	if (transform.parent.gameObject.GetComponent(MeshFilter)){
		parentMesh = transform.parent.gameObject.GetComponent(MeshFilter).sharedMesh;
	}
	
	
	InvokeRepeating("UnloadTex",30.0,60.0);
		

}







function Update() {
	

	//wave texture generation
	if (waveFPS > 0.0){
		autoTimer += Time.deltaTime;
		if (autoTimer >= (1.0/waveFPS)){
			autoTimer = 0.0;
			generateMap = true;
		}
	}
	if (generateMap){
		generateMap = false;
		Generate();
	}

	//useCamera = Camera.main.gameObject.camera;
	waveDirection = -moduleSurfaceObject.flow_dir;
	
}
















function Generate(){


	if (parentMesh){
		
		var vertices : Vector3[] = parentMesh.vertices;
		var ppos : Vector2[]  = new Vector2[vertices.Length];
		var wcolors = new float[vertices.Length];
		var scolors = new float[vertices.Length];
		var bounds : Bounds = parentMesh.bounds;

		var sideLength :int = Mathf.Floor(Mathf.Sqrt(vertices.Length));
		var meshWidth : int = sideLength;
		var meshHeight : int = sideLength;
	
		waveMapTex = null;
		waveMapTex = new Texture2D(meshWidth,meshHeight);
		
		//get pixel positions
		var ht : RaycastHit;
		var tstpos : Vector3;
		var tstpos2 : Vector3;

		
		//init wave scale values
		var getScale = Mathf.Lerp(0.2,2.0,moduleSurfaceObject.waveScale)*(10.0/transform.parent.transform.localScale.x); 
		var getScaleShore = moduleSurfaceObject.waveShoreHeight;
		var texSize : int = waveLarge.width*getScale;
		var useSize : float = (texSize/sideLength)*0.13;
		var shft : int = Mathf.Round(texSize*0.1855);
		var span : float = (1.3);
			
		for (var i = 0; i < vertices.Length; i++){
			ppos[i] = Vector2 (vertices[i].x*0.98,vertices[i].z*0.98);
			
			//get positions
			tstpos = transform.parent.transform.TransformPoint(Vector3(ppos[i].x,0.0,ppos[i].y));
		
			//calculate offset
			var offsX : int = Mathf.Round(texSize*(-transform.parent.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").x)); //*(1.0/transform.parent.transform.localScale.x)
			var offsY : int = Mathf.Round(texSize*(-transform.parent.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").y)); //*(1.0/transform.parent.transform.localScale.x)
			
			//get pixels
			var setDistance : float = waveLarge.GetPixel(Mathf.Round(tstpos.x*useSize)-shft+offsX,Mathf.Round(tstpos.z*useSize)-shft+offsY).r*2.0;
			var setDistance2 : float = waveMedium.GetPixel(Mathf.Round(tstpos.x*useSize)-shft-(offsX*0.25),Mathf.Round(tstpos.z*useSize)-shft-(offsY*0.5)).r*1.0;
			
			//get mask
			var setMask : Color = Color(0,0,0,1);
			if (maskMapTex != null){
				setMask = maskMapTex.GetPixel((sideLength*0.5)-ppos[i].x*span, (sideLength*0.5)-ppos[i].y*span);
			}
			
			//build texture
			wcolors[i] = ((setDistance - setDistance2) * (1.0-setMask.r)) * (1.0-setMask.g);
			waveMapTex.SetPixel((sideLength*0.5)-ppos[i].x*span, (sideLength*0.5)-ppos[i].y*span, Color(wcolors[i],0,0,1));


		}

		
		//apply all SetPixel calls
		waveMapTex.Apply(false,false);
		
		//resize texture
		//
		
		//set texture to renderer position
		if (transform.parent.GetComponent.<Renderer>()){
			waveMapTex.wrapMode = TextureWrapMode.Clamp;

			transform.parent.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveMap",waveMapTex);
			transform.parent.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_WaveMap",Vector2(1.0,1.0));

			
		}

	}
	
	
}






function UnloadTex(){
	
	//unloads unused textures to conserve memory
	#if UNITY_EDITOR
	EditorUtility.UnloadUnusedAssetsIgnoreManagedReferences();
	#endif
	//Resources.UnloadUnusedAssets();

}