<template>
    <div class="application-login-form-main">
        <div class="application-login-form">
            <h1 class="application-login-form__title">Календарь 3000</h1>
            <transition name="fade" mode="out-in">
                <LoginAuthorization @changeForm="changeForm" v-if="authorizationFormType === 'login'"/>
                <LoginRegistration @successfulRegistration="changeForm" @changeForm="changeForm" v-if="authorizationFormType === 'register'" />
                <LoginForgoutPassword @changeForm="changeForm" v-else />
            </transition>
        </div>
    </div>
</template>

<script>
export default {
    data: () => ({
        authorizationFormType: 'login'
    }),

    methods: {
        changeForm(state) {
            this.authorizationFormType = state;
      }
    },
  }
</script>

<style lang="scss">
.application-login {
    display: flex;
    align-items: center;
    justify-content: center;


    &-form {
        &__title {
            display: flex;
            justify-content: center;
        }

        &-main {
            height: 50%;
            width: 30%;
        }

        height: 100%;
        width: 100%;

        display: grid;
        grid-template-rows: 15% auto;
        grid-template-columns: 100%;

        form {
            button {
                margin: 0;
            }
        }
    }
}

.fade-enter-active,
.fade-leave-active {
  transition-property: opacity;
  transition-timing-function: ease-in-out;
  transition-duration: 300ms;
}

.fade-enter,
.fade-leave-to {
  opacity: 0;
}
</style>