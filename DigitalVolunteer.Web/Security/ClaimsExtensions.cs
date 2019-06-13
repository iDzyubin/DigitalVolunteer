using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DigitalVolunteer.Web.Security
{
    public static class ClaimsExtensions
    {
        public static Guid? GetId( this ClaimsPrincipal user )
        {
            var strId = user.FindFirstValue( DvClaimTypes.UserId );
            return strId == null ? null : (Guid?)Guid.Parse( strId );
        }

        public static string GetLogin( this ClaimsPrincipal user )
        {
            return user.FindFirstValue( ClaimTypes.Name );
        }

        public static bool IsAdmin( this ClaimsPrincipal user )
        {
            return user.HasBoolClaim( DvClaimTypes.IsAdmin );
        }

        private static bool HasBoolClaim( this ClaimsPrincipal user, string claimType )
        {
            var cv = user.FindFirstValue( claimType );
            return cv?.Trim().ToLower() == "true";
        }
    }
}
