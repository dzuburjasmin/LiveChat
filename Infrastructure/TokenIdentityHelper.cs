using System.Security.Claims;

namespace InformationProtocolSubSystem.Api.Infrastructure
{

    public static class TokenIdentityHelper
    {

        public static bool HasClaim(ClaimsPrincipal user, string claim)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return identity?.Claims.Any(a => a.Value == claim) ?? false;
        }

        public static List<string> GetClaimValues(ClaimsPrincipal user, string claimType)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return identity?.Claims.Where(a => a.Type.ToLower() == claimType?.ToLower()).Select(a => a.Value).ToList();
        }
        public static string GetUserName(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return identity?.Claims.FirstOrDefault(a => a.Type == "preferred_username")?.Value;
        }
        public static string GetName(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return identity?.Claims.FirstOrDefault(a => a.Type == "name")?.Value;
        }
        public static Guid GetUserIdFromToken(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return new Guid(identity?.Claims.FirstOrDefault(a => a.Type == "sub")?.Value);
       }
        public static string GetUserEmail(ClaimsPrincipal user)
        {
            var identity = (ClaimsIdentity)user.Identity;
            return identity?.Claims.FirstOrDefault(a => a.Type == "email")?.Value;
        }
    }
}