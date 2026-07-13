using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.Services.Interfaces;
using LibrarySeatReservation.Web.ViewModels;

namespace LibrarySeatReservation.Web.Services;

public class SeatService : ISeatService
{
    private readonly AppDbContext _db;

    public SeatService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Seat>> GetAllAsync()
    {
        return await _db.Seats.OrderBy(s => s.Floor).ThenBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<List<Seat>> GetAllSeatsAsync()
    {
        return await _db.Seats.OrderBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<List<Seat>> GetSeatsByFloorAsync(int floor)
    {
        return await _db.Seats.Where(s => s.Floor == floor).OrderBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await _db.Seats.FindAsync(id);
    }

    public async Task<Seat?> GetSeatByIdAsync(int id)
    {
        return await _db.Seats.FirstOrDefaultAsync(s => s.Id == id);
    }

    public async Task ToggleActiveAsync(int id)
    {
        var seat = await _db.Seats.FindAsync(id);
        if (seat != null)
        {
            seat.IsActive = !seat.IsActive;
            await _db.SaveChangesAsync();
        }
    }

    public async Task<Seat> CreateAsync(Seat seat)
    {
        _db.Seats.Add(seat);
        await _db.SaveChangesAsync();
        return seat;
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

    public async Task<List<SeatWithStatus>> GetSeatsWithStatusAsync(int? floor, string? area)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);
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

        var today = DateOnly.FromDateTime(DateTime.Today);
        var records = await _db.Reservations
            .Where(r => r.SeatId == id && r.Date >= today)
            .OrderBy(r => r.Date)
            .ThenBy(r => r.StartTime)
            .ToListAsync();

        return new SeatDetailViewModel
        {
            Id = seat.Id,
            SeatNumber = seat.SeatNumber,
            Floor = seat.Floor,
            Area = seat.Area,
            Description = seat.Description,
            IsActive = seat.IsActive,
            ReservationRecords = records.Select(r => new ReservationBrief
            {
                Date = r.Date,
                StartTime = r.StartTime,
                EndTime = r.EndTime,
                Status = r.Status
            }).ToList()
        };
    }
}
