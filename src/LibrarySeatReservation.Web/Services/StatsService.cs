using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Services.Interfaces;

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
}
