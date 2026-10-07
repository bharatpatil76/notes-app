import react from '@vitejs/plugin-react'
import { defineConfig } from 'vite'

// https://vite.dev/config/
export default defineConfig({
  plugins: [react()],
  // The API's CORS policy only allows http://localhost:5173, so fail instead of drifting to another port.
  server: { port: 5173, strictPort: true },
})
