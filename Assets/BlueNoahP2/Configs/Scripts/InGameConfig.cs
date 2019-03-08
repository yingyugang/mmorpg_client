using UnityEngine;

namespace TD.Config
{
    [System.Serializable]
    public class InGameConfig
    {

        public float cameraDragSpeed;
        public float rotateSpeed;

        static InGameConfig mInstance;

        public static InGameConfig Single
        {
            get
            {
                if (mInstance == null)
                {
                    TextAsset textAsset = Resources.Load<TextAsset>("configs/normals/j_in_game_config");
                    if (textAsset == null)
                    {
                        Debug.LogError("Config is null.");
                    }
                    string configText = textAsset.text;
                    mInstance = JsonUtility.FromJson<InGameConfig>(configText);
                }
                return mInstance;
            }
        }

        private InGameConfig()
        {

        }
    }
}
