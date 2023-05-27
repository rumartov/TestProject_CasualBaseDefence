using UnityEngine;

namespace Logic.Weapon
{
    public class Projectile : MonoBehaviour
    {
        private const string Enemy = "Enemy";
    
        public float LifeTime;
        public float Damage;

        private void OnTriggerEnter(Collider collision)
        {
            SetDestroyTimer();
        
            Transform collisionTransform = collision.transform;
            if (!collisionTransform.CompareTag(Enemy)) 
                return;
        
            collisionTransform.GetComponent<IHealth>().TakeDamage(Damage);
        
            Destroy(gameObject);
        }

        private void SetDestroyTimer() => Destroy(gameObject, LifeTime);
    }
}