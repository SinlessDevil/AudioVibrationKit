using System.Collections.Generic;
using Code.Infrastructure.AudioVibrationFX.Services.StaticData;
using Code.Infrastructure.AudioVibrationFX.StaticData;
using UnityEngine;

namespace Code.Infrastructure.AudioVibrationFX.Services.Sound
{
    public class SoundService : ISoundService
    {
        private readonly IAudioVibrationStaticDataService _audioVibrationStaticDataService;

        private readonly List<AudioSource> _2dAudioPool = new();
        private readonly List<AudioSource> _3dAudioPool = new();

        private readonly Dictionary<Sound2DType, SoundData> _cached2DSounds = new();

        private const int PoolSize = 5;

        private Transform _poolParent2D;
        private Transform _poolParent3D;

        public SoundService(IAudioVibrationStaticDataService audioVibrationStaticDataService)
        {
            _audioVibrationStaticDataService = audioVibrationStaticDataService;
        }

        public void Cache2DSounds()
        {
            foreach (var sound in _audioVibrationStaticDataService.SoundsData.Sounds2DData)
            {
                if (!_cached2DSounds.ContainsKey(sound.Sound2DType))
                    _cached2DSounds.Add(sound.Sound2DType, sound);
            }
        }
        
        public void CreateSoundsPool()
        {
            _poolParent2D = CreatePoolParent("[Audio2D Pool]");
            _poolParent3D = CreatePoolParent("[Audio3D Pool]");

            CreateAudioPool(_2dAudioPool, _poolParent2D, "Audio2D", PoolSize);
            CreateAudioPool(_3dAudioPool, _poolParent3D, "Audio3D", PoolSize);
        }

        public void PlaySound(Sound2DType type)
        {
            if (!_cached2DSounds.TryGetValue(type, out var data))
            {
                Debug.LogWarning($"[SoundService] No sound data found for 2D sound type: {type}");
                return;
            }

            if (!TryGetAvailableSource(_2dAudioPool, out var source))
            {
                Debug.LogWarning("[SoundService] No available AudioSource in 2D pool");
                return;
            }

            SetupSource(source, data);
            source.Play();
        }
        
        private Transform CreatePoolParent(string name)
        {
            var parent = new GameObject(name).transform;
            Object.DontDestroyOnLoad(parent);
            return parent;
        }

        private void CreateAudioPool(List<AudioSource> pool, Transform parent, string prefix, int size)
        {
            for (int i = 0; i < size; i++)
            {
                var go = new GameObject($"{prefix}_{i}");
                go.transform.SetParent(parent);
                var source = go.AddComponent<AudioSource>();
                source.playOnAwake = false;
                pool.Add(source);
            }
        }
        
        private bool TryGetAvailableSource(List<AudioSource> pool, out AudioSource source)
        {
            source = pool.Find(s => !s.isPlaying);
            return source != null;
        }

        private void SetupSource(AudioSource source, SoundData data)
        {
            source.clip = data.Clip;
            source.volume = data.Volume;
            source.loop = data.Loop;
            source.playOnAwake = data.PlayOnAwake;
        }
    }
}
