using System.Collections.Generic;
using Infrastructure.AssetManagement;
using Infrastructure.States;
using Logic;
using Logic.Enemy;
using Logic.Hero;
using Logic.UI;
using Logic.Weapon;
using Services.Input;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.ResetLevel;
using Services.StaticData;
using StaticData;
using UI;
using UnityEngine;

namespace Infrastructure.Factory
{
    public class GameFactory : IGameFactory
    {
        private readonly IAssetProvider _assets;
        private readonly IPersistentProgressService _progressService;
        private readonly GameStateMachine _stateMachine;
        private readonly IInputService _inputService;
        private readonly IStaticDataService _staticDataService;
        private readonly IRandomService _randomService;
        private readonly IResetLevelService _resetLevelService;

        public GameObject Hero { get; private set; }

        public GameFactory(
            IInputService inputService, 
            IAssetProvider assets,
            IPersistentProgressService progressService,
            IStaticDataService staticDataService, 
            IRandomService randomService,
            IResetLevelService resetLevelService)
        {
            _inputService = inputService;
            _assets = assets;
            _progressService = progressService;
            _staticDataService = staticDataService;
            _randomService = randomService;
            _resetLevelService = resetLevelService;
        }

        public List<ISavedProgressReader> ProgressReaders { get; } = new();
        public List<ISavedProgress> ProgressWriters { get; } = new();

        public GameObject CreateHero(Vector3 at)
        {
            GameObject hero = InstantiateRegistered(AssetPath.HeroPath, at);

            hero.GetComponent<HeroMove>().Construct(_inputService);

            HeroAttack heroAttack = hero.GetComponent<HeroAttack>();
            GameObject weapon = CreateWeapon(WeaponTypeId.Rifle, heroAttack.weaponHolder);
            heroAttack.Construct(weapon.GetComponent<Weapon>());
            
            hero.GetComponent<HeroBackpack>().Construct(this);
            
            Hero = hero;
            
            return hero;
        }

        private GameObject CreateWeapon(WeaponTypeId typeId, Transform holder)
        {
            WeaponStaticData weaponStaticData = _staticDataService.ForWeapon(typeId);
            
            GameObject weaponObj = _assets
                .Instantiate(
                    weaponStaticData.Prefab,
                    holder);

            Weapon weapon = weaponObj.GetComponent<Weapon>();
            weapon.Construct(this);

            Transform weaponObjTransform = weaponObj.transform;
            weaponObjTransform.localPosition = weaponStaticData.PositionInArms;
            weaponObjTransform.localRotation = Quaternion.Euler(weaponStaticData.RotationInArms);
            
            weapon.ProjectilePrefab = weaponStaticData.ProjectilePrefab;
            weapon.ProjectileSpeed = weaponStaticData.ProjectileSpeed;
            weapon.ShootRate = weaponStaticData.ShootRate;
            weapon.ProjectileLifeTime = weaponStaticData.ProjectileLifeTime;
            weapon.Damage = weaponStaticData.Damage;

            return weaponObj;
        }

        public GameObject CreateEnemy(Vector3 at)
        {
            GameObject enemy = InstantiateRegistered(AssetPath.EnemyPath, at);

            enemy.GetComponent<AgentMoveToPlayer>().Construct(Hero.transform, Hero.GetComponent<ZoneCheck>());
            enemy.GetComponent<EnemyAttack>().Construct(Hero.transform);
            
            IHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            enemy.GetComponent<ActorUI>().Construct(enemyHealth);

            LootSpawner lootSpawner = enemy.GetComponent<LootSpawner>();
            lootSpawner.Construct(this, _randomService);
            lootSpawner.SetLootValue(1);
            
            return enemy;
        }

        public LootPiece CreateLoot()
        {
            LootPiece lootPiece = InstantiateRegistered(AssetPath.Loot)
                .GetComponent<LootPiece>();
      
            lootPiece.Construct(_progressService.Progress.WorldData, 
                Hero.GetComponent<HeroBackpack>());

            return lootPiece;
        }

        public void CreateHud()
        {
            GameObject hud = InstantiateRegistered(AssetPath.HudPath);
            hud.GetComponentInChildren<LootCounter>()
                .Construct(_progressService.Progress.WorldData);
            
            hud.GetComponentInChildren<RestartScreen>()
                .Construct(Hero.GetComponent<HeroDeath>(), _resetLevelService);
        }

        public void CreateUniqueLevelId()
        {
            GameObject uniqueId = InstantiateRegistered(AssetPath.UniqueLevelId);
            uniqueId.GetComponentInChildren<UniqueId>()
                .GenerateId();
        }

        public void Cleanup()
        {
            ProgressReaders.Clear();
            ProgressWriters.Clear();
        }
        
        public GameObject CreateProjectile(Weapon weapon)
        {
            GameObject projectileObj = _assets
                .Instantiate(weapon.ProjectilePrefab, weapon.ShootPoint.position, weapon.ShootPoint.rotation);
            
            Projectile projectile = projectileObj.GetComponent<Projectile>();
            projectile.Damage = weapon.Damage;
            projectile.LifeTime = weapon.ProjectileLifeTime;

            return projectileObj;
        }

        private GameObject InstantiateRegistered(string prefabPath, Vector3 at)
        {
            var gameObject = _assets.Instantiate(prefabPath, at);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        public GameObject CreateEnemySpawner(Vector3 at)
        {
            GameObject enemySpawner = InstantiateRegistered(AssetPath.EnemySpawnerPath, at);

            enemySpawner.GetComponent<EnemySpawner>().Construct(this, _randomService);

            return enemySpawner;
        }

        private GameObject InstantiateRegistered(string prefabPath)
        {
            var gameObject = _assets.Instantiate(prefabPath);

            RegisterProgressWatchers(gameObject);
            return gameObject;
        }

        private void RegisterProgressWatchers(GameObject gameObject)
        {
            foreach (var progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
                Register(progressReader);
        }

        private void Register(ISavedProgressReader progressReader)
        {
            if (progressReader is ISavedProgress progressWriter)
                ProgressWriters.Add(progressWriter);

            ProgressReaders.Add(progressReader);
        }
    }
}