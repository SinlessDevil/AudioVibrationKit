using Code.Infrastructure.AudioVibrationFX.Test;
using Zenject;

namespace Code.Infrastructure.Factory
{
    public class GameFactory : Factory, IGameFactory
    {
        private const string TeleportingSoundEmitterPath = "Tests/TeleportingSoundEmitter";
        
        public GameFactory(IInstantiator instantiator) : base(instantiator)
        {
            
        }
        
        public TeleportingSoundEmitter CreateTeleportingSoundEmitter()
        {
            var teleportingSoundEmitterObject = Instantiate(TeleportingSoundEmitterPath).transform;
            return teleportingSoundEmitterObject.GetComponent<TeleportingSoundEmitter>();
        }
    }
}