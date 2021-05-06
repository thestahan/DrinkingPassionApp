<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 28rem; width: 100%">
      <div>
        <img
          class="p-mx-auto"
          src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
          alt="DrinkingPassion logo"
          style="height: 3rem"
        />
        <div class="p-text-center p-mt-3">
          <h2 class="p-text-bold" style="font-size: 2rem">Zaloguj się</h2>
          <p style="font-size: 0.9rem">
            lub
            <router-link
              to="/register"
              class="p-text-bold"
              style="color: var(--primary-color)"
              >załóż konto</router-link
            >
          </p>
        </div>
      </div>
      <form @submit.prevent="handleSubmit(!v$.$invalid)" class="p-fluid p-mt-5">
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="email"
              v-model="v$.email.$model"
              :class="{ 'p-invalid': v$.email.$invalid && submitted }"
            ></InputText>
            <label for="email">Adres email</label>
          </div>
          <span v-if="v$.email.$error && submitted"
            ><span
              id="email-error"
              v-for="(error, index) of v$.email.$errors"
              :key="index"
            >
              <small class="p-error">{{ error.$message }}</small>
            </span></span
          >
          <small
            v-else-if="
              (v$.email.$invalid && submitted) || v$.email.$pending.$response
            "
            class="p-error"
            >{{ v$.email.required.$message.replace("Value", "Email") }}</small
          >
        </div>
        <div class="p-field">
          <div class="p-float-label">
            <InputText
              id="password"
              type="password"
              v-model="password"
              :class="{ 'p-invalid': v$.password.$invalid && submitted }"
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
        <div class="p-d-flex p-jc-between" style="font-size: 0.9rem">
          <div class="p-field-checkbox">
            <Checkbox
              id="remember-me"
              v-model="rememberMe"
              :binary="true"
            ></Checkbox>
            <label for="remember-me">Zapamiętaj mnie</label>
          </div>
          <div>
            <router-link
              to="/register"
              class="p-text-bold"
              style="color: var(--primary-color)"
              >Zapomniałeś hasła?</router-link
            >
          </div>
        </div>
        <div>
          <Button
            type="submit"
            label="Zaloguj się"
            style="width: 100%"
          ></Button>
        </div>
      </form>
    </div>
  </div>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import InputText from "primevue/inputtext";
import Checkbox from "primevue/checkbox";
import Button from "primevue/button";
import useVuelidate from "@vuelidate/core";
import { required, email, helpers } from "@vuelidate/validators";

export default {
  components: {
    Spinner,
    InputText,
    Checkbox,
    Button,
  },
  setup() {
    return { v$: useVuelidate() };
  },
  data() {
    return {
      email: "",
      password: "",
      errors: [],
      isLoading: false,
      reponseError: null,
      submitted: false,
      rememberMe: false,
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
        await this.$store.dispatch("signIn", {
          email: this.email,
          password: this.password,
        });

        this.$router.replace("/cocktails");
      } catch (err) {
        this.errors.push(err.message || "Logowanie nie powiodło się");
      }

      this.isLoading = false;
      //this.toggleDialog();
    },
  },
  validations() {
    return {
      email: {
        required: helpers.withMessage("To pole jest wymagane", required),
        email: helpers.withMessage("Wprowadź poprawny adres email", email),
      },
      password: {
        required: helpers.withMessage("To pole jest wymagane", required),
      },
    };
  },
};
</script>
