# 图书馆座位预约系统 - 开发准备与 Sprint 0

> 文档版本：v1.0  
> 前置输入：docs/07-系统设计说明.md、docs/08-数据库设计.md、docs/09-关键链路详细设计.md  
> 下一步产出：开发前一致性总审计

---

## 1. 仓库结构

```
LibrarySeatSystem/              ← Git 仓库根目录（同时也是项目目录）
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
│   └── 项目任务板与迭代记录.md     ← 任务板
├── prototype/                   ← 静态原型
│   └── static-v1/
│       ├── *.html
│       └── css/custom.css
└── LibrarySeatSystem/           ← ASP.NET Core MVC 项目目录
    ├── Controllers/             ✓ 已有（Sprint 0 重构）
    ├── Models/                  ✓ 已有
    ├── ViewModels/              ◇ Sprint 0 新建
    ├── Services/                ◇ Sprint 0 新建
    ├── Filters/                 ◇ Sprint 0 新建
    ├── Data/                    ✓ 已有
    ├── Views/                   ✓ 已有
    ├── wwwroot/                 ✓ 已有
    ├── Program.cs               ✓ 已有（Sprint 0 补充 DI）
    ├── appsettings.json         ✓ 已有
    └── LibrarySeatSystem.csproj ✓ 已有
```

**注意：** 本项目采用 1 个仓库容纳 1 个项目的扁平结构，不创建外层 `.sln` 文件。  
如后续需要多项目（测试项目等），可在 Sprint 3 阶段补建 `.sln`。

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
| **M1** 骨架与数据层 | Sprint 0 | `dotnet build` 0 errors + `dotnet run` 自动建库 + 种子数据就位 + Service 层可被 Controller 调用 | Models + Data + Services + ViewModels + Filters + 重构后的 Controllers + Program.cs DI 配置 |
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
| T10-01 | 创建项目骨架（.csproj / .sln / 目录结构） | ① 确认 .csproj 存在且 `TargetFramework=net8.0` ② 确认 NuGet 包已配置（EF Core SqlServer + Tools）③ 按需创建 `.sln`（可选，1 个项目可以不建）④ 创建 ViewModels/ / Services/ / Filters/ 空目录 |
| T10-02 | 首次 `dotnet build` 和 `dotnet run` 验证 | ① `dotnet restore` 成功 ② `dotnet build` 0 errors 0 warnings ③ `dotnet run` 启动无异常 ④ 浏览器打开首页返回 200 ⑤ 确认 LocalDB 中已创建 LibrarySeatDb 库和三张表 |
| T10-03 | 复核 EF Core 实体类与 DbContext 关系 | ① 三实体类字段对齐 docs/08 §4 ② DbSet 三个属性 ③ OnModelCreating 配置 FK + `OnDelete(NoAction)` ④ User.Username 唯一索引 ⑤ `dotnet build` 无错误 |
| T10-04 | 确认种子数据与数据库自动初始化 | ① DbInitializer 含 1 管理员 + 3 学生 + 15 座位 ② Program.cs 中有 `EnsureCreated()` + `Seed()` 调用 ③ 删除数据库后重启能自动重建 |
| T10-05 | 实现 Service 层接口与实现 | ① ISeatService/SeatService 所有方法签名对齐 docs/09 ② IReservationService/ReservationService 所有方法签名对齐 docs/09 ③ IStatsService/StatsService 所有方法签名对齐 docs/09 ④ 包含冲突检测 / 编号查重 / CanCancel 计算 / 5 项统计口径 |
| T10-06 | 实现 ViewModels 数据类 | HomeIndexViewModel / SeatDetailViewModel / MyReservationsViewModel / AdminReservationsViewModel / AdminStatsViewModel / SeatWithStatus |
| T10-07 | 实现 AdminAuthFilter | IActionFilter 实现 + AJAX 兼容 + Program.cs 注册 + AdminController 标记 |
| T10-08 | 重构控制器：提取 Service 调用 | ① HomeController + AdminController 构造函数注入 Service ② 所有 `_db.` 直调改为 Service 方法 ③ 参数校验保留在 Controller ④ `dotnet build` 0 errors + 页面可正常打开 |
| T10-09 | 补充缺失的前端校验与 AJAX 交互 | ① 预约表单 date/time 约束 ② jQuery AJAX 调用 ③ toast/弹窗反馈统一 ④ `_ValidationScriptsPartial` 完整 |

### 5.3 Sprint 1 — 用户端开发（主 Sprint，可多轮推进）

**目标：** 链路一（学生预约座位）完全可走通。覆盖首页 → 座位列表 → 座位详情 → 预约提交 → 我的预约 + 取消子流程。

**阶段最低完成线：** 首页显示座位数和今日预约数 + 座位列表按楼层/区域筛选 + 座位详情展示预约记录 + 预约提交做冲突检测并成功写入 + 我的预约列表显示 CanCancel + 取消功能可用 + 账号切换可用。

**允许进入下一阶段条件（完成线检查点）：** 首页 + 座位列表 + 预约提交（成功+冲突两条路径）在浏览器中可完整操作。

**涉及的 docs/09 链路一方法：** `Index`、`Seats`、`Detail`、`Reserve(GET)`、`Reserve(POST)`、`MyReservations`、`Cancel`、`SwitchUser(GET/POST)`、`Logout`

| 任务卡 | 标题 |
|--------|------|
| T11-01 | 实现用户首页 |
| T11-02 | 实现座位列表页（筛选 + 占用标记） |
| T11-03 | 实现座位详情页 |
| T11-04 | 实现预约提交页（GET 表单） |
| T11-05 | 实现预约提交（POST AJAX + 冲突检测） |
| T11-06 | 实现我的预约页（含 CanCancel） |
| T11-07 | 实现取消预约（POST AJAX） |
| T11-08 | 实现账号切换与退出 |

### 5.4 Sprint 2 — 管理端开发（主 Sprint，可多轮推进）

**目标：** 链路二（管理员全流程管理）完全可走通。覆盖登录 → 座位管理（增/改/状态切换） → 预约管理（筛选） → 统计 → 退出。

**阶段最低完成线：** 管理员登录成功 + 座位列表显示全部座位 + 新增/编辑/切换状态可用 + 预约管理按状态和日期筛选 + 统计页显示四个板块数据。

**允许进入下一阶段条件（完成线检查点）：** 管理端 4 页可在浏览器中完整操作。

**涉及的 docs/09 链路二方法：** `Login(GET/POST)`、`Logout`、`Seats`、`SeatCreate(GET/POST)`、`SeatEdit(GET/POST)`、`SeatToggle`、`Reservations`、`Stats`

| 任务卡 | 标题 |
|--------|------|
| T12-01 | 实现管理端登录/登出 |
| T12-02 | 实现座位管理页（表格 + 启用/禁用行内操作） |
| T12-03 | 实现新增座位表单（含编号查重） |
| T12-04 | 实现编辑座位表单（含编号排除自身查重） |
| T12-05 | 实现座位状态切换（AJAX 行内交互） |
| T12-06 | 实现预约管理页（筛选 + 空状态） |
| T12-07 | 实现统计页（卡片 + 排行 + 趋势 + 空数据） |
| T12-08 | 实现 AdminAuthFilter 全页拦截 |

### 5.5 Sprint 3 — 集成与完善（主 Sprint，可多轮推进）

**目标：** 端到端全链路可用，高优先级原型评审问题修复，代码质量收尾，推送至远程仓库。

**阶段最低完成线：** 两条主链路在真实浏览器中完整走通无报错 + `dotnet build` 0 errors 0 warnings + README 可引导新用户成功启动。

**允许进入下一阶段条件：** 全部任务卡验证通过 → 项目交付。

| 任务卡 | 标题 |
|--------|------|
| T13-01 | 修复原型评审高优先级问题 |
| T13-02 | 端到端主链路集成测试（9 条验证场景） |
| T13-03 | 边界情况逐项验证（12 + 4 种异常场景） |
| T13-04 | 代码一致性审计（对照 docs/07/08/09 逐项核对） |
| T13-05 | 补充缺失注释与文档 |
| T13-06 | 推送至远程仓库（GitHub / Gitee） |

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

### 6.2 本地仓库初始化

```bash
cd D:\AiWeb\3
git init
git add .
git commit -m "chore: 初始化项目结构与设计文档"
git branch -M main
git checkout -b dev
```

### 6.3 远端仓库（待补远端推送）

> ⚠️ 当前未提供 GitHub 或 Gitee 远端地址，未执行远端推送。
> 获得远端地址后执行：

```bash
git remote add origin https://github.com/<用户名>/LibrarySeatSystem.git
# 或
git remote add origin https://gitee.com/<用户名>/LibrarySeatSystem.git

git push -u origin main
git push -u origin dev
```

### 6.4 首次推送后的分支保护

- GitHub：Settings → Branches → Add rule → `main` → Require pull request before merging（可选）
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
| 未建 `.sln` | 前序文档未要求。单项目时 `.csproj` 可直接 `dotnet run`，`.sln` 非必需 |
| Sprint 0 包含首次 build/run 验证 | 前序文档未在任务卡中显式提及。本文要求首个任务卡验证 build/run |
| 里程碑 ≤ 4 个 | 前序文档提及 M1→M2→M3，本文增加 M4 收尾，共 4 个，未超标 |
| 分支策略 | 前序文档未规定。本文采用 `main/dev/feat-xxx` 三级结构 |
| 提交规范 | 前序文档未规定。本文采用 `<type>(<scope>): <desc>` 格式 |
| 任务卡编号规则 | 前序文档未规定。本文采用 `T<主Sprint编号><序号>` 格式 |
| 端口冲突处理 | 前序文档提到端口 5000 曾被占用。本文在检查清单中补充端口检查 |
| README 目录结构分"已有/待生成" | 前序文档未要求，本文按本阶段要求做了区分 |
| "当前已存在"代码不满足设计文档 | 仓库现有 Controllers 直接使用 DbContext，不符合 docs/07 分层要求。Sprint 0 的核心工作就是重构到 Service 层 |
