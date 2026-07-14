using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySeatReservation.Web.Filters;

public class AdminAuthFilter : IAuthorizationFilter
{
    public void OnAuthorization(AuthorizationFilterContext context)
    {
        var action = context.RouteData.Values["action"]?.ToString();
        if (action == "Login" || action == "Logout")
            return;

        if (context.HttpContext.Session.GetInt32("AdminId") == null)
        {
            if (context.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
            {
                context.Result = new JsonResult(new { success = false, message = "登录已过期" })
                {
                    StatusCode = 401
                };
            }
            else
            {
                context.Result = new RedirectToActionResult("Login", "Admin", null);
            }
        }
    }
}
