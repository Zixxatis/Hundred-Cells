using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class InGameNavigationBlock : MonoBehaviour
    {
        [SerializeField] private Button swapSongButton;
        [SerializeField] private Button storeButton;
        [SerializeField] private Button pauseButton;

        private Action playNextSongAction;
        private Action openStorePanelAction;
        private Action openMenuAction;
        
        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            this.playNextSongAction = audioPlayer.PlayNextSong;
        }

        public void Initialize(Action openStorePanelAction, Action openMenuAction)
        {
            this.openStorePanelAction = openStorePanelAction;
            this.openMenuAction = openMenuAction;

            swapSongButton.onClick.AddListener(playNextSongAction.Invoke);
            storeButton.onClick.AddListener(this.openStorePanelAction.Invoke);
            pauseButton.onClick.AddListener(this.openMenuAction.Invoke);
        }

        private void OnDestroy()
        {
            swapSongButton.onClick.RemoveListener(playNextSongAction.Invoke);
            storeButton.onClick.RemoveListener(openStorePanelAction.Invoke);
            pauseButton.onClick.RemoveListener(openMenuAction.Invoke);
        }
    }
}