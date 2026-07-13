# 图书馆座位预约系统

> 单人课堂实训项目 · ASP.NET Core MVC (.NET 8) · SQL Server LocalDB · Bootstrap 5  
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
| 前端交互 | jQuery + AJAX |

**不引入：** AutoMapper / Serilog / Redis / SignalR / 第三方图表库

---

## 目录结构

> 标注「✓ 已有」为当前仓库已存在的文件和目录；  
> 标注「◇ 计划中」为后续 Sprint 计划生成的产物。

```
LibrarySeatSystem/                   ← ASP.NET Core MVC 项目根（✓ 已有）
├── Controllers/                      ✓ 已有
│   ├── HomeController.cs            首页 + 座位列表/详情/预约/取消/切换账号
│   └── AdminController.cs           管理员登录/退出/座位/预约/统计
├── Models/                           ✓ 已有（3 实体）
│   ├── User.cs                      用户
│   ├── Seat.cs                      座位
│   ├── Reservation.cs               预约记录
│   └── ErrorViewModel.cs
├── ViewModels/                       ✓ 已有（6 个 ViewModel）
│   ├── HomeIndexViewModel.cs        首页视图模型
│   ├── SeatWithStatus.cs            座位列表项（含占用状态）
│   ├── SeatDetailViewModel.cs       座位详情视图模型
│   ├── MyReservationsViewModel.cs   我的预约视图模型
│   ├── AdminReservationsViewModel.cs 管理员预约视图模型
│   └── AdminStatsViewModel.cs       统计视图模型（含 DashboardStats/SeatRanking/DailyTrend）
├── Services/                         ✓ 已有（4 接口 + 4 实现）
│   ├── ISeatService.cs + SeatService.cs
│   ├── IReservationService.cs + ReservationService.cs
│   ├── IStatsService.cs + StatsService.cs
│   └── IUserService.cs + UserService.cs
├── Data/                             ✓ 已有
│   ├── AppDbContext.cs               EF Core DbContext
│   └── DbInitializer.cs             种子数据
├── Filters/                          ✓ 已有
│   └── AdminAuthFilter.cs           管理员认证过滤器
├── Views/                            ✓ 已有
│   ├── Home/                         Index / Seats / Detail / Reserve / MyReservations / SwitchUser
│   ├── Admin/                        Login / Seats / SeatCreate / SeatEdit / Reservations / Stats
│   └── Shared/                       _Layout / _AdminLayout / Error
├── wwwroot/                          ✓ 已有
├── Program.cs                        ✓ 已有
├── appsettings.json                  ✓ 已有
└── LibrarySeatSystem.csproj          ✓ 已有
LibrarySeatSystem.Tests/             ← xUnit 集成测试项目（✓ 已有）
docs/                                ← 设计文档目录（✓ 已有）
prototype/                           ← 原型目录（✓ 已有）
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
| 管理 | 座位管理 | `/Admin/Seats` | Sprint 2 | ✅ 已有 |
| 管理 | 预约管理 | `/Admin/Reservations` | Sprint 2 | ✅ 已有 |
| 管理 | 统计 | `/Admin/Stats` | Sprint 2 | ✅ 已有 |

---

## 已实现范围

> 本段落随 Sprint 推进持续更新。当前为 Sprint 1 完成、Sprint 2 进行中（4/8）、Sprint 3 部分完成状态。

**当前仓库已有：**
- ✅ Sprint 0 骨架（分层架构 / EF Core / 4 Services / 3 实体 / AdminAuthFilter / 种子数据）
- ✅ Sprint 1 用户端完整闭环（首页 → 列表 → 详情 → 预约提交 → 我的预约 → 取消 → 切换账号）
- ✅ Sprint 2 管理端完整（登录/登出 → 座位管理 CRUD + 行内切换 → 预约筛选 → 统计页）
- ✅ 全部 9 页（P01~P09）已实现
- ✅ 30 项集成测试（T13-02/03 测试代码已完成）
- ✅ 全部设计文档（docs/01 ~ docs/14）

**Sprint 1 已交付（8 张卡）：**
- ✅ T11-01 用户首页 | T11-02 座位列表页 | T11-03 座位详情页
- ✅ T11-04 预约提交页 GET | T11-05 预约提交 POST AJAX | T11-06 我的预约页 | T11-07 取消预约 | T11-08 账号切换与退出

**Sprint 2 已交付（8 张卡，两轮完成）：**
- ✅ T12-01 管理端登录/登出 | T12-02 座位管理页 | T12-03 新增座位表单 | T12-04 编辑座位表单
- ✅ T12-05 座位状态切换 AJAX | T12-06 预约管理页 | T12-07 统计页 | T12-08 AdminAuthFilter

**种子数据：** 4 个用户（admin + zhangsan/lisi/wangwu）+ 15 个座位（A-01~C-05）

**Sprint 3 进度（3/6）：**
- ✅ T13-01 原型评审高优问题修复 | T13-04 代码一致性审计 | T13-05 补充注释与文档
- ◇ T13-02 端到端集成测试（代码 30 项 ✅，浏览器验证待环境）
- ◇ T13-03 边界场景测试（代码 20 项 ✅，浏览器验证待环境）
- ◇ T13-06 推送至远程仓库（需外网）

---

## 运行前提

- Windows 系统（LocalDB 仅 Windows 可用）
- .NET 10 SDK（`dotnet --list-sdks` 确认存在 10.0.x）
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
# 座位列表：http://localhost:5002/Home/Seats
# 管理员登录：http://localhost:5002/Admin/Login
```

---

## 数据库初始化方式

系统使用 **Code First + EF Core Migrations** 自动建库，首次 `dotnet run` 时自动执行迁移并写入种子数据。

Program.cs 中启动时自动流程：
1. `db.Database.Migrate()` → 自动执行所有迁移文件建库建表
2. `DbInitializer.Seed(db)` → Users 表为空时写入种子数据

> 如需重置数据库：
> ```bash
> sqllocaldb stop MSSQLLocalDB
> sqllocaldb delete MSSQLLocalDB
> dotnet run
> ```

---

## 演示账号

| 角色 | 用户名 | 说明 |
|------|--------|------|
| 管理员 | `admin` | 管理端登录用（无密码，仅校验用户名） |
| 学生 | `zhangsan` / `lisi` / `wangwu` | 前端通过下拉切换账号（无密码） |

所有账号为种子数据预设，不支持注册新用户。  
种子数据还包含 15 个座位（1F 自习区 A-01~05 / 2F 安静区 B-01~05 / 3F 电子阅览区 C-01~05）。

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
- [x] **Sprint 1 — 用户端开发**（8/8 ✅）
- [x] **Sprint 2 — 管理端开发**（8/8 ✅ 两轮完成）
- [ ] **Sprint 3 — 集成与完善**（进行中 · 3/6）

---

> ## 建议演示路径

```text
用户端链路：
1. 打开首页 http://localhost:5002                → 查看统计卡片 + 区域分布
2. 点击"张三"按钮                                 → 切换为张三
3. 点击"浏览座位"                                 → 座位列表页（全部 15 个座位）
4. 筛选楼层=1 + 区域=自习区                       → 查看筛选 + 空闲/已预约标记
5. 点击"查看详情"                                 → 座位详情页（信息 + 预约记录）
6. 点击"预约此座位"                               → 预约提交页（日期+时间选择）
7. 选今天 09:00-10:00 → 点击"提交预约"            → 绿色通知"预约成功"
8. 再次提交同一座位同一时段                       → 红色通知"已被预约"
9. 点击"查看我的预约"                             → 我的预约页（显示刚创建的记录）
10. 点击"取消"按钮 → 确认                         → 状态变为"已取消"

管理端链路：
11. 访问 http://localhost:5002/Admin/Login            → 管理员登录页
12. 输入 `admin` → 点击"登录"                        → 跳转座位管理页
13. 表格展示所有座位，含"启用"/"禁用"状态徽章        → 可查看全部（含禁用座位）
14. 点击"新增座位"                                    → 新增表单，填写编号 A-06 → 保存
15. 点击某座位行内的"禁用"按钮                       → 状态即时切换为"禁用"
16. 点击"预约管理"                                    → 预约记录表格，支持下拉状态 + 日期范围筛选
17. 点击"数据统计"                                    → 三张统计卡片 + 座位热度排行 + 14 日趋势
18. 点击"返回首页"（此时禁用座位不再出现在用户端）   → 用户端已屏蔽禁用座位
```

---

**开发者**：软件专业实训学生 · **开发周期**：4-5 周 · **交付物**：完整可运行代码仓库
