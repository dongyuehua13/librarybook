using LibrarySeatSystem.Models;
using LibrarySeatSystem.Services;

namespace LibrarySeatSystem.Tests;

public class AbnormalScenarioTests : IAsyncLifetime
{
    private readonly DatabaseFixture _fixture = new();

    public async Task InitializeAsync()
    {
        await _fixture.ResetAsync();
    }

    public Task DisposeAsync()
    {
        return Task.CompletedTask;
    }

    private const int StudentZhangSanId = 2;
    private const int StudentLiSiId = 3;
    private const int SeatA01Id = 1;
    private const int SeatA02Id = 2;

    [Fact]
    public async Task User_DailyLimit_Exceeded_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);

        await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        await svc.CreateAsync(StudentZhangSanId, SeatA02Id,
            date, new TimeSpan(13, 0, 0), new TimeSpan(15, 0, 0));

        var third = await svc.CreateAsync(StudentZhangSanId, 3,
            date, new TimeSpan(16, 0, 0), new TimeSpan(18, 0, 0));
        Assert.False(third.Success);
        Assert.Contains("上限", third.Message);
    }

    [Fact]
    public async Task User_SameUserTimeConflict_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);

        await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));

        var result = await svc.CreateAsync(StudentZhangSanId, SeatA02Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.False(result.Success);
        Assert.Contains("已预约其他座位", result.Message);
    }

    [Fact]
    public async Task User_SeatTimeConflict_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);

        await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));

        var result = await svc.CreateAsync(StudentLiSiId, SeatA01Id,
            date, new TimeSpan(10, 0, 0), new TimeSpan(12, 0, 0));
        Assert.False(result.Success);
        Assert.Contains("已被预约", result.Message);
    }

    [Fact]
    public async Task User_DisabledSeat_Fails()
    {
        var seatSvc = _fixture.CreateSeatService();
        var resvSvc = _fixture.CreateReservationService();

        await seatSvc.ToggleActiveAsync(SeatA01Id);

        var result = await resvSvc.CreateAsync(StudentZhangSanId, SeatA01Id,
            DateTime.Today.AddDays(7),
            new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.False(result.Success);
        Assert.Contains("不可用", result.Message);
    }

    [Fact]
    public async Task User_SeatNotFound_ReturnsNull()
    {
        var seatSvc = _fixture.CreateSeatService();

        Assert.Null(await seatSvc.GetSeatDetailAsync(9999));
        Assert.Null(await seatSvc.GetSeatByIdAsync(9999));
    }

    [Fact]
    public async Task User_CancelOtherUser_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);

        var created = await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.True(created.Success, created.Message);

        var reservations = await svc.GetUserReservationsAsync(StudentZhangSanId);
        var resvId = reservations.Items.First().Id;

        var result = await svc.CancelAsync(resvId, StudentLiSiId);
        Assert.False(result.Success);
        Assert.Contains("只能取消自己的", result.Message);
    }

    [Fact]
    public async Task User_CancelNonReservedStatus_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var db = _fixture.CreateDbContext();
        var date = DateTime.Today.AddDays(7);

        db.Reservations.Add(new Reservation
        {
            UserId = StudentZhangSanId,
            SeatId = SeatA01Id,
            Date = date,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(11, 0, 0),
            Status = "已取消",
            CreatedAt = DateTime.Now
        });
        await db.SaveChangesAsync();

        var cancelledId = db.Reservations
            .Where(r => r.UserId == StudentZhangSanId && r.Status == "已取消")
            .OrderByDescending(r => r.Id)
            .Select(r => r.Id)
            .First();

        var result = await svc.CancelAsync(cancelledId, StudentZhangSanId);
        Assert.False(result.Success);
        Assert.Contains("只有已预约", result.Message);
    }

    [Fact]
    public async Task User_CancelExpired_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var db = _fixture.CreateDbContext();
        var pastDate = DateTime.Today.AddDays(-1);

        db.Reservations.Add(new Reservation
        {
            UserId = StudentZhangSanId,
            SeatId = SeatA01Id,
            Date = pastDate,
            StartTime = new TimeSpan(9, 0, 0),
            EndTime = new TimeSpan(11, 0, 0),
            Status = "已预约",
            CreatedAt = DateTime.Now.AddDays(-2)
        });
        await db.SaveChangesAsync();

        var expiredId = db.Reservations
            .Where(r => r.UserId == StudentZhangSanId && r.Date == pastDate)
            .OrderByDescending(r => r.Id)
            .Select(r => r.Id)
            .First();

        var result = await svc.CancelAsync(expiredId, StudentZhangSanId);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task User_CancelStarted_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var db = _fixture.CreateDbContext();

        db.Reservations.Add(new Reservation
        {
            UserId = StudentZhangSanId,
            SeatId = SeatA01Id,
            Date = DateTime.Today,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(0, 30, 0),
            Status = "已预约",
            CreatedAt = DateTime.Now
        });
        await db.SaveChangesAsync();

        var startedId = db.Reservations
            .Where(r => r.UserId == StudentZhangSanId && r.Date == DateTime.Today)
            .OrderByDescending(r => r.Id)
            .Select(r => r.Id)
            .First();

        var result = await svc.CancelAsync(startedId, StudentZhangSanId);
        Assert.False(result.Success);
        Assert.Contains("已开始", result.Message);
    }

    [Fact]
    public async Task User_ReservationNotFound_OnCancel()
    {
        var svc = _fixture.CreateReservationService();
        var result = await svc.CancelAsync(9999, StudentZhangSanId);
        Assert.False(result.Success);
        Assert.Contains("不存在", result.Message);
    }

    [Fact]
    public async Task Admin_ValidateAdmin_NonExistentUser_ReturnsNull()
    {
        var userSvc = _fixture.CreateUserService();
        Assert.Null(await userSvc.ValidateAdminAsync("nonexistent"));
    }

    [Fact]
    public async Task Admin_ValidateAdmin_NonAdminUser_ReturnsNull()
    {
        var userSvc = _fixture.CreateUserService();
        Assert.Null(await userSvc.ValidateAdminAsync("zhangsan"));
    }

    [Fact]
    public async Task Admin_ValidateAdmin_ValidAdmin_ReturnsUser()
    {
        var userSvc = _fixture.CreateUserService();
        var result = await userSvc.ValidateAdminAsync("admin");
        Assert.NotNull(result);
        Assert.True(result.IsAdmin);
    }

    [Fact]
    public async Task Admin_CreateSeat_DuplicateNumber_Fails()
    {
        var seatSvc = _fixture.CreateSeatService();

        var result = await seatSvc.CreateSeatAsync(new Seat
        {
            SeatNumber = "A-01",
            Floor = 1,
            Area = "自习区"
        });
        Assert.False(result.Success);
        Assert.Contains("已存在", result.Message);
    }

    [Fact]
    public async Task Admin_UpdateSeat_DuplicateNumber_Fails()
    {
        var seatSvc = _fixture.CreateSeatService();

        var a02 = await seatSvc.GetSeatByIdAsync(2);
        Assert.NotNull(a02);

        a02.SeatNumber = "A-01";
        var result = await seatSvc.UpdateSeatAsync(a02);
        Assert.False(result.Success);
        Assert.Contains("已被其他座位使用", result.Message);
    }

    [Fact]
    public async Task Admin_UpdateSeat_NotFound_Fails()
    {
        var seatSvc = _fixture.CreateSeatService();

        var result = await seatSvc.UpdateSeatAsync(new Seat
        {
            Id = 9999,
            SeatNumber = "X-01",
            Floor = 1,
            Area = "自习区"
        });
        Assert.False(result.Success);
        Assert.Contains("不存在", result.Message);
    }

    [Fact]
    public async Task Admin_ToggleSeat_NotFound_Fails()
    {
        var seatSvc = _fixture.CreateSeatService();

        var result = await seatSvc.ToggleActiveAsync(9999);
        Assert.False(result.Success);
        Assert.Contains("不存在", result.Message);
    }

    [Fact]
    public async Task Admin_Stats_EmptyReservations()
    {
        var statsSvc = _fixture.CreateStatsService();

        var stats = await statsSvc.GetAllStatsAsync();
        Assert.Equal(15, stats.TotalActiveSeats);
        Assert.Equal(0, stats.TotalReservations);
        Assert.Equal(0, stats.TodayReservations);
        Assert.Empty(stats.SeatRankings);
        Assert.Empty(stats.DailyTrends);
    }

    [Fact]
    public async Task Admin_Reservations_NoMatch_ReturnsEmpty()
    {
        var resvSvc = _fixture.CreateReservationService();

        var result = await resvSvc.GetAllReservationsAsync("已预约", null, null);
        Assert.Empty(result.Items);
    }

    [Fact]
    public async Task Admin_GetStudents_ReturnsOnlyNonAdmin()
    {
        var userSvc = _fixture.CreateUserService();

        var students = await userSvc.GetStudentsAsync();
        Assert.Equal(3, students.Count);
        Assert.All(students, s => Assert.False(s.IsAdmin));
    }
}
