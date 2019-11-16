using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player.GUI
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Manages the GUI during the game.
    /// </summary>
    public class GuiManager : MonoBehaviour
    {
        public HealthBarController HealthBarController => _healthbarController;
        [SerializeField]
        private HealthBarController _healthbarController;

        public GameTimeController GameTimeController => _gameTimeController;
        [SerializeField]
        private GameTimeController _gameTimeController;
    }
}
