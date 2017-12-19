using UnityEngine;

namespace Assets.Scripts.BlasterWeapon
{
    internal sealed class BlasterBullet : MonoBehaviour
    {
        [HideInInspector]
        public float Speed = 1;
        [HideInInspector]
        public float LifeTime = 2f;
        public GameObject ImpactEffect;
        public GameObject BulletEffect;
        public bool DestroyOnCollision = true;

        private GameObject _bulletEffect;
        private Transform _transform;

        private void Start()
        {
            _bulletEffect = Instantiate(BulletEffect, transform.position, transform.rotation);
            _transform = transform;

            Destroy(_bulletEffect, LifeTime);
            Destroy(gameObject, LifeTime);
        }

        private void Update()
        {
            RaycastHit hit;
            if (Physics.Raycast(_transform.position, _transform.forward, out hit, Speed * Time.deltaTime * 2))
            {
                _transform.position = hit.point;

                var impactEffect = Instantiate(ImpactEffect, hit.point, new Quaternion());
                impactEffect.transform.LookAt(_transform.position + hit.normal);

                _bulletEffect.transform.position = hit.point;

                Destroy(impactEffect, LifeTime);

                if (DestroyOnCollision)
                {
                    Destroy(_bulletEffect);
                    Destroy(gameObject);
                }
            }

            if (_bulletEffect == null)
                return;

            _transform.position += _transform.forward * Speed * Time.deltaTime;
            _bulletEffect.transform.position = _transform.position;
        }
    }
}
