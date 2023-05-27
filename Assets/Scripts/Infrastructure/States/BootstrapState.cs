using Infrastructure.AssetManagement;
using Infrastructure.Factory;
using Services;
using Services.Input;
using Services.PersistentProgress;
using Services.Randomizer;
using Services.ResetLevel;
using Services.SaveLoad;
using Services.StaticData;
using UnityEngine;

namespace Infrastructure.States
{
    public class BootstrapState : IState
    {
        private readonly SceneLoader _sceneLoader;

        private readonly AllServices _services;

        private readonly GameStateMachine _stateMachine;

        public BootstrapState(GameStateMachine stateMachine, SceneLoader sceneLoader, AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.Load(Constants.InitialScene, EnterLoadLevel);
        }

        public void Exit()
        {

        }

        private void RegisterServices()
        {
            RegisterInputService();

            RegisterStaticDataService();

            _services.RegisterSingle<IRandomService>(new RandomService());
            
            _services.RegisterSingle<IAssetProvider>(new AssetProvider());
            _services.RegisterSingle<IPersistentProgressService>(new PersistentProgressService());
            
            _services.RegisterSingle<IResetLevelService>(
                new ResetLevelService(_services.Single<IPersistentProgressService>(), _stateMachine));
            
            _services.RegisterSingle<IGameFactory>(
                new GameFactory(_services.Single<IInputService>(), 
                    _services.Single<IAssetProvider>(), 
                    _services.Single<IPersistentProgressService>(),
                    _services.Single<IStaticDataService>(),
                    _services.Single<IRandomService>(), 
                    _services.Single<IResetLevelService>()));
            
            _services.RegisterSingle<ISaveLoadService>(
                new SaveLoadService(_services.Single<IPersistentProgressService>(),
                    _services.Single<IGameFactory>()));
        }

        private void RegisterStaticDataService()
        {
            IStaticDataService staticDataService = new StaticDataService();
            staticDataService.Load();
            _services.RegisterSingle<IStaticDataService>(staticDataService);
        }

        private void RegisterInputService()
        {
            IInputService inputService;
            if (Application.isMobilePlatform)
                inputService = new MobileInputService();
            else
                inputService = new StandaloneInputService();

            _services.RegisterSingle<IInputService>(inputService);
        }

        private void EnterLoadLevel()
        {
            _stateMachine.Enter<LoadProgressState>();
        }

    }
}
