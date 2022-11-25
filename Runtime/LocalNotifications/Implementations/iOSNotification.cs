#if UNITY_IOS
using System;
using UnityEngine;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    [Serializable]
    public class iOSNotification : INotification
    {
        private Unity.Notifications.iOS.iOSNotification _internalNotification;

        /// <summary>
        /// Gets the internal notification object used by the mobile notifications system.
        /// </summary>
        public Unity.Notifications.iOS.iOSNotification InternalNotification => _internalNotification;
        
        /// <inheritdoc />
        /// <remarks>
        /// Internally stored as a string. Gets parsed to an integer when retrieving.
        /// </remarks>
        /// <value>The identifier as an integer, or null if the identifier couldn't be parsed as a number.</value>
        public int? Id
        {
            get
            {
                if (!int.TryParse(_internalNotification.Identifier, out int value))
                {
                    Debug.LogWarning("Internal iOS notification's identifier isn't a number.");
                    return null;
                }

                return value;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _internalNotification.Identifier = value.Value.ToString();
            }
        }
        
        /// <inheritdoc />
        public string Title { get => InternalNotification.Title; set => _internalNotification.Title = value; }
        
        /// <inheritdoc />
        public string Body { get => InternalNotification.Body; set => _internalNotification.Body = value; }
        
        /// <inheritdoc />
        public string Subtitle { get => InternalNotification.Subtitle; set => _internalNotification.Subtitle = value; }
        
        /// <inheritdoc />
        public string Data { get => InternalNotification.Data; set => _internalNotification.Data = value; }
        
        /// <inheritdoc />
        /// <remarks>
        /// On iOS, this represents the notification's Category Identifier.
        /// </remarks>
        public string Group { get => InternalNotification.CategoryIdentifier; set => _internalNotification.CategoryIdentifier = value; }
        
        /// <inheritdoc />
        /// <remarks>
        /// On iOS, setting this causes the notification to be delivered on a calendar time.
        /// </remarks>
        public DateTime? DeliveryTime
        {
            get
            {
                if (!(InternalNotification.Trigger is Unity.Notifications.iOS.iOSNotificationCalendarTrigger calendarTrigger))
                {
                    return null;
                }

                DateTime now = DateTime.Now;
                var result = new DateTime
                (
                    calendarTrigger.Year ?? now.Year,
                    calendarTrigger.Month ?? now.Month,
                    calendarTrigger.Day ?? now.Day,
                    calendarTrigger.Hour ?? now.Hour,
                    calendarTrigger.Minute ?? now.Minute,
                    calendarTrigger.Second ?? now.Second,
                    DateTimeKind.Local
                );

                return result;
            }
            set
            {
                if (!value.HasValue)
                {
                    return;
                }

                DateTime date = value.Value.ToLocalTime();

                _internalNotification.Trigger = new Unity.Notifications.iOS.iOSNotificationCalendarTrigger
                {
                    Year = date.Year,
                    Month = date.Month,
                    Day = date.Day,
                    Hour = date.Hour,
                    Minute = date.Minute,
                    Second = date.Second,
                    Repeats = Repeats
                };
            }
        }
        
        /// <inheritdoc />
        public bool Repeats { get; set; }

        /// <inheritdoc />
        public bool Scheduled { get; set; }
        
        /// <summary>
        /// Does nothing on iOS.
        /// </summary>
        public string LargeIconId { get; set; }
        
        /// <summary>
        /// Does nothing on iOS.
        /// </summary>
        public string SmallIconId { get; set; }

        /// <summary>
        /// Instantiates an instance of iOSNotification.
        /// </summary>
        public iOSNotification()
        {
            _internalNotification = new Unity.Notifications.iOS.iOSNotification
            {
                ShowInForeground = true
            };
        }

        /// <summary>
        /// Sets the Scheduled flag.
        /// </summary>
        public void OnScheduled()
        {
            Scheduled = true;
        }
    }
}
#endif
