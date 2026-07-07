# 图书馆座位预约系统

> 单人课堂实训项目 · ASP.NET Core MVC (.NET 8) · SQL Server LocalDB · Bootstrap 5  
> GitHub：https://github.com/dongyuehua13/librarybook

学生在线查座位、预约时段、取消预约；管理员后台管理座位、查看预约和统计数据。

---

## 技术栈

| 层 | 技术 |
|------|------|
| 框架 | ASP.NET Core MVC (.NET 8) |
| 视图 | Razor (.cshtml) + Bootstrap 5 |
| 数据访问 | Entity Framework Core 8.x (Code First + EnsureCreated) |
| 数据库 | SQL Server LocalDB (MSSQLLocalDB) |
| 前端交互 | jQuery + AJAX |

**不引入：** AutoMapper / Serilog / Redis / SignalR / 第三方图表库

---

## 目录结构

> 标注「✓ 已有」为当前仓库已存在的文件和目录；  
> 标注「◇ 计划中」为后续 Sprint 计划生成的产物。

```
LibrarySeatSystem/                   ← ASP.NET Core MVC 项目根（✓ 已有）
├── Controllers/                      ✓ 已有
│   ├── HomeController.cs            首页 + 切换账号
│   └── AdminController.cs           管理员登录/退出
├── Models/                           ✓ 已有（3 实体）
│   ├── User.cs                      用户
│   ├── Seat.cs                      座位
│   ├── Reservation.cs               预约记录
│   └── ErrorViewModel.cs
├── ViewModels/                       ✓ 已有（Sprint 0 产出）
│   ├── HomeIndexViewModel.cs
│   ├── SeatWithStatus.cs
│   ├── SeatDetailViewModel.cs
│   ├── MyReservationsViewModel.cs
│   ├── AdminReservationsViewModel.cs
│   └── AdminStatsViewModel.cs
├── Services/                         ✓ 已有（Sprint 0 产出）
│   ├── ISeatService.cs
│   ├── IReservationService.cs
│   ├── IStatsService.cs
│   ├── IUserService.cs
│   ├── SeatService.cs
│   ├── ReservationService.cs
│   ├── StatsService.cs
│   └── UserService.cs
├── Data/                             ✓ 已有
│   ├── AppDbContext.cs               EF Core DbContext
│   └── DbInitializer.cs             种子数据
├── Filters/                          ✓ 已有
│   └── AdminAuthFilter.cs           管理员认证过滤器
├── Views/                            ✓ 已有
│   ├── Home/                         Index / Seats / Detail / Reserve / MyReservations / SwitchUser
│   ├── Admin/                        Login / Seats / SeatCreate / SeatEdit / Reservations / Stats
│   └── Shared/                       _Layout / _AdminLayout / Error
├── wwwroot/                          ✓ 已有（Bootstrap 离线资源）
├── Program.cs                        ✓ 已有
├── appsettings.json                  ✓ 已有
└── LibrarySeatSystem.csproj          ✓ 已有
docs/                                ← 设计文档目录（✓ 已有）
prototype/                           ← 原型目录（✓ 已有）
```

---

## 页面清单（共 9 页）

| 端 | 页面 | 路由 | Sprint 归属 | 状态 |
|----|------|------|------------|------|
| 用户 | 首页 | `/` | Sprint 0 | ✅ 已有 |
| 用户 | 座位列表 | `/Home/Seats` | Sprint 1 | ◇ 计划中 |
| 用户 | 座位详情 | `/Home/Detail/{id}` | Sprint 1 | ◇ 计划中 |
| 用户 | 预约提交 | `/Home/Reserve/{seatId}` | Sprint 1 | ◇ 计划中 |
| 用户 | 我的预约 | `/Home/MyReservations` | Sprint 1 | ◇ 计划中 |
| 管理 | 管理员登录 | `/Admin/Login` | Sprint 2 | ✅ 已有 |
| 管理 | 座位管理 | `/Admin/Seats` | Sprint 2 | ◇ 计划中 |
| 管理 | 预约管理 | `/Admin/Reservations` | Sprint 2 | ◇ 计划中 |
| 管理 | 统计 | `/Admin/Stats` | Sprint 2 | ◇ 计划中 |

---

## 已实现范围

> 本段落随 Sprint 推进持续更新。当前为 Sprint 0 完成状态。

**当前仓库已有（Sprint 0 产出）：**
- ✓ 分层架构骨架（Controllers / Services / Models / Data / Filters / Views）
- ✓ EF Core 迁移 + 自动建库建表（Users / Seats / Reservations 三表，含 FK + 索引）
- ✓ 种子数据（4 个用户：admin + zhangsan/lisi/wangwu；15 个座位：A-01~A-05 1F自习区 / B-01~B-05 2F安静区 / C-01~C-05 3F电子阅览区）
- ✓ Service 层 4 接口 + 4 实现（含预约业务规则骨架）
- ✓ 首页统计概览 + 切换体验账号 + 管理员登录
- ✓ AdminAuthFilter（管理端认证拦截过滤器）
- ✓ 全部设计文档（docs/01 ~ docs/12）

**Sprint 1 计划：**
- ◇ 座位列表页（按楼层/区域筛选 + 占用标记）
- ◇ 座位详情页（含预约记录）
- ◇ 预约提交表单 + AJAX 冲突检测
- ◇ 我的预约页（含取消功能）

**后续 Sprint 交付：**
- ◇ Sprint 2：管理端 4 页完整可用
- ◇ Sprint 3：集成测试 + 边界修复 + 远程推送

---

## 运行前提

- Windows 系统（LocalDB 仅 Windows 可用）
- .NET 8 SDK（`dotnet --list-sdks` 确认存在）
- SQL Server LocalDB（`sqllocaldb info MSSQLLocalDB` 确认可用）
- Git（`git --version` 确认已安装）

---

## 快速启动

```bash
# 1. 克隆仓库
git clone https://github.com/dongyuehua13/librarybook.git
cd librarybook

# 2. 进入项目目录
cd LibrarySeatSystem

# 3. 还原 NuGet 包
dotnet restore

# 4. 首次构建
dotnet build

# 5. 首次运行（自动建库建表 + 写入种子数据）
dotnet run --urls "http://localhost:5002"

# 6. 浏览器打开
# 用户端首页：http://localhost:5002
# 管理员登录：http://localhost:5002/Admin/Login
```

---

## 数据库初始化方式

系统使用 **Code First + EnsureCreated()** 自动建库，首次 `dotnet run` 时自动创建数据库和表，无需手动执行迁移命令。

Program.cs 中启动时自动流程：
1. `db.Database.EnsureCreated()` → 库不存在时创建库和所有表
2. `DbInitializer.Seed(db)` → Users 表为空时写入种子数据

> ⚠️ `EnsureCreated()` 不支持增量表结构变更。如需修改表结构，需先手动删除数据库再重启：
> ```bash
> dotnet run                                   # 启动后访问任意页面触发建库
> # 或手动删除 LocalDB:
> sqllocaldb stop MSSQLLocalDB
> sqllocaldb delete MSSQLLocalDB
> dotnet run
> ```
>
> 本设计为课堂实训约定，不用于生产环境。

---

## 演示账号

| 角色 | 用户名 | 说明 |
|------|--------|------|
| 管理员 | `admin` | 管理端登录用（无密码，仅校验用户名） |
| 学生 | `zhangsan` / `lisi` / `wangwu` | 前端通过下拉切换账号（无密码） |

所有账号为种子数据预设，不支持注册新用户。

---

## 已知限制

> 本段落持续更新，记录当前阶段已知但暂不解决的问题。

| 限制 | 说明 | 计划 |
|------|------|------|
| 不支持用户注册 | 仅 3 个预设学生账号，切换使用 | 课堂阶段不做 |
| 不支持密码 | 管理员和学生均无密码校验 | 课堂阶段不做 |
| LocalDB 仅 Windows | macOS/Linux 无法运行 | 课堂阶段不做跨平台 |
| 无定时任务 | 座位不会自动释放过期预约 | 课堂阶段不做 |
| 无分页 | 数据量小，直接 .ToList() | 如数据超 100 条可后续加 Skip/Take |

---

## 当前阶段

- [x] 项目立项（docs/01）
- [x] 需求分析与 MVP 确认（docs/02）
- [x] PRD-Lite（docs/03）
- [x] 页面树与业务流程（docs/04）
- [x] UI 规范（docs/05）
- [x] 静态原型（docs/06）
- [x] 原型评审
- [x] 系统设计（docs/07）
- [x] 数据库设计（docs/08）
- [x] 关键链路详细设计（docs/09）
- [x] 开发准备与 Sprint 0（docs/10）
- [x] 开发前一致性审计（docs/11）
- [x] **Sprint 0 — 工程骨架 ✓**
- [ ] Sprint 1 — 用户端开发
- [ ] Sprint 2 — 管理端开发
- [ ] Sprint 3 — 集成与完善

---

> **开发者**：软件专业实训学生 · **开发周期**：4-5 周 · **交付物**：完整可运行代码仓库
