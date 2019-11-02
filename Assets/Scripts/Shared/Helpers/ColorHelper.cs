using Assets.Scripts.Shared.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Shared.Helpers
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Class storing colors found in the game, like selected objects highlights (dependent on selecting entity's team).
    /// </summary>
    public static class ColorHelper
    {
        private static Dictionary<TeamEnum, Color> _highlightColor = new Dictionary<TeamEnum, Color>();

        static ColorHelper()
        {
            _highlightColor.Add(TeamEnum.Enemy, Color.magenta);
            _highlightColor.Add(TeamEnum.PlayerGreen, Color.green);
            _highlightColor.Add(TeamEnum.PlayerRed, Color.red);
        }

        public static Color GetHighlightColor(TeamEnum team)
        {
            Color color;
            if (_highlightColor.TryGetValue(team, out color) == false)
            {
                throw new Exception($"Error: team {team} has no color assigned!");
            }

            return color;
        }
    }
}
