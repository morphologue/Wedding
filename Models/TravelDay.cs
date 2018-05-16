using System.Collections.Immutable;
using System.Linq;

namespace Wedding.Models
{
    // A day in January when people might like to travel to/from Adelaide
    public class TravelDay
    {
        public static readonly ImmutableList<TravelDay> Days = new[] {
            new TravelDay(18, "Thu"),
            new TravelDay(19, "Fri"),
            new TravelDay(20, "Sat"),
            new TravelDay(21, "Sun"),
            new TravelDay(22, "Mon"),
            new TravelDay(23, "Tue")
        }.ToImmutableList();

        // The inverse of ToString()
        public static TravelDay FromString(string dom) => Days.FirstOrDefault(d => d.DayOfMonth.ToString() == dom);

        public int DayOfMonth { get; private set; }
        public string ShortDayOfWeek { get; private set; }

        public TravelDay(int day_of_month, string short_day_of_week)
        {
            this.DayOfMonth = day_of_month;
            this.ShortDayOfWeek = short_day_of_week;
        }

        // This form will be shown on the web page.
        public string Prettify() => $"{ShortDayOfWeek} {DayOfMonth} January";

        // This form will be used in JSON.
        public override string ToString() => DayOfMonth.ToString();
    }
}
