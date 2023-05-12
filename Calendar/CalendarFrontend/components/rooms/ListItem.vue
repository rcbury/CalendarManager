<template>
  <v-list-item-content>
    <v-list-item-title v-text="room.name"></v-list-item-title>
  </v-list-item-content>
</template>

<script>
export default {
  props: ["room"],
}
</script>

<style lang="scss">

</style>