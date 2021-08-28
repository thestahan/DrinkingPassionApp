<template>
  <form @submit.prevent="submitForm">
    <div class="p-grid p-fluid">
      <div class="p-col p-field">
        <label for="currentPassword" class="p-text-bold primary-color"
          >Aktualne hasło</label
        >
        <InputText
          id="email"
          v-model.trim="email"
          :class="{ 'p-invalid': v$.email.$error }"
        ></InputText>
        <p v-for="error of v$.email.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>
    </div>

    <div class="p-text-right">
      <Button type="submit" label="Zresetuj hasło"></Button>
    </div>
  </form>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import InputText from "primevue/inputtext";
import Spinner from "../../../utilities/Spinner.vue";
import Button from "primevue/button";

import useVuelidate from "@vuelidate/core";
import { required, email, helpers } from "@vuelidate/validators";

export default {
  components: {
    Spinner,
    InputText,
    Button,
  },
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  emits: ["show-success"],
  data() {
    return {
      email: null,
      isLoading: null,
      openModal: true,
    };
  },
  computed: {
    token() {
      return this.$store.getters.token;
    },
  },
  methods: {
    async submitForm() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      this.isLoading = true;

      console.log("email:  " + this.email);

      await fetch(
        "https://localhost:5001/api/accounts/SendForgottenPasswordLink?" +
          new URLSearchParams({
            email: this.email,
          })
      );

      this.isLoading = false;
      this.emitShowSuccess();
    },
    emitShowSuccess() {
      console.log("emitting success");
      this.$emit("show-success");
    },
  },
  validations() {
    return {
      email: {
        required: helpers.withMessage("To pole jest wymagane", required),
        email: helpers.withMessage("Wprowadź poprawny adres email", email),
      },
    };
  },
};
</script>
