using UnityEngine;
using TMPro;
using Zenject;

namespace CGames
{
    public class WalletDisplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI coinsAmountTMP;

        private Wallet wallet;

        [Inject]
        private void Construct(Wallet wallet)
        {
            this.wallet = wallet;
        }

        private void Awake()
        {
            wallet.OnCoinsValueChanged += UpdateCoinsAmountText;
        }

        private void Start()
        {
            UpdateCoinsAmountText();
        }

        private void UpdateCoinsAmountText() => coinsAmountTMP.text = wallet.CoinsAmountInString;

        private void OnDestroy()
        {
            wallet.OnCoinsValueChanged -= UpdateCoinsAmountText;
        }
    }
}