using Microsoft.EntityFrameworkCore;
using LibrarySeatSystem.Data;
using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;

    public UserService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> ValidateAdminAsync(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u =>
            u.Username == username && u.IsAdmin);
    }

    public async Task<List<User>> GetStudentsAsync()
    {
        return await _db.Users.Where(u => !u.IsAdmin).ToListAsync();
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        return await _db.Users.FindAsync(id);
    }
}
