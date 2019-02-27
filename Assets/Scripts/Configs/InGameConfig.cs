using UnityEngine;

namespace TD.Config
{
    [System.Serializable]
    public class InGameConfig
    {

        public Vector3 defaultMoveArePoisition;

        public Vector3 defaultMoveAreEulerAngles;

        public Vector3 defaultMoveAreScale;

        public Vector3 defaultCameraPosition;

        public Vector3 defaultCameraEulerAngles;

        public Vector3 editModeMoveArePosition;

        public Vector3 editModeMoveAreEulerAngles;

        public Vector3 editModeMoveAreScale;

        public Vector3 beforeFittingEulerAngles;

        public Vector3 editModeCameraPosition;

        public Vector3 editModeCameraEulerAngles;

        public float beforeFittingOrthographicSize;

        public float beforeFittingYOffset;

        public float beforeFittingXOffset;

        public float beforeFittingDuration;

        public float orthographicSizeByViewMode;

        public float minOrthographicSizeByViewMode;

        public float maxOrthographicSizeByViewMode;

        public float orthographicSizeByEditMode;

        public float minOrthographicSizeByEditMode;

        public float maxOrthographicSizeByEditMode;

        public float roomCameraMoveSpeed;

        public float orthographicSizeByTown;

        public float minOrthographicSizeByTown;

        public float maxOrthographicSizeByTown;

        public float townCameraMoveSpeed;

        public float dragOffsetHeight;

        public float dragOffsetForward;

        public int roomStartSize;

        public int roomHeight;

        public float roomNodeSize;

        public int roomGrowPerStep;

        public int roomMaxGrowStep;

        public int charaIdForSakura;

        public float defaultSpringBoneRadius;

        public int charaSpeed;

        public int meetInterval;

        public int meetCheckInterval;

        public float meetCheckMinDistance;

        public float meetCheckMaxDistance;

        static InGameConfig mInstance;

        public static InGameConfig Single
        {
            get
            {
                if (mInstance == null)
                {
                    string configText = Resources.Load<TextAsset>("configs/j_in_game_config").text;
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
