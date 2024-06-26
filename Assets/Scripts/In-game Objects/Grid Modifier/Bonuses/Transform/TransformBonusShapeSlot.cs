using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class TransformBonusShapeSlot : MonoBehaviour
    {
        [SerializeField] private ShapeHolder shapeHolder;
        [SerializeField] private Button button;
        private Func<ShapeType, Shape> getRandomShapeAction;

        private Shape shapeInSlot;

        [Inject]
        private void Construct(ShapesFactory shapesFactory)
        {
            this.getRandomShapeAction = shapesFactory.GetRandomShape;
        }

        public void Initialize(Action<ShapeData> selectShapeAction)
        {
            button.onClick.AddListener(() => selectShapeAction.Invoke(shapeInSlot.ShapeData));
        }

        public void ShowShape(ShapeType shapeType)
        {
            shapeInSlot = getRandomShapeAction(shapeType);
            shapeHolder.BindShapeToHolder(shapeInSlot);
        }

        public void ReleaseShape()
        {
            shapeHolder.UnbindShape();
            shapeInSlot = null;
        }

        private void OnDestroy() => button.onClick.RemoveAllListeners();
    }
}