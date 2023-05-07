<template>
    <div class="application-login-main__authorization">
        <v-form ref="form">
            <v-text-field
                v-model="loginField"
                :counter="20"
                :rules="baseRules"
                label="Write the email to which the account was registered"
                required
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
      ]
    }),

    methods: {
      async validate  () {
        const valid = await this.$refs.form.validate()

        if (valid) alert('send email')
      },

      reset () {
        this.$refs.form.reset()
      },
    },
}
</script>

<style>

</style>