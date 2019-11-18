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
        [SerializeField]
        private ResultsMenuController _resultsMenuController;
        public HealthBarController HealthBarController => _healthbarController;
        [SerializeField]
        private HealthBarController _healthbarController;
        
        [SerializeField]
        private GameTimeController _gameTimeController;

        void Start()
        {
            _gameTimeController.RestartCounting();
        }
        /// <summary>
        /// Gets the time which the player was playing for.
        /// </summary>
        /// <returns></returns>
        public int GetPlayTime()
        {
            return _gameTimeController.CurrentPlayTime;
        }
        /// <summary>
        /// Performs tasks connected with ending of a round, for example stopping the timer.
        /// </summary>
        public void RoundEnded()
        {
            _healthbarController.HideHealthBar();
            _gameTimeController.StopCounting();
            _gameTimeController.HideTimer();

            int playedTimeMs = _gameTimeController.CurrentPlayTime;
            _resultsMenuController.ShowResultsMenu(playedTimeMs);
        }
        /// <summary>
        /// Performs tasks connected with restarting the round.
        /// </summary>
        public void RestartRound()
        {
            _healthbarController.ShowHealthBar();
            _healthbarController.ResetHealthBar();
            _gameTimeController.ShowTimer();
            _gameTimeController.RestartCounting();
            _resultsMenuController.HideResultsMenu();
        }
    }
}
