using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace CGames
{
    public static class ButtonExtensions
    {
        /// <summary> Sets Button "interactable" to TRUE and changes child's TextMeshProUGUI to full-alpha. </summary>
        public static void EnableInteractivityWithText(this Button button)
        {
            button.interactable = true;

            TextMeshProUGUI textMeshProUGUI = button.GetComponentInChildren<TextMeshProUGUI>();

            if(textMeshProUGUI != null)
                textMeshProUGUI.alpha = 1f;
            else
                Debug.LogWarning("You're trying to change text's alpha without TextMeshProUGUI component. Use 'interactable' parameter instead.");
        }

        /// <summary> Sets Button "interactable" to FALSE and changes child's TextMeshProUGUI to semi-transparent alpha. </summary>
        public static void DisableInteractivityWithText(this Button button)
        {
            button.interactable = false;

            TextMeshProUGUI textMeshProUGUI = button.GetComponentInChildren<TextMeshProUGUI>();

            if(textMeshProUGUI != null)
                textMeshProUGUI.alpha = 0.6f;
            else
                Debug.LogWarning("You're trying to change alpha on a button without TextMeshProUGUI component. Use 'interactable' parameter instead.");
        }

        /// <summary> Sets Button "interactable" to a given value and changes child's TextMeshProUGUI to semi-transparent alpha. </summary>
        public static void ChangeInteractivityWithText(this Button button, bool shouldEnable)
        {
            if(shouldEnable)
                button.EnableInteractivityWithText();
            else
                button.DisableInteractivityWithText();
        }
    }
}