using System.Collections.Generic;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for the serialization and deserialization of notifications that are on trigger only once mode.
    /// </summary>
    public interface INotificationDefinitionHistorySerializer
    {
        /// <summary>
        /// Retrieve a list of triggered notifications.
        /// </summary>
        /// <param name="path">The path where the notifications were saved.</param>
        /// <returns>The deserialized collection of notifications, or null if the collection didn't exist.</returns>
        List<NotificationDefinitionHistory> Deserialize(string path);

        /// <summary>
        /// Save a list of triggered notifications.
        /// </summary>
        /// <param name="notificationDefinitionHistories">A collection of notifications to be saved.</param>
        /// <param name="path">The path to save the notifications.</param>
        void Serialize(List<NotificationDefinitionHistory> notificationDefinitionHistories, string path);
    }
}
