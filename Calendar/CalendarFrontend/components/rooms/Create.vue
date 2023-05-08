<template>
  <div class="application-rooms-list__create">
    <v-form ref="form">
      <v-btn
          color="success"
          class="mt-4"
          block
          @click="validate"
      >
          Create
      </v-btn>
      
      <v-text-field
          v-model="roomNamesField"
          :counter="20"
          :rules="roomNamesRules"
          label="Write name rooms for create"
          required
      ></v-text-field>
    </v-form>
  </div>
</template>
  
<script>
export default {
  data: () => ({
    roomNamesField: '',

    roomNamesRules: [
      v => !!v || 'Field is required',
      v => (v && v.length <= 30) || 'Name rooms must be less than 30 characters',
    ],

    passwordRules: [
      v => !!v || 'Password is required'
    ],
  }),

  methods: {
    async validate  () {
      const valid = await this.$refs.form.validate()
      
      if (valid) {
        this.$emit('addRooms', this.roomNamesField)
        // this.roomNamesField = "";
      }
    },

    reset () {
      this.$refs.form.reset()
    }
  },
}
</script>

<style lang="scss">
main {
  height: 100vh;
}

.application {
  &-rooms {

    &-list__create form {
        display: grid;
        justify-content: center;
        grid-template-columns: 1fr 5fr;
        gap: 2vh;
    }
  }
}
</style>