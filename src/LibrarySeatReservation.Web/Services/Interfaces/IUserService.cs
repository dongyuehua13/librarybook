using LibrarySeatReservation.Web.Models;

namespace LibrarySeatReservation.Web.Services.Interfaces;

public interface IUserService
{
    Task<List<User>> GetStudentsAsync();
    Task<User?> GetByIdAsync(int id);
    Task<User?> GetByUsernameAsync(string username);
    Task<bool> ValidateAdminAsync(string username);
}
