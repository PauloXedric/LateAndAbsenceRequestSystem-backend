namespace DLARS.Helpers
{
    public static class TimeHelper
    {

        private static readonly TimeZoneInfo _philippineTimeZone =
            TimeZoneInfo.FindSystemTimeZoneById("Singapore Standard Time");

        public static DateTime GetPhilippineTimeNow()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _philippineTimeZone);
        }

    }
}
