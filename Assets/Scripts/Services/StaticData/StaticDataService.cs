using System.Collections.Generic;
using System.Linq;
using StaticData;
using UnityEngine;

namespace Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string WeaponsDataPath = "Static Data/Weapons";

    private Dictionary<WeaponTypeId, WeaponStaticData> _weapons;


    public void Load()
    {
      _weapons = Resources
        .LoadAll<WeaponStaticData>(WeaponsDataPath)
        .ToDictionary(x => x.WeaponTypeId, x => x);
    }

    public WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId) =>
      _weapons.TryGetValue(weaponTypeId, out WeaponStaticData staticData)
        ? staticData
        : null;
  }
}