#pragma strict

var reflectPlaneObject : Transform;
var reflectionOffset : float = 0.35;


function Start () {

}




function Update () {

	if (reflectPlaneObject != null){
	

			//reflectPlaneObject.position.x = Camera.mainCamera.transform.position.x;
			//reflectPlaneObject.position.z = Camera.mainCamera.transform.position.z;
			reflectPlaneObject.position.y = this.transform.position.y+reflectionOffset; //???

			this.GetComponent.<Renderer>().sharedMaterial.SetTexture("_ReflectionTex",reflectPlaneObject.GetComponent.<Renderer>().sharedMaterial.GetTexture("_ReflectionTex"));

	}


}