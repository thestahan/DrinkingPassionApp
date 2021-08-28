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
      <Message
        v-show="displayError"
        class="error-message"
        severity="error"
      ></Message>
      <form @submit.prevent="submitForm()" class="p-fluid p-mt-5">
        <div class="p-field p-pb-2">
          <div class="p-float-label">
            <InputText
              id="email"
              v-model="email"
              :class="{ 'p-invalid': v$.email.$error }"
            ></InputText>
            <label for="email">Adres email</label>
          </div>
          <p v-for="error of v$.email.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
        </div>
        <div class="p-field">
          <div class="p-float-label">
            <InputText
              id="password"
              type="password"
              v-model="password"
              :class="{ 'p-invalid': v$.password.$error }"
            ></InputText>
            <label for="password">Hasło</label>
          </div>
          <p v-for="error of v$.password.$errors" :key="error.$uid">
            <small class="p-error">{{ error.$message }}</small>
          </p>
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
            <button
              type="button"
              class="p-text-bold"
              style="color: var(--primary-color)"
              @click="displayPasswordDialog"
            >
              Zapomniałeś hasła?
            </button>
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

  <Dialog
    v-model:visible="displayForgottenPasswordDialog"
    :modal="true"
    :closeOnEscape="true"
    :dismissableMask="true"
    :breakpoints="{ '960px': '95vw' }"
    :style="{ width: '50vw' }"
    header="Przypomnienie hasła"
  >
    <reset-password
      @show-success="displaySentPasswordLinkDialog"
    ></reset-password>
  </Dialog>

  <base-success-modal
    title="Link resetujący hasło"
    content="Jeśli podany adres email jest poprawny, to link umożliwiający zmianę hasła został wysłany."
    :open="displaySentLinkDialog"
    @close-modal="closeSentPasswordLinkDialog"
  ></base-success-modal>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import BaseSuccessModal from "../../../utilities/modals/BaseSuccessModal.vue";
import ResetPassword from "../UserLogin/ResetPassword.vue";
import Spinner from "../../../utilities/Spinner.vue";
import InputText from "primevue/inputtext";
import Checkbox from "primevue/checkbox";
import Button from "primevue/button";
import Message from "primevue/message";
import Dialog from "primevue/dialog";
import useVuelidate from "@vuelidate/core";
import { required, email, helpers, minLength } from "@vuelidate/validators";

export default {
  components: {
    BaseSuccessModal,
    ResetPassword,
    Spinner,
    InputText,
    Checkbox,
    Button,
    Message,
    Dialog,
  },
  setup() {
    return { v$: useVuelidate() };
  },
  data() {
    return {
      email: "",
      password: "",
      isLoading: false,
      reponseError: null,
      rememberMe: false,
      displaySentLinkDialog: false,
      displayError: false,
      displayForgottenPasswordDialog: false,
    };
  },
  methods: {
    async submitForm() {
      this.v$.$touch();

      if (this.v$.$error) {
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
        const errorContent = document
          .querySelector(".error-message")
          .querySelector(".p-message-text");

        errorContent.innerHTML = err;

        this.displayError = true;
      }

      this.isLoading = false;
    },
    displaySentPasswordLinkDialog() {
      this.displaySentLinkDialog = true;
      this.displayForgottenPasswordDialog = false;
    },
    closeSentPasswordLinkDialog() {
      this.displaySentLinkDialog = false;
    },
    displayPasswordDialog() {
      this.displayForgottenPasswordDialog = true;
    },
    hidePasswordDialog() {
      this.displayForgottenPasswordDialog = false;
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
        minLength: helpers.withMessage(
          "Hasło zawiera co najmniej 8 znaków",
          minLength(8)
        ),
      },
    };
  },
};
</script>
