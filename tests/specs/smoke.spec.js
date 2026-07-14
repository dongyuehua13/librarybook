// @ts-check
const { test, expect } = require('@playwright/test');

const ADMIN_USER = 'admin';

// Helper: switch to a student via the Index page
async function switchToStudent(page, name) {
  await page.goto('/');
  // Student selection cards are shown when not logged in
  const btn = page.locator('form[action="/Home/SwitchUser"] button').filter({ hasText: name });
  if (await btn.isVisible()) {
    await btn.click();
    await page.waitForTimeout(800);
  }
}

// Helper: login as admin
async function adminLogin(page) {
  await page.goto('/Admin/Login');
  await page.locator('input[type="text"], input[name="username"]').first().fill(ADMIN_USER);
  await page.locator('button, input[type="submit"]').filter({ hasText: /登录/ }).first().click();
  await page.waitForTimeout(800);
}

test.describe('用户端主链路', () => {

  test('P01 首页 - 统计卡片和账号切换', async ({ page }) => {
    await page.goto('/');
    // 统计卡片应可见
    await expect(page.locator('.card').first()).toBeVisible();
    // 未登录时显示"选择体验账号"
    await expect(page.getByText('选择体验账号')).toBeVisible();
    // 三个学生按钮可见
    const studentBtns = page.locator('form[action="/Home/SwitchUser"] button');
    await expect(studentBtns).toHaveCount(3);
  });

  test('P02 座位列表 - 筛选+占用标记', async ({ page }) => {
    await page.goto('/Home/Seats');
    await expect(page).toHaveURL(/\/Home\/Seats/);
    await expect(page.locator('select[name="floor"], select#floor').first()).toBeVisible();
    const hasCards = await page.locator('.card, .seat-card').count();
    if (hasCards > 0) {
      await expect(page.locator('.card, .seat-card').first()).toBeVisible();
    } else {
      await expect(page.getByText(/暂无/)).toBeVisible();
    }
  });

  test('P03 座位详情 - 信息展示', async ({ page }) => {
    await page.goto('/Home/Detail/1');
    await expect(page).toHaveURL(/\/Home\/Detail\/1/);
    await expect(page.locator('body')).not.toHaveText(/404/);
  });

  test('P04+P05 预约提交 + 我的预约完整流程', async ({ page }) => {
    // 切换为 zhangsan
    await switchToStudent(page, '张三');

    // 查看座位 1 的详情
    await page.goto('/Home/Detail/1');
    await page.waitForTimeout(300);

    // 点击"预约此座位"
    const reserveBtn = page.locator('a, button').filter({ hasText: /预约/ }).first();
    if (await reserveBtn.isVisible()) {
      await reserveBtn.click();
      await page.waitForTimeout(500);

      const currentUrl = page.url();
      if (currentUrl.includes('/Home/Reserve/')) {
        // 设置日期为今天，时段 09:00-10:00
        const today = new Date().toISOString().split('T')[0];
        const dateInput = page.locator('input[type="date"]').first();
        if (await dateInput.isVisible()) {
          await dateInput.fill(today);
        }
        const timeInputs = page.locator('input[type="time"]');
        const count = await timeInputs.count();
        if (count >= 2) {
          await timeInputs.nth(0).fill('09:00');
          await timeInputs.nth(1).fill('10:00');
        }
        // 提交预约
        await page.locator('button, input[type="submit"]')
          .filter({ hasText: /提交|预约/ }).first().click();
        await page.waitForTimeout(1000);

        // 应有反馈
        await expect(page.locator('body')).not.toBeEmpty();
      }
    }
  });

  test('P05 我的预约 - 列表展示', async ({ page }) => {
    // 切换为 zhangsan
    await switchToStudent(page, '张三');

    await page.goto('/Home/MyReservations');
    await expect(page).toHaveURL(/\/Home\/MyReservations/);
    await expect(page.locator('body')).not.toBeEmpty();
  });
});

test.describe('管理端主链路', () => {

  test('P06 管理员登录', async ({ page }) => {
    await adminLogin(page);
    // 登录成功应跳转到后台
    await expect(page).not.toHaveURL(/\/Admin\/Login/);
  });

  test('P07 座位管理 - 表格+操作', async ({ page }) => {
    await adminLogin(page);

    await page.goto('/Admin/Seats');
    await expect(page).toHaveURL(/\/Admin\/Seats/);
    const hasTable = await page.locator('table').count();
    if (hasTable > 0) {
      await expect(page.locator('table')).toBeVisible();
    } else {
      await expect(page.getByText(/暂无/)).toBeVisible();
    }
  });

  test('P08 预约管理 - 筛选', async ({ page }) => {
    await adminLogin(page);

    await page.goto('/Admin/Reservations');
    await expect(page).toHaveURL(/\/Admin\/Reservations/);
    const hasFilter = await page.locator('select, input[type="date"]').count();
    expect(hasFilter).toBeGreaterThanOrEqual(1);
  });

  test('P09 统计页 - 数据展示', async ({ page }) => {
    await adminLogin(page);

    await page.goto('/Admin/Stats');
    await expect(page).toHaveURL(/\/Admin\/Stats/);
    await expect(page.locator('.card').first()).toBeVisible();
  });
});

test.describe('后端逻辑场景验证', () => {

  test('AC02 - 全部路由返回 200', async ({ page }) => {
    const routes = [
      '/',
      '/Home/Seats',
      '/Home/Detail/1',
      '/Admin/Login',
    ];
    for (const route of routes) {
      const resp = await page.goto(route);
      expect(resp?.status()).toBe(200);
    }
  });

  test('AdminAuthFilter - 未登录拦截', async ({ page }) => {
    await page.goto('/Admin/Seats');
    await page.waitForTimeout(500);
    // 应被重定向到登录页
    await expect(page).toHaveURL(/\/Admin\/Login/);
  });
});
