<template>
  <v-navigation-drawer v-model="drawer" permanent width="15vw" app>
    <nuxt-link to="/profile" :custom="true">
      <div v-if="$auth.loggedIn" class="application-navigation-info">
        <v-avatar :color=stringToColor(this.$auth.user?.userName) size="45">
          <img v-if="$auth.user.avatarPath" :src="$auth.user.avatarPath" />
          <div v-else>
            {{ this.$auth.user.userName.at(0) }}
          </div>
        </v-avatar>
        <div class="application-navigation-info__username">{{ this.$auth.user.userName }}</div>
      </div>
    </nuxt-link>
    <v-list>
      <v-list-item v-for="(item, i) in commonItems" :key="i" :to="item.to" router exact>
        <v-list-item-action>
          <v-icon>{{ item.icon }}</v-icon>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title>{{ item.title }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
      <v-list-item v-for="(item, i) in authorizedItems" :key="i + 100" :to="item.to"
        :disabled="!$store.state.activeRoom.id" router exact>
        <v-list-item-action>
          <v-icon>{{ item.icon }}</v-icon>
        </v-list-item-action>
        <v-list-item-content>
          <v-list-item-title>{{ item.title }}</v-list-item-title>
        </v-list-item-content>
      </v-list-item>
    </v-list>

    <div class="application-navigation-footer">
      <v-btn v-on="on" icon @click="toggleDarkTheme">
        <v-icon>
          {{ $vuetify.theme.dark ? 'mdi-white-balance-sunny' : 'mdi-moon-waxing-crescent' }}
        </v-icon>
      </v-btn>
      <v-btn color="error" small :min-width="'75px'" @click="logout">
        logout
      </v-btn>
    </div>
  </v-navigation-drawer>
</template>

<script>
export default {
  name: 'NavigationDrawer',
  data() {
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
          icon: 'mdi-tune',
          title: 'Room management',
          to: `/roomManagement?roomId=${this.$store.state.activeRoom.id}`,
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
  },

  watch: {
    '$store.state.activeRoom.id': function () {
      //bad
      this.authorizedItems[0].to = `/roomManagement?roomId=${this.$store.state.activeRoom.id}`
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
    cursor: pointer;

    &__username {
      width: 70%;
      display: flex;
      justify-content: center;
      align-items: center;
    }
  }

  &-footer {
    display: flex;
    align-items: center;
    justify-content: space-between;
    margin-left: 10px;
    margin-right: 10px;
    margin-bottom: 10px;
  }
}

.v-navigation-drawer__content {
  display: flex;
  flex-direction: column;
  justify-content: space-between;

  .v-list {
    margin-bottom: 50vh;
  }
}
</style>
