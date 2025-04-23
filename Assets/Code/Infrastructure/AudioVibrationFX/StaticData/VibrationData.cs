using System;
using Code.Infrastructure.AudioVibrationFX.Services.Vibration;
using MoreMountains.NiceVibrations;
using UnityEngine;

namespace Code.Infrastructure.AudioVibrationFX.StaticData
{
    [Serializable]
    public class VibrationData
    {
        public string Name;
        public HapticTypes HapticType;
        public VibrationType VibrationType = VibrationType.Unknown;
    }
}