using Microsoft.EntityFrameworkCore;
using LibrarySeatReservation.Web.Data;
using LibrarySeatReservation.Web.Models;
using LibrarySeatReservation.Web.Services.Interfaces;

namespace LibrarySeatReservation.Web.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<User>> GetStudentsAsync()
    {
        return await _db.Users.Where(u => !u.IsAdmin).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task<bool> ValidateAdminAsync(string username)
    {
        return await _db.Users.AnyAsync(u => u.Username == username && u.IsAdmin);
    }
}
