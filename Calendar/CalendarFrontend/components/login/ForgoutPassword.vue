<template>
    <div class="application-login-main__authorization">
        <v-form ref="form">
            <v-text-field
                v-model="emailField"
                :counter="62"
                :rules="baseRules"
                label="Write the email to which the account was registered"
                required
            ></v-text-field>

            <v-text-field
                v-if="passwordResponse"
                v-model="passwordResponse"
                hint="Use this password to login"
                persistent-hint
                readonly
            ></v-text-field>

            <v-btn class="mt-4" color="success" block @click="validate">
                reset password
            </v-btn>    

            <v-btn class="mt-4" @click="$emit('changeForm', 'login')">
                Authorization
            </v-btn>    
        </v-form>
    </div>
</template>

<script>
export default {
    data: () => ({
      emailField: '',

      baseRules: [
        v => !!v || 'Is required',
        v => (v && v.indexOf("@") >= 0) || 'Invalid email',
      ],

      passwordResponse: ''
    }),

    methods: {
      async validate  () {
        const valid = await this.$refs.form.validate()

        if (valid){
          try{
            let result = await this.$axios.$post(`/User/self/reset?email=${this.emailField}`)
            this.passwordResponse = result.newPassword
          } catch {
            console.log("error")
          }
        }
      },

      reset () {
        this.$refs.form.reset()
      },
    },
}
</script>

<style>

</style>