using System.Collections.Generic;
using Logic.Enemy;
using Logic.Weapon;
using Services;
using Services.PersistentProgress;
using UnityEngine;

namespace Infrastructure.Factory
{
    public interface IGameFactory : IService
    {
        List<ISavedProgressReader> ProgressReaders { get; }
        List<ISavedProgress> ProgressWriters { get; }
        GameObject Hero { get; }
        GameObject CreateHero(Vector3 at);
        GameObject CreateEnemy(Vector3 at);
        LootPiece CreateLoot();
        void CreateHud();
        void CreateUniqueLevelId();
        void Cleanup();
        GameObject CreateProjectile(Weapon weapon);
        GameObject CreateEnemySpawner(Vector3 at);
    }
}