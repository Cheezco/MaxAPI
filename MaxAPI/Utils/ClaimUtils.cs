using MaxAPI.Enums;
using System.Security.Claims;

namespace MaxAPI.Utils
{
    public static class ClaimUtils
    {
        public static Role GetRole(Claim claim)
        {
            if (claim is null || !Enum.TryParse(claim.Value, out Role claimRole))
            {
                return Role.Patient;
            }

            return claimRole;
        }

        public static Role GetRole(HttpContext context)
        {
            var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Role");
            if (claim is null)
            {
                return Role.Patient;
            }

            return GetRole(claim);
        }

        public static int GetId(HttpContext context)
        {
            var claim = context.User.Claims.FirstOrDefault(x => x.Type == "Id");
            if (claim is null)
            {
                return -1;
            }

            return Convert.ToInt32(claim.Value);
        }
    }
}
