
using System.Collections.Generic;
using System.Linq;
using Studio23.SS2.Settings.Video.Core;
using Studio23.SS2.Settings.Video.Data;
using Studio23.SS2.UIKit.Components;
using UnityEngine;

namespace Studio23.SS2
{
    public class DisplaySettingsUI : MonoBehaviour, IApplyAction
    {

        [SerializeField] private LocalizedRotatorData _screenMode;
        [SerializeField] private LocalizedRotatorData _resolution;
        [SerializeField] private LocalizedLabeledSliderData _renderScale;
        [SerializeField] private LocalizedLabeledSliderData _brightness;
        [SerializeField] private LocalizedRotatorData _vsync;

        private VideoSettingsData _tempVideoSettingsData;
        private VideoSettingsData _currentVideoSettingsData;
        private DisplayController _displayController;
        private VideoSettingsSaver _videoSettingsSaver;

        private List<Resolution> _resolutionData;


        void OnEnable()
        {
            GetComponent<IApplyAction>().SubscribeEvent();
        }

        void OnDisable()
        {
            SettingsSaveManager.Instance.OnLoadComplete.RemoveListener(Initialize);
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.RemoveListener(Initialize);
            GetComponent<IApplyAction>().UnSubscribeEvent();
            SetApplyAction();
        }
        void Start()
        { Initialize();
           SettingsSaveManager.Instance.OnLoadComplete.AddListener(Initialize);
           SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.AddListener(Initialize);
        }

        public void Initialize()
        {
            Debug.Log("Initialize  Load completed!");
            _videoSettingsSaver = VideoSettingsManager.Instance.GetComponent<VideoSettingsSaver>();
            _displayController = VideoSettingsManager.Instance.DisplayController;
            _tempVideoSettingsData = _videoSettingsSaver.GetCurrentData();
            _currentVideoSettingsData = new VideoSettingsData(_tempVideoSettingsData);

            InitializeUi();
            InitializeUiEvent();
        }

        private void InitializeUi()
        {
            _screenMode.RotatorButton.InitializeData(_screenMode.LabelLocalizedString, _screenMode.DataLocalizedTable, _tempVideoSettingsData.ScreenModeIndex);

            _resolutionData = new List<Resolution>();
            _resolutionData = _displayController.GetSupportedResolutions().ToList();
            _resolutionData.Reverse();
            _resolution.RotatorButton.InitializeData(_resolution.LabelLocalizedString,_resolutionData, _tempVideoSettingsData.ResolutionIndex);

            _renderScale.LabeledSlider.InitializeData(_renderScale.LabeledLocalizedString, _tempVideoSettingsData.MinRenderScale, _tempVideoSettingsData.MaxRenderScale, _tempVideoSettingsData.RenderScale);

            float currentBrightnessLevel = ConvertValueToRange(-3, 1, .1f,
                1f, _tempVideoSettingsData.BrightnessLevel);

            _brightness.LabeledSlider.InitializeData(_brightness.LabeledLocalizedString, 0.1f, 1.0f, currentBrightnessLevel);
            _vsync.RotatorButton.InitializeData(_vsync.LabelLocalizedString, _vsync.DataLocalizedTable, _tempVideoSettingsData.VSyncCount);
        }

        private void InitializeUiEvent()
        {
            _screenMode.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempVideoSettingsData.ScreenModeIndex = index;
            }));

            _resolution.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempVideoSettingsData.ResolutionIndex = index;
            }));
            _renderScale.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _displayController.SetRenderScale(value);
                _tempVideoSettingsData.RenderScale = value;
            }));
            _brightness.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                var brightnessLevel = ConvertValueToRange(.1f, 1f, _tempVideoSettingsData.MinBrightnessLevel,
                    _tempVideoSettingsData.MaxBrightnessLevel, value);
                _displayController.SetBrightness(brightnessLevel);
                _tempVideoSettingsData.BrightnessLevel = brightnessLevel;
            }));
            _vsync.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempVideoSettingsData.VSyncCount = index;
            }));
        }

        public float ConvertValueToRange(float inputMin, float inputMax, float outputMin, float outputMax, float inputValue)
        {
            float value = outputMin + ((outputMax - outputMin) * ((inputValue - inputMin) / (inputMax - inputMin)));

            return value;
        }

        private async void ApplyAction()
        {
            GetComponent<IApplyAction>().UnSubscribeEvent();
            PopulateCurrentVideoSettings();
            SetApplyAction();
            await SettingsSaveManager.Instance.Save(_videoSettingsSaver);
            GetComponent<IApplyAction>().SubscribeEvent();
        }


        private void SetApplyAction()
        {
            _displayController.ChangeFullScreenMode(_currentVideoSettingsData.ScreenModeIndex);
            _displayController.ChangeResolution((_resolutionData.Count - 1) -_currentVideoSettingsData.ResolutionIndex);
            _displayController.ChangeVSync(_currentVideoSettingsData.VSyncCount);
            _displayController.SetRenderScale(_currentVideoSettingsData.RenderScale);
            _displayController.SetBrightness(_currentVideoSettingsData.BrightnessLevel);
        }

        private void PopulateCurrentVideoSettings()
        {
            _currentVideoSettingsData = new VideoSettingsData(_tempVideoSettingsData);
        }


        public void Save()
        {
            ApplyAction();
        }

        public void Reset()
        {
            _videoSettingsSaver.SetCurrentData(_videoSettingsSaver.GetDefaultData());
            _tempVideoSettingsData = _videoSettingsSaver.GetCurrentData();
            ApplyAction();
            InitializeUi();
        }
    }


}
