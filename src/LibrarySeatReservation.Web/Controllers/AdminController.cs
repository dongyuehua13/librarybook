using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Filters;
using LibrarySeatReservation.Web.Services.Interfaces;

namespace LibrarySeatReservation.Web.Controllers;

[ServiceFilter(typeof(AdminAuthFilter))]
public class AdminController : Controller
{
    private readonly IUserService _userService;
    private readonly ISeatService _seatService;

    public AdminController(IUserService userService, ISeatService seatService)
    {
        _userService = userService;
        _seatService = seatService;
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("AdminId") != null)
            return RedirectToAction("Seats");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
        {
            ModelState.AddModelError("", "请输入管理员用户名");
            return View();
        }

        var user = await _userService.GetByUsernameAsync(username);
        if (user == null || !user.IsAdmin)
        {
            ModelState.AddModelError("", "管理员账号不存在");
            return View();
        }

        HttpContext.Session.SetInt32("AdminId", user.Id);
        HttpContext.Session.SetString("AdminName", user.DisplayName);
        return RedirectToAction("Seats");
    }

    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Remove("AdminId");
        HttpContext.Session.Remove("AdminName");
        return RedirectToAction("Login");
    }

    public async Task<IActionResult> Seats()
    {
        var seats = await _seatService.GetAllSeatsAsync();
        return View(seats);
    }

    [HttpPost]
    public async Task<IActionResult> SeatToggle(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);
        if (seat == null)
            return Json(new { success = false, message = "座位不存在" });

        await _seatService.ToggleActiveAsync(id);
        return Json(new { success = true, isActive = seat.IsActive });
    }
}
