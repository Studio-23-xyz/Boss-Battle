
using Studio23.SS2.AudioSystem.fmod.Core;
using Studio23.SS2.AudioSystem.fmod.Data;
using Studio23.SS2.Settings;
using Studio23.SS2.Settings.Audio.fmod.Data;
using UnityEngine;

namespace Studio23.SS2
{
    public class AudioSettingsManager : MonoBehaviour
    {
        public static AudioSettingsManager Instance;

        public FMODVCASettings MasterSetting;
        public FMODVCASettings MusicSetting;
        public FMODVCASettings SFXSetting;
        public FMODVCASettings VOSetting;

        private AudioSettingsData _audioSettingsData;

        private void Awake()
        {
            if (Instance == null)
                Instance = this;
            else DestroyImmediate(this);
        }


        public void Initialize()
        {
            _audioSettingsData = GetComponent<AudioSettingsSaver>().GetCurrentData();
            MasterSetting.Initialize(FMODVCAList.Master, _audioSettingsData.MasterVolume);
            MusicSetting.Initialize(FMODVCAList.Music, _audioSettingsData.MusicVolume);
            SFXSetting.Initialize(FMODVCAList.SFX, _audioSettingsData.SfxVolume);
            VOSetting.Initialize(FMODVCAList.VO, _audioSettingsData.VoVolume);
        }

        public AudioSettingsData GenerateDefaultAudioSettingsData()
        {
            return new AudioSettingsData(MasterSetting.GetDefaultVolume(),
                MusicSetting.GetDefaultVolume(), SFXSetting.GetDefaultVolume(), VOSetting.GetDefaultVolume());
        }
    }
}
