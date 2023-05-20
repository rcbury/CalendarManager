<template>
  <div class="application-login-main__authorization">
    <v-form ref="form">
      <v-text-field v-model="loginField" :counter="62" :rules="loginRules" :error-messages="Array.isArray(backendErrorMessages) ? backendErrorMessages
                .filter(x => x.code.toLowerCase().includes('email'))
                .map(error => error.description) : []" label="Email" required></v-text-field>

      <v-text-field v-model="passwordField" :rules="passwordRules" :error-messages="Array.isArray(backendErrorMessages) ? backendErrorMessages
                .filter(x => x.code.toLowerCase().includes('password'))
                .map(error => error.description) : []" type="password" label="Password"
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
    backendErrorMessages: {},

    loginRules: [
      (v) => !!v || 'Login is required',
      (v) => (v && v.length <= 62) || 'Login must be less than 62 characters',
    ],

    passwordRules: [(v) => !!v || 'Password is required'],
  }),

  methods: {
    async validate() {
      const valid = await this.$refs.form.validate()
      if (valid) {
        try {
          const requestBody = {
            Email: this.loginField,
            Password: this.passwordField,
          }

          await this.$auth.loginWith('refresh', { data: { ...requestBody } })

        } catch (error) {
          console.log(error)
          this.backendErrorMessages = error.response.data.errors;
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
