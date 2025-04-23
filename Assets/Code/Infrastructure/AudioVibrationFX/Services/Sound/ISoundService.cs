namespace Code.Infrastructure.AudioVibrationFX.Services.Sound
{
    public interface ISoundService
    {
        void Cache2DSounds();
        void CreateSoundsPool();
        void PlaySound(Sound2DType type);
    }
}