using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Represents a string which may assume different values during runtime depending on circumstances, e. g. what language the game is set to.
    /// Made to be extended and used in conjunction with packages like I2Localization.
    /// </summary>
    public abstract class NotificationLocalizedString : ScriptableObject
    {
        /// <summary>
        /// Retrieves a string to be used in a notification.
        /// </summary>
        /// <returns>The string.</returns>
        public abstract string GetString();
    }
}
