
namespace Studio23.SS2.Settings
{
    [System.Serializable]
    public class AudioSettingsData
    {
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;
        public float VoVolume;

        public AudioSettingsData() {}

        public AudioSettingsData(AudioSettingsData data)
        {
            MasterVolume = data.MasterVolume;
            MusicVolume = data.MusicVolume;
            SfxVolume = data.SfxVolume;
            VoVolume = data.VoVolume;
        }

        public AudioSettingsData(float masterVol, float musicVol, float sfxVol, float voVol)
        {
            MasterVolume = masterVol;
            MusicVolume = musicVol;
            SfxVolume = sfxVol;
            VoVolume = voVol;
        }
    }
}
