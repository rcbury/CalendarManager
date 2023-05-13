<template>
  <div class="profile__container">
    <div class="">
      <v-form ref="form">
        <div class="profile-form__upper-block">
          <v-avatar size="100">
            <img v-if="$auth.user.avatarPath" :src="$auth.user.avatarPath" />
            <div v-else>
              {{ this.$auth.user.userName.at(0) }}
            </div>
          </v-avatar>
          <div class="profile-form__name-block">
            <v-text-field :width="'100%'" v-model="firstNameField" :rules="baseRules" :counter="32" label="Name" required></v-text-field>
            <v-text-field v-model="lastNameField" :rules="baseRules" :counter="32" label="Last name" required />
          </div>
        </div>

        <v-text-field v-model="loginField"  :counter="20" :rules="loginRules" label="Login" disabled></v-text-field>

        <v-text-field v-model="emailField" :rules="emailRules" :counter="32" disabled label="Email"
          required></v-text-field>


        <div class="d-flex flex-column">
          <v-btn color="info" class="mt-4" block @click="validate">
            Save
          </v-btn>
        </div>
      </v-form>
    </div>
  </div>
</template>
    
<script>
export default {
  data() {
    return {
      userList: [],
      inviteLink: "invite link",
      firstNameField: this.$auth.user.firstName,
      lastNameField: this.$auth.user.lastName,
      loginField: this.$auth.user.userName,
      emailField: this.$auth.user.email,
    }
  },

  methods: {
    async validate () {
      const data = {
        ...this.$auth.user,
        firstName: this.firstNameField,
        lastName: this.lastNameField,        
      }

      let result = await this.$axios.$put("/User/self", data)

      await this.$auth.fetchUser()
    },
  },

  async mounted() {
    
  }
}
</script>
    
<style lang="scss">
.profile {
  &__container {
    display: flex;
    flex-direction: column;
    column-gap: 20px;
    margin-left: 15vw;
    width: 50%;
    max-width: 500px;
    margin-top: 3vh;
  }

  &-form {
    &__upper-block {
      display: flex;
      align-items: center;
      flex: 1 2 auto;
      column-gap: 50px;
      justify-content: space-between;
    }

    &__name-block {
      max-width: 250px;
      align-items: center;
      flex: 1 2 auto;
      justify-content: space-between;
    }
  }

  &-rooms__container {
    display: flex;
    justify-content: center;
    margin-top: 10%;
  }
}

.v-card {
  margin-top: 3vh;
}
</style>