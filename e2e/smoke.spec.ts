import { test, expect } from '@playwright/test';

test.describe('烟雾测试 - 页面可访问', () => {

  test('首页 200', async ({ page }) => {
    const resp = await page.goto('/');
    expect(resp?.status()).toBe(200);
  });

  test('座位列表页 200', async ({ page }) => {
    const resp = await page.goto('/Home/Seats');
    expect(resp?.status()).toBe(200);
  });

  test('管理员登录页 200', async ({ page }) => {
    const resp = await page.goto('/Admin/Login');
    expect(resp?.status()).toBe(200);
  });

  test('座位详情页 200', async ({ page }) => {
    const resp = await page.goto('/Home/Detail/1');
    expect(resp?.status()).toBe(200);
  });
});
