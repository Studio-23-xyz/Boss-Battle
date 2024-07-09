using Studio23.SS2.Settings.Video.Core;
using Studio23.SS2.Settings.Video.Data;
using UnityEngine;

namespace Studio23.SS2
{
    public class GraphicsSettingsUI : MonoBehaviour, IApplyAction
    {
        [SerializeField] private LocalizedRotatorData _ambientOcculsion;
        [SerializeField] private LocalizedRotatorData _bloom;

        private VideoSettingsData _tempGraphicsSettingsData;
        private VideoSettingsData _currentGraphicsData;
        private GraphicsController _graphicsController;
        private VideoSettingsSaver _videoSettingsSaver;
        
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

        public void Initialize()
        {
            _graphicsController = VideoSettingsManager.Instance.GraphicsController;
            _videoSettingsSaver = VideoSettingsManager.Instance.GetComponent<VideoSettingsSaver>();
            _tempGraphicsSettingsData = _videoSettingsSaver.GetCurrentData();
            _currentGraphicsData = new VideoSettingsData(_tempGraphicsSettingsData);

            InitializeUi();
            InitializeUiEvent();
        }

        private void InitializeUi()
        {
            _ambientOcculsion.RotatorButton.InitializeData(_ambientOcculsion.LabelLocalizedString, _ambientOcculsion.DataLocalizedTable, _tempGraphicsSettingsData.AmbientOcclusionState);
            _bloom.RotatorButton.InitializeData(_bloom.LabelLocalizedString, _bloom.DataLocalizedTable,  _tempGraphicsSettingsData.BloomState);
        }

        private void InitializeUiEvent()
        {
            _ambientOcculsion.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempGraphicsSettingsData.AmbientOcclusionState = index;
            }));

            _bloom.RotatorButton.SelectedIndexUpdated.AddListener((index =>
            {
                _tempGraphicsSettingsData.BloomState = index;
            }));
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
            _graphicsController.SetAmbientOcculsionState(_currentGraphicsData.AmbientOcclusionState);
            _graphicsController.SetBloomState(_currentGraphicsData.BloomState);

        }

        private void PopulateCurrentVideoSettings()
        {
            _currentGraphicsData = new VideoSettingsData(_tempGraphicsSettingsData);
        }


        public void Save()
        {
            ApplyAction();
        }

        public void Reset()
        {
            _videoSettingsSaver.SetCurrentData(_videoSettingsSaver.GetDefaultData());
            _tempGraphicsSettingsData = _videoSettingsSaver.GetCurrentData();
            ApplyAction();
            InitializeUi();
        }


    }
}
