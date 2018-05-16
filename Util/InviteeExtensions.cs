using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Wedding.Models;

namespace Wedding.Util
{
    // Operations on the Invitee entity
    public static class InviteeExtensions
    {
        public static async Task SetAuthTokenAsync(this Invitee guest, Ef ef)
        {
            // Log out all invitees with the same invitation code.
            foreach (Invitee other in await ef.Invitees.Where(i => i.Code == guest.Code).ToListAsync()) {
                other.AuthToken = null;
                other.AuthTokenValidUntil = null;
            }

            // Record login.
            string unique_token;
            do unique_token = Guid.NewGuid().ToString();
            while (await ef.Invitees.AnyAsync(i => i.AuthToken == unique_token));
            guest.AuthToken = Guid.NewGuid().ToString();
            guest.AuthTokenValidUntil = DateTime.UtcNow.AddMinutes(60);

            await ef.SaveChangesAsync();
        }

        public static async Task<Rsvp> ToRsvpAsync(this Invitee guest, Ef ef)
        {
            // Identify the latest response for the invitation code (or null if there is none).
            Response latest = await ef.Responses
                .Where(r => r.Invitee.Code == guest.Code)
                .OrderByDescending(r => r.ResponseId)
                .FirstOrDefaultAsync();

            Survey survey;
            if (latest == null)
                // The is the blank survey we serve when there is no response in the DB.
                survey = new Survey { adults = 1 };
            else {
                // Copy the guts.
                survey = new Survey
                {
                    adults = latest.Adults,
                    driving = latest.Driving,
                    busFrom = latest.BusFrom,
                    busTo = latest.BusTo,
                    moochFrom = latest.MoochFrom,
                    moochTo = latest.MoochTo,
                    wineTour = latest.WineTour,
                    dietary = latest.Dietary,
                    comments = latest.Comments
                };

                // Add in any ResponseOffer records.
                foreach (ResponseOffer roffer in latest.ResponseOffers)
                    survey.offer[roffer.OfferDay] = roffer.OfferCount;
            }

            // Build the final result.
            return new Rsvp
            {
                authToken = guest.AuthToken,
                survey = survey
            };
        }

        public static async Task AddResponseAsync(this Invitee guest, Ef ef, Survey survey)
        {
            Response creation;
            if (survey.adults == 0)
                creation = new Response();
            else {
                if (!survey.driving.HasValue)
                    throw new ArgumentNullException("'driving' must be non-null when at least one guest is attending");

                creation = new Response
                {
                    Adults = survey.adults,
                    Driving = survey.driving.Value,
                    WineTour = survey.wineTour,
                    Dietary = survey.dietary
                };

                if (survey.driving.Value) {
                    foreach (KeyValuePair<string, int> pair in survey.offer
                            .Where(kv => TravelDay.FromString(kv.Key) != null && kv.Value > 0))
                        creation.ResponseOffers.Add(new ResponseOffer
                        {
                            OfferDay = pair.Key,
                            OfferCount = pair.Value
                        });
                } else {
                    creation.BusFrom = survey.busFrom;
                    creation.BusTo = survey.busTo;
                    creation.MoochFrom = survey.moochFrom;
                    creation.MoochTo = survey.moochTo;
                }
            }

            creation.InviteeId = guest.InviteeId;
            creation.Comments = survey.comments;
            ef.Responses.Add(creation);
            await ef.SaveChangesAsync();
        }
    }
}
