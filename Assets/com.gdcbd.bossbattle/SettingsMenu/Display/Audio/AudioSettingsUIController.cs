using Studio23.SS2.UIKit.Components;
using System.Collections;
using System.Collections.Generic;
using com.gdcbd.bossbattle;
using UnityEngine;

namespace Studio23.SS2.Settings
{
    public class AudioSettingsUIController : MonoBehaviour
    {
        [SerializeField] private LocalizedLabeledSliderData _masterVolumeSlider;
        [SerializeField] private LocalizedLabeledSliderData _musicVolumeSlider;
        [SerializeField] private LocalizedLabeledSliderData _sfxVolumeSlider;
        [SerializeField] private LocalizedLabeledSliderData _voVolumeSlider;


        private AudioSettingsManager _controller;
        private AudioSettingsData _audioSettingsData;
        private AudioSettingsSaver _audioSettingsSaver;


        void OnEnable()
        {
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.AddListener(Initialize);
            InputManager.Instance.OnHoldResetActionCompleted += Reset;
        }

        void OnDisable()
        {
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.RemoveListener(Initialize);
            InputManager.Instance.OnHoldResetActionCompleted -= Reset;
        }

        void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _controller = AudioSettingsManager.Instance;
            _audioSettingsSaver = _controller.GetComponent<AudioSettingsSaver>();
            _audioSettingsData = _audioSettingsSaver.GetCurrentData();


            InitializeUi();
            InitializeUiEvent();
        }


        private void InitializeUi()
        {
            _masterVolumeSlider.LabeledSlider.InitializeData(_masterVolumeSlider.LabeledLocalizedString, 0.0f, 1.0f, _audioSettingsData.MasterVolume);
            _musicVolumeSlider.LabeledSlider.InitializeData(_musicVolumeSlider.LabeledLocalizedString, 0f, 1f, _audioSettingsData.MusicVolume);
            _sfxVolumeSlider.LabeledSlider.InitializeData(_sfxVolumeSlider.LabeledLocalizedString, 0f, 1f, _audioSettingsData.SfxVolume);
            _voVolumeSlider.LabeledSlider.InitializeData(_voVolumeSlider.LabeledLocalizedString, 0f, 1f, _audioSettingsData.VoVolume);
        }

        private void InitializeUiEvent()
        {
            _masterVolumeSlider.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _controller.MasterSetting.UpdateVolume(value);
                _audioSettingsData.MasterVolume = value;
                Save();
            }));

            _musicVolumeSlider.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _controller.MusicSetting.UpdateVolume(value);
                _audioSettingsData.MusicVolume = value;
                Save();
            }));

            _sfxVolumeSlider.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _controller.SFXSetting.UpdateVolume(value);
                _audioSettingsData.SfxVolume = value;
                Save();
            }));

            _voVolumeSlider.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _controller.VOSetting.UpdateVolume(value);
                _audioSettingsData.VoVolume = value;
                Save();
            }));

        }
        public void SaveNow()=> SettingsSaveManager.Instance.Save(_audioSettingsSaver);
        private async void Save()
        {
            await SettingsSaveManager.Instance.Save(_audioSettingsSaver);
        }

        public void Reset()
        {
            _audioSettingsSaver.SetCurrentData(_audioSettingsSaver.GetDefaultData());
            _audioSettingsData = _audioSettingsSaver.GetCurrentData();
            InitializeUi();
            ApplyAction();
            Save();

        }

        private void ApplyAction()
        {
            _controller.MasterSetting.UpdateVolume(_audioSettingsData.MasterVolume);
            _controller.MusicSetting.UpdateVolume(_audioSettingsData.MusicVolume);
            _controller.SFXSetting.UpdateVolume(_audioSettingsData.SfxVolume);
            _controller.VOSetting.UpdateVolume(_audioSettingsData.VoVolume);
        }
    }
}
