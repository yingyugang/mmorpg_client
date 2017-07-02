#pragma strict

var effectPrefab : Transform ;
//var explosion_2 : Transform ;
var positionTF : Transform;
var degree:float=25;

function effectspawner(effect:Transform)
{
Instantiate(effect,positionTF.position,Quaternion.Euler(Vector3(degree,0,0)));

}



function Update () {
if(Input.GetButtonDown("Fire1"))
   {  
	
	var ray : Ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	var playerPlane = new Plane(Vector3.up, transform.position);
   	var hitdist = 0.0;
   
   	if (playerPlane.Raycast (ray, hitdist)) {
		    
			var targetPosition = ray.GetPoint(hitdist);
			
			transform.position = targetPosition;
		
			
		
		
		}
	
   
    effectspawner(effectPrefab);   
        
   }
}