
namespace Studio23.SS2.Settings
{
    [System.Serializable]
    public class AccessibilitySettingsData
    {
        public int ShowCameraShake;
        public float CameraFOV;
        public int ShowSubtitle;
        public int LanguageIndex;

        public AccessibilitySettingsData() { }
        public AccessibilitySettingsData(int cameraShake, float cameraFov, int subtitle, int language)
        {
            ShowCameraShake = cameraShake;
            CameraFOV = cameraFov;
            ShowSubtitle = subtitle;
            LanguageIndex = language;
        }

        public AccessibilitySettingsData(AccessibilitySettingsData data)
        {
            ShowCameraShake = data.ShowCameraShake;
            CameraFOV = data.CameraFOV;
            ShowSubtitle = data.ShowSubtitle;
            LanguageIndex = data.LanguageIndex;
        }


    }
}
