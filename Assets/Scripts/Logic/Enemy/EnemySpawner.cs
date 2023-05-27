using Infrastructure.Factory;
using Services.Randomizer;
using UnityEngine;

namespace Logic.Enemy
{
    public class EnemySpawner : MonoBehaviour
    {
        [SerializeField] private int radius;
        [SerializeField] private int maxAliveEnemies;
        [SerializeField] private int spawnTime;
    
        private IGameFactory _factory;
        private IRandomService _randomService;
        private float _timer;

        private int _aliveEnemies;

        public void Construct(IGameFactory factory, IRandomService randomService)
        {
            _factory = factory;
            _randomService = randomService;
        }
        
        private void Update()
        {
            DecrementTimer();
            if (IsTimerOver() && CanSpawn())
            {
                CooldownTimer();
                SpawnEnemy();
            }
        }

        private bool CanSpawn()
        {
            return _aliveEnemies <= maxAliveEnemies;
        }

        private bool IsTimerOver()
        {
            return _timer <= 0;
        }

        private void DecrementTimer()
        {
            _timer -= Time.deltaTime;
        }

        private void CooldownTimer()
        {
            _timer = spawnTime;
        }

        public void SpawnEnemy()
        {
            var position = transform.position;
            Vector3 randomSpawnPosition = new Vector3(position.x + _randomService.Next(0, radius),
                position.y,
                position.z + _randomService.Next(0, radius));

            GameObject enemy = _factory.CreateEnemy(randomSpawnPosition);
            enemy.GetComponent<EnemyDeath>().Happened += EnemyDied;

            _aliveEnemies++;
        }

        private void EnemyDied()
        {
            _aliveEnemies--;
        }
    }
}