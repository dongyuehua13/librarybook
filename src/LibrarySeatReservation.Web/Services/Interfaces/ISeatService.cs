using LibrarySeatReservation.Web.Models;

namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface ISeatService
{
    Task<List<Seat>> GetAllAsync();
    Task<List<Seat>> GetByFloorAsync(int floor);
    Task<Seat?> GetByIdAsync(int id);
    Task ToggleActiveAsync(int id);
    Task<Seat> CreateAsync(Seat seat);
}
