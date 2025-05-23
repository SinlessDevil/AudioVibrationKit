using Code.Infrastructure.AudioVibrationFX.Services.Music;
using Code.Infrastructure.AudioVibrationFX.Services.Sound;
using Code.Infrastructure.AudioVibrationFX.Services.StaticData;
using Code.Infrastructure.AudioVibrationFX.Services.Vibration;
using Code.Infrastructure.Factory;
using Code.Infrastructure.Services.PersistenceProgress;
using Code.Infrastructure.Services.SaveLoad;
using Code.Infrastructure.Services.StaticData;
using Services.PersistenceProgress;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure
{
    public class BootstrapInstaller : MonoInstaller, IInitializable
    {
        private const string SceneName = "Game";
        
        public override void InstallBindings()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();

            BindFactory();
            BindSaveLoad();
            BindProgressData();
            BindAudioVibration();
            BindStaticData();
        }

        private void BindFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
            Container.BindInterfacesTo<GameFactory>().AsSingle();
        }
        
        private void BindSaveLoad()
        {
            Container.Bind<ISaveLoadService>().To<PrefsSaveLoadService>().AsSingle();
        }

        private void BindProgressData() =>
            Container.Bind<IPersistenceProgressService>().To<PersistenceProgressService>().AsSingle();
        
        
        private void BindStaticData()
        {
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IAudioVibrationStaticDataService>().To<AudioVibrationStaticDataService>().AsSingle();
        }
        
        private void BindAudioVibration()
        {
            Container.Bind<ISoundService>().To<SoundService>().AsSingle();
            Container.Bind<IMusicService>().To<MusicService>().AsSingle();
            Container.Bind<IVibrationService>().To<VibrationService>().AsSingle();
        }
        
        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().LoadData();
            Container.Resolve<IAudioVibrationStaticDataService>().LoadData();
            
            Container.Resolve<ISoundService>().Cache2DSounds();
            Container.Resolve<ISoundService>().CreateSoundsPool();
            
            Container.Resolve<IMusicService>().CacheMusic();
            Container.Resolve<IMusicService>().CreateMusicRoot();
            
            SceneManager.LoadScene(SceneName);
        }
    }
}