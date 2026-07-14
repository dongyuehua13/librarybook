using LibrarySeatSystem.ViewModels;

namespace LibrarySeatSystem.Services;

public interface IReservationService
{
    /// <summary>创建预约（含冲突检测）</summary>
    /// <param name="userId">用户 ID</param>
    /// <param name="seatId">座位 ID</param>
    /// <param name="date">预约日期</param>
    /// <param name="startTime">开始时间</param>
    /// <param name="endTime">结束时间</param>
    /// <returns>(成功, 消息)</returns>
    Task<(bool Success, string Message)> CreateAsync(int userId, int seatId,
        DateTime date, TimeSpan startTime, TimeSpan endTime);
    /// <summary>获取指定用户的预约列表（含 CanCancel 标志）</summary>
    Task<MyReservationsViewModel> GetUserReservationsAsync(int userId);
    /// <summary>取消预约（含权限校验 + 时效校验）</summary>
    /// <param name="reservationId">预约记录 ID</param>
    /// <param name="userId">请求取消的用户 ID</param>
    /// <returns>(成功, 消息)</returns>
    Task<(bool Success, string Message)> CancelAsync(int reservationId, int userId);
    /// <summary>获取全部预约（管理端，支持状态和日期筛选）</summary>
    Task<AdminReservationsViewModel> GetAllReservationsAsync(
        string? status, DateTime? dateFrom, DateTime? dateTo);
}
