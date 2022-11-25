using System.Collections.Generic;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for the serialization and deserialization of pending notifications.
    /// </summary>
    public interface IPendingNotificationSerializer
    {
        /// <summary>
        /// Saves a list of pending notifications.
        /// </summary>
        /// <param name="pendingNotifications">The collection of notifications to be saved.</param>
        /// <param name="path">The path to save the notifications.</param>
        void Serialize(List<PendingNotification> pendingNotifications, string path);

        /// <summary>
        /// Retrieve a list of pending notifications.
        /// </summary>
        /// <param name="platform">The object that handles the creation of notifications for the current platform.</param>
        /// <param name="path">The path where the notifications were saved.</param>
        /// <returns></returns>
        List<PendingNotification> Deserialize(INotificationPlatform platform, string path);
    }
}
