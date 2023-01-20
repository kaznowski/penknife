namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Any type that handles notifications for a specific game platform.
    /// </summary>
    public interface INotificationPlatform
    {
        /// <summary>
        /// Creates new instance of a notification for this platform.
        /// </summary>
        /// <returns>A new platform-appropriate notification object.</returns>
        INotification CreateNotification();

        /// <summary>
        /// Schedules a notification to be delivered.
        /// </summary>
        /// <param name="notification">The notification to deliver.</param>
        void ScheduleNotification(INotification notification);

        /// <summary>
        /// Reschedules a pending notification.
        /// </summary>
        /// <param name="notification">A previously scheduled notification.</param>
        void RescheduleNotification(INotification notification);

        /// <summary>
        /// Cancels a scheduled notification.
        /// </summary>
        /// <param name="notification">The notification to cancel.</param>
        void CancelScheduledNotification(INotification notification);

        /// <summary>
        /// Cancels a scheduled notification.
        /// </summary>
        /// <param name="notificationId">The Id of the notification to cancel.</param>
        void CancelScheduledNotification(int notificationId);

        /// <summary>
        /// Cancels all scheduled notifications.
        /// </summary>
        void CancelAllScheduledNotifications();

        /// <summary>
        /// Clears a displayed notification.
        /// </summary>
        /// <param name="notification">The notification to be cleared.</param>
        void ClearDisplayedNotification(INotification notification);

        /// <summary>
        /// Clears a displayed notification.
        /// </summary>
        /// <param name="notificationId">The ID of the notification to be cleared.</param>
        void ClearDisplayedNotification(int notificationId);

        /// <summary>
        /// Clears all displayed notifications.
        /// </summary>
        void ClearAllDisplayedNotifications();

        /// <summary>
        /// If the user taps a notification to open the app, this retrieves the data assigned to it.
        /// </summary>
        /// <param name="data">The data assigned to the notification used by the player to open the app.</param>
        /// <returns>True if the data was retrieved, false if it was not retrieved, probably because the app was not openend by tapping a notification.</returns>
        bool TryGetLastRespondedNotificationData(out INotification notification);
    }

    public interface INotificationPlatform<TNotificationType> : INotificationPlatform
        where TNotificationType : INotification
    {
        new TNotificationType CreateNotification();
        void ScheduleNotification(TNotificationType notification);
        void RescheduleNotification(TNotificationType notification);
        void CancelScheduledNotification(TNotificationType notification);
        void ClearDisplayedNotification(TNotificationType notification);
    }
}
