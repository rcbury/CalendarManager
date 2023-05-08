<template>
  <div class="application-rooms__container">
    <div class="application-rooms">
      <RoomsCreate @addRooms="addRooms" />
      <RoomsList :roomsList="roomsList" />
    </div>
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