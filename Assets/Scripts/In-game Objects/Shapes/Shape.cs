using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    [SelectionBase, RequireComponent(typeof(ShapeDragHandler))]
    public class Shape : MonoBehaviour
    {
        [Header("Shape Data")]
        [SerializeField] private ShapeType shapeType;
        [field: SerializeField] public ShapeSize ShapeSize { get; private set; }
        [field: SerializeField] public bool IsRotatable { get; private set; } = true;
        
        [Header("Shape Elements")]
        [SerializeField] private GridLayoutGroup gridLayoutGroup;

        private IDimensionsNotifier dimensionsNotifier;
        private IGameModeEnteredNotifier gameModeEnteredNotifier;
        private Action unbindFromHolderAction;  
        private Action<Shape> removeShapeAction;

        public ShapeData ShapeData { get; private set; }
        public ShapeType ShapeType => shapeType;
        public CellColor CellColor => ShapeData.CellColor;
        public RotationDegrees RotationDegrees => ShapeData.RotationDegrees;

        public ShapeDragHandler ShapeDragHandler { get; private set; }
        private RectTransform rectTransform;
        private List<ShapeCell> shapeCells;
        private Dictionary<RotationDegrees, List<Vector2Int>> cellsCoordinationsDictionary;

        public List<Vector2Int> CellsCoordinates => cellsCoordinationsDictionary[RotationDegrees];
        
        [Inject]
        private void Construct(IGameModeEnteredNotifier gameModeEnteredNotifier, IDimensionsNotifier dimensionsNotifier)
        {
            this.dimensionsNotifier = dimensionsNotifier;
            this.gameModeEnteredNotifier = gameModeEnteredNotifier;

            gameModeEnteredNotifier.OnGameModeEntered += AdjustSizes;
        }

        private void Awake()
        {
            rectTransform = GetComponent<RectTransform>();
            ShapeDragHandler = GetComponent<ShapeDragHandler>();
            ShapeData = new(shapeType, CellColor.None, RotationDegrees.None);

            InitializeShapeCells();
        }

        private void OnEnable() => AdjustSizes();

        private void InitializeShapeCells()
        {
            shapeCells = new List<ShapeCell>();
            List<Vector2Int> defaultLocalCellsCoordinates = new();

            for (int i = 0; i < gridLayoutGroup.transform.childCount; i++)
            {
                Transform cellsTransform = gridLayoutGroup.transform.GetChild(i);

                if (cellsTransform.TryGetComponent(out ShapeCell shapeCell))
                {
                    shapeCells.Add(shapeCell);

                    int rowIndex = i / GameConstants.MaxShapeDimensionsInCells;
                    int columnIndex = i % GameConstants.MaxShapeDimensionsInCells;
                    
                    defaultLocalCellsCoordinates.Add(new(rowIndex, columnIndex));
                }
            }

            cellsCoordinationsDictionary = ShapeCoordinatesAssigner.CreateCoordinatesDictionary(defaultLocalCellsCoordinates);
        }   

        public void SetColor(CellColor cellColor)
        {
            ShapeData.ChangeColor(cellColor);
            shapeCells.ForEach(x => x.UpdateColor(cellColor));
        }

        public void ChangeRotationDegree(RotationDegrees rotationDegrees)
        {
            if(IsRotatable)
            {
                rectTransform.localEulerAngles = new(0, 0, (float)rotationDegrees);
                ShapeData.UpdateRotationValue(rotationDegrees);
            }
            else
                ShapeData.UpdateRotationValue(RotationDegrees.None);
        }

        public void AssignUnbindAction(Action unbindFromHolderAction) => this.unbindFromHolderAction ??= unbindFromHolderAction;
        public void AssignReleaseAction(Action<Shape> releaseAction) => this.removeShapeAction ??= releaseAction;
        
        public void ReleaseShape()
        {
            unbindFromHolderAction.Invoke();
            removeShapeAction.Invoke(this);

            ResetScale();         
        }

        public bool TryToPlaceCellsOnGrid(out List<Cell> cells)
        {
            cells = new();

            foreach (ShapeCell shapeCell in shapeCells)
            {
                Cell cell = shapeCell.FindCellBelow();

                if(cell == null || cell.IsFilled)
                {
                    cells = null;
                    return false;
                }
                else
                    cells.Add(cell);
            }

            return true;
        }

        public void SetScale(float newScale) => rectTransform.localScale = new(newScale, newScale, newScale);
        public void ResetScale() => rectTransform.localScale = Vector3.one;

        private void AdjustSizes()
        {
            gridLayoutGroup.cellSize = dimensionsNotifier.CellDimensions;
            gridLayoutGroup.spacing = dimensionsNotifier.GridSpacing;
        }

        private void OnDestroy()
        {
            gameModeEnteredNotifier.OnGameModeEntered -= AdjustSizes;
        }
    }
}