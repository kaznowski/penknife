using System.Collections;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Represents the authorization to display notifications in a specific platform.
    /// </summary>
    public interface INotificationAuthorization
    {
        /// <summary>
        /// Requests authorization from the user to display notifications.
        /// </summary>
        /// <returns></returns>
        IEnumerator RequestAuthorization();
    }
}
