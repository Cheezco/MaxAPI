using MaxAPI.Enums;
using MaxAPI.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MaxAPI.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class AuthorizationRoleAttribute : Attribute, IAuthorizationFilter
    {
        private readonly Role _role;

        public AuthorizationRoleAttribute(Role role)
        {
            _role = role;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (!_role.HasFlag(ClaimUtils.GetRole(context.HttpContext)))
            {
                context.Result = new UnauthorizedResult();
            }
        }
    }
}
