# 数据库初始化说明

## 采用方式

Code First（EF Core `EnsureCreated`），非手工 SQL 脚本。

## 首次建库建表

系统在 `Program.cs` 启动时自动执行：

```csharp
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();   // 库/表不存在时自动创建
    DbInitializer.Seed(db);        // 种子数据写入
}
```

`EnsureCreated()` 根据 `AppDbContext` 中的 3 个 `DbSet` 属性自动生成表结构：

- `Users` → 用户表
- `Seats` → 座位表
- `Reservations` → 预约记录表

> 注意：`EnsureCreated` 不会增量更新已有数据库。如需修改表结构，需先删库再重新创建（见下方重置步骤）。

## 种子数据初始化

`DbInitializer.Seed()` 在 `Users` 表为空时写入预设数据：

### 用户（User）

| 用户名 | 显示名 | 是否管理员 |
|--------|--------|-----------|
| `admin` | 管理员 | ✅ 是 |
| `zhangsan` | 张三 | ❌ |
| `lisi` | 李四 | ❌ |
| `wangwu` | 王五 | ❌ |

### 座位（Seat）

共 15 个座位，按楼层和区域分布：

| 编号 | 楼层 | 区域 | 描述 |
|------|------|------|------|
| A-01 ~ A-05 | 1F | 自习区 | 靠窗/中间/靠门，部分有插座 |
| B-01 ~ B-05 | 2F | 安静区 | 独立隔间/双人桌，部分有台灯 |
| C-01 ~ C-05 | 3F | 电子阅览区 | 带电脑位/普通座位，部分靠窗 |

所有座位初始均为 `IsActive = true`（启用状态）。

### 预约记录（Reservation）

种子数据中无预约记录。首次启动后通过用户端操作创建。

## 重置数据库

```bash
sqllocaldb stop MSSQLLocalDB
sqllocaldb delete MSSQLLocalDB
dotnet run --urls "http://localhost:5002"
```

系统会自动重新创建数据库并写入种子数据。

## 数据库连接串

配置在 `LibrarySeatSystem/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\MSSQLLocalDB;Database=LibrarySeatDb;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## 实体关系图

```
User (1) ──── (N) Reservation (N) ──── (1) Seat
  ├ Id (PK, int)            ├ Id (PK, int)            ├ Id (PK, int)
  ├ Username (string, UK)    ├ UserId (FK, int)        ├ SeatNumber (string)
  ├ DisplayName (string)     ├ SeatId (FK, int)        ├ Floor (int)
  └ IsAdmin (bool)           ├ Date (date)             ├ Area (string)
                             ├ StartTime (time)        ├ Description (string?)
                             ├ EndTime (time)          └ IsActive (bool)
                             ├ Status (string)
                             └ CreatedAt (datetime)
```

- `Reservation.UserId` → `User.Id`（FK, NoAction）
- `Reservation.SeatId` → `Seat.Id`（FK, NoAction）
- `User.Username` 有唯一索引
