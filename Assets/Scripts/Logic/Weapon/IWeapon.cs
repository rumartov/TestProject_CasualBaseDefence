using UnityEngine;

namespace Logic.Weapon
{
    internal interface IWeapon
    {
        public void Shoot(Vector3 to);
    }
}