namespace LibrarySeatReservation.Web.ViewModels;

public class MyReservationsViewModel
{
    public List<ReservationItem> Items { get; set; } = new();
}

public class ReservationItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool CanCancel { get; set; }
}
