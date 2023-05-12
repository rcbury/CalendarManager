<template>
    <v-navigation-drawer
      v-model="drawer"
      permanent
      width="15vw"
      app
    >
    <div v-if="$auth.loggedIn" class="application-navigation-info">
        <v-avatar
          :color=stringToColor(this.$auth.user?.userName)
          size="45"
        >{{ this.$auth.user.userName.at(0) }}</v-avatar>
        <div class="application-navigation-info__username">{{ this.$auth.user.userName }}</div>
    </div>
      <v-list>
        <v-list-item
          v-for="(item, i) in commonItems"
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
        <v-list-item
          v-if="$auth.loggedIn"
          v-for="(item, i) in authorizedItems"
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

      <div class=".d-flex .justify-space-between">
        <v-btn v-on="on" icon @click="toggleDarkTheme">
          <v-icon>
            {{ $vuetify.theme.dark ? 'mdi-white-balance-sunny' : 'mdi-moon-waxing-crescent' }}
          </v-icon>
        </v-btn>
        <v-btn 
          color="error" 
          small 
          :width=10 
          :min-width=10 
          class="mt-4" 
          block 
          @click="logout"
        >
          Test
        </v-btn>
      </div>
    </v-navigation-drawer>
</template>

<script>
export default {
  name: 'NavigationDrawer',
  data () {
    return {
      clipped: false,
      drawer: true,
      commonItems: [
        {
          icon: 'mdi-apps',
          title: 'Main menu',
          to: '/',
        },
        {
          icon: 'mdi-chart-bubble',
          title: 'Rooms',
          to: '/rooms',
        },
      ],
      authorizedItems: [
      {
          icon: 'mdi-chart-bubble',
          title: 'Room management',
          to: '/room?roomId={}',
          isShown: true,
        },
      ]
    }
  },

  methods: {
     toggleDarkTheme() {
        this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
        localStorage.setItem('DarkMode', this.$vuetify.theme.dark)
    },

    async logout() {
      await this.$auth.logout()
    },

    stringToColor(str) {
      if (!str) {
        return "#FFFFFF"
      }

      var hash = 0;
      for (var i = 0; i < str.length; i++) {
        hash = str.charCodeAt(i) + ((hash << 5) - hash);
      }

      var colour = '#';
      for (var i = 0; i < 3; i++) {
        var value = (hash >> (i * 8)) & 0xFF;
        colour += ('00' + value.toString(16)).substr(-2);
      }

      return colour;
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
.application-navigation {
  &-info {
    margin-top: 3vh;
    display: flex;
    justify-content: center;
    gap: 1vh;

    &__username {
      width: 70%;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }
}

.v-navigation-drawer__content {
    button {
        bottom: 10px;
        left: 10px;
        position: absolute;
    }

    .v-list {
      margin-top: 10vh;
    }
}
</style>
