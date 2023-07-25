import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    host: "0.0.0.0",
    port: 3000,
    // port: 80, this port is for production
  },
  plugins: [react()],
  resolve: {
    alias: {
      src: "/src",
      components: "/src/components",
    },
  },
});
