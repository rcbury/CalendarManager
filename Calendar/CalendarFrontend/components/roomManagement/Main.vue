<template>
  <div class="room-management__container">
    <div class="room-management">
      <div v-if="$store.state.activeRoom.authorizedUserRoleId === 1" class="room-management-invite-link">
        <v-text-field :hint="'link will be active for a month'" label="Invite link" persistent-hint v-model="inviteLink" readonly />
        <v-btn color="success" small @click="getInviteLink">Refresh</v-btn>

      </div>
      <RoomManagementUserList :userList="userList" @adminToggle="toggleAdmin" />
    </div>
  </div>
</template>
  
<script>
export default {
  data: () => ({
    userList: [],
    inviteLink: "",
  }),
  
  methods: {
    async fetchUsers() {
      try {
        let users = await this.$axios.$get(`/Room/${this.$store.state.activeRoom.id}/Users`)

        this.userList = users
      } catch (error) {
        console.log(error)
      }
    },

    async toggleAdmin(userId, callback) {
      try {
        let result = await this.$axios.$put(`/Room/${this.$store.state.activeRoom.id}/toggleAdmin?userId=${userId}`)

        await this.fetchUsers()
      } 
      catch
      {

      }
      callback()
    },

    async getInviteLink() {
      try {
        let inviteLink = await this.$axios.$get(`/Room/${this.$store.state.activeRoom.id}/inviteLink`)
        
        this.inviteLink = inviteLink.inviteLink
      } 
      catch (error)
      {
        console.log(error)
      }
    }
  },

  async beforeMount() {
    if (this.$store.state.activeRoom.authorizedUserRoleId === 1) {
      await this.getInviteLink()
    }
    await this.fetchUsers()
  }
}
</script>
  
<style lang="scss">
.room-management {
  width: 60%;
  min-width: 300px;

  &__container {
    display: flex;
    justify-content: center;
    margin-top: 10%;
  }

  &-invite-link {
    display: flex;
    align-items: center;
    column-gap: 20px;
  }
}
</style>