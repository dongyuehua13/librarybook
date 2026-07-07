using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySeatReservation.Web.Filters;

public class AdminAuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var isAdmin = context.HttpContext.Session.GetString("IsAdmin");
        if (isAdmin != "true")
        {
            context.Result = new RedirectToActionResult("Login", "Admin", null);
        }
    }
}
