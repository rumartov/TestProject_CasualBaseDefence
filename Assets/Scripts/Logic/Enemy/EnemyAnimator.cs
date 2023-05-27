﻿using System;
using Logic.Animation;
using UnityEngine;

namespace Logic.Enemy
{
  public class EnemyAnimator : MonoBehaviour, IAnimationStateReader
  {
    [SerializeField] private Animator animator;
    
    private static readonly int Attack = Animator.StringToHash("Attack");
    private static readonly int Speed = Animator.StringToHash("WalkingSpeed");
    private static readonly int Die = Animator.StringToHash("Die");

    private readonly int _attackStateHash = Animator.StringToHash("Attack");
    private readonly int _walkingStateHash = Animator.StringToHash("Walking");
    private readonly int _deathStateHash = Animator.StringToHash("Die");
    
    public event Action<AnimatorState> StateEntered;
    public event Action<AnimatorState> StateExited;

    public AnimatorState State { get; private set; }
    public void PlayDeath() => animator.SetTrigger(Die);

    public void Move(float speed)
    {
      animator.SetFloat(Speed, speed);
    }

    public void PlayAttack() => animator.SetTrigger(Attack);

    public void EnteredState(int stateHash)
    {
      State = StateFor(stateHash);
      StateEntered?.Invoke(State);
    }
    
    public void ExitedState(int stateHash) => 
      StateExited?.Invoke(StateFor(stateHash));

    private AnimatorState StateFor(int stateHash)
    {
      AnimatorState state;
      if (stateHash == _attackStateHash)
        state = AnimatorState.Attack;
      else if (stateHash == _walkingStateHash)
        state = AnimatorState.Walking;
      else if (stateHash == _deathStateHash)
        state = AnimatorState.Died;
      else
        state = AnimatorState.Unknown;
      
      return state;
    }
  }
}