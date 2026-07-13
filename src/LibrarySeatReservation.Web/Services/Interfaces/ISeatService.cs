using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.ViewModels;

namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface ISeatService
{
    Task<List<Seat>> GetAllAsync();
    Task<List<Seat>> GetAllSeatsAsync();
    Task<List<Seat>> GetSeatsByFloorAsync(int floor);
    Task<Seat?> GetByIdAsync(int id);
    Task<Seat?> GetSeatByIdAsync(int id);
    Task ToggleActiveAsync(int id);
    Task<Seat> CreateAsync(Seat seat);

    Task<List<SeatWithStatus>> GetSeatsWithStatusAsync(int? floor, string? area);
    Task<SeatDetailViewModel?> GetSeatDetailAsync(int id);
}
