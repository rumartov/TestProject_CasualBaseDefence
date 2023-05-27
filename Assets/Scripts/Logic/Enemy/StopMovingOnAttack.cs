using Logic.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
  [RequireComponent(typeof(NavMeshAgent))]
  public class StopMovingOnAttack : MonoBehaviour
  {
    private EnemyAnimator _animator;
    private NavMeshAgent _agent;

    private void Awake()
    {
      _agent = GetComponent<NavMeshAgent>();
      _animator = GetComponent<EnemyAnimator>();
    }

    private void Start()
    {
      _animator.StateEntered += SwitchMovementOff;
      _animator.StateExited += SwitchMovementOn;
    }
    
    private void OnDestroy()
    {
      _animator.StateEntered -= SwitchMovementOff;
      _animator.StateExited -= SwitchMovementOn;
    }

    private void SwitchMovementOn(AnimatorState animatorState)
    {
      if (animatorState == AnimatorState.Attack)
        _agent.isStopped = false;
    }

    private void SwitchMovementOff(AnimatorState animatorState)
    {
      if (animatorState == AnimatorState.Attack)
        _agent.isStopped = true;
    }
  }
}