using UnityEngine;
using TMPro;

namespace CGames
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class GameVersionUpdater : MonoBehaviour
    {	
        private void Awake()
        {
            GetComponent<TextMeshProUGUI>().text = $"v.{Application.version}";
        }
    }
}