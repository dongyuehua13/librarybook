using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.Services.Interfaces;

namespace LibrarySeatReservation.Web.Services;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _db;

    public ReservationService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Reservation>> GetUserReservationsAsync(int userId)
    {
        return await _db.Reservations
            .Include(r => r.Seat)
            .Where(r => r.UserId == userId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _db.Reservations
            .Include(r => r.User)
            .Include(r => r.Seat)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<List<Reservation>> GetBySeatAsync(int seatId)
    {
        return await _db.Reservations
            .Where(r => r.SeatId == seatId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();
    }

    public async Task<(bool Success, string Message)> CreateAsync(int userId, int seatId, DateOnly date, TimeOnly startTime, TimeOnly endTime)
    {
        var seat = await _db.Seats.FindAsync(seatId);
        if (seat == null || !seat.IsActive)
            return (false, "该座位不存在或已禁用");

        var user = await _db.Users.FindAsync(userId);
        if (user == null || user.IsAdmin)
            return (false, "无效的用户");

        if (startTime >= endTime)
            return (false, "开始时间必须早于结束时间");

        var duration = endTime - startTime;
        if (duration.TotalHours > 4)
            return (false, "单次预约不能超过4小时");

        var todayCount = await _db.Reservations
            .CountAsync(r => r.UserId == userId && r.Date == date && r.Status == "已预约");
        if (todayCount >= 2)
            return (false, "同一日期预约不能超过2次");

        var hasOverlap = await _db.Reservations
            .AnyAsync(r => r.UserId == userId && r.Date == date && r.Status == "已预约"
                && startTime < r.EndTime && endTime > r.StartTime);
        if (hasOverlap)
            return (false, "该时段已有预约，存在时间冲突");

        var seatOccupied = await _db.Reservations
            .AnyAsync(r => r.SeatId == seatId && r.Date == date && r.Status == "已预约"
                && startTime < r.EndTime && endTime > r.StartTime);
        if (seatOccupied)
            return (false, "该座位在此时间段已被预约");

        var reservation = new Reservation
        {
            UserId = userId,
            SeatId = seatId,
            Date = date,
            StartTime = startTime,
            EndTime = endTime,
            Status = "已预约",
            CreatedAt = DateTime.Now
        };

        _db.Reservations.Add(reservation);
        await _db.SaveChangesAsync();
        return (true, "预约成功");
    }

    public async Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId)
    {
        var reservation = await _db.Reservations
            .Include(r => r.Seat)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation == null)
            return (false, "预约记录不存在");

        if (reservation.UserId != userId)
            return (false, "只能取消自己的预约");

        if (reservation.Status != "已预约")
            return (false, "该预约已取消");

        var now = TimeOnly.FromDateTime(DateTime.Now);
        if (now >= reservation.StartTime)
            return (false, "预约已经开始或已过期，无法取消");

        reservation.Status = "已取消";
        await _db.SaveChangesAsync();
        return (true, "取消成功");
    }
}
