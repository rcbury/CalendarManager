<template>
    <v-navigation-drawer
      v-model="drawer"
      permanent
      width="15%"
      app
    >
      <v-list>
        <v-list-item
          v-for="(item, i) in items"
          :key="i"
          :to="item.to"
          router
          exact
        >
          <v-list-item-action>
            <v-icon>{{ item.icon }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title>{{ item.title }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>

      <v-btn v-on="on" icon @click="toggleDarkTheme">
        <v-icon>
          {{ $vuetify.theme.dark ? 'mdi-white-balance-sunny' : 'mdi-moon-waxing-crescent' }}
        </v-icon>
      </v-btn>
    </v-navigation-drawer>
</template>

<script>
export default {
  name: 'NavigationDrawer',
  data () {
    return {
      clipped: false,
      drawer: true,
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
    }
  },

  methods: {
     toggleDarkTheme() {
        this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
        localStorage.setItem('DarkMode', this.$vuetify.theme.dark)
    }
  },

  created() {
    if (process.browser) {
      if (localStorage.getItem('DarkMode')) {
        this.$vuetify.theme.dark = localStorage.getItem('DarkMode') === 'true';
      }
    }
  }
}
</script>

<style lang="scss">
.v-navigation-drawer__content {
    button {
        bottom: 10px;
        left: 10px;
        position: absolute;
    }
}
</style>