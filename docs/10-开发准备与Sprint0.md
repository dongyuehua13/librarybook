# 图书馆座位预约系统 - 开发准备与 Sprint 0

> 文档版本：v1.0  
> 前置输入：docs/07-系统设计说明.md、docs/08-数据库设计.md、docs/09-关键链路详细设计.md  
> 下一步产出：开发前一致性总审计

---

## 1. 仓库结构

```
librarybook/                    ← Git 仓库根目录（GitHub: dongyuehua13/librarybook）
├── .gitignore                  ← 忽略 bin/obj/.vs/*.user/appsettings.Development.json
├── README.md                   ← 项目简介（随 Sprint 持续更新）
├── docs/                       ← 全部设计文档
│   ├── 01-项目立项单.md
│   ├── 02-需求分析与MVP确认.md
│   ├── 03-PRD-Lite.md
│   ├── 04-页面树与业务流程.md
│   ├── 05-页面卡与UI规范.md
│   ├── 06-静态原型与原型评审.md
│   ├── 07-系统设计说明.md
│   ├── 08-数据库设计.md
│   ├── 09-关键链路详细设计.md
│   ├── 10-开发准备与Sprint0.md    ← 本文
│   ├── 11-开发前一致性总审计.md
│   ├── 12-开发起步与骨架记录.md
│   └── 项目任务板与迭代记录.md     ← 任务板
├── prototype/                   ← 静态原型
│   ├── review-1/
│   └── static-v1/
│       ├── *.html
│       └── css/custom.css
└── LibrarySeatSystem/           ← ASP.NET Core MVC 项目根（✓ 已有）
    ├── Controllers/             ✓ 已有（注入 Service 调用）
    ├── Models/                  ✓ 已有（3 实体）
    ├── ViewModels/              ✓ 已有（Sprint 0 产出）
    ├── Services/                ✓ 已有（Sprint 0 产出）
    ├── Data/                    ✓ 已有（DbContext + 种子数据）
    ├── Filters/                 ✓ 已有（AdminAuthFilter）
    ├── Views/                   ✓ 已有（已实现页面）
    ├── wwwroot/                 ✓ 已有（Bootstrap 离线资源）
    ├── Program.cs               ✓ 已有（DI + Session + EnsureCreated + Seed）
    ├── appsettings.json         ✓ 已有
    └── LibrarySeatSystem.csproj ✓ 已有
```

---

## 2. 分支策略

| 分支 | 用途 | 保护 |
|------|------|------|
| `main` | 只合入经过验证的稳定代码，随时可 `dotnet run` 演示 | 是（不直接推送） |
| `dev` | 日常开发集成分支，Sprint 结束时合入 main | 否 |
| `feat/xxx` | 从 dev 拉出，每张任务卡一个独立分支 | 否 |

**分支生命周期：**

```
main  ←── dev (Sprint 结束合并)
               ↑
          feat/xxx (任务卡分支)
               ↑
          开发者在此修改代码
```

**分支命名规则：**

| 前缀 | 示例 |
|------|------|
| `feat/` | `feat/T10-01-setup-skeleton` |
| `fix/` | `fix/T13-01-overlap-check` |
| `refactor/` | `refactor/T10-08-extract-service` |
| `docs/` | `docs/add-08-db-design` |

---

## 3. 提交规范

### 3.1 提交格式

每行不超过 72 字符。

```
<type>(<scope>): <简短描述>

<可选详细说明>
```

### 3.2 提交类型

| 类型 | 示例 |
|------|------|
| `feat` | `feat(seats): 新增按楼层/区域筛选座位` |
| `fix` | `fix(reserve): 修复时段冲突检测边界条件` |
| `refactor` | `refactor(home): 抽取 ReservationService` |
| `docs` | `docs: 添加数据库设计文档` |
| `style` | `style: 统一命名空间格式` |
| `chore` | `chore: 添加 .gitignore` |

### 3.3 提交颗粒度

- 一个任务卡对应 1-3 次提交
- 不要攒 5 个任务一次性 commit
- 提交前确保 `dotnet build` 无错误

---

## 4. 里程碑节点（≤ 4 个）

| 里程碑 | 对应 Sprint | 截止标准 | 交付物 |
|--------|------------|---------|--------|
| **M1** 骨架与数据层 | Sprint 0 | `dotnet build` 0 errors + `dotnet run` 自动建库 + 种子数据就位 + Service 层可被 Controller 调用 | Models + Data + Services + Filters + 重构后的 Controllers + Program.cs DI 配置 + 首次 git commit |
| **M2** 用户端完成 | Sprint 1 | 用户端 5 页可完整走通预约→取消闭环，9 条验证场景（docs/09 §4.4）全部通过 | 链路一完整实现 |
| **M3** 管理端完成 | Sprint 2 | 管理端 4 页可完整走通登录→管理→统计→退出闭环，CRUD 全部可用 | 链路二完整实现 |
| **M4** 集成与收尾 | Sprint 3 | 端到端无报错 + 高优先级评审问题修复 + README 更新 + 推送至远程仓库 | 完整可交付代码仓库 |

---

## 5. Sprint 规划

### 5.1 Sprint 节奏

| 参数 | 值 |
|------|-----|
| 单个 Sprint 时长 | ~1 周（课堂 4-5 周 = 1 个 Sprint 0 + 3 个开发 Sprint） |
| 推进方式 | **每个主 Sprint 允许多轮推进**，不限制一次性完成 |
| 轮次命名 | `Sprint1-R1` / `Sprint1-R2` … |
| 完成标准 | 该 Sprint 内所有任务卡验证通过 → 合并 dev → 标记当前 Sprint 完成 |

### 5.2 Sprint 0 — 工程骨架

**目标：** 搭建符合设计文档的分层骨架。Models + Data 层已存在但需复核；Service / ViewModel / Filter 层全部新建；Controllers 从直调 DbContext 重构为调用 Service；确保首次 `dotnet build` 和 `dotnet run` 通过，数据库自动创建并写入种子数据。

**阶段最低完成线：** `dotnet build` 0 errors 0 warnings + `dotnet run` 自动建表 + 种子数据成功写入 + 首页返回 200。

| 任务卡 | 标题 | 验收标准 |
|--------|------|---------|
| T10-01 | 创建项目骨架（.sln / 目录结构） | ① 创建 `LibrarySeatReservation.sln` ② 创建 `src/LibrarySeatReservation.Web/` MVC 项目 ③ 创建 Services/ / Filters/ / ViewModels/ 目录 ④ 确认 `.csproj` 含 EF Core SqlServer + Tools + Design 引用 ⑤ `dotnet build` 0 errors |
| T10-02 | 首次 `dotnet build` 和 `dotnet run` 验证 | ① `dotnet restore` 成功 ② `dotnet build` 0 errors ③ `dotnet run` 启动无异常 ④ 首页返回 200 ⑤ LocalDB 中已创建数据库和 Users/Seats/Reservations 三表 |
| T10-03 | 建立 EF Core + LocalDB 连接 | ① appsettings.json 配置 LocalDB 连接串 ② Program.cs 注册 DbContext ③ `dotnet build` 通过 |
| T10-04 | 建立 Users/Seats/Reservations 实体 | ① User.cs 字段对齐 docs/08 §4.1 ② Seat.cs 字段对齐 docs/08 §4.2 ③ Reservation.cs 字段对齐 docs/08 §4.3 ④ AppDbContext 含 3 个 DbSet + FK 配置 + Username 唯一索引 |
| T10-05 | 新建 IUserService + UserService | ① `ValidateAdminAsync(username)` ② `GetStudentsAsync()` ③ `GetByIdAsync(id)` ④ `GetByUsernameAsync(username)` |
| T10-06 | 新建 ISeatService + SeatService | ① `GetAllAsync()` ② `GetByFloorAsync(floor)` ③ `GetByIdAsync(id)` ④ `ToggleActiveAsync(id)` ⑤ `CreateAsync(seat)` |
| T10-07 | 新建 IReservationService + ReservationService | ① `GetUserReservationsAsync(userId)` ② `GetAllAsync()` ③ `GetBySeatAsync(seatId)` ④ `CreateAsync()`含冲突检测/时长限制/每日上限 ⑤ `CancelAsync()`含 5 项校验 |
| T10-08 | 新建 IStatsService + StatsService | ① `GetTodayReservationCountAsync()` ② `GetActiveSeatCountAsync()` ③ `GetAreaDistributionAsync()` |
| T10-09 | 新建 AdminAuthFilter | ① 实现 `IAuthorizationFilter` ② 无 Session → 重定向 Login ③ Program.cs 注册 + AdminController 标记 |
| T10-10 | 创建 EF Core 首次迁移 | ① `dotnet ef migrations add InitialCreate` 成功 ② 迁移文件生成至 Migrations/ 目录 |
| T10-11 | 创建 README.md Sprint 0 段 | ① 项目简介 + 技术栈 + 目录结构 + 运行前提 + 快速启动 ② 演示账号 + 数据库方式 + 已知限制 ③ 当前阶段标记 |
| T10-12 | 创建 docs/12-开发起步与骨架记录.md | ① 记录骨架决策 ② 差异说明 ③ 种子数据明细 ④ 验证状态 |

### 5.3 Sprint 1 — 用户端开发（主 Sprint，可多轮推进）

**目标：** 链路一（学生预约座位）完全可走通。覆盖首页 → 座位列表 → 座位详情 → 预约提交 → 我的预约 + 取消子流程。

**阶段最低完成线：** 首页显示座位数和今日预约数 + 座位列表按楼层/区域筛选 + 座位详情展示预约记录 + 预约提交做冲突检测并成功写入 + 我的预约列表显示 CanCancel + 取消功能可用 + 账号切换可用。

**允许进入下一阶段条件（完成线检查点）：** 首页 + 座位列表 + 预约提交（成功+冲突两条路径）在浏览器中可完整操作。

**涉及的 docs/09 链路一方法：** `Index`、`Seats`、`Detail`、`Reserve(GET)`、`Reserve(POST)`、`MyReservations`、`Cancel`、`SwitchUser(GET/POST)`、`Logout`

| 任务卡 | 标题 | 验收标准 |
|--------|------|---------|
| T11-01 | 实现用户首页 | ① 调用 `StatsService` 展示总座位数和今日预约数 ② 导航栏显示当前登录用户名 ③ "浏览座位"→`/Home/Seats` 按钮 ④ 页面适配移动端 |
| T11-02 | 实现座位列表页（筛选 + 占用标记） | ① 展示所有 IsActive=true 的座位 ② 每张卡片显示编号/楼层/区域/描述 ③ 空闲绿色/已预约灰色标记 ④ 按楼层和区域下拉筛选 |
| T11-03 | 实现座位详情页 | ① 展示座位基本信息 ② 展示该座位未来预约记录列表 ③ "预约此座位"按钮（已登录跳转/未登录提示）④ 无效 seatId 返回 404 |
| T11-04 | 实现预约提交页（GET 表单） | ① 显示座位编号和基本信息 ② 日期选择 min=今天 ③ 开始时间 08:00~21:00 ④ 结束时间=开始+1h 联动 ⑤ 未选账号跳转 SwitchUser |
| T11-05 | 实现预约提交（POST AJAX + 冲突检测） | ① jQuery AJAX POST 提交 ② 后端校验时段合法性 ③ 冲突检测返回 `{success:false, message:"..."}` ④ 成功返回 `{success:true}` + toast 提示 |
| T11-06 | 实现我的预约页（含 CanCancel） | ① 展示当前用户全部记录 ② 每条显示座位编号/日期/时段/状态徽章/创建时间 ③ 可取消按钮（仅 CanCancel=true 可见）④ 无记录显示引导 |
| T11-07 | 实现取消预约（POST AJAX） | ① 确认弹窗 → AJAX POST ② 后端校验：存在/本人/已预约/未过期/未开始 ③ 成功 UPDATE Status→toast ④ 失败 toast 显示错误 |
| T11-08 | 实现账号切换与退出 | ① 首页展示所有学生账号 ② POST 写入 Session → 重定向首页 ③ 导航栏显示当前用户名 ④ "退出"清除 Session |

### 5.4 Sprint 2 — 管理端开发（主 Sprint，可多轮推进）

**目标：** 链路二（管理员全流程管理）完全可走通。覆盖登录 → 座位管理（增/改/状态切换） → 预约管理（筛选） → 统计 → 退出。

**阶段最低完成线：** 管理员登录成功 + 座位列表显示全部座位 + 新增/编辑/切换状态可用 + 预约管理按状态和日期筛选 + 统计页显示四个板块数据。

**允许进入下一阶段条件（完成线检查点）：** 管理端 4 页可在浏览器中完整操作。

**涉及的 docs/09 链路二方法：** `Login(GET/POST)`、`Logout`、`Seats`、`SeatCreate(GET/POST)`、`SeatEdit(GET/POST)`、`SeatToggle`、`Reservations`、`Stats`

| 任务卡 | 标题 | 验收标准 |
|--------|------|---------|
| T12-01 | 实现管理端登录/登出 | ① Login GET 显示用户名输入框 ② POST 调用 `UserService.ValidateAdminAsync()` → 写入 Session ③ 用户名不存在 → ModelError ④ 已登录自动跳转 ⑤ Logout 清除 Session |
| T12-02 | 实现座位管理页（表格） | ① 表格展示全部座位 ② 列：编号/楼层/区域/描述/启用状态/操作 ③ 状态徽章（启用=绿/禁用=灰）④ 无数据空状态 |
| T12-03 | 实现新增座位表单（含编号查重） | ① GET 显示空表单 ② POST 调用 `CreateAsync` 含编号查重 ③ 编号重复 → ModelError ④ 成功重定向到座位列表 |
| T12-04 | 实现编辑座位表单（含查重） | ① GET 回填现有数据 ② 不存在 id 重定向列表 ③ POST 含排除自身查重 ④ 成功重定向 |
| T12-05 | 实现座位状态切换（AJAX） | ① AJAX POST `/Admin/SeatToggle` ② 返回 `{success:true, isActive:bool}` ③ 前端行内更新不刷新 |
| T12-06 | 实现预约管理页（筛选） | ① 全部预约记录表格 ② 状态下拉筛选 ③ 日期范围筛选 ④ 无匹配空状态 |
| T12-07 | 实现统计页 | ① 3 个统计卡片（可用座位/总预约/今日预约）② 座位热度排行 TOP10 ③ 近 14 日趋势 ④ 空数据状态 |
| T12-08 | 实现 AdminAuthFilter 全页拦截 | ① 全部 Admin Action 被拦截（除 Login）② 未登录→Login ③ AJAX → 401 JSON |

### 5.5 Sprint 3 — 集成与完善（主 Sprint，可多轮推进）

**目标：** 端到端全链路可用，高优先级原型评审问题修复，代码质量收尾。

**阶段最低完成线：** 两条主链路在真实浏览器中完整走通无报错 + `dotnet build` 0 errors 0 warnings + README 可引导新用户成功启动。

**允许进入下一阶段条件：** 全部任务卡验证通过 → 项目交付。

| 任务卡 | 标题 | 验收标准 |
|--------|------|---------|
| T13-01 | 修复原型评审高优先级问题 | ① 统计页空状态展示 ② 表单校验 invalid-feedback 样式 ③ toast 提示替代 alert() ④ `dotnet build` 0 errors |
| T13-02 | 端到端主链路集成测试 | ① docs/09 §4.4 全部 9 条验证场景逐一通过 ② 冲突检测/跨日期/相邻时段/取消重约/不可取消 ③ 不可约隐藏/禁用前台隐藏 |
| T13-03 | 边界情况逐项验证 | ① docs/09 §2.8 的 12 种用户端异常场景 ② docs/09 §3.8 的 4 种管理端异常场景 ③ 全部通过 |
| T13-04 | 代码一致性审计 | ① 目录对齐 docs/07 §3 ② 实体对齐 docs/08 §4 ③ 方法签名对齐 docs/09 |
| T13-05 | 补充缺失注释与文档 | ① Service 接口 XML 注释 ② README 更新为当前实际状态 ③ `dotnet build` 0 warnings |

### 5.6 任务卡编号规则

```
T<主Sprint编号><该Sprint内序号>

示例：
T10-01 → Sprint 0 的第 1 张卡
T11-05 → Sprint 1 的第 5 张卡
T12-03 → Sprint 2 的第 3 张卡
T13-06 → Sprint 3 的第 6 张卡
```

---

## 6. 本地仓库初始化与首次提交

### 6.1 .gitignore（已就位）

最低要求包含：

```
bin/
obj/
.vs/
*.user
appsettings.Development.json
```

### 6.2 本地仓库初始化（已完成）

```bash
cd D:\AiWeb\3
git init
git add .
git commit -m "feat(init): project skeleton with Sprint 0 scaffolding"
git branch -M main
git checkout -b dev
```

### 6.3 远端仓库（已推送）

> ✅ 远端地址：https://github.com/dongyuehua13/librarybook

已执行推送命令（Sprint 0 阶段实际完成）：

```bash
git remote add origin https://github.com/dongyuehua13/librarybook.git
git push -u origin main
git push -u origin dev
```

### 6.4 分支保护建议（可选）

- GitHub：Settings → Branches → Add rule → `main` → Require pull request before merging
- Gitee：Settings → Protected Branches → `main` → 开启保护

---

## 7. 开发环境检查清单

| 检查项 | 确认 | 检查方法 |
|--------|------|---------|
| .NET SDK ≥ 8.0 | ☐ | `dotnet --list-sdks` |
| LocalDB 可用 | ☐ | `sqllocaldb info MSSQLLocalDB` |
| 端口 5000/5001 可用 | ☐ | 启动后浏览器可打开 |
| Git 已安装 | ☐ | `git --version` |
| NuGet 包已恢复 | ☐ | `dotnet restore` |
| 首次构建成功 | ☐ | `dotnet build` |
| 首次运行成功 | ☐ | `dotnet run` + 浏览器打开首页 |

---

## 8. 默认补足项 / 当前假设

| 补足项 | 说明 |
|--------|------|
| 解决方案名 `LibrarySeatReservation.sln` | 前序文档未指定。实际代码在 `src/` 子目录下，故建 `.sln` 便于管理 |
| 项目名 `LibrarySeatReservation.Web` | 前序文档使用 `LibrarySeatSystem`。实际因 `.sln` 命名变化而调整 |
| Sprint 0 含 12 张任务卡 | 前序文档仅列出 9 张。实际拆分了 Service 新建/迁移/README 等为独立卡 |
| 里程碑 M4 对应需求 | 前序文档仅定义 M1-M3，M4 收尾为本文补充，但未超过 4 个上限 |
| 任务卡编号 T10/T11/T12/T13 | 前序文档未规定。本文从 Sprint 0=10 起始编号 |
| 分支策略 `main/dev/feat-xxx` | 前序文档未规定。本文按常规 Git Flow 简化版 |
| 提交规范 `<type>(scope): desc` | 前序文档未规定。本文采用 conventional commits 简化版 |
| 种子数据实际 45 个座位 | 前序文档写 15 个样例。按 3层×3区域×5=45 逻辑生成 |
| 目标框架 net10.0 | 前序文档写 .NET 8，但环境仅 SDK 10.0 可用，调整为实际版本 |
