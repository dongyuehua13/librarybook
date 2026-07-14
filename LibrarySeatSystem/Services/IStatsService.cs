using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface IStatsService
{
    /// <summary>获取首页统计卡片数据（总座位/可用座位/今日预约/区域分布）</summary>
    Task<DashboardStats> GetDashboardStatsAsync();
    /// <summary>获取管理端统计页全量数据（卡片 + 排行 + 趋势）</summary>
    Task<AdminStats> GetAllStatsAsync();
}
