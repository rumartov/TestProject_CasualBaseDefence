using Infrastructure.Factory;
using UnityEngine;

namespace Logic.Weapon
{
    public abstract class Weapon : MonoBehaviour, IWeapon
    {
        public GameObject ProjectilePrefab;
        public Transform ShootPoint;
        public float ProjectileLifeTime;
        public float Damage;
        public float ShootRate;
        public float ProjectileSpeed;
    
        private IGameFactory _factory;
        private float _shootTimer;
        private bool _canShoot;
    
        private void Update()
        {
            if (!_canShoot)
            {
                _shootTimer -= Time.deltaTime;   
            }

            if (_shootTimer <= 0)
            {
                _shootTimer = ShootRate;
                _canShoot = true;
            }
        }

        public void Construct(IGameFactory factory)
        {
            _factory = factory;
            _canShoot = false;
        }

        public virtual void Shoot(Vector3 to)
        {
            if (_canShoot)
            {
                ShootProjectile(to);

                _canShoot = false;
            }
        }

        private void ShootProjectile(Vector3 to)
        {
            GameObject projectileObj = _factory.CreateProjectile(this);
        
            projectileObj.GetComponent<Rigidbody>().velocity = (to - ShootPoint.transform.position) * ProjectileSpeed;
        }
    }
}