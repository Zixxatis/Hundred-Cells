using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class EffectsSS : SettingSlot
    {       
        [Header("Settings - Effects")]
        [SerializeField] private Slider effectsVolumeSlider;
        [SerializeField] private TextMeshProUGUI effectsVolumeTMP;

        private AudioSystem audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        public override void PrepareSettingSlot()
        {
            effectsVolumeSlider.minValue = 0;
            effectsVolumeSlider.maxValue = 1;
            effectsVolumeSlider.wholeNumbers = false;

            effectsVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }

        public override void MatchValuesToCurrent()
        {
            effectsVolumeSlider.value = audioSystem.EffectsVolume;
            effectsVolumeTMP.text = $"{audioSystem.EffectsVolume * 100}";
        }

        private void ChangeSFXVolume(float value)
        {   
            float volumeValue = (float)Math.Round(value, 2);

            effectsVolumeTMP.text = $"{volumeValue * 100}";
            audioSystem.ChangeEffectsVolume(volumeValue);
        }

        private void OnDestroy()
        {
            effectsVolumeSlider.onValueChanged.RemoveListener(ChangeSFXVolume);
        }
    }
}