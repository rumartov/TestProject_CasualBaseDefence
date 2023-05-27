using Data;
using Services.PersistentProgress;
using UnityEngine;

namespace Logic.Hero
{
  public class HeroAttack : MonoBehaviour, ISavedProgressReader
  {
    public Transform weaponHolder;
    
    public Transform target;

    [SerializeField] private HeroAnimator animator;

    [SerializeField] private HeroAggro aggroZone;

    [SerializeField] private ZoneCheck zoneCheck;
    
    private Weapon.Weapon _weapon;
    
    public void Construct(Weapon.Weapon weapon)
    {
      _weapon = weapon;
    }

    private void Update()
    {
      SetTarget();
      
      if (target && zoneCheck.CurrentZone != Zone.Base)
      {
        Attack(target.transform.position);
        if(!animator.IsAttacking)
          animator.PlayAttack();
      }
    }

    private void SetTarget()
    {
      target = aggroZone.currentTarget;
    }

    private void Attack(Vector3 to)
    {
      _weapon.Shoot(to);
    }
    
    public void LoadProgress(PlayerProgress progress)
    {
      //_stats = progress.HeroStats;
    }
  }
}