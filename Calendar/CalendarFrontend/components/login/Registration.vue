<template>
    <div class="application-login-main__authorization">
        <v-text-field
            v-model="loginField"
            :counter="20"
            :rules="baseRules"
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

        <v-text-field
            v-model="emailField"
            :rules="baseRules"
            :counter="32"
            label="Email"
            required
        ></v-text-field>

       <v-text-field
            v-model="fullNameField"
            :rules="baseRules"
            :counter="32"
            label="FullName"
            required
        ></v-text-field>

        <div class="d-flex flex-column">
            <v-btn
                color="info"
                class="mt-4"
                block
                @click="validate"
            >
                Registration
            </v-btn>
        </div>

        <v-btn class="mt-4" @click="toggleForm()">
            Authorization
        </v-btn>    
    </div>
</template>

<script>
export default {
    props: ["toggleForm"],

    data: () => ({
      loginField: '',
      passwordField: '',
      emailField: '',
      fullNameField: '',

      baseRules: [
        v => !!v || 'Is required',
        v => (v && v.length <= 20) || 'Field must be less than 20 characters',
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
      },
    },
}
</script>

<style>

</style>