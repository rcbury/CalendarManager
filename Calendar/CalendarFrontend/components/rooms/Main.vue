<template>
  <div class="application-rooms">
    <RoomsCreate @addRooms="addRooms" />
    <RoomsList :roomsList="roomsList" />
  </div>
</template>

<script>
export default {
  data: () => ({
      roomsList: [
          {
              name: "Команда 1",
              id: 12
          },
          {
              name: "Команда 2",
              id: 13
          },
          {
              name: "Команда 3",
              id: 13
          }
      ]
  }),
  
  methods: {
    addRooms(name) {
      this.roomsList.push({name: name, id: -1})
    }
  }
}
</script>

<style lang="scss">
.v-main__wrap {
  .container {
    margin-top: 10%;
    width: 70%;
  }
}

.application {
  &-rooms v-card {
    grid-template-rows: 1fr;
    margin-top: 3vh;
  }
}

.v-card {
  margin-top: 3vh;
}
</style>