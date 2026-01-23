using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Habit_Tracker_Backend.Filters
{
    public class AdminOnlyAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var role = context.HttpContext.Session.GetString("ROLE");

            if (role != "ADMIN")
            {
                context.Result = new ObjectResult(new
                {
                    success = false,
                    message = "Admin access only"
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
        }
    }
}
