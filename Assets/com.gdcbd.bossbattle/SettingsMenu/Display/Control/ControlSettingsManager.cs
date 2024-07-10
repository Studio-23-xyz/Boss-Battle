using Studio23.SS2.Settings.Control.Data;
using Studio23.SS2.Settings.Core;
using UnityEngine;

namespace Studio23.SS2.Settings
{
    public class ControlSettingsManager : MonoBehaviour
    {
        public static ControlSettingsManager Instance;
        public ControlSettings ControlSettings;
        
        private ControlSettingsConfiguration _controlSettingsConfiguration;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(this);
        }

        public void Initialize()
        {
            _controlSettingsConfiguration = GetComponent<ControlSettingsSaver>().GetCurrentData();
            ControlSettings.Initialize(_controlSettingsConfiguration);
        }
    }
}
