using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
  [RequireComponent(typeof(NavMeshAgent))]
  [RequireComponent(typeof(EnemyAnimator))]
  public class AnimateAlongAgent : MonoBehaviour
  {
    [SerializeField] private NavMeshAgent Agent;
    [SerializeField] private EnemyAnimator Animator;

    private void Update()
    {
      Animator.Move(Agent.velocity.magnitude);
    }
  }
}