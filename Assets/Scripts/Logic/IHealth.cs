using System;

namespace Logic
{
  public interface IHealth
  {
    event Action HealthChanged;
    float Current { get; }
    float Max { get; }
    void TakeDamage(float damage);
  }
}