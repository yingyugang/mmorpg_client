#pragma strict
#pragma implicit
#pragma downcast






//wave generate test
var debugShowMesh : boolean = false;
var generateMap : boolean = false;
var autoGenerate : float = 5.0;
var waveMapTex : Texture2D;
var physMapTex : Texture2D;
var maskMapTex : Texture2D;
var parentMesh : Mesh;
var autoTimer : float = 0.0;





//PUBLIC VARIABLES MESH
var updatePhysics : boolean = true;
var waveFPS : float = 24;
var meshUpdate : float = 0.5;


var calculateTangents : boolean = true;
var createUVs : boolean = true;
var numCells : int = 1;
var numTriangles : int = 2;







//PRIVATE VARIABLES MESH
private var currentTime : float = 0.0;







//PUBLIC VARIABLE MORPH
var beaufortscale: float = 0.0;
var wakeFoamSpread : float = 1.0;
var wakeFoamFade : float = 1.0;
var showWakeDebug : boolean = false;
var imageScale : float = 1.0;
var waveLarge : Texture2D;
var waveMedium : Texture2D;
var morphAmount : float = 1.0;
var waveBlend : float = 0.0;
var waveScale1 : float = 1.0;
var waveScale2 : float = 1.0;
var waveSpeed : float = 1.0;


var updatePhysicsStep : float = 0.5;
var shallowDepthTest : boolean = false;
var shallowWaveSpeed : float = 1.0;
var shallowWaveHeight : float = 0.2;
var shallowDepthHeight : float = 3.0;
var depthTestBlock : int = 1;
var shallowTestLayers : LayerMask = 0;
var currentMesh : int = 0;
var currentPhysics : int = 3;
var verticesHeightsUpdate : boolean = true;

//PRIVATE VARIABLES MORPH
private var waveMask : Texture2D;

private var waveDirection : Vector2 = Vector2(1,1);

private var recTime : float = 0.0;
private var start : boolean = false;
private var mesh : Mesh;
private var vertices : Vector3[];
private var triangles : int[];
private var verticestarget : Vector3[];

private var normals : Vector3[];
private var colors : Color[];
private var verticesHeights : Vector3[];
private var setheight : float = 0.0;
private var offset : Vector2 = Vector2(0.0,0.0);
private var useUpdatePhysicsStep : float = 0.5;
private var vertLength : int = 0;
private var physvertLength : int = 0;
private var vertSQ : int = 0;
private var physvertSQ : int = 0;
private var useMesh : int = 0;
private var usePhysics : int = 3;
private var xp : int = 0;
private var xpp : int = 0;
private var x : int = 0;
private var v : int = 0;
private var xsize : float;
private var xset : float;
private var ysize : float;
private var yset : float;
private var xs : float;
private var ys : float;


private var checkColor1 : Color = Color(0.0,0.0,0.0,0.0);
private var checkColor2 : Color = Color(0.0,0.0,0.0,0.0);
private var maskColor : Color = Color(0.0,0.0,0.0,0.0);
private var useCamera : Camera;
private var updateVert : boolean = false;
private var chkVrt : Vector3;
private var meshTime : float = 0.0;


private var suimonoModuleObject : GameObject;
private var moduleSplashObject : SuimonoModule;
private var moduleSurfaceObject : SuimonoObject;






function Start(){
	
	//GET OBJECTS
	suimonoModuleObject = GameObject.Find("SUIMONO_Module").gameObject;
	if (suimonoModuleObject != null) moduleSplashObject = suimonoModuleObject.GetComponent(SuimonoModule);
	moduleSurfaceObject = gameObject.GetComponent(SuimonoObject);
		
	//INITIATE WATER AND PHYSICS MESH OBJECTS
	mesh = transform.GetComponent(MeshCollider).sharedMesh;
	vertices = mesh.vertices;
	verticestarget = mesh.vertices;
	colors = new Color[vertices.length];
	
	//reset mesh attributes
	verticestarget = vertices;
	colors = new Color[vertices.length];
	vertLength = vertices.length;
	vertSQ = Mathf.Sqrt(vertLength);
	
	waveLarge = transform.GetComponent.<Renderer>().sharedMaterial.GetTexture("_Surface1");
	
	
	//mesh generation test
	if (gameObject.GetComponent(MeshFilter)){
		parentMesh = this.gameObject.GetComponent(MeshFilter).sharedMesh;
	}
		

}







function Update() {
	

	//wave texture generation
	if (autoGenerate > 0.0){
		autoTimer += Time.deltaTime;
		if (autoTimer >= (1.0/autoGenerate)){
			autoTimer = 0.0;
			generateMap = true;
		}
	}
	if (generateMap){
		generateMap = false;
		Generate();
		//GeneratePhys();
	}
	
 	

 	//AutoUpdate Clock
 	if (updatePhysics){
 	currentTime += Time.deltaTime;
 	if (currentTime >= (1.0/waveFPS)){
 		currentTime = 0.0;
 		
 		

		//Recalculate Wave Heights
		setVertices();
		mesh.vertices = vertices;
		
		transform.GetComponent(MeshCollider).sharedMesh = null;
		transform.GetComponent(MeshCollider).sharedMesh = mesh;
		
		
		//build mesh from triangle array
		//mesh.triangles = triangles;
		//mesh.RecalculateNormals();
		//mesh.RecalculateBounds();

   		//lerp vertex colors
    	//for(var c : int = 0; c < vertices.Length; c++){
    	//	colors[c] = Color.Lerp(colors[c],Color(1,0,0,1),Time.deltaTime*wakeFoamFade);
   		//}	
    	//mesh.colors = colors;
    	
	
 	}
	
		
	}
	

	
	//Set Beaufort scale attributes
	beaufortscale = Mathf.Clamp(beaufortscale,0.0,12.0);
	
	morphAmount = Mathf.Lerp(0.0,35.0,(beaufortscale/12.0));
	waveBlend = 0.0 + Mathf.Lerp(0.0,0.3,(beaufortscale/12.0)*4.0);
	waveBlend += Mathf.Lerp(0.0,0.3,((beaufortscale-5.0)/(12.0-5.0))*4.0);

	useCamera = Camera.main.gameObject.GetComponent.<Camera>();
	
	//CLAMP VALUES
	updatePhysicsStep = Mathf.Clamp(updatePhysicsStep,0.0,60.0);
	waveBlend = Mathf.Clamp(waveBlend,0.0,1.0);
	//waveDirection.x = Mathf.Clamp(waveDirection.x,-1.0,1.0);
	//waveDirection.y = Mathf.Clamp(waveDirection.y,-1.0,1.0);
	
	waveDirection = -moduleSurfaceObject.flow_dir;
	
}












//#################################
//##   MESH MORPH FUNCTIONS   ##
//#################################

function OnTriggerStayTEST(other : Collider) {

		
		// Only if we hit something, do we continue
		var hitPos : Vector3 = other.transform.position;
		
		hitPos.y+=50.0;
		hitPos.x += Random.Range(-wakeFoamSpread,wakeFoamSpread);
		hitPos.z += Random.Range(-wakeFoamSpread,wakeFoamSpread);
		
		var hits : RaycastHit[];
		hits = Physics.RaycastAll(hitPos,-Vector3.up,55.0);

		for (var i = 0;i < hits.Length; i++) {
		var hit : RaycastHit = hits[i];
		if (hit.collider == this.GetComponent.<Collider>()){
		
		// Just in case, make sure the collider also has a renderer
		// material and texture
		var meshCollider = hit.collider as MeshCollider;
		if (meshCollider == null || meshCollider.sharedMesh == null)
			return;

		var hmesh : Mesh = meshCollider.sharedMesh;
		var hvertices = hmesh.vertices;
		var htriangles = hmesh.triangles;

		// Extract local space vertices that were hit
		var v0 = hvertices[htriangles[hit.triangleIndex * 3 + 0]];
		var v1 = hvertices[htriangles[hit.triangleIndex * 3 + 1]];    
		var v2 = hvertices[htriangles[hit.triangleIndex * 3 + 2]];   

		// Transform local space vertices to world space
		var hitTransform : Transform = hit.collider.transform;
		var p0 = hitTransform.TransformPoint(v0);
		var p1 = hitTransform.TransformPoint(v1);
		var p2 = hitTransform.TransformPoint(v2);
		
		var setV : int = Mathf.Floor(Random.Range(0.0,3.0));
		var checkv = hvertices[htriangles[hit.triangleIndex * 3 + setV]];
		var setSTR = Random.Range(0.0,1.0);

		//determined closest vertex
		var chkDist : float;
		var chkPos : Vector3;
		var useVert : int;
		chkPos = hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 0]]);
		chkDist = Vector3.Distance(hitPos,chkPos);
		useVert = 0;
		

		if (chkDist > Vector3.Distance(hitPos,hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 1]]))){
			useVert = 1;
			chkDist = Vector3.Distance(hitPos,hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 1]]));
			chkPos = hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 1]]);
		}
		

		if (chkDist > Vector3.Distance(hitPos,hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 2]]))){
			useVert = 2;
			chkDist = Vector3.Distance(hitPos,hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 2]]));
			chkPos = hitTransform.TransformPoint(hvertices[htriangles[hit.triangleIndex * 3 + 2]]);
		}
		
		
		//loop through all vertices to find position
    	for(var vc : int = 0; vc < vertices.Length; vc++){
			//set chosen vertex color
    		if (vertices[vc] == hvertices[htriangles[hit.triangleIndex * 3 + useVert]]) colors[vc] = Color.Lerp(colors[vc],Color(0,0.75,0,1), Time.deltaTime * 20.0);
   		}	
   		
		//clamp all vertex colors to max green value
		colors[htriangles[hit.triangleIndex * 3 + 0]].g = Mathf.Clamp(colors[htriangles[hit.triangleIndex * 3 + 0]].g, 0.0,1.0);
		colors[htriangles[hit.triangleIndex * 3 + 1]].g = Mathf.Clamp(colors[htriangles[hit.triangleIndex * 3 + 1]].g, 0.0,1.0);
		colors[htriangles[hit.triangleIndex * 3 + 2]].g = Mathf.Clamp(colors[htriangles[hit.triangleIndex * 3 + 2]].g, 0.0,1.0);

    	mesh.colors = colors;
		
		
		
		//show debug if applicable
		if (showWakeDebug){
			Debug.DrawLine(p0, p1);
			Debug.DrawLine(p1, p2);
			Debug.DrawLine(p2, p0);
			Debug.DrawRay(chkPos, Vector3.up * 4, Color.red);
		}


			
		
		}
		}
		

}
	
	


    















function setVertices(){

	var getScaleHeight = moduleSurfaceObject.waveHeight;
	
	var ppos : Vector2[]  = new Vector2[vertices.Length];
	var tstpos : Vector3;
	
	var Colors = physMapTex.GetPixels();

	for (var v : int = 0; v < vertices.Length; v++){
		
		//ppos[v] = Vector2(vertices[v].x,vertices[v].z);
		//tstpos = transform.TransformPoint(Vector3(ppos[v].x,0.0,ppos[v].y));

		//checkColor1.r = waveMapTex.GetPixel(tstpos.x*2,tstpos.z*2).r;// * waveScale1;
		
		//set vertice
		//verticestarget[v].y = checkColor1.r + Mathf.Lerp(0.0,Mathf.Lerp(0.0,checkColor2.r,waveBlend),checkColor1.r*0.5);
		vertices[v].y = (Colors[v].r*(getScaleHeight*4.0));//maskColor.r;

	}

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
	
		//waveMapTex = null;
		//if (waveMapTex != null) Destroy(waveMapTex);
		waveMapTex = null;
		waveMapTex = new Texture2D(meshWidth,meshHeight);
		
		//get pixel positions
		var ht : RaycastHit;
		var tstpos : Vector3;
		var tstpos2 : Vector3;

		
		//init wave scale values
		var getScale = Mathf.Lerp(0.2,2.0,moduleSurfaceObject.waveScale)*(10.0/transform.localScale.x); 
		var getScaleShore = moduleSurfaceObject.waveShoreHeight;
		var texSize : int = waveLarge.width*getScale;
		var useSize : float = (texSize/sideLength)*0.13;
		var shft : int = Mathf.Round(texSize*0.1855);
		var span : float = (1.3);
			
		for (var i = 0; i < vertices.Length; i++){
			ppos[i] = Vector2 (vertices[i].x*0.98,vertices[i].z*0.98);
			
			//get positions
			tstpos = transform.TransformPoint(Vector3(ppos[i].x,0.0,ppos[i].y));
		
			//calculate offset
			var offsX : int = Mathf.Round(texSize*(-this.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").x*(1.0/transform.localScale.x)));
			var offsY : int = Mathf.Round(texSize*(-this.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").y*(1.0/transform.localScale.x)));
			
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
		if (transform.GetComponent.<Renderer>()){
			waveMapTex.wrapMode = TextureWrapMode.Clamp;
			if (debugShowMesh == false){
			this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveMap",waveMapTex);
			this.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_WaveMap",Vector2(1.0,1.0));
			}
			
		}

	}
	
	
}










function GeneratePhys(){


	
	if (mesh != null){
		
		
		var vertices : Vector3[] = mesh.vertices;
		var ppos : Vector2[]  = new Vector2[vertices.Length];
		var wcolors = new float[vertices.Length];
		var scolors = new float[vertices.Length];
		var bounds : Bounds = mesh.bounds;

		var sideLength :int = Mathf.Floor(Mathf.Sqrt(vertices.Length));
		var meshWidth : int = sideLength;
		var meshHeight : int = sideLength;
	
		physMapTex = new Texture2D(meshWidth,meshHeight);
		
		//get pixel positions
		var ht : RaycastHit;
		var tstpos : Vector3;
		var tstpos2 : Vector3;

		
		//init wave scale values
		var getScale = Mathf.Lerp(0.2,4.0,moduleSurfaceObject.waveScale)*(10.0/transform.localScale.x); 
		var getScaleShore = moduleSurfaceObject.waveShoreHeight;
		var texSize : int = waveLarge.width*getScale;
		var useSize : float = (texSize/sideLength)*0.13*0.26543;
		var shft : int = Mathf.Round(texSize*0.1855);
		var span : float = (1.3*0.5);
		
		for (var i = 0; i < vertices.Length; i++){
			ppos[i] = Vector2 (vertices[i].x*0.98,vertices[i].z*0.98);
			
			//get positions
			tstpos = transform.TransformPoint(Vector3(ppos[i].x,0.0,ppos[i].y));
		
			//calculate offset
			var offsX : int = Mathf.Round(texSize*(-this.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").x*(1.0/transform.localScale.x)));
			var offsY : int = Mathf.Round(texSize*(-this.GetComponent.<Renderer>().sharedMaterial.GetTextureOffset("_Surface1").y*(1.0/transform.localScale.x)));
			
			//get pixels
			var setDistance : float = waveLarge.GetPixel(Mathf.Round(tstpos.x*useSize)-shft+offsX+240,Mathf.Round(tstpos.z*useSize)-shft+offsY+240).r*2.0;
			var setDistance2 : float = waveMedium.GetPixel(Mathf.Round(tstpos.x*useSize)-shft-(offsX*0.25)+240,Mathf.Round(tstpos.z*useSize)-shft-(offsY*0.5)+240).r*1.0;
			
			//get mask
			var setMask : Color = maskMapTex.GetPixel((sideLength*1.0)-ppos[i].x*(span*2.0), (sideLength*1.0)-ppos[i].y*(span*2.0));

			//build texture
			wcolors[i] = ((setDistance - setDistance2) * (1.0-setMask.r)) * (1.0-setMask.g);
			wcolors[i] = setDistance;
			physMapTex.SetPixel((sideLength*0.5)-ppos[i].x*(span), (sideLength*0.5)-ppos[i].y*(span), Color(wcolors[i],0,0,1));

		}

		
		//apply all SetPixel calls
		physMapTex.Apply(false,false);
		physMapTex.wrapMode = TextureWrapMode.Clamp;
		
		if (transform.GetComponent.<Renderer>()){
		if (debugShowMesh == true){
			this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_WaveMap",physMapTex);
			this.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_WaveMap",Vector2(1.0,1.0));
		}
		}
		

	}
	
	
}



