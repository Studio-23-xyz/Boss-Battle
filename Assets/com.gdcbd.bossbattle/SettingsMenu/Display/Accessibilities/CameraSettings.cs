using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;

namespace Studio23.SS2.Settings
{
    public class CameraSettings : MonoBehaviour
    {
        [SerializeField] private CameraSettingsConfiguration _cameraSettingsConfiguration;

        private CinemachineBrain _brain;
        private CinemachineFreeLook _vCam;
        private float _currentFieldOfView;
        private int _currentNoiseSettings;
        private List<CinemachineBasicMultiChannelPerlin> _vCamRigNoises;

        public void Initialize(int cameraShake, float cameraFov)
        {
            _currentFieldOfView = cameraShake;
            _currentNoiseSettings = cameraShake;

            SetCameraSettings(cameraShake, cameraFov);
        }

        private void SetCameraSettings(int cameraShake, float cameraFov)
        {
            ToggleCameraShake(cameraShake);
            ChangeCameraFov(cameraFov);
        }

        public void ChangeCameraFov(float targetFov)
        {
            _currentFieldOfView = targetFov;
            if (_vCam == null) return;
            _vCam.m_Lens.FieldOfView = _currentFieldOfView;
        }

        public void ToggleCameraShake(int cameraShake)
        {
            _currentNoiseSettings = cameraShake;
            if (_vCam == null ||
                _cameraSettingsConfiguration.ShakeNoiseAsset == null||
                _cameraSettingsConfiguration.EmptyNoiseAsset == null) 
                return;

            var selectedNoiseProfile = cameraShake > 0
                ? _cameraSettingsConfiguration.ShakeNoiseAsset
                : _cameraSettingsConfiguration.EmptyNoiseAsset;

            foreach (var vCamNoise in _vCamRigNoises)
                vCamNoise.NoiseProfile = selectedNoiseProfile;
        }

        public float ReturnMaximumFov => _cameraSettingsConfiguration.MaximumCameraFOV;
        public float ReturnMinimumFov => _cameraSettingsConfiguration.MinimumCameraFOV;
        public float ReturnStartingFov => _cameraSettingsConfiguration.StartingCameraFOV;
        public int ReturnDefaultCameraShake => Convert.ToInt32(_cameraSettingsConfiguration.CameraShakeAtStart);


    }
}
