namespace Code.Infrastructure.AudioVibrationFX.Services.Sound
{
    public interface IMusicService
    {
        void CreateMusicRoot();
        void CacheMusic();
        void Play(MusicType type, bool loop = true);
        void Pause();
        void Resume();
        void Stop();
    }
}