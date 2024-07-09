using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.Settings.Video.Core;
using Studio23.SS2.Settings.Video.Data;
using UnityEngine;

namespace Studio23.SS2
{
    public class VideoSettingsSaver : MonoBehaviour, ISavableSettings
    {
        [SerializeField]private VideoSettingsData _defaultVideoSettingsData;
        private VideoSettingsData _currentVideoSettingsData;

        public bool IsDirty { get; set; }
        private void OnDisable()
        {
            SettingsSaveManager.Instance.OnLoadComplete.RemoveListener(Initialize);
        }

        private void Start()
        {
            VideoSettingsManager.Instance.Initialize();
            SettingsSaveManager.Instance.OnLoadComplete.AddListener(Initialize);
        }

        public void Initialize()
        {
            VideoSettingsManager.Instance.DisplayController.ApplySettings(_currentVideoSettingsData);
            VideoSettingsManager.Instance.GraphicsController.ApplySettings(_currentVideoSettingsData);
        }


        public string GetUniqueID()
        {
            return "VideoSettings";
        }

        public UniTask<string> GetSerializedData()
        {
            string data = JsonConvert.SerializeObject(_currentVideoSettingsData);
            return new UniTask<string>(data);
        }

        public UniTask AssignSerializedData(string data)
        {
            _currentVideoSettingsData = new VideoSettingsData(JsonConvert.DeserializeObject<VideoSettingsData>(data));
            return UniTask.CompletedTask;
        }

        public async UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            Debug.Log($"ManageSaveLoadException started");
            _currentVideoSettingsData = new VideoSettingsData(_defaultVideoSettingsData);
            Debug.Log($"ManageSaveLoadException ended");
            await SettingsSaveManager.Instance.Save(this);
            await UniTask.CompletedTask;
        }

        public VideoSettingsData GetCurrentData()
        {
        
            return _currentVideoSettingsData;
        }

        public VideoSettingsData GetDefaultData()
        {
            return _defaultVideoSettingsData;
        }

        public void SetCurrentData(VideoSettingsData data)
        {
            _currentVideoSettingsData = new VideoSettingsData(data);
        }
    }
}
