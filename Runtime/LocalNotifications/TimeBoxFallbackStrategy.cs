using System;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Represents the strategy to be used when limiting a notification within a time box and it's scheduled outside of it.
    /// </summary>
    [Serializable]
    public enum TimeBoxFallbackStrategy
    {
        /// <summary>
        /// The notification will be scheduled for the next available time box.
        /// </summary>
        DelayUntilNextTimeBox,

        /// <summary>
        /// The notification will be scheduled for the previous available time box. In case there is none, works like DelayUntilNextTimeBox mode.
        /// </summary>
        AnticipateToPreviousAvailableTimeBox
    }
}
