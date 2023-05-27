using System;
using System.Collections;
using System.Reflection.Emit;
using UnityEngine;

namespace Logic.Enemy
{
  [RequireComponent(typeof(EnemyHealth), typeof(EnemyAnimator))]
  public class EnemyDeath : MonoBehaviour
  {
    private const string Corpse = "Corpse";
    public bool isDead { get; private set; }
    
    [SerializeField] private EnemyAttack enemyAttack;
    [SerializeField] private AgentMoveToPlayer agentMoveToPlayer;

    public EnemyHealth Health;
    public EnemyAnimator Animator;

    public GameObject DeathFx;

    public event Action Happened;


    private void Start()
    {
      Health.HealthChanged += OnHealthChanged;
      isDead = false;
    }

    private void OnDestroy()
    {
      Health.HealthChanged -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
      if (Health.Current <= 0)
        Die();
    }

    private void Die()
    {
      Health.HealthChanged -= OnHealthChanged;

      enemyAttack.enabled = false;
      agentMoveToPlayer.enabled = false;
      
      Animator.PlayDeath();
      SpawnDeathFx();

      StartCoroutine(DestroyTimer());
      
      Happened?.Invoke();

      isDead = true;
      
      ChangeLayerToCorpse();
    }

    private void ChangeLayerToCorpse()
    {
      int corpseLayer = LayerMask.NameToLayer(Corpse);
      gameObject.layer = corpseLayer;
    }

    private void SpawnDeathFx()
    {
      Instantiate(DeathFx, transform.position, Quaternion.identity);
    }

    private IEnumerator DestroyTimer()
    {
      yield return new WaitForSeconds(3);
      Destroy(gameObject);
    }
  }
}