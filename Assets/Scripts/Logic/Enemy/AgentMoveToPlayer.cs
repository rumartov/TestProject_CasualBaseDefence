using Data;
using Infrastructure.Factory;
using Logic.Hero;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
  public class AgentMoveToPlayer : Follow
  {
    [SerializeField] private NavMeshAgent agent;
    [SerializeField] private EnemyDeath enemyDeath;

    private const float MinimalDistance = 1f;

    private IGameFactory _gameFactory;
    private Transform _heroTransform;
    private ZoneCheck _heroZoneCheck;

    public void Construct(Transform heroTransform, ZoneCheck heroZoneCheck)
    {
      _heroZoneCheck = heroZoneCheck;
      _heroTransform = heroTransform;
    }

    private void Update()
    {
      if (_heroTransform &&
          IsHeroNotReached()
          && _heroZoneCheck.CurrentZone != Zone.Base
          && !enemyDeath.isDead)
      {
        agent.isStopped = false;
        agent.destination = _heroTransform.position;  
      }
      else
      {
        agent.isStopped = true;
      }
    }
    
    private bool IsHeroNotReached()
    {
      float sqrMagnitudeTo = agent.transform.position.SqrMagnitudeTo(_heroTransform.position);
      return sqrMagnitudeTo >= MinimalDistance;
    }
  }
}