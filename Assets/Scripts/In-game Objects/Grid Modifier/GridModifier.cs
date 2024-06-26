using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public class GridModifier : MonoBehaviour
    {   
        [Header("General Elements")]
        [SerializeField] private Image gridOverlayImage;
        [SerializeField] private Image handOverlayImage;
        [Space]
        [SerializeField] private Color blockerColor = new(0, 0, 0, 0.9f);
        [Space]
        [SerializeField] private BonusInformer bonusInformer;

        [Header("Activators")]
        [SerializeField] private DestroyBonusActivator destroyBonusActivator;
        [SerializeField] private MoveBonusActivator moveBonusActivator;
        [SerializeField] private RotateBonusActivator rotateBonusActivator;
        [SerializeField] private RevertBonusActivator revertBonusActivator;
        [SerializeField] private RefreshBonusActivator refreshBonusActivator;
        [SerializeField] private TransformBonusActivator transformBonusActivator;

        private readonly List<BonusActivator> bonusActivatorsList = new();

        private void Awake()
        {
            gridOverlayImage.color = blockerColor;
            handOverlayImage.color = blockerColor;

            gridOverlayImage.DeactivateGameObject();
            handOverlayImage.DeactivateGameObject();
        }

        public void Initialize(Cell[,] cellsMatrix)
        {
            CreateActivatorsList();
            bonusActivatorsList.ForEach
            (
                x => x.Initialize
                (
                    cellsMatrix, 
                    delegate 
                    { 
                        HideOverlays(); 
                        bonusInformer.HideHelperInfo(); 
                    } 
                )
            );
        }

        private void CreateActivatorsList()
        {
            bonusActivatorsList.Add(destroyBonusActivator);
            bonusActivatorsList.Add(moveBonusActivator);
            bonusActivatorsList.Add(rotateBonusActivator);
            bonusActivatorsList.Add(revertBonusActivator);
            bonusActivatorsList.Add(refreshBonusActivator);
            bonusActivatorsList.Add(transformBonusActivator);
        }

        public void ApplyBonus(BonusType bonusType)
        {
            BonusActivator bonusActivator = bonusActivatorsList.Find(x => x.BonusType == bonusType);

            if(bonusActivator != null)
            {
                ShowOverlays(bonusActivator);
                bonusActivator.Activate();

                bonusInformer.ShowHelperInfo(bonusType);
            }
            else
                throw new ArgumentOutOfRangeException($"{bonusType} is not a supported bonus type. Add it to the \"Grid Modifier\".");
        }

        private void ShowOverlays(BonusActivator bonusActivator)
        {
            gridOverlayImage.ChangeGameObjectActivation(bonusActivator.ShouldBlockGrid);
            handOverlayImage.ChangeGameObjectActivation(bonusActivator.ShouldBlockHand);
        }

        private void HideOverlays()
        {
            gridOverlayImage.DeactivateGameObject();
            handOverlayImage.DeactivateGameObject();
        }
    }
}