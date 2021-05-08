<template>
  <div class="p-d-flex p-jc-center p-mt-6">
    <div class="p-px-3" style="max-width: 28rem; width: 100%">
      <div>
        <img
          class="p-mx-auto"
          src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
          alt="DrinkingPassion logo"
          style="height: 3rem"
        />
        <div class="p-text-center p-mt-3">
          <h2 class="p-text-bold" style="font-size: 2rem">Załóż konto</h2>
          <p style="font-size: 0.9rem">
            lub
            <router-link
              to="/login"
              class="p-text-bold"
              style="color: var(--primary-color)"
              >zaloguj się na istniejące konto</router-link
            >
          </p>
        </div>
      </div>
      <form @submit.prevent="handleSubmit(!v$.$invalid)" class="p-fluid p-mt-5">
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="displayName"
              v-model="v$.displayName.$model"
              :class="{ 'p-invalid': v$.displayName.$invalid && submitted }"
            ></InputText>
            <label for="displayName">Nazwa wyświetlana</label>
          </div>
          <small
            v-if="
              (v$.displayName.$invalid && submitted) ||
              v$.displayName.$pending.$response
            "
            class="p-error"
            >{{ v$.displayName.required.$message }}</small
          >
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="email"
              v-model="v$.email.$model"
              :class="{ 'p-invalid': v$.email.$invalid && submitted }"
            ></InputText>
            <label for="email">Adres email</label>
          </div>
          <small
            v-if="
              (v$.email.$invalid && submitted) || v$.email.$pending.$response
            "
            class="p-error"
            >{{ v$.email.required.$message }}</small
          >
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="password"
              v-model="v$.password.$model"
              :class="{ 'p-invalid': v$.password.$invalid && submitted }"
              type="password"
            ></InputText>
            <label for="password">Hasło</label>
          </div>
          <small
            v-if="
              (v$.password.$invalid && submitted) ||
              v$.password.$pending.$response
            "
            class="p-error"
            >{{ v$.password.required.$message }}</small
          >
        </div>
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="passwordConfirm"
              v-model="v$.passwordConfirm.$model"
              :class="{ 'p-invalid': v$.passwordConfirm.$invalid && submitted }"
              type="password"
            ></InputText>
            <label for="passwordConfirm">Powtórz hasło</label>
          </div>
          <small
            v-if="
              (v$.passwordConfirm.$invalid && submitted) ||
              v$.passwordConfirm.$pending.$response
            "
            class="p-error"
            >{{ v$.passwordConfirm.repeatPassword.$message }}</small
          >
        </div>
        <div>
          <Button
            type="submit"
            label="Zarejestruj się"
            style="width: 100%"
          ></Button>
        </div>
      </form>
    </div>
  </div>

  <spinner v-if="isLoading"></spinner>
  <base-success-modal
    title="Sukces!"
    content="Rejestracja przebiegła pomyślnie. Możesz zalogować się na swoje konto."
    :open="openModal"
    @close-modal="redirectToLogin"
  ></base-success-modal>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import BaseSuccessModal from "../../utilities/modals/BaseSuccessModal.vue";
import InputText from "primevue/inputtext";
import Button from "primevue/button";
import useVuelidate from "@vuelidate/core";
import {
  required,
  email,
  helpers,
  sameAs,
  minLength,
} from "@vuelidate/validators";

export default {
  components: {
    Spinner,
    BaseSuccessModal,
    InputText,
    Button,
  },
  setup() {
    return { v$: useVuelidate() };
  },
  data() {
    return {
      email: "",
      password: "",
      passwordConfirm: "",
      displayName: "",
      isLoading: false,
      openModal: false,
      submitted: false,
    };
  },
  methods: {
    async handleSubmit(isFormValid) {
      this.submitted = true;

      if (!isFormValid) {
        return;
      }

      this.isLoading = true;

      try {
        await this.$store.dispatch("signUp", {
          email: this.email.trim(),
          password: this.password.trim(),
          displayName: this.displayName.trim(),
        });

        this.redirectToLogin();
      } catch (err) {
        console.log("Logowanie nie powiodło się");
      }

      this.isLoading = false;

      this.openModal = true;
    },

    redirectToLogin() {
      this.$router.replace("/login");
    },
  },
  validations() {
    return {
      displayName: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Nazwa musi zawierać więcej niż 3 znaki",
          minLength(3)
        ),
      },
      email: {
        required: helpers.withMessage("To pole jest wymagane", required),
        email: helpers.withMessage("Wprowadź poprawny adres email", email),
      },
      password: {
        required: helpers.withMessage("To pole jest wymagane", required),
      },
      passwordConfirm: {
        repeatPassword: helpers.withMessage(
          "Podane hasła muszą być identyczne",
          sameAs("password")
        ),
      },
    };
  },
};
</script>
