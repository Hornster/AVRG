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

        [SerializeField]
        private PauseMenuController _pauseMenuController;

        void Start()
        {
            _gameTimeController.RestartCounting();
        }
        /// <summary>
        /// Gets the time which the player was playing for.
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetPlayTime()
        {
            return _gameTimeController.CurrentPlayTime;
        }
        /// <summary>
        /// Turns off pause GUI, resumes time measurement.
        /// </summary>
        public void RoundResumed()
        {
            _gameTimeController.ResumeCounting();
            _pauseMenuController.HidePauseMenu();
        }
        /// <summary>
        /// Turns on pause GUI, stops time measurement.
        /// </summary>
        public void RoundPaused()
        {
            _gameTimeController.StopCounting();
            _pauseMenuController.ShowPauseMenu();
        }
        /// <summary>
        /// Performs tasks connected with ending of a round, for example stopping the timer.
        /// </summary>
        public void RoundEnded()
        {
            _healthbarController.HideHealthBar();
            _gameTimeController.StopCounting();
            _gameTimeController.HideTimer();

            TimeSpan playedTimeMs = _gameTimeController.CurrentPlayTime;
            _resultsMenuController.ShowResultsMenu(playedTimeMs);
            _pauseMenuController.HidePauseMenu();
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
            _pauseMenuController.HidePauseMenu();
        }
    }
}
