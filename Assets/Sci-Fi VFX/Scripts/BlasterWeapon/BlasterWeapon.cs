using UnityEngine;

namespace Assets.Scripts.BlasterWeapon
{
    internal sealed class BlasterWeapon : MonoBehaviour
    {
        public float BulletSpeed = 1;
        public GameObject BulletPrefab;
        public bool Enabled;
        public float LifeTime = 2f;

        private void Start()
        {
            InvokeRepeating("Fire", 1f, 0.5f);
        }

        private void Update()
        {
        }

        private void Fire()
        {
            if (!Enabled)
                return;

            var bulletClone = Instantiate(BulletPrefab, transform.position, transform.rotation);
            var blasterBullet = bulletClone.GetComponent<BlasterBullet>();
            blasterBullet.Speed = BulletSpeed;
            blasterBullet.LifeTime = LifeTime;

            Destroy(bulletClone, LifeTime);
        }
    }
}
