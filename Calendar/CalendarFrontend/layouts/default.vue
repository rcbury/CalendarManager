<template>
  <v-app>
    <div class="application-core" v-if="isAuthorization">
      <NavigationDrawer/>
      
      <v-app-bar
        :clipped-left="clipped"
        fixed
        app
      >
      </v-app-bar>
      <v-main>
        <v-container>
          <Nuxt />
        </v-container>
      </v-main>
    </div>


    <div class="application-login" v-else>
        <LoginForm/>
    </div>
  </v-app>
</template>

<script>
export default {
  name: 'DefaultLayout',
  data () {
    return {
      clipped: false,
      drawer: true,
      fixed: false,
      items: [
        {
          icon: 'mdi-apps',
          title: 'Welcome',
          to: '/'
        },
        {
          icon: 'mdi-chart-bubble',
          title: 'Inspire',
          to: '/inspire'
        }
      ],
      miniVariant: false,
      title: 'Vuetify.js'
    }
  },

  computed: {
    isAuthorization() {
      return this.$store.state.authorization.isAuthorization;
    }
  },
}
</script>

<style lang="scss">
.application {
  &-login {
    height: 100%;
    width: 100%;
  }
}
</style>