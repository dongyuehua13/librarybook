using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface IStatsService
{
    Task<DashboardStats> GetDashboardStatsAsync();
    Task<AdminStats> GetAllStatsAsync();
}
