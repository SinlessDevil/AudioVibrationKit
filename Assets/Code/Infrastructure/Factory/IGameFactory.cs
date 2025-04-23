using Code.Infrastructure.AudioVibrationFX.Test;

namespace Code.Infrastructure.Factory
{
    public interface IGameFactory
    {
        public TeleportingSoundEmitter CreateTeleportingSoundEmitter();
    }
}