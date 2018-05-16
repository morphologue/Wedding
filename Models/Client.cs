/* The classes in this file represent how we interface with the client. */

using System.Collections.Generic;

namespace Wedding.Models
{
    // This mirrors the Survey class in Model.ts.
    public class Survey
    {
        public Survey()
        {
            this.offer = new Dictionary<string, int>();
            foreach (TravelDay day in TravelDay.Days)
                offer.Add(day.ToString(), 0);

            this.moochFrom = this.moochTo = this.dietary = this.comments = string.Empty;
        }

        public int adults { get; set; }
        public bool? driving { get; set; }
        public bool busFrom { get; set; }
        public bool busTo { get; set; }
        public Dictionary<string, int> offer { get; set; }
        public string moochFrom { get; set; }
        public string moochTo { get; set; }
        public bool wineTour { get; set; }
        public string dietary { get; set; }
        public string comments { get; set; }
    }

    // This mirrors the Rsvp class in Model.ts.
    public class Rsvp
    {
        public string authToken { get; set; }
        public Survey survey { get; set; }
    }

    // This structure is used in TS but the type is not named there.
    public class Login
    {
        public string name { get; set; }
        public int code { get; set; }
    }
}
