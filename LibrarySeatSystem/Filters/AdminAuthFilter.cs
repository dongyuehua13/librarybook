using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LibrarySeatSystem.Filters;

public class AdminAuthFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        var actionName = context.RouteData.Values["action"]?.ToString();
        if (actionName == "Login" || actionName == "Logout")
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

    public void OnActionExecuted(ActionExecutedContext context) { }
}
