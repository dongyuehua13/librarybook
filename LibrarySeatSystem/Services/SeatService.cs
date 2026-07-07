using Microsoft.EntityFrameworkCore;
using LibrarySeatSystem.Data;
using LibrarySeatSystem.Models;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public class SeatService : ISeatService
{
    private readonly AppDbContext _db;

    public SeatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<SeatWithStatus>> GetSeatsWithStatusAsync(int? floor, string? area)
    {
        var today = DateTime.Today;

        var reservedSeatIds = await _db.Reservations
            .Where(r => r.Date == today && r.Status == "已预约")
            .Select(r => r.SeatId)
            .Distinct()
            .ToListAsync();

        var query = _db.Seats.Where(s => s.IsActive);
        if (floor.HasValue)
            query = query.Where(s => s.Floor == floor.Value);
        if (!string.IsNullOrEmpty(area))
            query = query.Where(s => s.Area == area);

        var seats = await query.OrderBy(s => s.SeatNumber).ToListAsync();

        return seats.Select(s => new SeatWithStatus
        {
            Id = s.Id,
            SeatNumber = s.SeatNumber,
            Floor = s.Floor,
            Area = s.Area,
            Description = s.Description,
            IsOccupied = reservedSeatIds.Contains(s.Id)
        }).ToList();
    }

    public async Task<SeatDetailViewModel?> GetSeatDetailAsync(int id)
    {
        var seat = await _db.Seats.FirstOrDefaultAsync(s => s.Id == id);
        if (seat == null) return null;

        var today = DateTime.Today;
        var records = await _db.Reservations
            .Where(r => r.SeatId == id && r.Date >= today)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.StartTime)
            .ToListAsync();

        return new SeatDetailViewModel
        {
            Seat = seat,
            ReservationRecords = records
        };
    }

    public async Task<Seat?> GetSeatByIdAsync(int id)
    {
        return await _db.Seats.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task<List<Seat>> GetAllSeatsAsync()
    {
        return await _db.Seats.OrderBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<(bool Success, string Message)> CreateSeatAsync(Seat seat)
    {
        var exists = await _db.Seats.AnyAsync(s => s.SeatNumber == seat.SeatNumber);
        if (exists)
            return (false, "座位编号已存在");

        _db.Seats.Add(seat);
        await _db.SaveChangesAsync();
        return (true, "新增成功");
    }

    public async Task<(bool Success, string Message)> UpdateSeatAsync(Seat seat)
    {
        var existing = await _db.Seats.FindAsync(seat.Id);
        if (existing == null)
            return (false, "座位不存在");

        var duplicate = await _db.Seats.AnyAsync(s =>
            s.SeatNumber == seat.SeatNumber && s.Id != seat.Id);
        if (duplicate)
            return (false, "座位编号已被其他座位使用");

        existing.SeatNumber = seat.SeatNumber;
        existing.Floor = seat.Floor;
        existing.Area = seat.Area;
        existing.Description = seat.Description;

        await _db.SaveChangesAsync();
        return (true, "更新成功");
    }

    public async Task SaveChangesAsync()
    {
        await _db.SaveChangesAsync();
    }
}
