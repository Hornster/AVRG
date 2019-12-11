using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Player.GUI.VR
{
    public class MenuManager : MonoBehaviour
    {
        /// <summary>
        /// Currently shown panel. This panel is displayed as first during startup.
        /// </summary>
        [SerializeField]
        private MenuPanel _currentPanel = null;
        private List<MenuPanel> _panelHistory = new List<MenuPanel>();

        private void Start()
        {
            SetupPanels();
        }

        private void SetupPanels()
        {
            MenuPanel[] panels = GetComponentsInChildren<MenuPanel>();
            foreach (var panel in panels)
            {
                panel.Setup(this);
            }

            _currentPanel.Show();
        }
    }
}
