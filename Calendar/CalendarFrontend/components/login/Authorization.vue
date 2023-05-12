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
        <v-btn color="success" class="mt-4" block @click="test">
          Test
        </v-btn>
      </div>

      <v-btn class="mt-4" @click="$emit('changeForm', 'register')">
        Registration
      </v-btn>
      <v-btn class="mt-4 flex" color="info" @click="$emit('changeForm', 'forgout')">
        Forgot your password?
      </v-btn>
    </v-form>
  </div>
</template>

<script>
export default {
  data: () => ({
    loginField: 'Pidaras2@gmail.com',
    passwordField: 'Ya_Lublu_Zhr6t_G0vn0',

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

    async test() {
      console.log(this.$auth.logout())
    },

    reset() {
      this.$refs.form.reset()
    },
  },
}
</script>

<style lang="scss"></style>
