using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;

namespace Studio23.SS2.Settings
{
    public class AccessibilitySettingsSaver : MonoBehaviour, ISavableSettings
    {
        private AccessibilitySettingsData _currentAccessibilitySettingsData;

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
            AccessibilitySettingsController.Instance.Initialize();
        }

        public bool IsDirty { get; set; }

        public string GetUniqueID()
        {
            return "AccessibilitySettings";
        }

        public UniTask<string> GetSerializedData()
        {
            string data = JsonConvert.SerializeObject(_currentAccessibilitySettingsData);
            return new UniTask<string>(data);
        }

        public UniTask AssignSerializedData(string data)
        {
            _currentAccessibilitySettingsData = new AccessibilitySettingsData(JsonConvert.DeserializeObject<AccessibilitySettingsData>(data));
            return UniTask.CompletedTask;
        }

        public async UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            _currentAccessibilitySettingsData = new AccessibilitySettingsData(AccessibilitySettingsController.Instance.GenerateDefaultAccessibilitySettings());
            await SettingsSaveManager.Instance.Save(this);
            await UniTask.CompletedTask;
        }

        public AccessibilitySettingsData GetCurrentData()
        {
            return _currentAccessibilitySettingsData;
        }

        public AccessibilitySettingsData GetDefaultData()
        {
            return new AccessibilitySettingsData(AccessibilitySettingsController.Instance.GenerateDefaultAccessibilitySettings());
        }

        public void SetCurrentData(AccessibilitySettingsData data)
        {
            _currentAccessibilitySettingsData = new AccessibilitySettingsData(data);
        }
    }
}
