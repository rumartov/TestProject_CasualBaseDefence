using Logic.Animation;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class StopMovingOnDeath : MonoBehaviour
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
        }

        private void OnDestroy()
        {
            _animator.StateEntered -= SwitchMovementOff;
        }

        private void SwitchMovementOff(AnimatorState animatorState)
        {
            if (animatorState == AnimatorState.Died)
                _agent.isStopped = true;
        }
        
    }
}