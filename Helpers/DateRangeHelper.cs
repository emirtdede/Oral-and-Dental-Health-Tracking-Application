namespace DisSagligiTakip.Helpers
{
    public static class DateRangeHelper
    {
        // [start, end) – end: bugün + 1 gün 00:00
        public static (DateTime start, DateTime end, List<DateOnly> days) Last7Days()
        {
            var today = DateTime.Today;
            var start = today.AddDays(-6); // bugünü dahil 7 gün
            var end = today.AddDays(1);    // üst sınır
            var list = new List<DateOnly>();
            for (var d = 0; d < 7; d++)
            {
                list.Add(DateOnly.FromDateTime(start.AddDays(d)));
            }
            return (start, end, list);
        }
    }
}
