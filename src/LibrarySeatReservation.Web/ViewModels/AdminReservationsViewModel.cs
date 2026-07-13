namespace LibrarySeatReservation.Web.ViewModels;

public class AdminReservationsViewModel
{
    public List<AdminReservationItem> Items { get; set; } = new();
}

public class AdminReservationItem
{
    public int Id { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string SeatNumber { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}
