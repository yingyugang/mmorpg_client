#pragma strict

var generateOnStart : boolean = true;
var generateMap : boolean = false;

var autoGenerateFPS : float = 0.0;

var shoreRange : float = 3.0;
var waveRange : float = 10.0;
var detectLayers : LayerMask;

var shoreMapTex : Texture2D;
var parentMesh : Mesh;

private var autoTimer : float = 0.0;
private var waveObject : Suimono_waveGenerator;


function Start () {
	
	//get parent object and mesh
	if (transform.parent.gameObject.GetComponent(MeshFilter)){
		parentMesh = this.transform.parent.gameObject.GetComponent(MeshFilter).sharedMesh;
	}
	
	waveObject = transform.parent.gameObject.Find("Suimono_waveObject").gameObject.GetComponent(Suimono_waveGenerator);

	if (generateOnStart) Generate();

	//InvokeRepeating("UnloadTex",30.0,60.0);
	
}

function Update () {

	shoreRange = Mathf.Clamp(shoreRange,0.0,1000.0);
	waveRange = Mathf.Clamp(waveRange,0.0,1000.0);
	
	if (autoGenerateFPS > 0.0){
		autoTimer += Time.deltaTime;
		if (autoTimer >= autoGenerateFPS){
			autoTimer = 0.0;
			generateMap = true;
		}
	}
	
	if (generateMap){
		generateMap = false;
		Generate();
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
	
		shoreMapTex = null;
		shoreMapTex = new Texture2D(meshWidth,meshHeight);
		
		//get pixel positions
		var ht : RaycastHit;
		var tstpos : Vector3;
		var tstpos2 : Vector3;
		var setDistance : float = 0.0;
		var setDistance2 : float = 0.0;	
		
		for (var i = 0; i < vertices.Length; i++){
			ppos[i] = Vector2 (vertices[i].x*0.98,vertices[i].z*0.98);
			
			//get heights
			tstpos = transform.parent.transform.TransformPoint(Vector3(ppos[i].x,0.0,ppos[i].y));
			tstpos2 = transform.parent.transform.TransformPoint(Vector3(ppos[i].x,(0.0-(transform.parent.transform.position.y*2.0)),ppos[i].y));
			
			setDistance = 1.0;
			setDistance2 = 1.0;
			if (Physics.Raycast (tstpos, -Vector3.up, ht,1000.0, detectLayers)) {
				
				setDistance = ht.distance/waveRange;
				if (setDistance > 1.0) setDistance = 1.0;
				if (setDistance < 0.0) setDistance = 0.0;
				setDistance = 1.0-setDistance;
				
				setDistance2 = ht.distance/shoreRange;
				if (setDistance2 > 1.0) setDistance2 = 1.0;
				if (setDistance2 < 0.0) setDistance2 = 0.0;
				setDistance2 = 1.0-setDistance2;
				
			}

			//assign colors
			wcolors[i] = setDistance;
			scolors[i] = setDistance2;
			shoreMapTex.SetPixel((sideLength*0.5)-ppos[i].x*1.3, (sideLength*0.5)-ppos[i].y*1.3, Color(wcolors[i],scolors[i],0,1));

		}

		
		//apply all SetPixel calls
		shoreMapTex.Apply(false,false);
		
		//resize texture
		//
		
		//set texture to renderer position
		if (transform.parent.GetComponent.<Renderer>()){
			shoreMapTex.wrapMode = TextureWrapMode.Clamp;
			transform.parent.GetComponent.<Renderer>().sharedMaterial.SetTexture("_FlowMap",shoreMapTex);
			transform.parent.GetComponent.<Renderer>().sharedMaterial.SetTextureScale("_FlowMap",Vector2(1.0,1.0));
			if (waveObject != null) waveObject.maskMapTex = shoreMapTex;
		}
	}
	
	
}




function UnloadTex(){
	
	//unloads unused textures to conserve memory
	#if UNITY_EDITOR
	EditorUtility.UnloadUnusedAssetsIgnoreManagedReferences();
#endif
}