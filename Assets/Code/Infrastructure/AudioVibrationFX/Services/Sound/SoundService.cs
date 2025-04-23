using System.Collections.Generic;
using Code.Infrastructure.AudioVibrationFX.Services.StaticData;
using UnityEngine;

namespace Code.Infrastructure.AudioVibrationFX.Services.Sound
{
    public class SoundService : ISoundService
    {
        private readonly IAudioVibrationStaticDataService _audioVibrationStaticDataService;

        private readonly List<AudioSource> _2dAudioPool = new();
        private readonly List<AudioSource> _3dAudioPool = new();

        private const int PoolSize2D = 10;
        private const int PoolSize3D = 10;

        private Transform _poolParent2D;
        private Transform _poolParent3D;

        public SoundService(IAudioVibrationStaticDataService audioVibrationStaticDataService)
        {
            _audioVibrationStaticDataService = audioVibrationStaticDataService;
        }

        public void CreateSoundsPool()
        {
            _poolParent2D = new GameObject("[Audio2D Pool]").transform;
            _poolParent3D = new GameObject("[Audio3D Pool]").transform;

            Object.DontDestroyOnLoad(_poolParent2D);
            Object.DontDestroyOnLoad(_poolParent3D);

            for (int i = 0; i < PoolSize2D; i++)
            {
                var go = new GameObject($"Audio2D_{i}");
                go.transform.SetParent(_poolParent2D);
                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _2dAudioPool.Add(source);
            }

            for (int i = 0; i < PoolSize3D; i++)
            {
                var go = new GameObject($"Audio3D_{i}");
                go.transform.SetParent(_poolParent3D);
                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                _3dAudioPool.Add(source);
            }
        }

        public void PlaySound(Sound2DType type)
        {
            var data = _audioVibrationStaticDataService.SoundsData.Sounds2DData.Find(s => s.Name == type.ToString());
            if (data == null)
            {
                Debug.LogWarning($"[SoundService] No sound data found for 2D sound type: {type}");
                return;
            }

            var source = _2dAudioPool.Find(s => !s.isPlaying);
            if (source == null)
            {
                Debug.LogWarning("[SoundService] No available AudioSource in 2D pool");
                return;
            }

            source.clip = data.Clip;
            source.volume = data.Volume;
            source.spatialBlend = data.SpatialBlend;
            source.loop = data.Loop;
            source.rolloffMode = data.RolloffMode;
            source.minDistance = data.MinDistance;
            source.maxDistance = data.MaxDistance;

            source.Play();
        }
    }
}
