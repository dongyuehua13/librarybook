using Microsoft.EntityFrameworkCore;
using LibrarySeatSystem.Data;
using LibrarySeatSystem.Models;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<(bool Success, string Message)> CreateAsync(int userId, int seatId,
        DateTime date, TimeSpan startTime, TimeSpan endTime)
    {
        var todayCount = await _db.Reservations.CountAsync(r =>
            r.UserId == userId && r.Date == date && r.Status == "已预约");
        if (todayCount >= 2)
            return (false, "当日预约已达上限（最多2次）");

        var hasConflict = await _db.Reservations.AnyAsync(r =>
            r.SeatId == seatId &&
            r.Date == date &&
            r.Status == "已预约" &&
            startTime < r.EndTime &&
            endTime > r.StartTime);

        if (hasConflict)
            return (false, "该时段已被预约，请选择其他时段或座位");

        var userConflict = await _db.Reservations.AnyAsync(r =>
            r.UserId == userId &&
            r.Date == date &&
            r.Status == "已预约" &&
            startTime < r.EndTime &&
            endTime > r.StartTime);

        if (userConflict)
            return (false, "该时段您已预约其他座位");

        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null || !seat.IsActive)
            return (false, "该座位不可用");

        _db.Reservations.Add(new Reservation
        {
            UserId = userId,
            SeatId = seatId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Status = "已预约",
            CreatedAt = DateTime.Now
        });
        await _db.SaveChangesAsync();

        return (true, "预约成功");
    }

    public async Task<MyReservationsViewModel> GetUserReservationsAsync(int userId)
    {
        var now = DateTime.Now;
        var today = DateTime.Today;

        var records = await _db.Reservations
            .Where(r => r.UserId == userId)
            .Include(r => r.Seat)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var items = records.Select(r => new ReservationItem
        {
            Id = r.Id,
            SeatNumber = r.Seat.SeatNumber,
            Date = r.Date,
            TimeSlot = $"{r.StartTime:hh\\:mm} - {r.EndTime:hh\\:mm}",
            Status = r.Status,
            CreatedAt = r.CreatedAt,
            CanCancel = r.Status == "已预约" &&
                        (r.Date > today ||
                         (r.Date == today && r.StartTime > now.TimeOfDay))
        }).ToList();

        return new MyReservationsViewModel { Items = items };
    }

    public async Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId)
    {
        var reservation = await _db.Reservations.FindAsync(reservationId);
        if (reservation == null)
            return (false, "预约记录不存在");

        if (reservation.UserId != userId)
            return (false, "只能取消自己的预约");

        if (reservation.Status != "已预约")
            return (false, "只有已预约状态的记录可以取消");

        var now = DateTime.Now;
        var today = DateTime.Today;
        if (reservation.Date < today)
            return (false, "已过期的预约无法取消");
        if (reservation.Date == today && reservation.StartTime <= now.TimeOfDay)
            return (false, "预约已开始，无法取消");

        reservation.Status = "已取消";
        await _db.SaveChangesAsync();

        return (true, "取消成功");
    }

    public async Task<AdminReservationsViewModel> GetAllReservationsAsync(
        string? status, DateTime? dateFrom, DateTime? dateTo)
    {
        var query = _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Seat)
            .AsQueryable();

        if (!string.IsNullOrEmpty(status))
            query = query.Where(r => r.Status == status);
        if (dateFrom.HasValue)
            query = query.Where(r => r.Date >= dateFrom.Value);
        if (dateTo.HasValue)
            query = query.Where(r => r.Date <= dateTo.Value);

        var records = await query.OrderByDescending(r => r.CreatedAt).ToListAsync();

        return new AdminReservationsViewModel
        {
            Items = records.Select(r => new AdminReservationItem
            {
                Id = r.Id,
                UserName = r.User.DisplayName,
                SeatNumber = r.Seat.SeatNumber,
                Date = r.Date,
                TimeSlot = $"{r.StartTime:hh\\:mm} - {r.EndTime:hh\\:mm}",
                Status = r.Status,
                CreatedAt = r.CreatedAt
            }).ToList()
        };
    }
}
