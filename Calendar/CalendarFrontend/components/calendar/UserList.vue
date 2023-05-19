<template>
  <v-select
    v-model="changeUsers"
    item-text="UserName"
    :items="users"
    label="Select"
    multiple
    hint="Pick users for task"
    persistent-hint
    return-object
    @change="onChangeUser"
  ></v-select>
</template>

<script>
export default {
    props: ["selectUsers"],
    
    data () {
      return {
        users: [],
        changeUsers: [],
      }
    },

    async fetch() {
        const data = await this.$axios.$get(`/Room/${this.$store.state.activeRoom.id}/Users`)
        
        for (var item of data) {
            this.users.push({UserName: item.userName, id: item.id, Email: item.email, FirstName: item.firstName, LastName: item.lastName})
        }
    },

    created() {
        if (this.selectUsers) {
            for (var item of this.selectUsers) {
                this.changeUsers.push({UserName: item.userName, id: item.id, Email: item.email, FirstName: item.firstName, LastName: item.lastName})
            }
        }
    },

    methods: {
        onChangeUser(value) {
            this.$emit('change', value);
        }
    }
}
</script>

<style>

</style>