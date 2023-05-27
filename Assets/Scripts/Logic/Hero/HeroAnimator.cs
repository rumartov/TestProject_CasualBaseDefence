using System;
using Logic.Animation;
using UnityEngine;

namespace Logic.Hero
{
  public class HeroAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] private CharacterController characterController;
    [SerializeField] public Animator animator;

    private static readonly int WalkingSpeedHash = Animator.StringToHash("WalkingSpeed");
    private static readonly int AttackHash = Animator.StringToHash("Shoot");
    private static readonly int DieHash = Animator.StringToHash("Die");

    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _walkingStateHash = Animator.StringToHash("Walking");
    private readonly int _deathStateHash = Animator.StringToHash("Die");

    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public bool IsAttacking => State == AnimatorState.Attack;

    private void Update() => animator.SetFloat(
      WalkingSpeedHash, characterController.velocity.magnitude, 0.1f, Time.deltaTime);
    
    public void PlayAttack()
    { 
      animator.SetTrigger(AttackHash);
    }

    public void PlayDeath()
    {
      animator.SetTrigger(DieHash);
    }

    public void EnteredState(int stateHash)
    {
      State = StateFor(stateHash);
      StateEntered?.Invoke(State);
    }

    public void ExitedState(int stateHash)
    {
      StateExited?.Invoke(StateFor(stateHash));
    }

    private AnimatorState StateFor(int stateHash)
    {
      AnimatorState state;
      if (stateHash == _attackStateHash)
      {
        state = AnimatorState.Attack;
      }
      else if (stateHash == _walkingStateHash)
      {
        state = AnimatorState.Walking;
      }
      else if (stateHash == _deathStateHash)
      {
        state = AnimatorState.Died;
      }
      else
      {
        state = AnimatorState.Unknown;
      }

      return state;
    }
  }
}