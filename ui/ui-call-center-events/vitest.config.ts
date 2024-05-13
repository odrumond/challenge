import { defineConfig } from "vitest/config";
import vue from "@vitejs/plugin-vue";
import { resolve as r } from "path";

export default defineConfig({
  plugins: [vue()],
  test: {
    environment: "happy-dom",
    include: ['**/*.spec.ts'],
  },
  resolve: {
    alias: {
      "~~": r("."),
      "~~/": r("./"),
      "@@": r("."),
      "@@/": r("./"),
    },
  },
});
