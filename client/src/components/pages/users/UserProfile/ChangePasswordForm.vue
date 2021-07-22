<template>
  <form @submit.prevent="emitChangePassword()">
    <div class="p-grid p-fluid">
      <div class="p-col p-field">
        <label for="currentPassword" class="p-text-bold primary-color"
          >Aktualne hasło</label
        >
        <Password
          v-model="currentPassword"
          :feedback="false"
          toggleMask
          style="width: 100%"
        />
        <p v-for="error of v$.currentPassword.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>
    </div>

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
        <p v-for="error of v$.newPasswordRepeated.$errors" :key="error.$uid">
          <small class="p-error">{{ error.$message }}</small>
        </p>
      </div>
    </div>

    <div class="p-grid p-mt-2">
      <div class="p-col" style="text-align: right">
        <Button
          label="Anuluj"
          icon="pi pi-times"
          class="p-button-text"
          @click="closeChangePasswordDialog"
        />
        <Button type="submit" label="Zapisz" icon="pi pi-check" />
      </div>
    </div>
  </form>
</template>

<script>
import Button from "primevue/button";
import Password from "primevue/password";
import useVuelidate from "@vuelidate/core";
import { required, helpers, sameAs, minLength } from "@vuelidate/validators";

export default {
  components: {
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
      currentPassword: null,
      newPassword: null,
      newPasswordRepeated: null,
    };
  },
  methods: {
    emitChangePassword() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      const model = {
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
        newPasswordRepeated: this.newPasswordRepeated,
      };

      this.$emit("submit-change-password", model);
    },
    closeChangePasswordDialog() {
      this.$emit("close-password-dialog", this.user);
    },
  },
  validations() {
    return {
      currentPassword: {
        required: helpers.withMessage("To pole jest wymagane", required),
        minLength: helpers.withMessage(
          "Hasło musi zawierać minimum 8 znaków",
          minLength(8)
        ),
      },
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
