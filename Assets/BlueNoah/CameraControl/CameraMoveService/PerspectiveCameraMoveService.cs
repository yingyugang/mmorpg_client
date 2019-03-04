using UnityEngine;

namespace BlueNoah.CameraControl
{
    public class PerspectiveCameraMoveService : BaseCameraMoveService
    {

        CameraController mCameraController;

        public PerspectiveCameraMoveService(Camera camera)
        {
            mCamera = camera;
            this.mMoveSpeed = 0.5f;
        }

		protected override Vector3 GetMoveAreOffset(Vector3 targetPos)
		{
            Vector3 offset = Vector3.zero;
            if(mMoveArea!=null){
#if UNITY_EDITOR
                RaycastHit raycastHit;
                if (Physics.Raycast(NearTopRightCorner, (FarTopRightCorner - NearTopRightCorner).normalized, out raycastHit,Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    pos0 = raycastHit.point;
                }
                if (Physics.Raycast(NearTopLeftCorner, (FarTopLeftCorner - NearTopLeftCorner).normalized, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    pos1 = raycastHit.point;
                }
                if (Physics.Raycast(NearBottomLeftCorner, (FarBottomLeftCorner - NearBottomLeftCorner).normalized, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    pos2 = raycastHit.point;
                }
                if (Physics.Raycast(NearBottomRightCorner, (FarBottomRightCorner - NearBottomRightCorner).normalized, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
                {
                    pos3 = raycastHit.point;
                }
#endif
                Vector3 offset0 = GetOffset(NearTopRightCorner, (FarTopRightCorner - NearTopRightCorner).normalized);
                Vector3 offset1 = GetOffset(NearTopLeftCorner, (FarTopLeftCorner - NearTopLeftCorner).normalized);
                Vector3 offset2 = GetOffset(NearBottomLeftCorner, (FarBottomLeftCorner - NearBottomLeftCorner).normalized);
                Vector3 offset3 = GetOffset(NearBottomRightCorner, (FarBottomRightCorner - NearBottomRightCorner).normalized);
                if (offset0.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset0;
                }
                if (offset1.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset1;
                }
                if (offset2.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset2;
                }
                if (offset3.sqrMagnitude > offset.sqrMagnitude)
                {
                    offset = offset3;
                }

            }
            return offset;
		}

        Vector3 GetOffset(Vector3 startPos, Vector3 forward)
        {

            Vector3 groundPosition = CameraController.GetIntersectWithLineAndPlane(startPos, forward, planeNormal, planeNormalPoint);

            Vector3 closePos = mMoveArea.ClosestPoint(groundPosition);

            Vector3 offset = groundPosition - closePos;

            return offset;
        }

        Vector3 NearTopRightCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(1, 1, mCamera.nearClipPlane));
            }
        }

        Vector3 NearTopLeftCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(0, 1, mCamera.nearClipPlane));
            }
        }

        Vector3 NearBottomLeftCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(0, 0, mCamera.nearClipPlane));

            }
        }

        Vector3 NearBottomRightCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(1, 0, mCamera.nearClipPlane));

            }
        }

        Vector3 FarTopRightCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(1, 1, mCamera.farClipPlane));

            }
        }

        Vector3 FarTopLeftCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(0, 1, mCamera.farClipPlane));

            }
        }

        Vector3 FarBottomRightCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(1, 0, mCamera.farClipPlane));

            }
        }

        Vector3 FarBottomLeftCorner
        {
            get
            {
                return mCamera.ViewportToWorldPoint(new Vector3(0, 0, mCamera.farClipPlane));

            }
        }

    }
}
