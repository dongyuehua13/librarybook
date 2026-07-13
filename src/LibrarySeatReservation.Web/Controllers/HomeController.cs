using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Services.Interfaces;
using LibrarySeatReservation.Web.ViewModels;

namespace LibrarySeatReservation.Web.Controllers;

public class HomeController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IReservationService _reservationService;
    private readonly IStatsService _statsService;
    private readonly IUserService _userService;

    public HomeController(
        ISeatService seatService,
        IReservationService reservationService,
        IStatsService statsService,
        IUserService userService)
    {
        _seatService = seatService;
        _reservationService = reservationService;
        _statsService = statsService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var students = await _userService.GetStudentsAsync();
        var stats = await _statsService.GetDashboardStatsAsync();

        var vm = new HomeIndexViewModel
        {
            TotalActiveSeats = stats.TotalActiveSeats,
            TodayReservations = stats.TodayReservations,
            AreaDistribution = stats.AreaDistribution,
            CurrentUserId = HttpContext.Session.GetInt32("UserId"),
            CurrentUserName = HttpContext.Session.GetString("UserName"),
            Students = students
        };

        return View(vm);
    }

    public async Task<IActionResult> Seats(int? floor, string? area)
    {
        var seats = await _seatService.GetSeatsWithStatusAsync(floor, area);
        return View(seats);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var detail = await _seatService.GetSeatDetailAsync(id);
        if (detail == null)
            return NotFound();

        detail.IsCurrentUserLoggedIn = HttpContext.Session.GetInt32("UserId") != null;
        return View(detail);
    }

    [HttpPost]
    public async Task<IActionResult> SwitchUser(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.DisplayName);
            if (user.IsAdmin)
                HttpContext.Session.SetString("IsAdmin", "true");
        }
        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("UserName");
        HttpContext.Session.Remove("IsAdmin");
        return RedirectToAction("Index");
    }
}
