using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(GridLayoutGroup), typeof(AspectRatioFitter))]
    public class GridAspectRatioMaintainer : MonoBehaviour
    {
        [SerializeField, Min(1)] private float rowsAmount = 10f; 
        [SerializeField, Min(1)] private float columnsAmount = 10f; 

        private GridLayoutGroup gridLayoutGroup;
        private RectTransform parentRT;
        private IGameModeEnteredNotifier gameModeEnteredNotifier;

        [Inject]
        private void Construct(IGameModeEnteredNotifier gameModeEnteredNotifier)
        {
            this.gameModeEnteredNotifier = gameModeEnteredNotifier;
            gameModeEnteredNotifier.OnGameModeEntered += AdjustCellsSize;
        }      

    #if UNITY_EDITOR
        private void Update()
        {
            if(Application.isPlaying == false)
                AdjustCellsSize();
        }
    #endif

        public void AdjustCellsSize()
        {
            if(gridLayoutGroup == null)
                gridLayoutGroup = GetComponent<GridLayoutGroup>();

            if(parentRT == null)
                parentRT = GetComponent<RectTransform>();

            float width = parentRT.rect.width;
            float leftOffset = parentRT.offsetMin.x;
            float rightOffset = parentRT.offsetMax.x;

            float cellDimensionsSize = Mathf.Round((width - leftOffset - rightOffset - gridLayoutGroup.spacing.x * rowsAmount) / columnsAmount);
            gridLayoutGroup.cellSize = new(cellDimensionsSize, cellDimensionsSize);
        }

        private void OnDestroy()
        {
            if(Application.isPlaying)
                gameModeEnteredNotifier.OnGameModeEntered -= AdjustCellsSize;
        }
    }
}