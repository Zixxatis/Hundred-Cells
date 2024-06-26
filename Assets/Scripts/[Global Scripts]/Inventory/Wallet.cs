using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class Wallet : ISavable<PlayerData>
    {
        public event Action OnCoinsValueChanged;

        private Action<CoreSoundEffectType> playEffectAction;

        private ushort coins;
        public string CoinsAmountInString => coins.ToString();

        [Inject]
        private void Construct(AudioPlayer audioPlayer)
        {
            this.playEffectAction = (x) => audioPlayer.PlayCoreSFX(x);
        }

        public void ReceiveData(PlayerData data)
        {
            coins = data.Coins;
        }

        public void PassData(PlayerData data)
        {
            data.Coins = coins;
        }

        public void GainCoins(int coinsAmountToGain)
        {
            if(coins + coinsAmountToGain > ushort.MaxValue) 
                coins = ushort.MaxValue;
            else
                coins += (ushort)coinsAmountToGain;

            playEffectAction.Invoke(CoreSoundEffectType.GainedCoins);
            OnCoinsValueChanged?.Invoke();
        }

        public void SpendCoins(int coinsAmountToSpend)
        {
            coins = (ushort)Mathf.Max(coins - coinsAmountToSpend, 0);

            playEffectAction.Invoke(CoreSoundEffectType.SpentCoins);
            OnCoinsValueChanged?.Invoke();
        }

        public bool IsEnoughCoinsFor(int compareTarget) => coins >= compareTarget;
    }
}