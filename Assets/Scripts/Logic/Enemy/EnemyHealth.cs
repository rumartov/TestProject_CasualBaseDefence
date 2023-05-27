using System;
using UnityEngine;

namespace Logic.Enemy
{
  public class EnemyHealth : MonoBehaviour, IHealth
  {
    [SerializeField] private float current;

    [SerializeField] private GameObject hitFx;

    [SerializeField] private float max;

    public event Action HealthChanged;

    public float Current
    {
      get => current;
      set => current = value;
    }

    public float Max
    {
      get => max;
      set => max = value;
    }

    public void TakeDamage(float damage)
    {
      Current -= damage;
      
      SpawnHitFx();

      HealthChanged?.Invoke();
    }

    private void SpawnHitFx()
    {
      GameObject hitFx = Instantiate(this.hitFx, transform.position, Quaternion.identity);
      Destroy(hitFx, 3);
    }
  }
}