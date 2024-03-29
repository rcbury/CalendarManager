import colors from 'vuetify/es5/util/colors'

export default {
  // Global page headers: https://go.nuxtjs.dev/config-head
  head: {
    titleTemplate: '%s - Calendar',
    title: 'Calendar',
    htmlAttrs: {
      lang: 'ru',
    },
    meta: [
      { charset: 'utf-8' },
      { name: 'viewport', content: 'width=device-width, initial-scale=1' },
      { hid: 'description', name: 'description', content: '' },
      { name: 'format-detection', content: 'telephone=no' },
    ],
    link: [{ rel: 'icon', type: 'image/x-icon', href: '/favicon.ico' }],
  },

  // Global CSS: https://go.nuxtjs.dev/config-css
  css: [],

  // Plugins to run before rendering page: https://go.nuxtjs.dev/config-plugins
  plugins: [],

  // Auto import components: https://go.nuxtjs.dev/config-components
  components: true,

  // Modules for dev and build (recommended): https://go.nuxtjs.dev/config-modules
  buildModules: [
    // https://go.nuxtjs.dev/vuetify
    '@nuxtjs/vuetify',
  ],

  // Modules: https://go.nuxtjs.dev/config-modules
  modules: ['@nuxtjs/axios', '@nuxtjs/auth-next'],

  // router: {
  //   middleware: ['auth']
  // },

axios: {
    headers: {
      common: {
        'Accept': 'application/json, text/plain, */*',
        'ContentType': 'application/json; charset=utf-8'
      },
      delete: {},
      get: {},
      head: {},
      post: { 'Content-Type': 'application/json; charset=utf-8' },
      put: { 'Content-Type': 'application/json; charset=utf-8' },
      patch: {},
    },
    baseURL: 'https://localhost:7132/',
  },

  auth: {
    strategies: {
      refresh: {
        scheme: 'refresh',
        token: {
          property: 'accessToken',
          global: true,
          maxAge:  5,
          // required: true,
          // type: 'Bearer'
        },
        refreshToken: {
          property: 'refreshToken',
          data: 'refreshToken',
          maxAge: 60 * 10,
          // required: true,
          // type: 'Bearer'
        },
        user: {
          property: false,
          autoFetch: true,
        },
        endpoints: {
          login: { url: '/Authorization', method: 'post' },
          refresh: {
            url: '/Authorization/token',
            method: 'post',
          },
          logout: false,
          user: { url: '/User/self', method: 'get' },
        },
      },
    },
  },

  // Vuetify module configuration: https://go.nuxtjs.dev/config-vuetify
  vuetify: {
    treeShake: false,
    customVariables: ['~/assets/variables.scss'],
    theme: {
      dark: true,
      themes: {
        dark: {
          primary: colors.blue.darken2,
          accent: colors.grey.darken3,
          secondary: colors.amber.darken3,
          info: colors.teal.lighten1,
          warning: colors.amber.base,
          error: colors.deepOrange.accent4,
          success: colors.green.accent3,
        },
      },
    },
  },

  // Build Configuration: https://go.nuxtjs.dev/config-build
  build: {
    transpile: ['defu'],
  },
}
