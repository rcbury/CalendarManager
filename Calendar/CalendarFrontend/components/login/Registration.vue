<template>
    <div class="application-login-main__authorization">
      <v-form ref="form">
        <v-text-field
            v-model="loginField"
            :counter="20"
            :rules="loginRules"
            :error-messages="Array.isArray(backendErrorMessages) ? 
              backendErrorMessages
                .filter(x => x.code.toLowerCase().includes('username'))
                .map(error => error.description) : []"
            label="Login"
            required
        ></v-text-field>

        <v-text-field
            v-model="passwordField"
            :rules="passwordRules"
            :error-messages="Array.isArray(backendErrorMessages) ? 
              backendErrorMessages
                .filter(x => x.code.toLowerCase().includes('password'))
                .map(error => error.description) : []"
            type="password"
            label="Password"
            required
        ></v-text-field>

        <v-text-field
            v-model="emailField"
            :rules="emailRules"
            :counter="32"
            :error-messages="backendErrorMessages.Email"
            label="Email"
            required
        ></v-text-field>

       <v-text-field
            v-model="firstNameField"
            :rules="baseRules"
            :counter="32"
            label="Name"
            required
        ></v-text-field>
        <v-text-field
            v-model="lastNameField"
            :rules="baseRules"
            :counter="32"
            label="Last name"
            required
        />

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
      </v-form>


        <v-btn class="mt-4" small @click="$emit('changeForm', 'login')">
            Authorization
        </v-btn>    
    </div>
</template>

<script>
export default {
    data: () => ({
      loginField: '',
      passwordField: '',
      emailField: '',
      firstNameField: '',
      lastNameField: '',

      baseRules: [
      ],

      passwordRules: [
        v => !!v || 'Password is required'
      ],

      loginRules: [
        v => !!v || 'Login is required'
      ],

      emailRules: [
        v => !!v || 'Email is required'
      ],

      backendErrorMessages: {},
    }),

    methods: {
      async validate  () {
        const valid = await this.$refs.form.validate()

        if (valid) {
          const data = {
            username: this.loginField,
            password: this.passwordField,
            email: this.emailField,
            firstName: this.firstNameField,
            lastName: this.lastNameField, 
          }

          try
          {
            const result = await this.$axios.post("/User", data)


            
            this.backendErrorMessages = {}
            this.reset()
            this.$emit("successfulRegistration", "login")
          }
          catch(error) 
          {
            console.log(error.response.data.errors)

            this.backendErrorMessages = error.response.data.errors;
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