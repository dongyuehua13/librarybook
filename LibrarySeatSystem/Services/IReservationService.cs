using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface IReservationService
{
    Task<(bool Success, string Message)> CreateAsync(int userId, int seatId,
        DateTime date, TimeSpan startTime, TimeSpan endTime);
    Task<MyReservationsViewModel> GetUserReservationsAsync(int userId);
    Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId);
    Task<AdminReservationsViewModel> GetAllReservationsAsync(
        string? status, DateTime? dateFrom, DateTime? dateTo);
}
