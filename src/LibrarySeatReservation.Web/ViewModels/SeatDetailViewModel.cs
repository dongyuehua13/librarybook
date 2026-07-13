namespace LibrarySeatReservation.Web.ViewModels;

public class SeatDetailViewModel
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Area { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public bool IsCurrentUserLoggedIn { get; set; }
    public List<ReservationBrief> ReservationRecords { get; set; } = new();
}

public class ReservationBrief
{
    public DateOnly Date { get; set; }
    public TimeOnly StartTime { get; set; }
    public TimeOnly EndTime { get; set; }
    public string Status { get; set; } = string.Empty;
}
