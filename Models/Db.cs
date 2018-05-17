/* The classes in this file represent the database structure. */

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Wedding.Models
{
    public class Invitee
    {
        public int InviteeId { get; set; }

        public string Name { get; set; }

        public int Code { get; set; }

        public string AuthToken { get; set; }

        public DateTime? AuthTokenValidUntil { get; set; }

        public ICollection<Response> Responses { get; set; } = new HashSet<Response>();
    }

    public class Response
    {
        public int ResponseId { get; set; }

        public int InviteeId { get; set; }

        public int Adults { get; set; }

        public bool Driving { get; set; }

        public bool BusFrom { get; set; }

        public bool BusTo { get; set; }

        [Column(TypeName = "char(2)")]
        public string MoochFrom { get; set; }

        [Column(TypeName = "char(2)")]
        public string MoochTo { get; set; }

        public bool WineTour { get; set; }

        public string Dietary { get; set; }

        public string Comments { get; set; }

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public Invitee Invitee { get; set; }

        public ICollection<ResponseOffer> ResponseOffers { get; set; } = new HashSet<ResponseOffer>();
    }

    public class ResponseOffer
    {
        public int ResponseOfferId { get; set; }

        public int ResponseId { get; set; }

        [Column(TypeName = "char(2)")]
        public string OfferDay { get; set; }

        public int OfferCount { get; set; }

        public Response Response { get; set; }
    }

    public class Ef : DbContext
    {
        public Ef(DbContextOptions<Ef> options) : base(options) {}
        
        public DbSet<Invitee> Invitees { get; set; }
        public DbSet<Response> Responses { get; set; }
        public DbSet<ResponseOffer> ResponseOffers { get; set; }
    }
}
