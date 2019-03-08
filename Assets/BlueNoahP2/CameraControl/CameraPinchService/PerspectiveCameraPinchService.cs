using UnityEngine;
using System.Collections;
using BlueNoah.Event;

namespace BlueNoah.CameraControl
{

    public class PerspectiveCameraPinchService : BaseCameraPinchService
    {

        private float mMinDistance = 10;
        private float mMaxDistance = 40;
        private float mCurrentDistance = 30;

        public override float minSize
        {
            get
            {
                return mMinDistance;
            }
            set
            {
                mMinDistance = value;
            }
        }
        public override float maxSize
        {
            get
            {
                return mMaxDistance;
            }
            set
            {
                mMaxDistance = value;
            }
        }

        public override void Init()
        {
            SetCameraDistance();
        }

        public PerspectiveCameraPinchService(Camera camera)
        {
            this.mCamera = camera;
        }

        public override void OnPinchBegin()
        {

        }

        public override void OnPinch(EventData eventData)
        {
            float detalDistance = eventData.deltaTwoFingerDistance;
            mCurrentDistance -= detalDistance * mPinchRadiu;
            mCurrentDistance = Mathf.Clamp(mCurrentDistance, mMinDistance, mMaxDistance);
            SetCameraDistance();
        }

        protected override void OnMouseScrollWheel()
        {
            EventData eventData = new EventData();
            eventData.deltaTwoFingerDistance = Input.GetAxis("Mouse ScrollWheel") * 100f;
            OnPinch(eventData);
        }

        void SetCameraDistance()
        {
            RaycastHit raycastHit;
            if (Physics.Raycast(mCamera.transform.position, mCamera.transform.forward, out raycastHit, Mathf.Infinity, 1 << LayerConstant.LAYER_GROUND))
            {
                Vector3 point = raycastHit.point;
                mCamera.transform.position = point - mCamera.transform.forward * mCurrentDistance;
            }
        }
    }

}

