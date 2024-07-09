using Studio23.SS2.Settings;
using Studio23.SS2.UIKit.Components;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using static Studio23.SS2.Settings.LanguageSettings;

namespace Studio23.SS2
{
    public class AccessibilitySettingsUI : MonoBehaviour, IApplyAction
    {
        [SerializeField] private LocalizedLabeledSliderData _cameraFov;
        [SerializeField] private LocalizedRotatorData _cameraShake;
        [SerializeField] private LocalizedRotatorData _toggleSubtitle;
        [SerializeField] private LocalizedRotatorData _language;

        private AccessibilitySettingsController _controller;
        private AccessibilitySettingsData _tempAccessibilitySettingsData;
        private AccessibilitySettingsData _currentAccessibilitySettingsData;
        private AccessibilitySettingsSaver _accessibilitySettingsSaver;


        void OnEnable()
        {
            GetComponent<IApplyAction>().SubscribeEvent();
        }

        void OnDisable()
        {
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.RemoveListener(Initialize);
            GetComponent<IApplyAction>().UnSubscribeEvent();
            SetApplyAction();
        }

        void Start()
        {
            Initialize();
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.AddListener(Initialize);

        }

        private void Initialize()
        {
            _controller = AccessibilitySettingsController.Instance;
            _accessibilitySettingsSaver = _controller.GetComponent<AccessibilitySettingsSaver>();
            _tempAccessibilitySettingsData = _accessibilitySettingsSaver.GetCurrentData();
            _currentAccessibilitySettingsData = new AccessibilitySettingsData(_tempAccessibilitySettingsData);

            InitializeUi();
            InitializeUiEvent();

        }

        private void InitializeUiEvent()
        {
            _cameraFov.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _tempAccessibilitySettingsData.CameraFOV = value;
            }));

            _cameraShake.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempAccessibilitySettingsData.ShowCameraShake = index;
            }));

            _toggleSubtitle.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempAccessibilitySettingsData.ShowSubtitle = index;
            }));

            _language.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempAccessibilitySettingsData.LanguageIndex = index;
            }));
        }

        private void InitializeUi()
        {
            _cameraFov.LabeledSlider.InitializeData(_cameraFov.LabeledLocalizedString, _controller.CameraSettings.ReturnMinimumFov, _controller.CameraSettings.ReturnMaximumFov, _tempAccessibilitySettingsData.CameraFOV);
            _cameraShake.RotatorButton.InitializeData(_cameraShake.LabelLocalizedString, _cameraShake.DataLocalizedTable, _tempAccessibilitySettingsData.ShowCameraShake);
            _toggleSubtitle.RotatorButton.InitializeData(_toggleSubtitle.LabelLocalizedString, _toggleSubtitle.DataLocalizedTable, _tempAccessibilitySettingsData.ShowSubtitle);
            _language.RotatorButton.InitializeData(_language.LabelLocalizedString, _language.DataLocalizedTable, _tempAccessibilitySettingsData.LanguageIndex);
        }


        private async void ApplyAction()
        {
            GetComponent<IApplyAction>().UnSubscribeEvent();
            PopulateCurrentVideoSettings();
            SetApplyAction();
            await SettingsSaveManager.Instance.Save(_accessibilitySettingsSaver);
            GetComponent<IApplyAction>().SubscribeEvent();
        }


        private void SetApplyAction()
        {
            _controller.CameraSettings.ChangeCameraFov(_currentAccessibilitySettingsData.CameraFOV);
            _controller.CameraSettings.ToggleCameraShake(_currentAccessibilitySettingsData.ShowCameraShake);
            _controller.LanguageSettings.ToggleSubtitle(_currentAccessibilitySettingsData.ShowSubtitle);
            _controller.LanguageSettings.ChangeLanguage(_currentAccessibilitySettingsData.LanguageIndex);
        }

        private void PopulateCurrentVideoSettings()
        {
            _currentAccessibilitySettingsData = new AccessibilitySettingsData(_tempAccessibilitySettingsData);
        }


        public void Save()
        {
            ApplyAction();
        }

        public void Reset()
        {
            _accessibilitySettingsSaver.SetCurrentData(_accessibilitySettingsSaver.GetDefaultData());
            _tempAccessibilitySettingsData = _accessibilitySettingsSaver.GetCurrentData();
            ApplyAction();
            InitializeUi();
        }
    }
}
