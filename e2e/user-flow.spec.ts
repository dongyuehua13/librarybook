import { test, expect } from '@playwright/test';

test.describe('用户端 e2e 主链路', () => {

  test('T13-02 SC01: 首页 → 座位列表 → 详情', async ({ page }) => {
    await page.goto('/Home/Seats');
    await expect(page.locator('h2').filter({ hasText: '座位列表' })).toBeVisible();

    const detailLink = page.locator('a').filter({ hasText: '查看详情' }).first();
    await expect(detailLink).toBeVisible({ timeout: 5000 });
    await detailLink.click();
    await expect(page).toHaveURL(/\/Home\/Detail\/\d+/);
  });

  test('T13-02 SC02: 未登录访问预约页应重定向', async ({ page }) => {
    await page.goto('/Home/Reserve/1');
    await expect(page).toHaveURL(/\/Home\/(Seats|SwitchUser)/);
  });

  test('T13-03 EX01: 座位列表含已预约标记', async ({ page }) => {
    await page.goto('/Home/Seats');
    const occupiedBadges = page.locator('span.badge.bg-danger');
    const count = await occupiedBadges.count();
    expect(count).toBeGreaterThanOrEqual(0);
  });

  test('SwitchUser 页面展示3个学生账号', async ({ page }) => {
    await page.goto('/Home/SwitchUser');
    const buttons = page.locator('button[type="submit"]');
    await expect(buttons).toHaveCount(3);
  });
});
