using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Services.Interfaces;
using LibrarySeatReservation.Web.ViewModels;

namespace LibrarySeatReservation.Web.Services;

public class StatsService : IStatsService
{
    private readonly AppDbContext _db;

    public StatsService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int> GetTodayReservationCountAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        return await _db.Reservations.CountAsync(r => r.Date == today && r.Status == "已预约");
    }

    public async Task<int> GetActiveSeatCountAsync()
    {
        return await _db.Seats.CountAsync(s => s.IsActive);
    }

    public async Task<Dictionary<string, int>> GetAreaDistributionAsync()
    {
        return await _db.Seats
            .GroupBy(s => s.Area)
            .Select(g => new { Area = g.Key, Count = g.Count() })
            .ToDictionaryAsync(x => x.Area, x => x.Count);
    }

    public async Task<DashboardStats> GetDashboardStatsAsync()
    {
        return new DashboardStats
        {
            TotalActiveSeats = await GetActiveSeatCountAsync(),
            TodayReservations = await GetTodayReservationCountAsync(),
            AreaDistribution = await GetAreaDistributionAsync()
        };
    }

    public async Task<AdminStats> GetAllStatsAsync()
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
        var fourteenDaysAgo = today.AddDays(-13);

        var totalActiveSeats = await _db.Seats.CountAsync(s => s.IsActive);
        var totalReservations = await _db.Reservations.CountAsync();
        var todayReservations = await _db.Reservations.CountAsync(r => r.Date == today);

        var seatRankings = await _db.Reservations
            .Where(r => r.Status == "已预约")
            .GroupBy(r => r.SeatId)
            .Select(g => new { SeatId = g.Key, Count = g.Count() })
            .OrderByDescending(x => x.Count)
            .Take(10)
            .ToListAsync();

        var seatIds = seatRankings.Select(x => x.SeatId).ToList();
        var seats = await _db.Seats.Where(s => seatIds.Contains(s.Id)).ToDictionaryAsync(s => s.Id, s => s.SeatNumber);

        var rankingViewModels = seatRankings.Select(x => new SeatRanking
        {
            SeatNumber = seats.GetValueOrDefault(x.SeatId, "未知"),
            Count = x.Count
        }).ToList();

        var dailyTrends = await _db.Reservations
            .Where(r => r.Date >= fourteenDaysAgo && r.Date <= today)
            .GroupBy(r => r.Date)
            .Select(g => new DailyTrend
            {
                Date = g.Key,
                Count = g.Count()
            })
            .OrderBy(t => t.Date)
            .ToListAsync();

        var maxCount = rankingViewModels.Any() ? rankingViewModels.Max(r => r.Count) : 0;
        var trendMax = dailyTrends.Any() ? dailyTrends.Max(t => t.Count) : 0;

        foreach (var r in rankingViewModels)
            r.Percentage = maxCount > 0 ? (int)(r.Count * 100.0 / maxCount) : 0;
        foreach (var t in dailyTrends)
            t.Percentage = trendMax > 0 ? (int)(t.Count * 100.0 / trendMax) : 0;

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
