using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.Services;

public interface IUserService
{
    Task<User?> ValidateAdminAsync(string username);
    Task<List<User>> GetStudentsAsync();
    Task<User?> GetByIdAsync(int id);
}
