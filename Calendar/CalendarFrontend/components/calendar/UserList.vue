<template>
  <v-select
    v-model="changeUsers"
    item-text="userName"
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
            this.users.push({userName: item.userName, id: item.id, email: item.email, firstName: item.firstName, lastName: item.lastName, avatarPath: item.path})
        }
    },

    created() {
        if (this.selectUsers) {
            for (var item of this.selectUsers) {
                this.changeUsers.push({userName: item.userName, id: item.id, email: item.email, firstName: item.firstName, lastName: item.lastName, avatarPath: item.path})
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