using UnityEngine;

namespace Assets.Scripts.Player.GUI.VR
{
    public class MenuPanel : MonoBehaviour
    {
        private Canvas _canvas = null;
        private MenuManager _menuManager = null;

        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
        }

        public void Setup(MenuManager menuManager)
        {
            this._menuManager = menuManager;
            Hide();
        }

        public void Show()
        {
            _canvas.enabled = true;
        }

        public void Hide()
        {
            _canvas.enabled = false;
        }
    }
}
