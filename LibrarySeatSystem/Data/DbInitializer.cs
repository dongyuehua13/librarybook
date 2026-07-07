using LibrarySeatSystem.Models;

namespace LibrarySeatSystem.Data;

public static class DbInitializer
{
    public static void Seed(AppDbContext db)
    {
        if (db.Users.Any()) return;

        db.Users.AddRange(
            new User { Username = "admin", DisplayName = "管理员", IsAdmin = true },
            new User { Username = "zhangsan", DisplayName = "张三", IsAdmin = false },
            new User { Username = "lisi", DisplayName = "李四", IsAdmin = false },
            new User { Username = "wangwu", DisplayName = "王五", IsAdmin = false }
        );

        db.Seats.AddRange(
            new Seat { SeatNumber = "A-01", Floor = 1, Area = "自习区", Description = "靠窗位置，有插座" },
            new Seat { SeatNumber = "A-02", Floor = 1, Area = "自习区", Description = "靠窗位置" },
            new Seat { SeatNumber = "A-03", Floor = 1, Area = "自习区", Description = "中间位置" },
            new Seat { SeatNumber = "A-04", Floor = 1, Area = "自习区", Description = "中间位置，有插座" },
            new Seat { SeatNumber = "A-05", Floor = 1, Area = "自习区", Description = "靠门位置" },
            new Seat { SeatNumber = "B-01", Floor = 2, Area = "安静区", Description = "独立隔间" },
            new Seat { SeatNumber = "B-02", Floor = 2, Area = "安静区", Description = "独立隔间" },
            new Seat { SeatNumber = "B-03", Floor = 2, Area = "安静区", Description = "双人桌" },
            new Seat { SeatNumber = "B-04", Floor = 2, Area = "安静区", Description = "双人桌，有台灯" },
            new Seat { SeatNumber = "B-05", Floor = 2, Area = "安静区", Description = "靠窗隔间" },
            new Seat { SeatNumber = "C-01", Floor = 3, Area = "电子阅览区", Description = "带电脑位" },
            new Seat { SeatNumber = "C-02", Floor = 3, Area = "电子阅览区", Description = "带电脑位" },
            new Seat { SeatNumber = "C-03", Floor = 3, Area = "电子阅览区", Description = "普通座位" },
            new Seat { SeatNumber = "C-04", Floor = 3, Area = "电子阅览区", Description = "普通座位" },
            new Seat { SeatNumber = "C-05", Floor = 3, Area = "电子阅览区", Description = "靠窗带电脑位" }
        );

        db.SaveChanges();
    }
}
