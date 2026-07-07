using LibrarySeatSystem.Models;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface ISeatService
{
    Task<List<SeatWithStatus>> GetSeatsWithStatusAsync(int? floor, string? area);
    Task<SeatDetailViewModel?> GetSeatDetailAsync(int id);
    Task<Seat?> GetSeatByIdAsync(int id);
    Task<List<Seat>> GetAllSeatsAsync();
    Task<(bool Success, string Message)> CreateSeatAsync(Seat seat);
    Task<(bool Success, string Message)> UpdateSeatAsync(Seat seat);
    Task SaveChangesAsync();
}
