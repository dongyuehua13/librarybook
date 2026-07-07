namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface IStatsService
{
    Task<int> GetTodayReservationCountAsync();
    Task<int> GetActiveSeatCountAsync();
    Task<Dictionary<string, int>> GetAreaDistributionAsync();
}
