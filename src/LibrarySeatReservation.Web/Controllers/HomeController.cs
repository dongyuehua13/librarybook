using Microsoft.AspNetCore.Mvc;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Filters;
using LibrarySeatReservation.Web.Services.Interfaces;

namespace LibrarySeatReservation.Web.Controllers;

public class HomeController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IReservationService _reservationService;
    private readonly IStatsService _statsService;
    private readonly IUserService _userService;
    private readonly AppDbContext _db;

    public HomeController(
        ISeatService seatService,
        IReservationService reservationService,
        IStatsService statsService,
        IUserService userService,
        AppDbContext db)
    {
        _seatService = seatService;
        _reservationService = reservationService;
        _statsService = statsService;
        _userService = userService;
        _db = db;
    }

    public async Task<IActionResult> Index()
    {
        ViewBag.Students = await _userService.GetStudentsAsync();
        ViewBag.CurrentUserId = HttpContext.Session.GetInt32("UserId");
        ViewBag.CurrentUserName = HttpContext.Session.GetString("UserName");

        ViewBag.TotalSeats = (await _seatService.GetAllAsync()).Count;
        ViewBag.ActiveSeats = await _statsService.GetActiveSeatCountAsync();
        ViewBag.TodayReservations = await _statsService.GetTodayReservationCountAsync();
        ViewBag.AreaDistribution = await _statsService.GetAreaDistributionAsync();

        return View();
    }

    [HttpPost]
    public IActionResult SwitchUser(int userId)
    {
        var user = _db.Users.Find(userId);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.DisplayName);
            if (user.IsAdmin)
            {
                HttpContext.Session.SetString("IsAdmin", "true");
            }
        }
        return RedirectToAction("Index");
    }
}
