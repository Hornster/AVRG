using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Shared.Enums
{
    /// <summary>
    /// Author: Karol Kozuch
    ///
    /// Possible results of trying to hook any object with equipped by player glove.
    /// </summary>
    public enum HookingResultEnum
    {
        /// <summary>
        /// Indicates that the object has been successfully hooked to the glove.
        /// </summary>
        ObjectHooked,
        /// <summary>
        /// Indicates that the type of the object differs from type of used glove.
        /// </summary>
        WrongType,
        /// <summary>
        /// Indicates that there was no object to hook. Tell player to aim better.
        /// </summary>
        NoObjectFound
    }
}
