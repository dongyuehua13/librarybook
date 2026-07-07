using LibrarySeatReservation.Web.Models;

namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface IReservationService
{
    Task<List<Reservation>> GetUserReservationsAsync(int userId);
    Task<List<Reservation>> GetAllAsync();
    Task<List<Reservation>> GetBySeatAsync(int seatId);
    Task<(bool Success, string Message)> CreateAsync(int userId, int seatId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId);
}
