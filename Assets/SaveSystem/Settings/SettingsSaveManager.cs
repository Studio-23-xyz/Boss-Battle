using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cysharp.Threading.Tasks;
using Studio23.SS2.SaveSystem.Core;
using Studio23.SS2.SaveSystem.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace Studio23.SS2
{
    public class SettingsSaveManager : MonoBehaviour
    {
        public static SettingsSaveManager Instance;
        [SerializeField]private FileProcessor _fileProcessor;
        public string SaveFilePath;
        public UnityEvent OnLoadComplete;

        void Awake()
        {
            if(Instance == null)
                Instance = this;
            else 
                Destroy(this);
            
        }

        void Start()
        {
            SaveFilePath = Path.Combine(Application.persistentDataPath, "SaveData", "SettingsConfig");
            Debug.Log($"save file path {SaveFilePath}");
            if (!Directory.Exists(SaveFilePath))
                Directory.CreateDirectory(SaveFilePath);
            LoadAll();
        }

        public async void LoadAll()
        {
            Debug.Log($"OnLoadComplete Started");
            var savableComponents = FindObjectsOfType<MonoBehaviour>(true).OfType<ISavableSettings>();
            foreach (var component in savableComponents)
            {
                await Load(component);
            }

            OnLoadComplete?.Invoke();
            Debug.Log($"OnLoadComplete Ended");
        }

        public async UniTask Save(ISavableSettings savable)
        {
            var key = savable.GetUniqueID();
            string data = await savable.GetSerializedData();

            var filepath = Path.Combine(SaveFilePath, key);

            await _fileProcessor.Save(data, filepath);
            Debug.Log($"<color=green>Saved</color> With Key: {key} ");
        }


        public async UniTask Load(ISavableSettings savable)
        {
            var key = savable.GetUniqueID();
            var filepath = Path.Combine(SaveFilePath, key);
            if (!File.Exists(filepath))
            {
                await savable.ManageSaveLoadException(new FileNotFoundException(
                    $"ISavable <color=white>Key: {key}</color> Method: <color=yellow>LoadAllSavable()</color>"));
            }

            var data = await _fileProcessor.Load<string>(filepath);
            await savable.AssignSerializedData(data);
        }
    }
}
