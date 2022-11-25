#if UNITY_ANDROID
using System;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <inheritdoc />
    [Serializable]
    public class AndroidNotification : INotification
    {
        private Unity.Notifications.Android.AndroidNotification _internalNotification;

        /// <summary>
        /// Gets the internal notification object used by the mobile notifications system.
        /// </summary>
        public Unity.Notifications.Android.AndroidNotification InternalNotification => _internalNotification;

        /// <inheritdoc />
        public int? Id { get; set; }
        
        /// <inheritdoc />
        public string Title { get => InternalNotification.Title; set => _internalNotification.Title = value; }
        
        /// <inheritdoc />
        public string Body { get => InternalNotification.Text; set => _internalNotification.Text = value; }
        
        /// <summary>
        /// Does nothing on Android.
        /// </summary>
        public string Subtitle { get; set; }
        
        /// <inheritdoc />
        public string Data { get => InternalNotification.IntentData; set => _internalNotification.IntentData = value; }
        
        /// <inheritdoc />
        /// <remarks>
        /// On Android, this represents the notification's channel, and is required.
        /// </remarks>
        public string Group { get; set; }
        
        /// <inheritdoc />
        public DateTime? DeliveryTime
        {
            get => InternalNotification.FireTime;
            set
            {
                _internalNotification.FireTime = value ?? throw new ArgumentNullException(nameof(value));
                _internalNotification.RepeatInterval = Repeats ? value.Value.Subtract(DateTime.Now) : null;
            }
        }

        /// <inheritdoc />
        public bool Repeats { get; set; }
        
        /// <inheritdoc />
        public bool Scheduled { get; set; }
        
        /// <inheritdoc />
        public string LargeIconId { get => InternalNotification.LargeIcon; set => _internalNotification.LargeIcon = value; }
        
        /// <inheritdoc />
        public string SmallIconId { get => InternalNotification.SmallIcon; set => _internalNotification.SmallIcon = value; }

        /// <summary>
        /// Instantiates a new instance of AndroidNotification.
        /// </summary>
        public AndroidNotification()
        {
            _internalNotification = new Unity.Notifications.Android.AndroidNotification();
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
