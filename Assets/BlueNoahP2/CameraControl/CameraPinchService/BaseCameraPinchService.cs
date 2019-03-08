using UnityEngine;
using BlueNoah.Event;

namespace BlueNoah.CameraControl
{
    public abstract class BaseCameraPinchService
    {
        protected bool mIsPinching;
        protected bool mIsPinchable = true;
        protected float mPinchRadiu = 0.009f;
        protected float mMaxPinchOver = 0.5f;
        protected Camera mCamera;

        public virtual void Init()
        {

        }

        public virtual void OnUpdate()
        {

        }

        public virtual void OnLateUpdate()
        {
            if (Input.GetMouseButton(2) )
            {
                if(Input.GetAxis("Mouse ScrollWheel") != 0f)
                    OnMouseScrollWheel();
                mIsPinching = true;
            }
            else
            {
                mIsPinching = false;
            }
        }

        protected abstract void OnMouseScrollWheel();

        public abstract float minSize
        {
            get;
            set;
        }

        public abstract float maxSize
        {
            get;
            set;
        }

        public bool IsPinchable
        {
            get
            {
                return mIsPinchable;
            }
            set
            {
                mIsPinchable = value;
            }
        }

        public bool IsPinching
        {
            get
            {
                return mIsPinching;
            }
        }

        public abstract void OnPinchBegin();

        public abstract void OnPinch(EventData eventData);

        public void OnPinchEnd()
        {
            if (mIsPinching)
            {
                mIsPinching = false;
            }
        }

        public void CancelPinch()
        {
            mIsPinching = false;
        }

    }
}