<template>
  <v-card class="mx-auto" max-width="100%" tile>
    <v-list rounded :min-width="400">
      <v-subheader>Room users</v-subheader>
      <v-list-item v-for="(user, i) in userList" :key="user.id">
        <v-list-item-content>
          <div class="d-flex justify-space-between">

            <v-list-item-title v-text="`${user.userName} ${$auth.user.id === user.id ? '(you)' : ''}`"></v-list-item-title>
            <v-checkbox :min-width="100" :label="'is admin'" :disabled="!isRoomAdmin() || isLoading || $auth.user.id === user.id"
              :input-value="user.userRoleId === 1" @change="onAdminToggle(user.id)" />
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
    isRoomAdmin() {
      return this.$store.state.activeRoom.authorizedUserRoleId === 1
    },
    async onAdminToggle(userId) {
      this.isLoading = true
      this.$emit('adminToggle', userId, () => {this.isLoading = false})
    }
  },

  watch: {
    userList: (value) => {
      console.log(value)
    }

  },

}
</script>
    
<style lang="scss"></style>