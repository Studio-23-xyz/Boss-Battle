
using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Localization.Settings;

namespace Studio23.SS2.Settings
{
    public class LanguageSettings : MonoBehaviour
    {
        [SerializeField] private bool _showSubtitleAtStart;
        [SerializeField] private GameObject _subtitleObject;
        [SerializeField] public AccessibilityLanguage _defaultLanguage;
        public UnityEvent<int> LanguageChanged;


        public async UniTask Initialize(int showSubtitle, int languageId)
        {
            await UpdateLanguage(languageId);
            ToggleSubtitle(showSubtitle);
        }

        public void ToggleSubtitle(int showSubtitle)
        {
            if (_subtitleObject == null) return;
            _subtitleObject.SetActive(showSubtitle > 0);
        }

        public async void ChangeLanguage(int localId)
        {
            await UpdateLanguage(localId);
        }

        private async UniTask UpdateLanguage(int localId)
        {
            await LocalizationSettings.InitializationOperation;
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[localId];
            await UniTask.CompletedTask;
            LanguageChanged?.Invoke(localId);
        }

        public int ReturnDefaultSubtitleState() => Convert.ToInt32(_showSubtitleAtStart);
        public int ReturnDefaultLanguage() => (int)_defaultLanguage;
    }

    public enum AccessibilityLanguage
    {
        EN = 0,
        BN = 1,
        JP = 2,
        RS = 3
    }
}
