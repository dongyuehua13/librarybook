namespace LibrarySeatReservation.Web.ViewModels;

public class DashboardStats
{
    public int TotalActiveSeats { get; set; }
    public int TodayReservations { get; set; }
    public Dictionary<string, int> AreaDistribution { get; set; } = new();
}
