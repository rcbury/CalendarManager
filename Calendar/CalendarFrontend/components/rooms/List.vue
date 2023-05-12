<template>
<v-card
    class="mx-auto"
    max-width="100%"
    tile
  >
    <v-list rounded>
      <v-subheader>List Rooms</v-subheader>
      <v-list-item-group
        v-model="selectedRoom"
        color="primary"
      >
        <v-list-item
          v-for="(room, i) in roomsList"
          :key="room.id"
          @click="() => onSelect(roomId)"
        >
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
  data(){
    console.log(this.$store.state.activeRoom.id)
    return {
      selectedRoom: this.$store.state.activeRoom.id
    }
  },
  methods: {
    onSelect(roomId) {
      this.$store.commit("activeRoom/setId", roomId)
      console.log(this.selectedRoom)
    //  this.selectedRoom = this.$store.state.activeRoom.id
    },

    isActive(roomId) {
      console.log(roomId)
      const currentSelectedRoom = this.$store.state.activeRoom.id
      console.log(roomId === currentSelectedRoom)
      return roomId === currentSelectedRoom
    },
  },
  computed: {
    
  },

  mounted () {
    console.log('value on mount: ' + this.$store.state.activeRoom.id)
    this.selectedRoom = this.$store.state.activeRoom.id
  }
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