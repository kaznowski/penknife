using System;

namespace DoubleDash.CodingTools.LocalNotifications
{
    /// <summary>
    /// Responsible for calculating the delivery time of notifications.
    /// </summary>
    public class NotificationDeliveryTimeCalculator
    {
        /// <summary>
        /// Calculates the delivery time of a notification.
        /// </summary>
        /// <param name="timeTrigger">How many minutes in the future the notification should the scheduled for.</param>
        /// <param name="useTimeBox">Whether the notification should be limited within a time box.</param>
        /// <param name="minHour">The minimum hour of the time box.</param>
        /// <param name="minMinute">The minimum minute of the time box.</param>
        /// <param name="maxHour">The maximum hour of the time box.</param>
        /// <param name="maxMinute">The maximum minute of the time box.</param>
        /// <param name="fallbackStrategy">The strategy to be used in case the notification is scheduled outside the desired time box.</param>
        /// <param name="fallbackHour">The fallback hour the notification should be scheduled to. Must be between min and maxHour.</param>
        /// <param name="fallbackMinute">The fallback minute the notification should be scheduled to. Must be between min and maxMinute.</param>
        /// <returns>The DateTime that corresponds to when the notification has been scheduled for.</returns>
        public static DateTime CalculateDeliveryTime(double timeTrigger, bool useTimeBox, int minHour, int minMinute,
            int maxHour, int maxMinute, TimeBoxFallbackStrategy fallbackStrategy, int fallbackHour, int fallbackMinute)
        {
            var deliveryTime = DateTime.Now.AddMinutes(timeTrigger);

            if (!useTimeBox)
                return deliveryTime;

            var maxTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, maxHour, maxMinute, 0);
            var minTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, minHour, minMinute, 0);

            if (IsDeliveryTimeOutsideTimeBox(deliveryTime, minTime, maxTime))
            {
                var fallbackTime = new DateTime(deliveryTime.Year, deliveryTime.Month, deliveryTime.Day, fallbackHour,
                    fallbackMinute, 0);

                if (fallbackStrategy == TimeBoxFallbackStrategy.DelayUntilNextTimeBox)
                    deliveryTime = FindNextTimeBox(deliveryTime, minTime, maxTime, fallbackTime);
                else
                    deliveryTime = FindPreviousAvailableTimeBox(deliveryTime, minTime, maxTime, fallbackTime);
            }

            return deliveryTime;
        }

        private static bool IsDeliveryTimeOutsideTimeBox(DateTime deliveryTime, DateTime minTime, DateTime maxTime)
        {
            return deliveryTime < minTime || deliveryTime > maxTime;
        }

        private static DateTime FindNextTimeBox(DateTime deliveryTime, DateTime minTime, DateTime maxTime,
            DateTime fallbackTime)
        {
            if (deliveryTime < minTime)
            {
                var sameDayAtFallbackTime = new DateTime(
                    deliveryTime.Year,
                    deliveryTime.Month,
                    deliveryTime.Day,
                    fallbackTime.Hour,
                    fallbackTime.Minute,
                    0);

                deliveryTime = sameDayAtFallbackTime;
            }
            else if (deliveryTime > maxTime)
            {
                var nextDayAtFallbackTime = deliveryTime.AddDays(1);

                nextDayAtFallbackTime = new DateTime(
                    nextDayAtFallbackTime.Year,
                    nextDayAtFallbackTime.Month,
                    nextDayAtFallbackTime.Day,
                    fallbackTime.Hour,
                    fallbackTime.Minute,
                    0);

                deliveryTime = nextDayAtFallbackTime;
            }

            return deliveryTime;
        }

        private static DateTime FindPreviousAvailableTimeBox(DateTime deliveryTime, DateTime minTime, DateTime maxTime,
            DateTime fallbackTime)
        {
            if (deliveryTime < minTime)
            {
                var previousDayAtFallbackTime = deliveryTime.AddDays(-1);

                previousDayAtFallbackTime = new DateTime(
                    previousDayAtFallbackTime.Year,
                    previousDayAtFallbackTime.Month,
                    previousDayAtFallbackTime.Day,
                    fallbackTime.Hour,
                    fallbackTime.Minute,
                    0);

                if (DateTime.Now > previousDayAtFallbackTime)
                {
                    deliveryTime = FindNextTimeBox(deliveryTime, minTime, maxTime, fallbackTime);
                }
                else
                {
                    deliveryTime = previousDayAtFallbackTime;
                }
            }
            else if (deliveryTime > maxTime)
            {
                var sameDayAtFallbackTime = new DateTime(
                    deliveryTime.Year,
                    deliveryTime.Month,
                    deliveryTime.Day,
                    fallbackTime.Hour,
                    fallbackTime.Minute,
                    0);

                if (DateTime.Now > sameDayAtFallbackTime)
                {
                    deliveryTime = FindNextTimeBox(deliveryTime, minTime, maxTime, fallbackTime);
                }
                else
                {
                    deliveryTime = sameDayAtFallbackTime;
                }
            }

            return deliveryTime;
        }
    }
}
