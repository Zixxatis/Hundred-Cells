using UnityEngine;

namespace CGames
{
    public abstract class StorePanel : MonoBehaviour
    {
        public void DisplayPanel()
        {
            PrepareBeforeOpening();
            this.ActivateGameObject();
        }
        protected abstract void PrepareBeforeOpening();

        public void HidePanel()
        {
            PrepareBeforeClosing();
            this.DeactivateGameObject();;
        }

        protected abstract void PrepareBeforeClosing();
    }
}