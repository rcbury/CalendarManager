<template>
  <div class="application-login-main__authorization">
    <v-form ref="form">
      <v-text-field v-model="loginField" :counter="20" :rules="loginRules" label="Login" required></v-text-field>

      <v-text-field v-model="passwordField" :rules="passwordRules" type="password" label="Password"
        required></v-text-field>

      <div class="d-flex flex-column">
        <v-btn color="success" class="mt-4" block @click="validate">
          Login
        </v-btn>
      </div>

      <div class="d-flex mt-4 justify-space-between">
        <v-btn small @click="$emit('changeForm', 'register')">
          Registration
        </v-btn>
        <v-btn small color="info" @click="$emit('changeForm', 'forgout')">
          Forgot your password?
        </v-btn>
      </div>
    </v-form>
  </div>
</template>

<script>
export default {
  data: () => ({
    loginField: 'test@test.test',
    passwordField: 'Testtest123!',

    loginRules: [
      (v) => !!v || 'Login is required',
      (v) => (v && v.length <= 20) || 'Login must be less than 20 characters',
    ],

    passwordRules: [(v) => !!v || 'Password is required'],
  }),

  methods: {
    async validate() {
      const valid = await this.$refs.form.validate()
      if (valid) {
        //this.$store.commit('authorization/set', true);
        try {
          const requestBody = {
            Email: this.loginField,
            Password: this.passwordField,
          }

          await this.$auth.loginWith('refresh', { data: { ...requestBody } })

        } catch (error) {
          console.log(error)
        }
      }
    },

    reset() {
      this.$refs.form.reset()
    },
  },
}
</script>

<style lang="scss"></style>
