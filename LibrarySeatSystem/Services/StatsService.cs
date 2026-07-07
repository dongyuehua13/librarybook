using Microsoft.EntityFrameworkCore;
using LibrarySeatSystem.Data;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public class StatsService : IStatsService
{
    private readonly AppDbContext _db;

    public StatsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        var today = DateTime.Today;
        return new DashboardStats
        {
            TotalActiveSeats = await _db.Seats.CountAsync(s => s.IsActive),
            TodayReservations = await _db.Reservations
                .CountAsync(r => r.Date == today && r.Status == "已预约")
        };
    }

    public async Task<AdminStats> GetAllStatsAsync()
    {
        var today = DateTime.Today;
        var fourteenDaysAgo = today.AddDays(-13);

        var totalActiveSeats = await _db.Seats.CountAsync(s => s.IsActive);
        var totalReservations = await _db.Reservations.CountAsync();
        var todayReservations = await _db.Reservations
            .CountAsync(r => r.Date == today);

        var seatRankings = await _db.Reservations
            .Where(r => r.Status == "已预约")
            .GroupBy(r => r.SeatId)
            .Select(g => new { SeatId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();

        var rankingViewModels = seatRankings.Select(x => new SeatRanking
        {
            SeatNumber = _db.Seats.Find(x.SeatId)?.SeatNumber ?? "未知",
            Count = x.Count
        }).ToList();

        var dailyTrends = await _db.Reservations
            .Where(r => r.Date >= fourteenDaysAgo && r.Date <= today)
            .GroupBy(r => r.Date)
            .Select(g => new DailyTrend { Date = g.Key, Count = g.Count() })
            .OrderBy(t => t.Date)
            .ToListAsync();

        return new AdminStats
        {
            TotalActiveSeats = totalActiveSeats,
            TotalReservations = totalReservations,
            TodayReservations = todayReservations,
            SeatRankings = rankingViewModels,
            DailyTrends = dailyTrends
        };
    }
}
