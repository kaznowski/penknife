#if UNITY_IOS
using System;
using Unity.Notifications.iOS;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    public class iOSNotificationPlatform : INotificationPlatform<iOSNotification>
    {
        /// <inheritdoc />
        INotification INotificationPlatform.CreateNotification()
        {
            return CreateNotification();
        }

        /// <inheritdoc />
        public iOSNotification CreateNotification()
        {
            return new iOSNotification();
        }

        /// <inheritdoc />
        public void ScheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            
            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            ScheduleNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void ScheduleNotification(iOSNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            iOSNotificationCenter.ScheduleNotification(notification.InternalNotification);
            notification.OnScheduled();
        }

        /// <inheritdoc />
        public void RescheduleNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            
            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            RescheduleNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void RescheduleNotification(iOSNotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            
            if (!notification.Id.HasValue)
            {
                throw new InvalidOperationException("Unable to reschedule a notification without an Id.");
            }

            var scheduledNotifications = iOSNotificationCenter.GetScheduledNotifications();
            iOSNotificationCenter.RemoveAllScheduledNotifications();

            foreach (var scheduledNotification in scheduledNotifications)
            {
                iOSNotificationCenter.ScheduleNotification(scheduledNotification.Identifier != notification.Id.ToString()
                    ? scheduledNotification
                    : notification.InternalNotification);
            }
        }

        /// <inheritdoc />
        public void CancelNotification(INotification notification)
        {
            if (notification == null)
            {
                throw new ArgumentNullException(nameof(notification));
            }
            
            if (notification is not iOSNotification iOSNotification)
            {
                throw new InvalidOperationException(
                    "Notification provided to iOSNotificationPlatform isn't an iOSNotification.");
            }

            CancelNotification(iOSNotification);
        }

        /// <inheritdoc />
        public void CancelNotification(iOSNotification notification)
        {
            if (notification is not {Scheduled: true} || notification.Id == null)
                return;

            iOSNotificationCenter.RemoveScheduledNotification(notification.Id.Value.ToString());

            if (notification.DeliveryTime < DateTime.Now)
                iOSNotificationCenter.RemoveDeliveredNotification(notification.Id.Value.ToString());
        }

        /// <inheritdoc />
        public void CancelNotification(int notificationId)
        {
            iOSNotificationCenter.RemoveScheduledNotification(notificationId.ToString());
            iOSNotificationCenter.RemoveDeliveredNotification(notificationId.ToString());
        }

        /// <inheritdoc />
        public void CancelAllNotifications()
        {
            iOSNotificationCenter.RemoveAllScheduledNotifications();
            iOSNotificationCenter.RemoveAllDeliveredNotifications();
        }

        /// <inheritdoc />
        public bool TryGetLastRespondedNotificationData(out string data)
        {
            var notification = iOSNotificationCenter.GetLastRespondedNotification();

            if (notification != null)
            {
                data = notification.Data;
                return true;
            }

            data = null;
            return false;
        }
    }
}
#endif
