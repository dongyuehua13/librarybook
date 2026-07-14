using LibrarySeatSystem.Models;
using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface ISeatService
{
    /// <summary>获取座位列表（含今日占用状态），支持楼层和区域筛选</summary>
    /// <param name="floor">楼层筛选（null=全部）</param>
    /// <param name="area">区域筛选（null=全部）</param>
    /// <returns>座位视图模型列表，每项含 IsOccupied 标志</returns>
    Task<List<SeatWithStatus>> GetSeatsWithStatusAsync(int? floor, string? area);
    /// <summary>获取座位详情（含未来预约记录）</summary>
    /// <param name="id">座位 ID</param>
    Task<SeatDetailViewModel?> GetSeatDetailAsync(int id);
    /// <summary>根据 ID 获取座位实体</summary>
    Task<Seat?> GetSeatByIdAsync(int id);
    /// <summary>获取全部座位（含禁用），用于管理端</summary>
    Task<List<Seat>> GetAllSeatsAsync();
    /// <summary>创建新座位（含编号查重）</summary>
    /// <param name="seat">座位实体</param>
    /// <returns>(成功, 消息)</returns>
    Task<(bool Success, string Message)> CreateSeatAsync(Seat seat);
    /// <summary>更新座位信息（含排除自身编号查重）</summary>
    Task<(bool Success, string Message)> UpdateSeatAsync(Seat seat);
    /// <summary>切换座位启用/禁用状态</summary>
    /// <param name="id">座位 ID</param>
    /// <returns>(成功, 消息, 新状态)</returns>
    Task<(bool Success, string Message, bool IsActive)> ToggleActiveAsync(int id);
    /// <summary>保存 EF Core 变更</summary>
    Task SaveChangesAsync();
}
