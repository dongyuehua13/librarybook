using Microsoft.AspNetCore.Mvc;
using LibrarySeatSystem.Models;
using LibrarySeatSystem.Services;
using LibrarySeatSystem.Filters;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Controllers;

[ServiceFilter(typeof(AdminAuthFilter))]
public class AdminController : Controller
{
    private readonly ISeatService _seatService;
    private readonly IReservationService _reservationService;
    private readonly IStatsService _statsService;
    private readonly IUserService _userService;

    public AdminController(ISeatService seatService, IReservationService reservationService,
        IStatsService statsService, IUserService userService)
    {
        _seatService = seatService;
        _reservationService = reservationService;
        _statsService = statsService;
        _userService = userService;
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("AdminId").HasValue)
            return RedirectToAction("Seats");
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string username)
    {
        var user = await _userService.ValidateAdminAsync(username);
        if (user == null)
        {
            ViewBag.Error = "管理员账号不存在";
            return View();
        }
        HttpContext.Session.SetInt32("AdminId", user.Id);
        HttpContext.Session.SetString("AdminName", user.DisplayName);
        return RedirectToAction("Seats");
    }

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

    public IActionResult SeatCreate()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> SeatCreate(Seat seat)
    {
        var result = await _seatService.CreateSeatAsync(seat);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(seat);
        }
        return RedirectToAction("Seats");
    }

    public async Task<IActionResult> SeatEdit(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);
        if (seat == null) return RedirectToAction("Seats");
        return View(seat);
    }

    [HttpPost]
    public async Task<IActionResult> SeatEdit(Seat seat)
    {
        var result = await _seatService.UpdateSeatAsync(seat);
        if (!result.Success)
        {
            ModelState.AddModelError("", result.Message);
            return View(seat);
        }
        return RedirectToAction("Seats");
    }

    [HttpPost]
    public async Task<IActionResult> SeatToggle(int id)
    {
        var seat = await _seatService.GetSeatByIdAsync(id);
        if (seat != null)
        {
            seat.IsActive = !seat.IsActive;
            await _seatService.SaveChangesAsync();
        }
        return Json(new { success = true });
    }

    public async Task<IActionResult> Reservations(string? status, DateTime? dateFrom, DateTime? dateTo)
    {
        var vm = await _reservationService.GetAllReservationsAsync(status, dateFrom, dateTo);
        ViewBag.Status = status;
        ViewBag.DateFrom = dateFrom?.ToString("yyyy-MM-dd");
        ViewBag.DateTo = dateTo?.ToString("yyyy-MM-dd");
        return View(vm);
    }

    public async Task<IActionResult> Stats()
    {
        var stats = await _statsService.GetAllStatsAsync();
        return View(stats);
    }
}
