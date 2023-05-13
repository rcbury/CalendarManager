<template>
  <v-card class="mx-auto" max-width="100%" tile>
    <v-list rounded>
      <v-subheader>List Rooms</v-subheader>
      <v-list-item-group v-model="selectedRoom" color="primary">
        <v-list-item v-for="(room, i) in roomsList" :key="room.id" @click="() => onSelect(room.id)">
          <v-list-item-content>
            <v-list-item-title v-text="room.name"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </v-card>
</template>

<script>
export default {
  props: ["roomsList"],
  data() {
    console.log(this.roomsList)
    return {
      selectedRoom: this.findRoomIndexById()
    }
  },
  methods: {
    async onSelect(roomId) {
      const activeRoomId = this.$store.state.activeRoom.id
      const newRoomId = roomId == activeRoomId ? undefined : roomId
      const authorizedUserId = this.$auth.user.id;

      if (newRoomId === undefined) {
        this.$store.commit("activeRoom/setAuthorizedUserRoleId", 0)
      } else {
        try {
          let authorizedUserRole = await this.$axios.$get(`/Room/${newRoomId}/User/${authorizedUserId}/Role`)
          this.$store.commit("activeRoom/setAuthorizedUserRoleId", authorizedUserRole.id)
        }
        catch{
          this.$store.commit("activeRoom/setAuthorizedUserRoleId", 0)
        }
      }

      this.$store.commit("activeRoom/setId", newRoomId)
    },

    findRoomIndexById(rooms) {
      if (rooms === undefined) return undefined

      let foundIndex = rooms
        .findIndex(room => room.id == this.$store.state.activeRoom.id)

      return foundIndex === -1 ? undefined : foundIndex
    },

    isActive(roomId) {
      const currentSelectedRoom = this.$store.state.activeRoom.id
      return roomId === currentSelectedRoom
    },
  },

  watch: {
    'roomsList'(newValue) {
      this.selectedRoom = this.findRoomIndexById(newValue)
    }
  },

}
</script>

<style lang="scss">
.application-rooms-list {
  &__fields {
    display: grid;
  }

  &__field {
    display: flex;
    align-items: center;
    margin-top: 3vh;
    height: 5vh;
    border-radius: 2px red;
  }
}
</style>