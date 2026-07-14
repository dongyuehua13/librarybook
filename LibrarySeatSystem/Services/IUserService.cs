using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.Services;

public interface IUserService
{
    /// <summary>验证管理员账号是否存在且为管理员角色</summary>
    /// <param name="username">用户名</param>
    /// <returns>用户实体（null=不存在或非管理员）</returns>
    Task<User?> ValidateAdminAsync(string username);
    /// <summary>获取所有非管理员用户（学生列表）</summary>
    Task<List<User>> GetStudentsAsync();
    /// <summary>根据用户 ID 获取用户</summary>
    Task<User?> GetByIdAsync(int id);
}
