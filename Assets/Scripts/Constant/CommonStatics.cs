using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonStatics : MonoBehaviour {

    public static bool onSceneChanging;

    public static string AVATOR_SAKURA_TAG = "SAKURA";
    public static string GROUBD_TAG = "GROUND";
    public static string CROSSSCENEOBJ_TAB = "CROSSSCENEOBJ";

    public static string SAKURAROOM_FURNITURE_CHAIR_TAG = "SAKURAROOM_FURNITURE_CHAIR";
    public static string SAKURAROOM_FURNITURE_BED_TAG= "SAKURAROOM_FURNITURE_BED";
    public static string SAKURAROOM_FURNITURE_CLOSET_TAG = "SAKURAROOM_FURNITURE_CLOSET";
    public static string SAKURAROOM_FURNITURE_TRANSF1_TAG = "SAKURAROOM_FURNITURE_TRANSF1";
    public static string SAKURAROOM_FURNITURE_TRANSF2_TAG = "SAKURAROOM_FURNITURE_TRANSF2";

    public static string LAYER_STRUCTURE = "STRUCTURE";
    public static string LAYER_AVATAR = "AVATAR";
    public static string LAYER_FURNITURE = "FURNITURE";
    public static string LAYER_GROUND = "GROUND";


    //outline shader prop
    public static string OUTLINE_SHADER_INT_WIDTH_PARAM ="_Outline_Width";
    public static string OUTLINE_SHADER_COLOR_COLOR_PARAM ="_Line_Color";
    
    
    /// <summary>
    /// シーン名を記載、シーンが増えたらここに追記しなければならない！！
    /// </summary>
    public enum PlaceEnum {
        NONE,
        START,
        SAKURAROOM,
        STREET,
        UTAGE
    }

    public static string getSceneNameOfPlace(PlaceEnum scene) {
        string ret = "";
        switch (scene) {
            case PlaceEnum.START:
            ret = "START";
            break;
            case PlaceEnum.SAKURAROOM:
            ret = "SAKURAROOM";
            break;
            case PlaceEnum.STREET:
            ret = "STREET";
            break;
            case PlaceEnum.UTAGE:
            ret = "Utage";
            break;
        }
        return ret;
    }


    public static PlaceEnum getPlaceOfSceneName(string SceneName) {

        if (SceneName == "START") {
            return PlaceEnum.START;
        } else if (SceneName == "SAKURAROOM") {
            return PlaceEnum.SAKURAROOM;
        } else if (SceneName == "STREET") {
            return PlaceEnum.STREET;
        } else if (SceneName == "UTAGE") {
            return PlaceEnum.UTAGE;
        } else {
            return PlaceEnum.NONE;
        }
    }


    public static GameObject GetClickedObject(string layer, out RaycastHit hit) {
        GameObject hitObj = null;
        int layerMask = LayerMask.GetMask(new string[] { layer });
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            hitObj = hit.collider.gameObject;
        }
        return hitObj;
    }

    public static Vector3 GetClickedGrobalPosition(string layer, out RaycastHit hit) {
        Vector3 gPosition = new Vector3(float.NaN,float.NaN,float.NaN);
        int layerMask = LayerMask.GetMask(new string[] { layer });
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask)) {
            gPosition = hit.point;
        }
        return gPosition;
    }


    public static Vector3 toXZV3(Vector3 point,float basey) {
        return new Vector3(point.x, basey, point.z);
    }

    public enum FurnitureTypeEnum {
        NONE,
        BED,
        CHAIR,
        CLOSET,
        TRANSF1,
        TRANSF2
    }

    public static Bounds getMeshBounds(GameObject obj) {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        MeshFilter[] meshfilters = obj.GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter ms in meshfilters) {
            float xmin = ms.transform.position.x - ms.mesh.bounds.size.x / 2;
            float xmax = ms.transform.position.x + ms.mesh.bounds.size.x / 2;
            float ymin = ms.transform.position.y - ms.mesh.bounds.size.y / 2;
            float ymax = ms.transform.position.y + ms.mesh.bounds.size.y / 2;
            float zmin = ms.transform.position.z - ms.mesh.bounds.size.z / 2;
            float zmax = ms.transform.position.z + ms.mesh.bounds.size.z / 2;
            Vector3 b1 = new Vector3(xmin, ymin, zmin);
            Vector3 b2 = new Vector3(xmax - xmin, ymax - ymin, zmax - zmin);
            Bounds meshbounds = new Bounds(b1, b2);
            bounds.Encapsulate(meshbounds);
        }

        return bounds;
    }
}
