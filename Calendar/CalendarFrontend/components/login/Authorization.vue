<template>
    <div class="application-login-main__authorization">
        <v-form ref="form">
            <v-text-field
                v-model="loginField"
                :counter="20"
                :rules="loginRules"
                label="Login"
                required
            ></v-text-field>

            <v-text-field
                v-model="passwordField"
                :rules="passwordRules"
                type="password"
                label="Password"
                required
            ></v-text-field>

            <div class="d-flex flex-column">
                <v-btn
                    color="success"
                    class="mt-4"
                    block
                    @click="validate"
                >
                    Login
                </v-btn>
            </div>

            <v-btn class="mt-4" @click="toggleForm()">
                Registration
            </v-btn>
        </v-form>
    </div>
</template>

<script>
export default {
    props: ["toggleForm"],

    data: () => ({
      loginField: 'test',
      passwordField: 'test',

      loginRules: [
        v => !!v || 'Login is required',
        v => (v && v.length <= 20) || 'Login must be less than 20 characters',
      ],

      passwordRules: [
        v => !!v || 'Password is required'
      ],
    }),

    methods: {
      async validate  () {
        const valid = await this.$refs.form.validate()
        if (valid) alert('Form is valid')
      },

      reset () {
        this.$refs.form.reset()
      }
    },
  }
</script>

<style lang="scss">
</style>