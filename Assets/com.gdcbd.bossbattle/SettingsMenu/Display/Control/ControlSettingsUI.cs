using Studio23.SS2.Settings.Control.Data;
using Studio23.SS2.UI.Misc;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Studio23.SS2.Settings
{
    public class ControlSettingsUI : MonoBehaviour, IApplyAction
    {
        [SerializeField] private LocalizedRotatorData _inputMethod;
        [SerializeField] private LocalizedLabeledSliderData _mouseSensitivity;
        [SerializeField] private LocalizedLabeledSliderData _controllerSensitivity;
        [SerializeField] private LocalizedRotatorData _invertX;
        [SerializeField] private LocalizedRotatorData _invertY;

        private ControlSettingsManager _controlSettingsManager;
        private ControlSettingsConfiguration _tempControlSettingsConfiguration;
        private ControlSettingsConfiguration _currentControlSettingsConfiguration;
        private ControlSettingsSaver _controlSettingsSaver;

        private List<GameObject> _onlyKeyBoardButtons;
        private List<GameObject> _onlyControllerButtons;


        void OnEnable()
        {
            GetComponent<IApplyAction>().SubscribeEvent();
            PopulateButtonList();
        }

        void OnDisable()
        {
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.RemoveListener(Initialize);
            SettingsSaveManager.Instance.OnLoadComplete.RemoveListener(Initialize);
            GetComponent<IApplyAction>().UnSubscribeEvent();
            SetApplyAction();
        }


        private void Start()
        {
            Initialize();
            SaveSystem.Core.SaveSystem.Instance.OnLoadComplete.AddListener(Initialize);
            SettingsSaveManager.Instance.OnLoadComplete.AddListener(Initialize);

        }

        public void Initialize()
        {
            _controlSettingsManager = ControlSettingsManager.Instance;
            _controlSettingsSaver = _controlSettingsManager.GetComponent<ControlSettingsSaver>();
            _tempControlSettingsConfiguration = _controlSettingsSaver.GetCurrentData();
            _currentControlSettingsConfiguration = new ControlSettingsConfiguration(_tempControlSettingsConfiguration);
            InitializeUi();
            InitializeUiEvent();
        }


        private void PopulateButtonList()
        {
            _onlyKeyBoardButtons = new List<GameObject>();
            _onlyControllerButtons = new List<GameObject>();
            var allButtons = GetComponentsInChildren<ButtonAvailabilityController>(true).ToList();

            List<Button> prunedButtons = new List<Button>();
            foreach (var button in allButtons)
            {
                if(button.gameObject.CompareTag("UI_KEYBOARD"))
                {
                    _onlyKeyBoardButtons.Add(button.gameObject);
                }
                else if(button.gameObject.CompareTag("UI_CONTROLLER"))
                {
                    _onlyControllerButtons.Add(button.gameObject);
                }
            }
        }

        private void ToggleButtons(int state)
        {
            foreach(var button in _onlyControllerButtons)
            {
                button.SetActive(state > 0);
            }

            foreach(var button in _onlyKeyBoardButtons)
            {
                button.SetActive(state <= 0);
            }
        }

        private void InitializeUi()
        {
            _inputMethod.RotatorButton.InitializeData(_inputMethod.LabelLocalizedString, _inputMethod.DataLocalizedTable, 0);
            _mouseSensitivity.LabeledSlider.InitializeData(_mouseSensitivity.LabeledLocalizedString, .1f, 2f, _tempControlSettingsConfiguration.MouseSensitivity);
            _controllerSensitivity.LabeledSlider.InitializeData(_controllerSensitivity.LabeledLocalizedString, .1f, 2f, _tempControlSettingsConfiguration.ControllerSensitivity);
            _invertX.RotatorButton.InitializeData(_invertX.LabelLocalizedString, _invertX.DataLocalizedTable, _tempControlSettingsConfiguration.IsInvertX);
            _invertY.RotatorButton.InitializeData(_invertY.LabelLocalizedString, _invertY.DataLocalizedTable, _tempControlSettingsConfiguration.IsInvertY);
        }

        private void InitializeUiEvent()
        {
            _inputMethod.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                ToggleButtons(index);
            }));

            _mouseSensitivity.LabeledSlider.SliderValueUpdated.AddListener((value =>
            {
                _tempControlSettingsConfiguration.MouseSensitivity = value;
            }));

            _controllerSensitivity.LabeledSlider.SliderValueUpdated.AddListener((value =>
            { 
                _tempControlSettingsConfiguration.ControllerSensitivity = value;
            }));

            _invertX.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempControlSettingsConfiguration.IsInvertX = index;
            }));

            _invertY.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempControlSettingsConfiguration.IsInvertY = index;
            }));
        }


        private async void ApplyAction()
        {
            GetComponent<IApplyAction>().UnSubscribeEvent();
            PopulateCurrentVideoSettings();
            SetApplyAction();
            await SettingsSaveManager.Instance.Save(_controlSettingsSaver);
            GetComponent<IApplyAction>().SubscribeEvent();
        }


        private void SetApplyAction()
        {
            _controlSettingsManager.ControlSettings.UpdateMouseSensitivity(_currentControlSettingsConfiguration.MouseSensitivity);
            _controlSettingsManager.ControlSettings.UpdateControllerSensitivity(_currentControlSettingsConfiguration.ControllerSensitivity);
            _controlSettingsManager.ControlSettings.InvertXAxis(_currentControlSettingsConfiguration.IsInvertX);
            _controlSettingsManager.ControlSettings.InvertYAxis(_currentControlSettingsConfiguration.IsInvertY);
        }

        private void PopulateCurrentVideoSettings()
        {
            _currentControlSettingsConfiguration = new ControlSettingsConfiguration(_tempControlSettingsConfiguration);
        }


        public void Save()
        {
            ApplyAction();
        }

        public void Reset()
        {
            _controlSettingsSaver.SetCurrentData(_controlSettingsSaver.GetDefaultData());
            _tempControlSettingsConfiguration = _controlSettingsSaver.GetCurrentData();
            ApplyAction();
            InitializeUi();
        }


    }

}
