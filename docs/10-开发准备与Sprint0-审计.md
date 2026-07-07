# 图书馆座位预约系统 - 开发准备与 Sprint 0 审计

> 审计日期：2026-07-07  
> 审计对象：README.md、docs/10-开发准备与Sprint0.md、docs/项目任务板与迭代记录.md  
> 审计依据：docs/07-系统设计说明.md、docs/08-数据库设计.md、docs/09-关键链路详细设计.md

---

## A. 总体判断

**结论：不予通过 — 存在 5 项 P0 必改项，全部修正后方可进入下一阶段。**

开发准备文档体系（docs/10 + 任务板 + README）在框架设计层面整体到位：分支策略、提交规范、里程碑、Sprint 规划、任务卡粒度、补足项说明均完整清晰，Sprint 0 实际交付内容与任务卡状态一致。但 README 作为仓库入口文档，存在**多处与实际项目结构严重不符**的硬伤，直接导致新用户按 README 指引无法启动项目。此外 Git 留痕尚未执行，docs/10 中远端地址信息已过时。

---

## B. 发现的问题清单

| 编号 | 严重度 | 文件 | 位置 | 问题描述 |
|------|--------|------|------|---------|
| P0-01 | **P0** | README.md | §技术栈 — .NET 10 | 声称 ".NET 10" 和 "EF Core 10.x"，实际 `LibrarySeatSystem.csproj` 中 `TargetFramework` 为 `net8.0`，NuGet 包为 EF Core 8.x。新用户看到 .NET 10 要求会误装错误 SDK 版本 |
| P0-02 | **P0** | README.md | §目录结构 + §快速启动 | 展示的目录结构为 `src/LibrarySeatReservation.Web/`，但实际项目直接位于 `LibrarySeatSystem/` 下，无 `src/` 嵌套。快速启动 `cd src/LibrarySeatReservation.Web` 命令在新的 clone 环境中将报错 |
| P0-03 | **P0** | README.md | §快速启动 + §数据库初始化方式 | 两处均描述使用 `dotnet ef database update`（迁移模式），但实际 `Program.cs` 使用 `db.Database.EnsureCreated()` + `DbInitializer.Seed()`，无 Migrations 目录。用户按 README 执行 `dotnet ef database update` 将因缺少迁移文件而失败 |
| P0-04 | **P0** | docs/10 §6.3 | 远端仓库 | 文档标明 "当前未提供 GitHub 或 Gitee 远端地址"，但用户已提供 GitHub 仓库 `https://github.com/dongyuehua13/librarybook` 及访问令牌。文档未更新且尚未推送 |
| P0-05 | **P0** | — | Git 留痕 | 当前环境未安装 Git，未执行 `git init / git add / git commit`。README 中 GitHub 地址已存在但远端仓库为空。无任何版本留痕 |
| P1-01 | **P1** | README.md + docs/10 | §目录结构 | 两文件均标注 `Migrations/ ✓ 已有`。实际不存在 `Migrations/` 目录（项目使用 `EnsureCreated()` 而非迁移）。应改为 `◇ N/A (使用 EnsureCreated)` |
| P1-02 | **P1** | README.md | §目录结构 | 展示 `Services/Interfaces/` 子目录结构，实际 4 个接口文件（ISeatService.cs 等）直接放在 `Services/` 根目录下，无 `Interfaces/` 子目录 |
| P1-03 | **P1** | README.md | §已实现范围 — 种子数据 | 写着 "45 个座位：3层×3区域×5"，实际 `DbInitializer.cs` 中种子数据为 15 个座位（3层×1区域×5：A-01~A-05 / B-01~B-05 / C-01~C-05） |
| P1-04 | **P1** | 任务板 | T10-09 验收标准 | 任务卡要求 "AJAX 反馈统一使用 Bootstrap Toast，不弹浏览器默认 alert()"。实际代码中 `MyReservations.cshtml` 的取消操作仍使用 `alert(res.msg)`，未替换为 Toast |
| P1-05 | **P1** | docs/10 §1 | 目录树 | `LibrarySeatReservation.sln` 标注 "✓ 已有"，实际无 `.sln` 文件。单项目可不建，但标记应与实际一致：改为 "◇ 按需（单项目可不建）" 或直接移除 |

---

## C. 必改项（5 项）

### P0-01：技术栈版本号修正
- **目标文件：** `README.md` §技术栈
- **修正要求：** 将 `.NET 10` → `.NET 8`，`EF Core 10.x` → `EF Core 8.x`
- **关联文档：** `LibrarySeatSystem.csproj` 中 `TargetFramework=net8.0`

### P0-02：目录结构与实际对齐
- **目标文件：** `README.md` §目录结构 + §快速启动
- **修正要求：**
  - 目录树根节点改为实际项目的 `LibrarySeatSystem/`（而非 `src/LibrarySeatReservation.Web/`）
  - 移除 `src/` 层，或如实描述项目在当前工作区的位置
  - 快速启动中 `cd` 路径同步修正
  - 页面清单表中路由路径是否正确？当前路由 `Home/Seats` 等正确，不需改动

### P0-03：数据库初始化方式修正
- **目标文件：** `README.md` §快速启动 + §数据库初始化方式
- **修正要求：**
  - 将 `dotnet ef database update` 改为对 `EnsureCreated()` + `Seed()` 的描述
  - 快速启动步骤中移除迁移步骤，改为 `dotnet run` 后自动建库
  - 删除或重写 "手动重建数据库" 小节（EnsureCreated 不支持增量变更）

### P0-04：远端地址信息更新
- **目标文件：** `docs/10-开发准备与Sprint0.md` §6.3
- **修正要求：** 将 "未提供远端地址" 替换为实际地址，并注明 Git 未安装、未推送的状态

### P0-05：Git 初始化和首次推送
- **目标文件：** —（操作项）
- **修正要求：**
  - 安装 Git 或切换到有 Git 的环境
  - 执行 `git init / git add / git commit`
  - 推送至 `https://github.com/dongyuehua13/librarybook.git`
  - 明确记录执行时间与环境

---

## D. 建议优化项（5 项）

### P1-01：Migrations 标记修正
- 将 README 和 docs/10 中 `Migrations/ ✓ 已有` 改为 `◇ N/A — 使用 EnsureCreated()`
- 如果未来转到迁移模式可在 Sprint 3 添加

### P1-02：Services 目录结构对齐
- 两个方案：① 在 `README.md` 目录树中将接口与实现平级列出（与实际一致） ② 在 `Services/` 下实际创建 `Interfaces/` 子目录并移动接口文件
- 推荐方案①（不改代码，只修文档）

### P1-03：种子数据数量修正
- `README.md` §已实现范围：`45 个座位` → `15 个座位`
- 同时核实描述格式使之与实际 DbInitializer.Seed() 保持一致

### P1-04：前端 Toast 替换 alert
- `Views/Home/MyReservations.cshtml` 取消操作的 `alert(res.msg)` 改为 Bootstrap Toast
- 这是 T10-09 验收标准的未完成项

### P1-05：.sln 标记修正
- `docs/10` §1 目录树中 `LibrarySeatReservation.sln ✓ 已有` → 移除或改为 `◇ 按需（单项目可不建）`

---

## E. 是否可以进入下一步"开发前一致性总审计"

**裁定：❌ 不可进入 — 等待必改项修正**

如果全部 5 项 P0 在本轮修正完毕 + `dotnet build` 保持 0 errors，可进入下一阶段。

修正完成后建议执行：
1. 依次修正 P0-01 ~ P0-05
2. `dotnet build` 0 errors 验证
3. 更新 3 个审计对象文件的版本号/日期
4. 在本文件末尾追加 "问题清零确认" 表格
5. 重新评估结论

---

> 审计人：opencode（自动审计）
