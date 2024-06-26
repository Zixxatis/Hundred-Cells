using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class AudioPlayer : MonoBehaviour
    {
        [Header("Audio Sources")]
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private AudioSource effectsSource;

        [Header("Files")]
        [SerializeField] private List<AudioClip> songsList;
        [SerializeField] private List<CoreSoundEffect> coreSoundEffectsList;

        private AudioSystem audioSystem;
        private int currentSongIndex;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        private void Awake()
        {
            audioSystem.OnMusicVolumeChanged += AdaptMusicVolume;
            audioSystem.OnEffectsVolumeChanged += AdaptEffectsVolume;

            AdaptMusicVolume();
            AdaptEffectsVolume();
        }

        private void Start()
        {
            musicSource.loop = true;
            PlayRandomSong();
        }

        public void AdaptMusicVolume() => musicSource.volume = audioSystem.MusicVolume;
        public void AdaptEffectsVolume() => effectsSource.volume = audioSystem.EffectsVolume;

        private void PlayRandomSong()
        {
            currentSongIndex = Random.Range(0, songsList.Count);

            musicSource.clip = songsList[currentSongIndex];
            musicSource.Play();
        }

        public void PlayNextSong()
        {
            currentSongIndex = (currentSongIndex + 1) % songsList.Count;

            musicSource.clip = songsList[currentSongIndex];
            musicSource.Play();
        }

        public void PlayCoreSFX(CoreSoundEffectType coreSoundEffectType)
        {
            PlaySFX(coreSoundEffectsList.First(x => x.CoreSoundEffectType == coreSoundEffectType).AudioClip);
        }

        public void PlaySFX(AudioClip audioClip)
        {
            effectsSource.PlayOneShot(audioClip, audioSystem.EffectsVolume);
        }

        private void OnDestroy()
        {
            audioSystem.OnMusicVolumeChanged -= AdaptMusicVolume;
            audioSystem.OnEffectsVolumeChanged -= AdaptEffectsVolume;
        }
    }
}