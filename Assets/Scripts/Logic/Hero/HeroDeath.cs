using System;
using UnityEngine;

namespace Logic.Hero
{
  public class HeroDeath : MonoBehaviour
  {
    [SerializeField] private HeroHealth health;
    [SerializeField] private HeroMove move;
    [SerializeField] private HeroAttack attack;
    [SerializeField] private HeroAnimator animator;

    [SerializeField] private GameObject deathFx;

    public event Action HeroDied; 
    
    private bool _isDead;

    private void Start()
    {
      health.HealthChanged += HealthChanged;
    }

    private void OnDestroy()
    {
      health.HealthChanged -= HealthChanged;
    }

    private void HealthChanged()
    {
      if (!_isDead && health.Current <= 0) 
        Die();
    }

    private void Die()
    {
      _isDead = true;
      move.enabled = false;
      attack.enabled = false;
      animator.PlayDeath();

      Instantiate(deathFx, transform.position, Quaternion.identity);
      
      HeroDied?.Invoke();
    }
  }
}