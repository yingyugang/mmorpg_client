using UnityEngine;

namespace TD.Config
{
    [System.Serializable]
    public class RoomConfig
    {

        public TransformEntity sceneTransform;

        public string floorRendererPath;

        public string[] wallRendererPaths;

        public CharacterForSceneConfig characters;

        static RoomConfig mInstance;

        public static RoomConfig Single
        {
            get
            {
                if (mInstance == null)
                {
                    string configText = Resources.Load<TextAsset>("configs/normals/j_room_config").text;
                    mInstance = JsonUtility.FromJson<RoomConfig>(configText);
                }
                return mInstance;
            }
        }

        private RoomConfig()
        {

        }
    }
}
