namespace LibrarySeatReservation.Web.ViewModels;

public class SeatWithStatus
{
    public int Id { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public int Floor { get; set; }
    public string Area { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsOccupied { get; set; }
}
