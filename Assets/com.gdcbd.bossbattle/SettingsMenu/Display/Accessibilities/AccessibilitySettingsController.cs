
using UnityEngine;

namespace Studio23.SS2.Settings
{
    public class AccessibilitySettingsController : MonoBehaviour
    {
        public static AccessibilitySettingsController Instance;

        public CameraSettings CameraSettings;
        public LanguageSettings LanguageSettings;
        private AccessibilitySettingsData _accessibilitySettingsData;

        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
                Destroy(this);
        }

        public async void Initialize()
        {
            _accessibilitySettingsData = GetComponent<AccessibilitySettingsSaver>().GetCurrentData();

            CameraSettings.Initialize(_accessibilitySettingsData.ShowCameraShake, _accessibilitySettingsData.CameraFOV);
            await LanguageSettings.Initialize(_accessibilitySettingsData.ShowSubtitle, _accessibilitySettingsData.LanguageIndex);
        }

        public AccessibilitySettingsData GenerateDefaultAccessibilitySettings()
        {
            return new AccessibilitySettingsData(CameraSettings.ReturnDefaultCameraShake,
                CameraSettings.ReturnStartingFov, LanguageSettings.ReturnDefaultSubtitleState(),
                LanguageSettings.ReturnDefaultLanguage());
        }
    }
}
