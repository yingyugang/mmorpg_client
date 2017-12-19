using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Dumper
{
    internal sealed class Dumper : MonoBehaviour
    {
        public bool ChangeStartPosition;
        public AnimationCurve DumpingOverTime = AnimationCurve.Linear(0, 1, 1, 1);
        public float Fps = 30;
        public float LifeTime = 4f;

        private Vector3 _originalStartPosition;
        private float _animationStartTime;
        private bool _isCoCanceled;
        private bool _isCoStarted;

        private void Start()
        {
            StartEffect();
        }

        private void StartEffect()
        {
            if (_isCoStarted)
                return;

            Invoke("StopEffect", LifeTime);

            _isCoCanceled = false;

            _originalStartPosition = transform.position;

            StartCoroutine(UpdateCorutine());
        }

        private IEnumerator UpdateCorutine()
        {
            _isCoStarted = true;

            _animationStartTime = Time.time;

            while (!_isCoCanceled)
            {
                var frameTime = Time.time - _animationStartTime;

                if (frameTime >= LifeTime)
                    break;

                var currentDump = DumpingOverTime.Evaluate(frameTime);

                var newX = Random.Range(_originalStartPosition.x - currentDump, _originalStartPosition.x + currentDump);
                var newY = Random.Range(_originalStartPosition.y - currentDump, _originalStartPosition.y + currentDump);
                var newZ = Random.Range(_originalStartPosition.z - currentDump, _originalStartPosition.z + currentDump);

                transform.position = new Vector3(newX, newY, newZ);

                yield return new WaitForSeconds(1f / Fps);
            }
        }

        private void StopEffect()
        {
            _isCoCanceled = true;
            StopAllCoroutines();
            _isCoStarted = false;
        }

        private void OnDestroy()
        {
            StopEffect();
        }
    }
}
