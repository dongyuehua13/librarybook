using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.Services.Interfaces;

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

    public async Task<List<Seat>> GetByFloorAsync(int floor)
    {
        return await _db.Seats.Where(s => s.Floor == floor).OrderBy(s => s.SeatNumber).ToListAsync();
    }

    public async Task<Seat?> GetByIdAsync(int id)
    {
        return await _db.Seats.FindAsync(id);
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
}
