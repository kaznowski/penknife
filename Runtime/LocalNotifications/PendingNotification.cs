namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Data structure used by the PendingNotificationsManager to save scheduled notifications.
    /// </summary>
    public class PendingNotification
    {
        public INotification Notification;

        public string NotificationDefinitionName;
        public bool RescheduleOnDeviceRestart;
        public bool CancelOnDeviceRestart;
        public double TimeTrigger;
        public bool UseTimeBox;
        public int MinHour;
        public int MinMinute;
        public int MaxHour;
        public int MaxMinute;
        public TimeBoxFallbackStrategy FallbackStrategy;
        public int FallbackHour;
        public int FallbackMinute;
    }
}
