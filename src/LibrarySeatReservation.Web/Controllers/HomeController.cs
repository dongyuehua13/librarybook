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

    public async Task<IActionResult> Reserve(int seatId)
    {
        if (HttpContext.Session.GetInt32("UserId") == null)
            return RedirectToAction("Index");

        var seat = await _seatService.GetSeatByIdAsync(seatId);
        if (seat == null || !seat.IsActive)
            return NotFound();

        return View(seat);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(int seatId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return Json(new { success = false, message = "请先选择学生账号" });

        if (startTime >= endTime)
            return Json(new { success = false, message = "结束时间必须晚于开始时间" });

        if (startTime.Hour < 8 || endTime.Hour > 22 || (endTime.Hour == 22 && endTime.Minute > 0))
            return Json(new { success = false, message = "预约时段必须在 8:00-22:00 之间" });

        var duration = endTime - startTime;
        if (duration > new TimeSpan(4, 0, 0))
            return Json(new { success = false, message = "单次预约最长4小时" });

        if (duration < new TimeSpan(1, 0, 0))
            return Json(new { success = false, message = "预约时长至少 1 小时" });

        if (date < DateOnly.FromDateTime(DateTime.Today))
            return Json(new { success = false, message = "不能预约过去的日期" });

        var (success, message) = await _reservationService.CreateAsync(
            userId.Value, seatId, date, startTime, endTime);

        return Json(new { success, message });
    }

    public async Task<IActionResult> MyReservations()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return RedirectToAction("Index");

        var viewModel = await _reservationService.GetUserReservationsAsync(userId.Value);
        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null)
            return Json(new { success = false, message = "请先选择学生账号" });

        var (success, message) = await _reservationService.CancelAsync(id, userId.Value);
        return Json(new { success, message });
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
