<template>
  <div class="profile__container">
    <div class="">
      <v-dialog max-width="600px" v-model="avatarDialogOpen">
        <v-card>
          <v-card-title>
            <span class="text-h5">Avatar upload</span>
          </v-card-title>
          <v-card-text>
            <v-file-input accept="image/png, image/jpeg, image/bmp" placeholder="Pick an avatar" prepend-icon="mdi-camera"
              label="Avatar" v-model="avatarToUpload"></v-file-input>
          </v-card-text>
          <v-card-actions>
            <v-spacer></v-spacer>
            <v-btn color="blue darken-1" text @click="toggleAvatarDialog">
              Close
            </v-btn>
            <v-btn color="blue darken-1" :disabled="!avatarToUpload" text @click="uploadAvatar">
              Save
            </v-btn>
          </v-card-actions>
        </v-card>

      </v-dialog>
      <v-form ref="form">
        <div class="profile-form__upper-block">
          <v-hover v-slot="{ hover }">
            <v-card elevation="0" class="rounded-circle ma-0">
              <v-icon v-if="hover" class="profile-form-avatar__upload-icon"> mdi-upload</v-icon>
              <v-avatar size="100" @click="toggleAvatarDialog">
                <v-img v-if="$auth.user.avatarPath" :src="avatarPath" class="profile-form-avatar__image" />
                <div v-else>
                  {{ $auth.user.userName.at(0) }}
                </div>

              </v-avatar>
            </v-card>
          </v-hover>
          <div class="profile-form__name-block">
            <v-text-field :width="'100%'" v-model="firstNameField" :rules="baseRules" :counter="32" label="Name"
              required></v-text-field>
            <v-text-field v-model="lastNameField" :rules="baseRules" :counter="32" label="Last name" required />
          </div>
        </div>
        <v-text-field v-model="loginField" :rules="loginRules" label="Login" disabled></v-text-field>
        <v-text-field v-model="emailField" :rules="emailRules" disabled label="Email" required></v-text-field>
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
      avatarDialogOpen: false,
      avatarToUpload: undefined,
      avatarPath: this.$auth.user.avatarPath,
    }
  },

  methods: {
    async validate() {
      const data = {
        ...this.$auth.user,
        firstName: this.firstNameField,
        lastName: this.lastNameField,
      }

      let result = await this.$axios.$put("/User/self", data)

      await this.$auth.fetchUser()
    },

    toggleAvatarDialog() {
      this.avatarDialogOpen = !this.avatarDialogOpen
    },

    async uploadAvatar() {
      var bodyFormData = new FormData();

      bodyFormData.append("profilePicture", this.avatarToUpload)

      this.avatarDialogOpen = this.$axios.$put('/User/self/avatar', bodyFormData)

      this.toggleAvatarDialog()

      await this.$auth.fetchUser()

      this.avatarPath = ""

      this.avatarPath = this.$auth.user.avatarPath
    }
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

    &-avatar__upload-icon {
      position: absolute !important;
      top: 40%;
      left: 40%;
      margin-left: auto;
      z-index: 10;
      pointer-events: none;
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

.v-avatar:hover {
  width: 100%;
  height: 100%;
  background: rgba(29, 29, 29, 0.5);
  border-radius: 50%;
  filter: brightness(85%);
}
</style>