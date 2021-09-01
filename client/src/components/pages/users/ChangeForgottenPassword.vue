<template>
  <div class="p-d-flex p-jc-center p-px-2">
    <div style="max-width: 60rem; width: 100%">
      <header class="p-mb-2">
        <h3 class="main-font secondary-heading-font">Zmiana hasła</h3>
      </header>
      <div>
        <form @submit.prevent="changePassword()">
          <div class="p-grid p-fluid">
            <div class="p-col p-field">
              <label for="newPassword" class="p-text-bold primary-color"
                >Nowe hasło</label
              >
              <Password
                v-model="newPassword"
                :feedback="false"
                toggleMask
                style="width: 100%"
              />
              <p v-for="error of v$.newPassword.$errors" :key="error.$uid">
                <small class="p-error">{{ error.$message }}</small>
              </p>
            </div>
          </div>

          <div class="p-grid p-fluid">
            <div class="p-col p-field">
              <label for="newPasswordRepeated" class="p-text-bold primary-color"
                >Powtórz nowe hasło</label
              >
              <Password
                v-model="newPasswordRepeated"
                :feedback="false"
                toggleMask
                style="width: 100%"
              />
              <p
                v-for="error of v$.newPasswordRepeated.$errors"
                :key="error.$uid"
              >
                <small class="p-error">{{ error.$message }}</small>
              </p>
            </div>
          </div>

          <div class="p-grid p-mt-2">
            <div class="p-col" style="text-align: right">
              <Button type="submit" label="Zmień hasło" />
            </div>
          </div>
        </form>
      </div>
    </div>
  </div>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import Button from "primevue/button";
import Password from "primevue/password";
import useVuelidate from "@vuelidate/core";
import { required, helpers, sameAs, minLength } from "@vuelidate/validators";

export default {
  components: {
    Spinner,
    Button,
    Password,
  },
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  data() {
    return {
      token: null,
      email: null,
      newPassword: "",
      newPasswordRepeated: "",
      confirmationSuccessful: false,
      confirmationNotSuccessful: false,
      isLoading: false,
    };
  },
  created() {
    this.token = this.$route.query.token;
    this.email = this.$route.query.email;
  },
  methods: {
    async changePassword() {
      this.isLoading = true;

      const response = await fetch(
        "https://localhost:5001/api/accounts/ChangeForgottenPassword",
        {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            token: this.token,
            email: this.email,
            newPassword: this.newPassword,
          }),
        }
      );

      this.isLoading = false;

      if (response.ok) {
        this.confirmationSuccessful = true;
      } else {
        this.confirmationNotSuccessful = true;
      }
    },
  },
  validations() {
    return {
      newPassword: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Hasło musi zawierać minimum 8 znaków",
          minLength(8)
        ),
      },
      newPasswordRepeated: {
        repeatPassword: helpers.withMessage(
          "Podane hasła muszą być identyczne",
          sameAs(this.newPassword)
        ),
      },
    };
  },
};
</script>
