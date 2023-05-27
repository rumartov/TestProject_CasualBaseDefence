using System;
using Data;
using Services.PersistentProgress;
using UnityEngine;

namespace Logic.Hero
{
  public class HeroHealth : MonoBehaviour, ISavedProgress, IHealth
  {
    public event Action HealthChanged;
    
    [SerializeField] private GameObject _hitFx;

    private State _state;

    public float Current
    {
      get => _state.CurrentHP;
      set
      {
        if (value != _state.CurrentHP)
        {
          _state.CurrentHP = value;
          
          HealthChanged?.Invoke();
        }
      }
    }

    public float Max
    {
      get => _state.MaxHP;
      set => _state.MaxHP = value;
    }


    public void LoadProgress(PlayerProgress progress)
    {
      _state = progress.HeroState;
      HealthChanged?.Invoke();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      progress.HeroState.CurrentHP = Current;
      progress.HeroState.MaxHP = Max;
    }

    public void TakeDamage(float damage)
    {
      if(Current <= 0)
        return;
      
      SpawnHitFx();

      Current -= damage;
    }

    private void SpawnHitFx()
    {
      GameObject hitFx = Instantiate(_hitFx, transform.position, Quaternion.identity);
      Destroy(hitFx, 3);
    }
  }
}