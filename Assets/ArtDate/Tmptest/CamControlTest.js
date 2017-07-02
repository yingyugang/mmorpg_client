var camera1:GameObject;
var camera2:GameObject;
var camera3:GameObject;

function Update () 
{
        if(Input.GetKeyUp(KeyCode.Alpha1))
        {
                onActiveFalse();
                camera1.active=true;
        }else if(Input.GetKeyUp(KeyCode.Alpha2))
        {
                onActiveFalse();
                camera2.active=true;
        
        }else if(Input.GetKeyUp(KeyCode.Alpha3))
        {
                onActiveFalse();
                camera3.active=true;
        }
}
function onActiveFalse()
{
        camera1.active=false;
        camera2.active=false;
        camera3.active=false;
}