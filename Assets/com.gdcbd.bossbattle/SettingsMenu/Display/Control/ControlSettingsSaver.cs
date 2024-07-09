using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.SaveSystem.Core;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.Settings;
using Studio23.SS2.Settings.Control.Data;
using UnityEngine;

namespace Studio23.SS2
{
    public class ControlSettingsSaver : MonoBehaviour,ISavableSettings
    {
        [SerializeField] private ControlSettingsConfiguration _defaultControlSettingsData;
        private ControlSettingsConfiguration _currentControlSettingsData;

        private void OnDisable()
        {
            SettingsSaveManager.Instance.OnLoadComplete.RemoveListener(Initialize);
        }

        private void Start()
        {
            SettingsSaveManager.Instance.OnLoadComplete.AddListener(Initialize);
        }

        public void Initialize()
        {
            ControlSettingsManager.Instance.Initialize();
        }

        public bool IsDirty { get; set; }

        public string GetUniqueID()
        {
            return "ControlSettings";
        }

        public UniTask<string> GetSerializedData()
        {
            string data = JsonConvert.SerializeObject(_currentControlSettingsData);
            return new UniTask<string>(data);
        }

        public UniTask AssignSerializedData(string data)
        {
            _currentControlSettingsData = new ControlSettingsConfiguration(JsonConvert.DeserializeObject<ControlSettingsConfiguration>(data));
            return UniTask.CompletedTask;
        }

        public async UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            _currentControlSettingsData = new ControlSettingsConfiguration(_defaultControlSettingsData);
            await SettingsSaveManager.Instance.Save(this);
            await UniTask.CompletedTask;
        }

        public ControlSettingsConfiguration GetCurrentData()
        {
            return _currentControlSettingsData;
        }

        public ControlSettingsConfiguration GetDefaultData()
        {
            return _defaultControlSettingsData;
        }


        public void SetCurrentData(ControlSettingsConfiguration data)
        {
            _currentControlSettingsData = new ControlSettingsConfiguration(data);
        }
    }
}
