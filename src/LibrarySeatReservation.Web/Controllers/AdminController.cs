using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Filters;
using LibrarySeatReservation.Web.Services.Interfaces;

namespace LibrarySeatReservation.Web.Controllers;

public class AdminController : Controller
{
    private readonly IUserService _userService;

    public AdminController(IUserService userService)
    {
        _userService = userService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username)
    {
        if (await _userService.ValidateAdminAsync(username))
        {
            var user = await _userService.GetByUsernameAsync(username);
            if (user != null)
            {
                HttpContext.Session.SetInt32("UserId", user.Id);
                HttpContext.Session.SetString("UserName", user.DisplayName);
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Home");
            }
        }
        ViewBag.Error = "管理员账号不存在";
        return View();
    }

    [ServiceFilter(typeof(AdminAuthFilter))]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}
