using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.ViewModels;

namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface IReservationService
{
    Task<MyReservationsViewModel> GetUserReservationsAsync(int userId);
    Task<List<Reservation>> GetAllAsync();
    Task<List<Reservation>> GetBySeatAsync(int seatId);
    Task<(bool Success, string Message)> CreateAsync(int userId, int seatId, DateOnly date, TimeOnly startTime, TimeOnly endTime);
    Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId);
    Task<AdminReservationsViewModel> GetAllReservationsAsync(string? status, DateOnly? dateFrom, DateOnly? dateTo);
}
