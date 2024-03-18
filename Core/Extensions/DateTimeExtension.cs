namespace SystemAggregator.Core.Extensions
{
    public static class DateTimeExtension
    {
        public static int CalculateDateDiffInMinutes(DateTime begin, DateTime end)
        {
            var minutes = CalculateDateDiff(begin, end).TotalMinutes;

            try
            {
                var a = Math.Round(minutes, MidpointRounding.AwayFromZero);

                return Convert.ToInt32(minutes);
            }
            catch (OverflowException ex)
            {
                return 0;
            }
        }

        public static TimeSpan CalculateDateDiff(DateTime begin, DateTime end)
        {
            return end - begin;
        }
    }
}
