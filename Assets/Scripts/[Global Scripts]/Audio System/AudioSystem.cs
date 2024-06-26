using System;

namespace CGames
{
    public class AudioSystem : ISavable<ConfigData>
    {
        public event Action OnMusicVolumeChanged;
        public event Action OnEffectsVolumeChanged;
        
        public float MusicVolume { get; private set; }
        public float EffectsVolume { get; private set; }

        public void ReceiveData(ConfigData data)
        {
            MusicVolume = data.MusicVolume;
            EffectsVolume = data.EffectsVolume;
        }

        public void PassData(ConfigData data)
        {
            data.MusicVolume = MusicVolume;
            data.EffectsVolume = EffectsVolume;
        }

        public void ChangeMusicVolume(float newValue)
        {
            MusicVolume = newValue;
            OnMusicVolumeChanged?.Invoke();
        }

        public void ChangeEffectsVolume(float newValue)
        {
            EffectsVolume = newValue;
            OnEffectsVolumeChanged?.Invoke();
        }
    }
}