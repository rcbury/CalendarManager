<template>
<v-card
    class="mx-auto"
    max-width="100%"
    tile
  >
    <v-list rounded>
      <v-subheader>List Rooms</v-subheader>
      <v-list-item-group
        v-model="selectedItem"
        color="primary"
      >
        <v-list-item
          v-for="(item, i) in roomsList"
          :key="i"
        >
          <v-list-item-content>
            <v-list-item-title v-text="item.name"></v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list-item-group>
    </v-list>
  </v-card>
</template>

<script>
export default {
    props: ["roomsList"]
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