using StaticData;

namespace Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    WeaponStaticData ForWeapon(WeaponTypeId weaponTypeId);
  }
}