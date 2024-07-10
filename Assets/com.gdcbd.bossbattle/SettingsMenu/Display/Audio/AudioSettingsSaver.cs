using System;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Studio23.SS2.AudioSystem.fmod.Core;
using Studio23.SS2.SaveSystem.Interfaces;
using Studio23.SS2.Settings;
using UnityEngine;

namespace Studio23.SS2
{
    public class AudioSettingsSaver : MonoBehaviour, ISavableSettings
    {

        private AudioSettingsData _currentAudioSettingsData;

        private void OnDisable()
        {
            SettingsSaveManager.Instance.OnLoadComplete.RemoveListener(Initialize);
        }

        private void Start()
        {
            FMODManager.Instance.Initialize();
            SettingsSaveManager.Instance.OnLoadComplete.AddListener(Initialize);
        }

        public void Initialize()
        {
            AudioSettingsManager.Instance.Initialize();
        }

        public bool IsDirty { get; set; }

        public string GetUniqueID()
        {
            return "AudioSettings";
        }

        public UniTask<string> GetSerializedData()
        {
            string data = JsonConvert.SerializeObject(_currentAudioSettingsData);
            return new UniTask<string>(data);
        }

        public async UniTask AssignSerializedData(string data)
        {
            _currentAudioSettingsData = new AudioSettingsData(JsonConvert.DeserializeObject<AudioSettingsData>(data));
            await UniTask.CompletedTask;
        }

        public async UniTask ManageSaveLoadException(Exception exception)
        {
            Debug.Log($"{exception.Message}");
            _currentAudioSettingsData = new AudioSettingsData(AudioSettingsManager.Instance.GenerateDefaultAudioSettingsData());
            await SettingsSaveManager.Instance.Save(this);
            await UniTask.CompletedTask;
        }

        public AudioSettingsData GetCurrentData()
        {
            return _currentAudioSettingsData;
        }

        public AudioSettingsData GetDefaultData()
        {
            return new AudioSettingsData(AudioSettingsManager.Instance.GenerateDefaultAudioSettingsData());
        }

        public void SetCurrentData(AudioSettingsData data)
        {
            _currentAudioSettingsData = new AudioSettingsData(data);
        }
    }
}
