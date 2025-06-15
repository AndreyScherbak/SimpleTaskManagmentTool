import { defineConfig } from 'vite';
import react from '@vitejs/plugin-react';
import tsconfigPaths from 'vite-tsconfig-paths';

export default defineConfig({
  plugins: [react(), tsconfigPaths()],
  server: {
    port: 5173,
    open: true,
    proxy: {
      '/api': {
        target: 'https://localhost:7273',
        changeOrigin: true,
        secure: false,
      },
    },
  },
});
