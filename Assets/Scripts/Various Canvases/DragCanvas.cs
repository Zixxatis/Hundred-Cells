using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Canvas))]
    public class DragCanvas : MonoBehaviour
    {
        public Transform DraggingZoneTransform => this.transform;

        public Canvas Canvas
        { 
            get
            {
                if(canvas == null)
                    canvas = GetComponent<Canvas>();

                return canvas;
            } 
        }
        
        private Canvas canvas;

        public void MoveToDraggingZone(Transform child)
        {
            child.SetParent(DraggingZoneTransform);
            child.localScale = Vector3.one;
        }
    }
}