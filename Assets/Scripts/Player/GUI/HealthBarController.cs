using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Player.GUI
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Controller for the player's GUI, seen while playing.
    /// </summary>
    public class HealthBarController : MonoBehaviour
    {
        /// <summary>
        /// Transform of the healthbar. Used for scaling.
        /// </summary>
        [SerializeField]
        private Transform _healthBarFg;
        /// <summary>
        /// Image of the healthbar, used to change the color.
        /// </summary>
        [SerializeField]
        private Image _healthBarImage;
        /// <summary>
        /// Defines color which will the healthbar progressively turn to when the health is being depleted.
        /// </summary>
        [SerializeField]
        private Color _lowHPBarColor;
        /// <summary>
        /// Defines color of full healthbar. Read from the image upon initialization.
        /// </summary>
        private Color _fullHealthBarColor;
        /// <summary>
        /// Max size of the healthbar. Read from _healthBarFg at initialization of the bar.
        /// </summary>
        private float _maxSize;

        void Start()
        {
            _maxSize = _healthBarFg.localScale.x;
            _fullHealthBarColor = _healthBarImage.color;
        }
        /// <summary>
        /// Updates the width of the health bar.
        /// </summary>
        /// <param name="seenPart">What percent of the bar shall be filled. Shall be from range [0; 1]</param>
        private void UpdateHealthBarWidth(float seenPart)
        {
            Vector3 currentScale = _healthBarFg.localScale;
            currentScale.x = _maxSize * seenPart;
            _healthBarFg.localScale = currentScale;
        }
        /// <summary>
        /// Changes the healthbar color accordingly to hp that's still left.
        /// </summary>
        /// <param name="seenPart">What percent of the bar shall be filled. Shall be from range [0; 1]</param>
        private void UpdateHealthBarColor(float seenPart)
        {
            float depletedColorAmount = 1 - seenPart;
            Color barColor = _fullHealthBarColor * seenPart + _lowHPBarColor * depletedColorAmount;
            _healthBarImage.color = barColor;
        }
        /// <summary>
        /// Changes the width, and color, of the healthbar accordingly to the HP left.
        /// </summary>
        /// <param name="seenPart">Percentage of healthbar shown. Shall be a number from range [0; 1]</param>
        public void UpdateHealthBar(float seenPart)
        {
            UpdateHealthBarWidth(seenPart);
            UpdateHealthBarColor(seenPart);
        }

    }
}
