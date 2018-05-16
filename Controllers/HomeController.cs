using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Wedding.Models;
using Wedding.Util;

namespace Wedding.Controllers
{
    public class HomeController : Controller
    {
        Ef ef;

        public HomeController(Ef ef) => this.ef = ef;

        public IActionResult Index() => View();

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] Login login)
        {
            string tram = login.name?.Trim();
            Invitee guest = await ef.Invitees.FirstOrDefaultAsync(i => i.Name == tram && i.Code == login.code);
            if (guest == null)
                return Unauthorized();
            await guest.SetAuthTokenAsync(ef);

            return Json(await guest.ToRsvpAsync(ef));
        }

        [HttpPost]
        public async Task<IActionResult> Rsvp([FromBody] Rsvp rsvp)
        {
            if (rsvp.authToken == null)
                return Unauthorized();
            Invitee guest = await ef.Invitees.FirstOrDefaultAsync(i => i.AuthToken == rsvp.authToken
                && i.AuthTokenValidUntil.HasValue && i.AuthTokenValidUntil.Value > DateTime.UtcNow);
            if (guest == null)
                return Unauthorized();
            await guest.SetAuthTokenAsync(ef);

            if (rsvp.survey != null)
                await guest.AddResponseAsync(ef, rsvp.survey);
            return Json(await guest.ToRsvpAsync(ef));
        }
    }
}
