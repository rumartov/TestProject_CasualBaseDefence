using UnityEngine;

namespace StaticData
{
    [CreateAssetMenu(fileName = "WeaponData", menuName = "Static Data/Weapon")]
    public class WeaponStaticData : ScriptableObject
    {
        public WeaponTypeId WeaponTypeId;

        [Range(1,100)]
        public float Damage = 10;
        
        [Range(0.1f, 10)]
        public float ShootRate = 3;

        [Range(1,30)]
        public float ProjectileSpeed = 1;

        [Range(1, 10)]
        public float ProjectileLifeTime = 3;
        
        public Vector3 RotationInArms;
        public Vector3 PositionInArms;

        public GameObject ProjectilePrefab;

        public GameObject Prefab;
    }
}