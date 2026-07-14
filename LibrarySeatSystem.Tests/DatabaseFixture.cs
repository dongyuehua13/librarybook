using Microsoft.EntityFrameworkCore;
using LibrarySeatSystem.Data;
using LibrarySeatSystem.Services;

namespace LibrarySeatSystem.Tests;

public class DatabaseFixture
{
    private const string DbName = "LibrarySeatSystem_Tests";

    public string ConnectionString =>
        $"Server=(localdb)\\MSSQLLocalDB;Database={DbName};Trusted_Connection=True;TrustServerCertificate=True;";

    /// <summary>
    /// Drops, recreates, and seeds the test database.
    /// Not thread-safe — caller must ensure sequential access.
    /// </summary>
    public async Task ResetAsync()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;

        // Use a dedicated connection for dropping to avoid "in use" errors
        await DeleteDatabaseAsync(options);
        await CreateDatabaseAsync(options);
    }

    private static async Task DeleteDatabaseAsync(DbContextOptions<AppDbContext> options)
    {
        using var db = new AppDbContext(options);
        await db.Database.EnsureDeletedAsync();
    }

    private static async Task CreateDatabaseAsync(DbContextOptions<AppDbContext> options)
    {
        using var db = new AppDbContext(options);
        await db.Database.EnsureCreatedAsync();
        DbInitializer.Seed(db);
    }

    public AppDbContext CreateDbContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(ConnectionString)
            .Options;
        return new AppDbContext(options);
    }

    public IReservationService CreateReservationService() =>
        new ReservationService(CreateDbContext());

    public ISeatService CreateSeatService() =>
        new SeatService(CreateDbContext());

    public IUserService CreateUserService() =>
        new UserService(CreateDbContext());

    public IStatsService CreateStatsService() =>
        new StatsService(CreateDbContext());
}
