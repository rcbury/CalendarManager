<template>
  <v-card class="mx-auto" max-width="100%" tile>
    <v-list rounded :min-width="400">
      <v-subheader>Room users</v-subheader>
      <v-list-item v-for="(user, i) in userList" :key="user.id">
        <v-list-item-content>
          <div class="d-flex justify-space-between align-center">
            <v-avatar class="mr-4" :color=stringToColor(user.userName.at(0)) size="45">
              <img v-if="user.avatarPath" :src="user.avatarPath" />
              <div v-else>
                {{ user.userName.at(0) }}
              </div>
            </v-avatar>
            <v-list-item-title
              v-text="`${user.userName} ${$auth.user.id === user.id ? '(you)' : ''}`"></v-list-item-title>
            <v-checkbox :min-width="100" :label="'is admin'"
              :disabled="!isRoomAdmin() || isLoading || $auth.user.id === user.id" :input-value="user.userRoleId === 1"
              @change="onAdminToggle(user.id)" />
            <v-btn :disabled="!isRoomAdmin() || isLoading || $auth.user.id === user.id" class="ml-5"
              @click="$emit('kickUser', user.id)" v-if="isRoomAdmin" fab small><v-icon>mdi-close</v-icon></v-btn>
          </div>

        </v-list-item-content>
      </v-list-item>
    </v-list>
  </v-card>
</template>
    
<script>
export default {
  props: ["userList"],
  name: "user-list",

  data() {
    return {
      isLoading: false,
    }
  },

  methods: {
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
    },

    isRoomAdmin() {
      return this.$store.state.activeRoom.authorizedUserRoleId === 1
    },
    async onAdminToggle(userId) {
      this.isLoading = true
      this.$emit('adminToggle', userId, () => { this.isLoading = false })
    }
  },

}
</script>
    
<style lang="scss"></style>