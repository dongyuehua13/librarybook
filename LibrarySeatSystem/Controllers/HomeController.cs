using Microsoft.AspNetCore.Mvc;
using LibrarySeatSystem.Services;

namespace LibrarySeatSystem.Controllers;

public class HomeController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IReservationService _reservationService;
    private readonly IStatsService _statsService;
    private readonly IUserService _userService;

    public HomeController(ISeatService seatService, IReservationService reservationService,
        IStatsService statsService, IUserService userService)
    {
        _seatService = seatService;
        _reservationService = reservationService;
        _statsService = statsService;
        _userService = userService;
    }

    public async Task<IActionResult> Index()
    {
        var stats = await _statsService.GetDashboardStatsAsync();
        ViewBag.TotalSeats = stats.TotalActiveSeats;
        ViewBag.TodayReservations = stats.TodayReservations;
        return View();
    }

    public async Task<IActionResult> Seats(int? floor, string? area)
    {
        var seats = await _seatService.GetSeatsWithStatusAsync(floor, area);
        ViewBag.Floors = seats.Select(s => s.Floor).Distinct().OrderBy(f => f).ToList();
        ViewBag.Areas = seats.Select(s => s.Area).Distinct().ToList();
        ViewBag.SelectedFloor = floor;
        ViewBag.SelectedArea = area;
        return View(seats);
    }

    public async Task<IActionResult> Detail(int id)
    {
        var detail = await _seatService.GetSeatDetailAsync(id);
        if (detail == null) return RedirectToAction("Seats");
        ViewBag.Reservations = detail.ReservationRecords;
        return View(detail.Seat);
    }

    public async Task<IActionResult> Reserve(int seatId)
    {
        var seat = await _seatService.GetSeatByIdAsync(seatId);
        if (seat == null || !seat.IsActive) return RedirectToAction("Seats");
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("SwitchUser");
        return View(seat);
    }

    [HttpPost]
    public async Task<IActionResult> Reserve(int seatId, DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Json(new { success = false, msg = "请先选择账号" });

        if (startTime >= endTime)
            return Json(new { success = false, msg = "开始时间必须早于结束时间" });

        if ((endTime - startTime).TotalHours > 4)
            return Json(new { success = false, msg = "单次预约最长4小时" });

        var result = await _reservationService.CreateAsync(userId.Value, seatId, date, startTime, endTime);
        return Json(new { success = result.Success, msg = result.Message });
    }

    public async Task<IActionResult> MyReservations()
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("SwitchUser");
        var vm = await _reservationService.GetUserReservationsAsync(userId.Value);
        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Cancel(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return Json(new { success = false, msg = "请先选择账号" });
        var result = await _reservationService.CancelAsync(id, userId.Value);
        return Json(new { success = result.Success, msg = result.Message });
    }

    public async Task<IActionResult> SwitchUser()
    {
        var users = await _userService.GetStudentsAsync();
        return View(users);
    }

    [HttpPost]
    public async Task<IActionResult> SwitchUser(int userId)
    {
        var user = await _userService.GetByIdAsync(userId);
        if (user != null)
        {
            HttpContext.Session.SetInt32("UserId", user.Id);
            HttpContext.Session.SetString("UserName", user.DisplayName);
        }
        return RedirectToAction("Index");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Remove("UserId");
        HttpContext.Session.Remove("UserName");
        return RedirectToAction("SwitchUser");
    }
}
