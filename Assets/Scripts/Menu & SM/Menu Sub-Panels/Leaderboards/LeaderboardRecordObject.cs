using UnityEngine;
using TMPro;

namespace CGames
{
    public class LeaderboardRecordObject : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI positionTMP;
        [SerializeField] private TextMeshProUGUI playerNameTMP;
        [SerializeField] private TextMeshProUGUI scoreTMP;

        public void ChangeRecordText(int position, string playerName, uint score)
        {
            positionTMP.text = $"{position}.";
            playerNameTMP.text = playerName;
            scoreTMP.text = score.ToString();
        }
    }
}