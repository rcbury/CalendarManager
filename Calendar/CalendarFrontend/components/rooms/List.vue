<template>
  <v-card class="mx-auto" max-width="100%" tile>
    <v-list rounded>
      <v-subheader>List Rooms</v-subheader>
      <v-list-item-group v-model="selectedRoom" color="primary">
        <v-list-item v-for="(room, i) in roomsList" :key="room.id">
          <template v-slot:default="{ active }">
            <v-list-item-content class="d-flex flex-row justify-space-between">
              <v-list-item-title v-text="room.name"></v-list-item-title>

            </v-list-item-content>
            <v-list-item-action>
              <v-btn v-if="room.authorId === $auth.user.id" fab small color="error"
                @click="deleteRoom(room.id)"><v-icon>mdi-delete</v-icon></v-btn>
            </v-list-item-action>
          </template>
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

    async deleteRoom(roomId) {
      try {
        await this.$axios.$delete(`/Room/${roomId}`)
        this.$emit('roomDelete', roomId)
      }
      catch (error) {
        console.log(error)
      }

    }
  },

  watch: {
    'roomsList'(newValue) {
      this.selectedRoom = this.findRoomIndexById(newValue)
    },
    async selectedRoom(newRoomIndex) {
      const activeRoomId = this.$store.state.activeRoom.id
      const authorizedUserId = this.$auth.user.id;
      const newRoomId = newRoomIndex === undefined ? 0 : this.roomsList[newRoomIndex].id

      if (newRoomId === 0) {
        this.$store.commit("activeRoom/setAuthorizedUserRoleId", 0)
      } else {
        try {
          let authorizedUserRole = await this.$axios.$get(`/Room/${newRoomId}/User/${authorizedUserId}/Role`)
          this.$store.commit("activeRoom/setAuthorizedUserRoleId", authorizedUserRole.id)
        }
        catch {
          this.$store.commit("activeRoom/setAuthorizedUserRoleId", 0)
        }
      }

      this.$store.commit("activeRoom/setId", newRoomId)
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