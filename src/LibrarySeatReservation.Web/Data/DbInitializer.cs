using LibrarySeatReservation.Web.Models;

namespace LibrarySeatReservation.Web.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any()) return;

        var admin = new User
        {
            Username = "admin",
            DisplayName = "系统管理员",
            IsAdmin = true
        };
        var student1 = new User
        {
            Username = "zhangsan",
            DisplayName = "张三",
            IsAdmin = false
        };
        var student2 = new User
        {
            Username = "lisi",
            DisplayName = "李四",
            IsAdmin = false
        };
        var student3 = new User
        {
            Username = "wangwu",
            DisplayName = "王五",
            IsAdmin = false
        };
        db.Users.AddRange(admin, student1, student2, student3);

        var seats = new List<Seat>();
        var areas = new[] { "自习区", "安静区", "电子阅览区" };
        int seatCounter = 1;
        for (int floor = 1; floor <= 3; floor++)
        {
            foreach (var area in areas)
            {
                for (int i = 1; i <= 5; i++)
                {
                    seats.Add(new Seat
                    {
                        SeatNumber = $"{(char)('A' + seatCounter - 1)}-{i:D2}",
                        Floor = floor,
                        Area = area,
                        Description = (floor, area) switch
                        {
                            (1, "自习区") => "靠近门口",
                            (2, "安静区") => "靠窗有插座",
                            (3, "电子阅览区") => "配备电脑",
                            _ => null
                        },
                        IsActive = true
                    });
                }
                seatCounter++;
            }
        }
        db.Seats.AddRange(seats);

        db.SaveChanges();
    }
}
