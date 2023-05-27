using Logic.Units;
using UnityEngine;

namespace Logic.Enemy
{
  [RequireComponent(typeof(EnemyAttack))]
  public class CheckAttackRange : MonoBehaviour
  {
    public EnemyAttack enemyAttack;
    public TriggerObserver TriggerObserver;

    private void Start()
    {
      TriggerObserver.TriggerEnter += TriggerEnter;
      TriggerObserver.TriggerExit += TriggerExit;
      
      enemyAttack.DisableAttack();
    }

    private void TriggerExit(Collider obj)
    {
      enemyAttack.DisableAttack();
    }

    private void TriggerEnter(Collider obj)
    {
      
      enemyAttack.EnableAttack();
    }
  }
}