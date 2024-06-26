using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class DraggingDistanceSS : SettingSlot
    {
        [Header("Settings - Hovering Mode")]
        [SerializeField] private Button leftDraggingDistanceButton;
        [SerializeField] private TextLocalizer draggingDistanceLTMP;
        [SerializeField] private Button rightDraggingDistanceButton;

        private PlayerPreferences playerPreferences;

        private int currentDraggingDistanceIndex;

        [Inject]
        private void Construct(PlayerPreferences playerPreferences)
        {
            this.playerPreferences = playerPreferences;
        }

        public override void PrepareSettingSlot()
        {
            leftDraggingDistanceButton.onClick.AddListener(DecreaseDraggingDistanceIndex);
            rightDraggingDistanceButton.onClick.AddListener(IncreaseDraggingDistanceIndex);
        }

        public override void MatchValuesToCurrent()
        {
            currentDraggingDistanceIndex = (int)playerPreferences.DraggingDistance;
            draggingDistanceLTMP.SetKeyAndUpdate(playerPreferences.CurrentDraggingDistanceLK);
        }

         private void DecreaseDraggingDistanceIndex()
        {
            currentDraggingDistanceIndex--;

            if(currentDraggingDistanceIndex == -1)
                currentDraggingDistanceIndex = Enum.GetValues(typeof(DraggingDistance)).Length - 1;

            ChangeDraggingDistance();
        }

        private void IncreaseDraggingDistanceIndex()
        {
            currentDraggingDistanceIndex++;
                
            if(currentDraggingDistanceIndex == Enum.GetValues(typeof(DraggingDistance)).Length)
                currentDraggingDistanceIndex = 0;

            ChangeDraggingDistance();
        }

        private void ChangeDraggingDistance()
        {
            playerPreferences.ChangeDraggingDistance(currentDraggingDistanceIndex);
            draggingDistanceLTMP.SetKeyAndUpdate(playerPreferences.CurrentDraggingDistanceLK);
        }

        private void OnDestroy()
        {
            leftDraggingDistanceButton.onClick.RemoveListener(DecreaseDraggingDistanceIndex);
            rightDraggingDistanceButton.onClick.RemoveListener(IncreaseDraggingDistanceIndex);
        }
    }
}