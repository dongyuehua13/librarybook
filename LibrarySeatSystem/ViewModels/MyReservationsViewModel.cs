namespace LibrarySeatSystem.ViewModels;

public class MyReservationsViewModel
{
    public List<ReservationItem> Items { get; set; } = new();
    public bool HasReservations => Items.Any();
}

public class ReservationItem
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool CanCancel { get; set; }
}
