using System.Collections.Immutable;
using System.Linq;
using Wedding.Models;

namespace Wedding.Util
{
    public static class Constants
    {
        public static readonly string URL_PATH_PREFIX = "/wedding";

        // The invitations which will be seeded when the database is created.
        public static readonly ImmutableList<Invitee> SEED_INVITEES = new[] {
            new Invitee {
                Code = 1234567,
                Name = "Invitation1 User1"
            },
            new Invitee {
                Code = 1234567,
                Name = "Invitation1 User2"
            },
            new Invitee {
                Code = 7654321,
                Name = "Invitation2 User1"
            }
        }.ToImmutableList();
    }
}
