<template>
  <form @submit.prevent="submitForm">
    <div class="p-grid p-fluid">
      <div class="p-col p-field">
        <label for="currentPassword" class="p-text-bold primary-color"
          >Adres email</label
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
      <Button type="submit" label="Wyślij link do resetowania hasła"></Button>
    </div>
  </form>
</template>

<script>
import InputText from "primevue/inputtext";
import Button from "primevue/button";

import useVuelidate from "@vuelidate/core";
import { required, email, helpers } from "@vuelidate/validators";

export default {
  components: {
    InputText,
    Button,
  },
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  emits: ["show-success", "is-loading", "stopped-loading"],
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

      this.emitIsLoading();

      await fetch(
        `${process.env.VUE_APP_API_URL}/SendForgottenPasswordLink?` +
          new URLSearchParams({
            email: this.email,
          })
      );

      this.emitStoppedLoading();
      this.emitShowSuccess();
    },
    emitIsLoading() {
      this.$emit("is-loading");
    },
    emitStoppedLoading() {
      this.$emit("stopped-loading");
    },
    emitShowSuccess() {
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
