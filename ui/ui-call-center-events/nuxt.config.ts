// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  devtools: { enabled: true },
  runtimeConfig: {
    api: {
      baseUrl: "http://localhost:8080/",
    },
  },
  modules: ["@nuxt/test-utils/module"],
});
