<template>
  <v-app>
    <div class="application-core" v-if="$auth.loggedIn">
      <NavigationDrawer/>

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
      title: 'Календарь 3000'
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
