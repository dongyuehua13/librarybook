import { defineConfig } from '@playwright/test';

export default defineConfig({
  testDir: '.',
  timeout: 30000,
  retries: 1,
  use: {
    channel: 'msedge',
    headless: true,
    baseURL: 'http://localhost:5002',
    screenshot: 'only-on-failure',
    trace: 'retain-on-failure',
  },
  projects: [
    {
      name: 'msedge',
      use: {
        channel: 'msedge',
        viewport: { width: 1280, height: 720 },
      },
    },
  ],
});
