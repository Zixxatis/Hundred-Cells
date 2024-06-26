using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class MusicSS : SettingSlot
    {
        [Header("Settings - Music")]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TextMeshProUGUI musicVolumeTMP;
        
        private AudioSystem audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        public override void PrepareSettingSlot()
        {
            musicVolumeSlider.minValue = 0;
            musicVolumeSlider.maxValue = 1;
            musicVolumeSlider.wholeNumbers = false;

            musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
        }

        public override void MatchValuesToCurrent()
        {
            musicVolumeSlider.value = audioSystem.MusicVolume;
            musicVolumeTMP.text = $"{audioSystem.MusicVolume * 100}";
        }

        private void ChangeMusicVolume(float value)
        {   
            float volumeValue = (float)Math.Round(value, 2);
            
            musicVolumeTMP.text = $"{volumeValue * 100}";
            audioSystem.ChangeMusicVolume(volumeValue);
        }

        private void OnDestroy()
        {
            musicVolumeSlider.onValueChanged.RemoveListener(ChangeMusicVolume);
        }
    }
}