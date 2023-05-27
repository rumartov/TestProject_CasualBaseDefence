using System.Collections.Generic;
using System.Linq;
using Logic.Enemy;
using Logic.Units;
using UnityEngine;

namespace Logic.Hero
{
    public class HeroAggro : MonoBehaviour
    {
        public Transform currentTarget;

        [SerializeField] private TriggerObserver triggerObserver;
        
        private List<Transform> _targets = new();
        private float _closestTargetDistance;

        private const string Enemy = "Enemy";

        private void Start()
        {
            triggerObserver.TriggerEnter += AddTarget;
            triggerObserver.TriggerExit += RemoveTarget;
        }

        private void Update()
        {
            currentTarget = GetClosestTarget();
        }

        private void OnDestroy()
        {
            triggerObserver.TriggerEnter -= AddTarget;
            triggerObserver.TriggerExit -= RemoveTarget;
        }
        
        private void AddTarget(Collider obj)
        {
            if (obj == null || !obj.CompareTag(Enemy))
                return;
            
            Debug.Log("Add " + obj.name);
            _targets.Add(obj.transform);
        }

        private Transform GetClosestTarget()
        {
            Transform closestTarget = null;
            foreach (var target in _targets)
            {
                if (target == null || target.GetComponent<EnemyDeath>().isDead)
                {
                    continue;
                }
                
                float distance = Vector3.Distance(target.position, transform.position);
                
                if (closestTarget == null)
                {
                    closestTarget = SetTarget(target, distance);
                }
                
                if (distance < _closestTargetDistance)
                {
                    closestTarget = SetTarget(target, distance);
                }
            }

            return closestTarget;
        }

        private Transform SetTarget(Transform target, float distance)
        {
            _closestTargetDistance = distance;
            return target;
        }

        private void RemoveTarget(Collider obj)
        {
            Debug.Log("Remove " + obj.name);

            _targets.Remove(obj.transform);
            _targets = _targets.Where(s => !string.IsNullOrWhiteSpace(s.name)).Distinct().ToList();
        }
    }
}