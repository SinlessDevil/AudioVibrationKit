using Code.Infrastructure.AudioVibrationFX.StaticData;
using UnityEngine;

namespace Code.Infrastructure.AudioVibrationFX.Services.StaticData
{
    public class AudioVibrationStaticDataService : IAudioVibrationStaticDataService
    {
        private const string SoundsDataPath = "StaticData/Sounds/Sounds";

        private SoundsData _soundsData;
        
        public SoundsData SoundsData => _soundsData;

        public void LoadData()
        {
            _soundsData = Resources.Load<SoundsData>(SoundsDataPath);
        }
    }
}