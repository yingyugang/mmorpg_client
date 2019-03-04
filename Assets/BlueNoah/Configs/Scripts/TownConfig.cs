using UnityEngine;
using System.Collections.Generic;

namespace TD.Config
{
    [System.Serializable]
    public class TownConfig
    {

        public FunctionTownBuildingEntity[] buildings;

        Dictionary<string, FunctionTownBuildingEntity> mFunctionTownBuildingDic;

        public CharacterForSceneConfig characters;

        public WayPointEntity[] wayPoints;

        static TownConfig mInstance;

        public static TownConfig Single
        {
            get
            {
                if (mInstance == null)
                {
                    string configText = Resources.Load<TextAsset>("configs/normals/j_town_config").text;
                    mInstance = JsonUtility.FromJson<TownConfig>(configText);
                    mInstance.mFunctionTownBuildingDic = new Dictionary<string, FunctionTownBuildingEntity>();
                    for (int i = 0; i < mInstance.buildings.Length; i++)
                    {
                        mInstance.mFunctionTownBuildingDic.Add(mInstance.buildings[i].objectName, mInstance.buildings[i]);
                    }
                }
                return mInstance;
            }
        }

        public FunctionTownBuildingEntity GetFunctionBuilding(string objectName)
        {
            if (mFunctionTownBuildingDic.ContainsKey(objectName))
            {
                return mFunctionTownBuildingDic[objectName];
            }
            return null;
        }

        private TownConfig()
        {

        }
    }
}