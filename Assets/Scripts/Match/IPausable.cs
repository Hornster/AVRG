using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Match
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Defines entities that can be paused.
    /// </summary>
    public interface IPausable
    {
        /// <summary>
        /// Causes the object to halt executing its behavior.
        /// </summary>
        void Pause();
        /// <summary>
        /// Causes the object to resume executing its behavior.
        /// </summary>
        void Resume();
    }
}
