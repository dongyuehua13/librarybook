namespace LibrarySeatSystem.ViewModels;

public class AdminStats
{
    public int TotalActiveSeats { get; set; }
    public int TotalReservations { get; set; }
    public int TodayReservations { get; set; }
    public List<SeatRanking> SeatRankings { get; set; } = new();
    public List<DailyTrend> DailyTrends { get; set; } = new();
}

public class SeatRanking
{
    public string SeatNumber { get; set; } = string.Empty;
    public int Count { get; set; }
}

public class DailyTrend
{
    public DateTime Date { get; set; }
    public int Count { get; set; }
}

public class DashboardStats
{
    public int TotalActiveSeats { get; set; }
    public int TodayReservations { get; set; }
}
