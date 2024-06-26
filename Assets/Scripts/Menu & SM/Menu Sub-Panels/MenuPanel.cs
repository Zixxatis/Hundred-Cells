using System;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public abstract class MenuPanel : MonoBehaviour
    {
        [Header("Sub Menu - Elements")]
        [SerializeField] protected Button returnButton;

        public event Action OnSubMenuOpened;
        public event Action OnSubMenuClosed;

        protected virtual void Awake()
        {
            returnButton.onClick.AddListener(ClosePanel);
            this.DeactivateGameObject();
        }

        public void OpenPanel()
        {
            PrepareBeforeOpening();
            this.ActivateGameObject();
            OnSubMenuOpened?.Invoke();
        }
        public abstract void PrepareBeforeOpening();

        public void ClosePanel()
        {
            PrepareBeforeClosing();
            this.DeactivateGameObject();
            OnSubMenuClosed?.Invoke();
        }
        public abstract void PrepareBeforeClosing();

        protected virtual void OnDestroy() => returnButton.onClick.RemoveListener(ClosePanel);
    }
}