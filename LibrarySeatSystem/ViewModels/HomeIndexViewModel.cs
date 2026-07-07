namespace LibrarySeatSystem.ViewModels;

public class HomeIndexViewModel
{
    public int TotalActiveSeats { get; set; }
    public int TodayReservations { get; set; }
    public string? CurrentUserName { get; set; }
    public bool IsLoggedIn { get; set; }
}
