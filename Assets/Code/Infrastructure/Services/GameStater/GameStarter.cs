using Code.Infrastructure.Factory;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.PersistenceProgress.Player;
using Code.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace Code.Infrastructure.Services.GameStater
{
    public class GameStarter : IGameStarter
    {
        private readonly IPersistenceProgressService _progressService;
        private readonly IUIFactory _uiFactory;
        private readonly ISaveLoadService _saveLoadService;
        private readonly IGameFactory _gameFactory;
        
        public GameStarter(
            IPersistenceProgressService progressService,
            IUIFactory uiFactory,
            ISaveLoadService saveLoadService, 
            IGameFactory gameFactory)
        {
            _progressService = progressService;
            _uiFactory = uiFactory;
            _saveLoadService = saveLoadService;
            _gameFactory = gameFactory;
        }

        public void Initialize()
        {
            Debug.Log("GameStarter.Initialize");
            
            InitProgress();
            InitUI();
            InitTests();
        }
        
        private void InitProgress()
        {
            _progressService.PlayerData = LoadProgress() ?? SetUpBaseProgress();   
        }
        
        private void InitUI()
        {
            _uiFactory.CreateUIRoot();
            _uiFactory.CreateGameHud();
        }
        
        private void InitTests()
        {
            _gameFactory.CreateTeleportingSoundEmitter();
        }
        
        private PlayerData LoadProgress()
        {
            Debug.Log("LoadProgress");

            return _saveLoadService.Load();
        }

        private PlayerData SetUpBaseProgress()
        {
            Debug.Log("InitializeProgress");
            var progress = new PlayerData();
            _progressService.PlayerData = progress;
            return progress;
        }
    }
}