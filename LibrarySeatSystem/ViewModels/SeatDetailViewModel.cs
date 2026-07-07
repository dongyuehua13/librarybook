using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.ViewModels;

public class SeatDetailViewModel
{
    public Seat Seat { get; set; } = null!;
    public List<Reservation> ReservationRecords { get; set; } = new();
    public bool IsCurrentUserLoggedIn { get; set; }
}
