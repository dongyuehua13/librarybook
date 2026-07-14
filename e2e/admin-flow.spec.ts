import { test, expect } from '@playwright/test';

test.describe('管理端 e2e 主链路', () => {

  test('T13-02 SC04: 管理端登录 → 座位管理页', async ({ page }) => {
    await page.goto('/Admin/Login');
    await expect(page.locator('h4').filter({ hasText: '管理员登录' })).toBeVisible();

    await page.fill('input[name="username"]', 'admin');
    await page.click('button[type="submit"]');
    await page.waitForURL(/\/Admin\/Seats/, { timeout: 10000 });
  });

  test('管理端: 预约管理页可访问', async ({ page }) => {
    await page.goto('/Admin/Login');
    await page.fill('input[name="username"]', 'admin');
    await page.click('button[type="submit"]');
    await page.waitForURL(/\/Admin\/Seats/, { timeout: 10000 });

    await page.goto('/Admin/Reservations');
    await expect(page).toHaveURL(/\/Admin\/Reservations/);
  });

  test('管理端: 统计页可访问', async ({ page }) => {
    await page.goto('/Admin/Login');
    await page.fill('input[name="username"]', 'admin');
    await page.click('button[type="submit"]');
    await page.waitForURL(/\/Admin\/Seats/, { timeout: 10000 });

    await page.goto('/Admin/Stats');
    await expect(page).toHaveURL(/\/Admin\/Stats/);
  });

  test('管理端: 未登录访问被拦截重定向到登录页', async ({ page }) => {
    await page.goto('/Admin/Seats');
    await expect(page).toHaveURL(/\/Admin\/Login/);
  });
});
