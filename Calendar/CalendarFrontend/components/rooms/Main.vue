<template>
  <div class="application-rooms__container">
    <div class="application-rooms">
      <RoomsCreate @addRooms="addRooms" />
      <RoomsList :roomsList="roomsList" @roomDelete="fetchRooms"/>
    </div>
  </div>
</template>

<script>
export default {
  data: () => ({
    roomsList: []
  }),

  methods: {
    async addRooms(name) {
      let requestBody = {
        name: name
      }

      await this.$axios.post("/Room", { ...requestBody })

      await this.fetchRooms()
    },

    onSelect(){

    },

    async fetchRooms() {
      try {
        let rooms = await this.$axios.$get("/Room")

        this.roomsList = rooms
        console.log(this.roomsList)
      } catch (error) {
        console.log(error)
      }
    }
  },

  async mounted() {
    await this.fetchRooms()
  }
}
</script>

<style lang="scss">
.application {
  &-rooms v-card {
    grid-template-rows: 1fr;
    width: 70%;
    margin-top: 3vh;
  }

  &-rooms__container {
    display: flex;
    justify-content: center;
    margin-top: 10%;
  }
}

.v-card {
  margin-top: 3vh;
}
</style>