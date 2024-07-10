
using Unity.Cinemachine;
using UnityEngine;

namespace Studio23.SS2.Settings
{
    [CreateAssetMenu(fileName = "CameraSettingsConfiguration", menuName = "Studio-23/Settings/Accessibility/CameraSettingsConfiguration", order = 1)]

    public class CameraSettingsConfiguration : ScriptableObject
    {
        public bool CameraShakeAtStart;
        public NoiseSettings EmptyNoiseAsset;
        public NoiseSettings ShakeNoiseAsset;
        public float StartingCameraFOV;
        public float MinimumCameraFOV;
        public float MaximumCameraFOV;
    }
}
