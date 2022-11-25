using System.Collections.Generic;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for saving and retrieving notifications scheduled on trigger only once mode.
    /// </summary>
    public class NotificationDefinitionHistoryManager
    {
        /// <summary>
        /// Retrieves whether a notification has already been scheduled.
        /// </summary>
        /// <param name="notificationDefinitionName">The name of the notification's definition.</param>
        /// <returns>Whether the notification has been scheduled.</returns>
        public static bool HasNotificationDefinitionBeenTriggered(string notificationDefinitionName)
        {
            var notificationDefinitionHistory = Deserialize();

            if (notificationDefinitionHistory == null)
                return false;

            return notificationDefinitionHistory.Exists(entry =>
                entry.NotificationDefinitionName.Equals(notificationDefinitionName));
        }

        /// <summary>
        /// Saves the info that a notification has been scheduled.
        /// </summary>
        /// <param name="notificationDefinition">The definition of the notification.</param>
        public static void RegisterTriggeredNotificationDefinition(NotificationDefinition notificationDefinition)
        {
            var notificationDefinitionHistory = Deserialize();

            if (notificationDefinitionHistory == null)
            {
                notificationDefinitionHistory = new List<NotificationDefinitionHistory>();
            }

            notificationDefinitionHistory.Add(new NotificationDefinitionHistory
            {
                NotificationDefinitionName = notificationDefinition.NotificationDefinitionName
            });

            Serialize(notificationDefinitionHistory);
        }

        private static List<NotificationDefinitionHistory> Deserialize()
        {
            var serializer = new NotificationDefinitionHistoryPlayerPrefsSerializer();
            return serializer.Deserialize(NotificationDefinitionHistoryPlayerPrefsSerializer.DefaultPlayerPrefsKey);
        }

        private static void Serialize(List<NotificationDefinitionHistory> notificationDefinitionHistories)
        {
            var serializer = new NotificationDefinitionHistoryPlayerPrefsSerializer();
            serializer.Serialize(notificationDefinitionHistories,
                NotificationDefinitionHistoryPlayerPrefsSerializer.DefaultPlayerPrefsKey);
        }
    }
}
