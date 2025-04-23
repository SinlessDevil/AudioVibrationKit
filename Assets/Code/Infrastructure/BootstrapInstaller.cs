using Code.Infrastructure.AudioVibrationFX.Services.Sound;
using Code.Infrastructure.AudioVibrationFX.Services.StaticData;
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
            BindStaticData();
            BindAudioVibration();
        }

        private void BindFactory()
        {
            Container.BindInterfacesTo<UIFactory>().AsSingle();
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
        }
        
        public void Initialize()
        {
            Container.Resolve<IStaticDataService>().LoadData();
            Container.Resolve<IAudioVibrationStaticDataService>().LoadData();
            
            Container.Resolve<ISoundService>().Cache2DSounds();
            Container.Resolve<ISoundService>().CreateSoundsPool();
            
            SceneManager.LoadScene(SceneName);
        }
    }
}