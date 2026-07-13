# 图书馆座位预约系统

> 单人课堂实训项目 · ASP.NET Core MVC (net10.0) · SQL Server LocalDB · Bootstrap 5  
> GitHub：https://github.com/dongyuehua13/librarybook

学生在线查座位、预约时段、取消预约；管理员后台管理座位、查看预约和统计数据。

---

## 技术栈

| 层 | 技术 |
|------|------|
| 框架 | ASP.NET Core MVC (net10.0) |
| 视图 | Razor (.cshtml) + Bootstrap 5 |
| 数据访问 | Entity Framework Core 10.x (Code First + Migrations) |
| 数据库 | SQL Server LocalDB (MSSQLLocalDB) |
| 前端交互 | AJAX (原生 + Bootstrap) |

**不引入：** AutoMapper / Serilog / Redis / SignalR / 第三方图表库

---

## 目录结构

> 标注「✓ 已有」为当前仓库已存在的文件和目录；  
> 标注「◇ 计划中」为后续 Sprint 计划生成的产物。

```
LibrarySeatReservation.sln
src/
└── LibrarySeatReservation.Web/       ← ASP.NET Core MVC 项目根（✓ 已有）
    ├── Controllers/                   ✓ 已有
    │   ├── HomeController.cs          首页 + 座位列表 + 座位详情 + 切换账号
    │   └── AdminController.cs         管理员登录/退出
    ├── Models/                        ✓ 已有（3 实体）
    │   ├── User.cs                    用户
    │   ├── Seat.cs                    座位
    │   ├── Reservation.cs             预约记录
    │   └── ErrorViewModel.cs
    ├── ViewModels/                    ✓ 已有（Sprint 1 第 1 轮填充）
    │   ├── HomeIndexViewModel.cs      首页视图模型
    │   ├── SeatWithStatus.cs          座位列表项（含占用状态）
    │   ├── SeatDetailViewModel.cs     座位详情视图模型
    │   ├── DashboardStats.cs          统计聚合
    │   ├── MyReservationsViewModel.cs  ◇ 计划中
    │   ├── AdminReservationsViewModel.cs ◇ 计划中
    │   └── AdminStatsViewModel.cs     ◇ 计划中
    ├── Services/                      ✓ 已有（含 Sprint 1 方法）
    │   ├── Interfaces/
    │   │   ├── ISeatService.cs
    │   │   ├── IReservationService.cs
    │   │   ├── IStatsService.cs
    │   │   └── IUserService.cs
    │   ├── SeatService.cs
    │   ├── ReservationService.cs
    │   ├── StatsService.cs
    │   └── UserService.cs
    ├── Data/                          ✓ 已有
    │   ├── AppDbContext.cs            EF Core DbContext
    │   └── DbInitializer.cs          种子数据
    ├── Filters/                       ✓ 已有
    │   └── AdminAuthFilter.cs        管理员认证过滤器
    ├── Views/                         ✓ 已有（Sprint 1 第 1 轮填充）
    │   ├── Home/                      Index / Seats / Detail / Reserve◇ / MyReservations◇
    │   ├── Admin/                     Login
    │   └── Shared/                    _Layout / _AdminLayout / Error
    ├── Migrations/                    ✓ 已有（InitialCreate）
    ├── wwwroot/                       ✓ 已有
    ├── Program.cs                     ✓ 已有
    └── appsettings.json               ✓ 已有
docs/                                 ← 设计文档目录（✓ 已有）
prototype/                            ← 原型目录（✓ 已有）
```

---

## 页面清单（共 9 页）

| 端 | 页面 | 路由 | Sprint 归属 | 状态 |
|----|------|------|------------|------|
| 用户 | 首页 | `/` | Sprint 0 + S1 | ✅ 已有 |
| 用户 | 座位列表 | `/Home/Seats` | Sprint 1 | ✅ 已有 |
| 用户 | 座位详情 | `/Home/Detail/{id}` | Sprint 1 | ✅ 已有 |
| 用户 | 预约提交 | `/Home/Reserve/{seatId}` | Sprint 1 | ✅ 已有 |
| 用户 | 我的预约 | `/Home/MyReservations` | Sprint 1 | ✅ 已有 |
| 管理 | 管理员登录 | `/Admin/Login` | Sprint 2 | ✅ 已有 |
| 管理 | 座位管理 | `/Admin/Seats` | Sprint 2 | ◇ 计划中 |
| 管理 | 预约管理 | `/Admin/Reservations` | Sprint 2 | ◇ 计划中 |
| 管理 | 统计 | `/Admin/Stats` | Sprint 2 | ◇ 计划中 |

---

## 已实现范围

> 本段落随 Sprint 推进持续更新。当前为 Sprint 1 第 1 轮完成状态。

**当前仓库已有：**
- ✅ Sprint 0 骨架（分层架构 / EF Core 迁移 / 4 Services / 3 实体 / AdminAuthFilter / 种子数据）
- ✅ 首页统计概览 + 切换体验账号 + 管理员登录
- ✅ 座位列表页（按楼层/区域筛选 + 空闲/已预约徽章 + 空状态 + 重置筛选）
- ✅ 座位详情页（座位信息 + Date≥today 预约记录表 + 预约入口）
- ✅ 预约提交页（GET 表单 + AJAX POST + 冲突检测 + 通知反馈）
- ✅ 我的预约页（记录表格 + CanCancel 判定 + 取消 AJAX）
- ✅ 导航栏（已登录用户名 + 下拉菜单/退出；未登录"选择账号""管理后台"）
- ✅ 全部设计文档（docs/01 ~ docs/13）

**Sprint 1 已交付（7 张卡，完成率 87.5%）：**
- ✅ T11-01 用户首页 | T11-02 座位列表页 | T11-03 座位详情页（第 1 轮）
- ✅ T11-04 预约提交页 GET | T11-05 预约提交 POST AJAX | T11-06 我的预约页（含 CanCancel）| T11-07 取消预约 POST AJAX（第 2 轮）

**Sprint 1 待完成（1 张卡）：**
- ◇ T11-08 账号切换与退出完善（P1 级，不阻塞进入 Sprint 2）

**种子数据：** 4 个用户（admin + zhangsan/lisi/wangwu）+ 45 个座位（3 层 × 3 区域 × 5 个 = A-01~I-05）

**后续 Sprint 交付：**
- ◇ Sprint 2：管理端 4 页完整可用
- ◇ Sprint 3：集成测试 + 边界修复 + 远程推送

---

## 运行前提

- Windows 系统（LocalDB 仅 Windows 可用）
- .NET 10 SDK（`dotnet --list-sdks` 确认存在 10.0.200+）
- SQL Server LocalDB（`sqllocaldb info MSSQLLocalDB` 确认可用）
- Git（`git --version` 确认已安装）

---

## 快速启动

```bash
# 1. 克隆仓库
git clone https://github.com/dongyuehua13/librarybook.git
cd librarybook

# 2. 进入项目目录
cd src\LibrarySeatReservation.Web

# 3. 还原 NuGet 包
dotnet restore

# 4. 首次构建
dotnet build

# 5. 首次运行（自动迁移 + 写入种子数据）
dotnet run --urls "http://localhost:5002"

# 6. 浏览器打开
# 用户端首页：http://localhost:5002
# 座位列表：http://localhost:5002/Home/Seats
# 管理员登录：http://localhost:5002/Admin/Login
```

---

## 数据库初始化方式

系统使用 **Code First + EF Core Migrations** 自动建库，首次 `dotnet run` 时自动应用迁移并写入种子数据。

Program.cs 中启动时自动流程：
1. `db.Database.Migrate()` → 应用全部待处理迁移（首次建库建表）
2. `DbInitializer.Seed(db)` → Users 表为空时写入种子数据

> ⚠️ `Migrate()` 支持增量表结构变更。如需新增表或字段：
> ```bash
> cd src\LibrarySeatReservation.Web
> dotnet ef migrations add <名称>
> dotnet ef database update
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
种子数据还包含 45 个座位（3 层 × 3 区域 × 5 个：自习区/安静区/电子阅览区，编号 A-01~I-05）。

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
| 前端 AJAX 交互未在真实浏览器验证 | 预约提交、取消等 AJAX 回调在无浏览器环境下仅通过代码审查验证 | Sprint 3 集成测试阶段需要在真实浏览器中逐项验证 |

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
- [ ] **Sprint 1 — 用户端开发**（第 2 轮完成 · 7/8 卡 · 87.5% · ⚠ 达到阶段最低完成线）→ [docs/13](docs/13-用户端主链路开发记录.md)
- [ ] Sprint 2 — 管理端开发
- [ ] Sprint 3 — 集成与完善

---

> ## 建议演示路径

```text
1. 打开首页 http://localhost:5002            → 查看统计卡片 + 区域分布
2. 点击"张三"按钮                              → 切换为张三
3. 点击"浏览座位"                              → 座位列表页（全部 45 个座位）
4. 筛选楼层=1 + 区域=自习区                    → 查看筛选 + 空闲/已预约标记
5. 点击"查看详情"                              → 座位详情页（信息 + 预约记录）
6. 点击"预约此座位"                            → 预约提交页（日期+时间选择）
7. 选今天 09:00-10:00 → 点击"提交预约"         → 绿色通知"预约成功"
8. 再次提交同一座位同一时段                    → 红色通知"已被预约"
9. 点击"查看我的预约"                          → 我的预约页（显示刚创建的记录）
10. 点击"取消"按钮 → 确认                      → 状态变为"已取消"
```

---

**开发者**：软件专业实训学生 · **开发周期**：4-5 周 · **交付物**：完整可运行代码仓库
