﻿using System;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Represents a notification that will be delivered for this application.
    /// </summary>
    public interface INotification
    {
        /// <summary>
        /// Gets or sets a unique identifier for this notification.
        /// </summary>
        /// <remarks>
        /// <para>
        /// If null, will be generated automatically once the notification is delivered, and then
        /// can be retrieved afterwards.
        /// </para>
        /// <para>On some platforms, this might be converted to a string identifier internally.</para>
        /// </remarks>
        /// <value>A unique integer identifier for this notification, or null (on some platforms) if not explicitly set.</value>
        int? Id { get; set; }

        /// <summary>
        /// Gets or sets the notification's title.
        /// </summary>
        /// <value>The title message for the notification.</value>
        string Title { get; set; }

        /// <summary>
        /// Gets or sets the body text of the notification.
        /// </summary>
        /// <value>The body message for the notification.</value>
        string Body { get; set; }

        /// <summary>
        /// Gets or sets a subtitle for the notification.
        /// </summary>
        /// <value>The subtitle message for the notification.</value>
        string Subtitle { get; set; }

        /// <summary>
        /// Gets or sets optional arbitrary data for the notification.
        /// </summary>
        /// <value>The data for the notification</value>
        string Data { get; set; }

        /// <summary>
        /// Gets or sets group to which this notification belongs.
        /// </summary>
        /// <value>A platform specific string identifier for the notification's group.</value>
        string Group { get; set; }

        /// <summary>
        /// Gets or sets time to deliver the notification.
        /// </summary>
        /// <value>The time of delivery in local time.</value>
        DateTime? DeliveryTime { get; set; }

        /// <summary>
        /// Gets or sets whether the notification should repeat peiodically or not.
        /// </summary>
        bool Repeats { get; set; }

        /// <summary>
        /// Gets whether this notification has been scheduled.
        /// </summary>
        /// <value>True if the notification has been scheduled with the underlying operating system.</value>
        bool Scheduled { get; set; }

        /// <summary>
        /// Notification large icon.
        /// </summary>
        string LargeIconId { get; set; }

        /// <summary>
        /// Notification small icon.
        /// </summary>
        string SmallIconId { get; set; }
    }
}