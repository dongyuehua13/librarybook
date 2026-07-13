using LibrarySeatReservation.Web.Models;

namespace LibrarySeatReservation.Web.ViewModels;

public class HomeIndexViewModel
{
    public int TotalActiveSeats { get; set; }
    public int TodayReservations { get; set; }
    public Dictionary<string, int> AreaDistribution { get; set; } = new();
    public string? CurrentUserName { get; set; }
    public int? CurrentUserId { get; set; }
    public bool IsLoggedIn => CurrentUserId != null;
    public List<User> Students { get; set; } = new();
}
