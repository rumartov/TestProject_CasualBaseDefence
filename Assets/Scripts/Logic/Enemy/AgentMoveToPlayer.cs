using Data;
using Infrastructure.Factory;
using Logic.Hero;
using UnityEngine;
using UnityEngine.AI;

namespace Logic.Enemy
{
  public class AgentMoveToPlayer : Follow
  {
    public NavMeshAgent Agent;
    
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
      if (_heroTransform && IsHeroNotReached() && _heroZoneCheck.CurrentZone != Zone.Base)
      {
        Agent.isStopped = false;
        Agent.destination = _heroTransform.position;  
      }
      else
      {
        Agent.isStopped = true;
      }
    }
    
    private bool IsHeroNotReached()
    {
      float sqrMagnitudeTo = Agent.transform.position.SqrMagnitudeTo(_heroTransform.position);
      return sqrMagnitudeTo >= MinimalDistance;
    }
  }
}