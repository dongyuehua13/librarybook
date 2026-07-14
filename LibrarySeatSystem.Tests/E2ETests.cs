using LibrarySeatSystem.Services;
using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.Tests;

public class E2ETests : IAsyncLifetime
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
    private const int SeatA01Id = 1;
    private const int SeatA02Id = 2;

    [Fact]
    public async Task T01_ConflictDetection_SameSlot_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(11, 0, 0);

        var first = await svc.CreateAsync(StudentZhangSanId, SeatA01Id, date, start, end);
        Assert.True(first.Success, first.Message);

        var second = await svc.CreateAsync(StudentZhangSanId, SeatA01Id, date, start, end);
        Assert.False(second.Success);
        Assert.Contains("已被预约", second.Message);
    }

    [Fact]
    public async Task T02_CrossDate_NoConflict_BothSucceed()
    {
        var svc = _fixture.CreateReservationService();

        var first = await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            DateTime.Today.AddDays(7), new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.True(first.Success, first.Message);

        var second = await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            DateTime.Today.AddDays(8), new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.True(second.Success, second.Message);
    }

    [Fact]
    public async Task T03_AdjacentSlots_NoConflict_BothSucceed()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);

        var first = await svc.CreateAsync(StudentZhangSanId, SeatA01Id,
            date, new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.True(first.Success, first.Message);

        var second = await svc.CreateAsync(StudentZhangSanId, SeatA02Id,
            date, new TimeSpan(11, 0, 0), new TimeSpan(13, 0, 0));
        Assert.True(second.Success, second.Message);
    }

    [Fact]
    public async Task T04_CancelAndReBook_SameSlot_Succeeds()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(7);
        var start = new TimeSpan(9, 0, 0);
        var end = new TimeSpan(11, 0, 0);

        var created = await svc.CreateAsync(StudentZhangSanId, SeatA01Id, date, start, end);
        Assert.True(created.Success, created.Message);

        var myReservations = await svc.GetUserReservationsAsync(StudentZhangSanId);
        var reservation = myReservations.Items.First();
        Assert.True(reservation.CanCancel);

        var cancelled = await svc.CancelAsync(reservation.Id, StudentZhangSanId);
        Assert.True(cancelled.Success, cancelled.Message);

        var rebooked = await svc.CreateAsync(StudentZhangSanId, SeatA01Id, date, start, end);
        Assert.True(rebooked.Success, rebooked.Message);
    }

    [Fact]
    public async Task T05_CancelExpired_Fails()
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
            CreatedAt = DateTime.Now.AddDays(-1)
        });
        await db.SaveChangesAsync();
        var expiredId = db.Reservations
            .Where(r => r.UserId == StudentZhangSanId && r.Date == pastDate)
            .OrderByDescending(r => r.Id)
            .Select(r => r.Id)
            .First();

        var result = await svc.CancelAsync(expiredId, StudentZhangSanId);
        Assert.False(result.Success);
        Assert.Contains("过期", result.Message);
    }

    [Fact]
    public async Task T06_CancelStarted_Fails()
    {
        var svc = _fixture.CreateReservationService();
        var db = _fixture.CreateDbContext();

        db.Reservations.Add(new Reservation
        {
            UserId = StudentZhangSanId,
            SeatId = SeatA01Id,
            Date = DateTime.Today,
            StartTime = new TimeSpan(0, 0, 0),
            EndTime = new TimeSpan(1, 0, 0),
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
    public async Task T07_DisabledSeat_CannotBook()
    {
        var seatSvc = _fixture.CreateSeatService();
        var resvSvc = _fixture.CreateReservationService();

        var toggleResult = await seatSvc.ToggleActiveAsync(SeatA01Id);
        Assert.True(toggleResult.Success);

        var result = await resvSvc.CreateAsync(StudentZhangSanId, SeatA01Id,
            DateTime.Today.AddDays(7),
            new TimeSpan(9, 0, 0), new TimeSpan(11, 0, 0));
        Assert.False(result.Success);
        Assert.Contains("不可用", result.Message);
    }

    [Fact]
    public async Task T08_DisabledSeat_HiddenFromList()
    {
        var seatSvc = _fixture.CreateSeatService();

        await seatSvc.ToggleActiveAsync(SeatA01Id);

        var seats = await seatSvc.GetSeatsWithStatusAsync(null, null);
        Assert.DoesNotContain(seats, s => s.Id == SeatA01Id);
    }

    [Fact]
    public async Task T09_AllSeatsListIncludesAll()
    {
        var seatSvc = _fixture.CreateSeatService();
        var all = await seatSvc.GetAllSeatsAsync();
        Assert.Equal(15, all.Count);
    }

    [Fact]
    public async Task T10_ReservationPersists()
    {
        var svc = _fixture.CreateReservationService();
        var date = DateTime.Today.AddDays(30);
        var start = new TimeSpan(14, 0, 0);
        var end = new TimeSpan(16, 0, 0);

        var created = await svc.CreateAsync(StudentZhangSanId, SeatA01Id, date, start, end);
        Assert.True(created.Success, created.Message);

        var freshSvc = _fixture.CreateReservationService();
        var reservations = await freshSvc.GetUserReservationsAsync(StudentZhangSanId);
        Assert.Contains(reservations.Items, r =>
            r.SeatNumber == "A-01" &&
            r.Date == date &&
            r.TimeSlot == "14:00 - 16:00" &&
            r.Status == "已预约");
    }
}
