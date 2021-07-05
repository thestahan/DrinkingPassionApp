<template>
  <form @submit.prevent="emitSaveProfile()">
    <div class="p-grid">
      <div class="p-col-12 p-md-6">
        <div class="p-grid p-fluid">
          <div class="p-col p-field">
            <label for="firstName" class="p-text-bold primary-color"
              >Imię</label
            >
            <InputText
              type="text"
              v-model="user.firstName"
              style="width: 100%"
            />
          </div>
          <div class="p-col p-field">
            <label for="lastName" class="p-text-bold primary-color"
              >Nazwisko</label
            >
            <InputText
              type="text"
              v-model="user.lastName"
              style="width: 100%"
            />
          </div>
        </div>

        <div class="p-grid p-fluid">
          <div class="p-col p-field">
            <label for="displayName" class="p-text-bold primary-color"
              >Nazwa wyświetlana</label
            >
            <InputText
              type="text"
              v-model="user.displayName"
              style="width: 100%"
            />
            <p v-for="error of v$.user.displayName.$errors" :key="error.$uid">
              <small class="p-error">{{ error.$message }}</small>
            </p>
          </div>
        </div>

        <div class="p-grid p-fliud">
          <div class="p-col p-field">
            <label for="email" class="p-text-bold primary-color"
              >Adres email</label
            >
            <InputText type="text" v-model="user.email" style="width: 100%" />
            <p v-for="error of v$.user.email.$errors" :key="error.$uid">
              <small class="p-error">{{ error.$message }}</small>
            </p>
          </div>
        </div>
      </div>
      <div class="p-col-12 p-md-6 p-d-flex p-flex-column p-jc-between">
        <div class="p-grid p-fluid">
          <div class="p-col p-field">
            <label for="bartenderType" class="p-text-bold primary-color"
              >Doświadczenie w barmaństwie</label
            >
            <Dropdown
              v-model="user.bartenderType"
              :modelValue="user.bartenderType"
              :options="bartenderTypeOptions"
              optionLabel="name"
            />
            <p v-for="error of v$.user.bartenderType.$errors" :key="error.$uid">
              <small class="p-error">{{ error.$message }}</small>
            </p>
          </div>
        </div>
        <div class="p-grid">
          <div class="p-col">
            <Button
              class="p-button-secondary change-password-button"
              label="Zmień hasło"
              style="margin-bottom: 1rem"
              @click="emitOpenChangePassword"
            ></Button>
          </div>
        </div>
      </div>
    </div>

    <div class="p-grid">
      <div class="p-col">
        <Button
          type="submit"
          class="save-button"
          label="Zapisz zmiany"
        ></Button>
      </div>
    </div>
  </form>
</template>

<script>
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import Dropdown from "primevue/dropdown";
import useVuelidate from "@vuelidate/core";
import { required, email, helpers, minLength } from "@vuelidate/validators";
export default {
  components: { Button, InputText, Dropdown },
  props: {
    userData: {
      type: Object,
      required: true,
    },
  },
  setup() {
    return {
      v$: useVuelidate(),
    };
  },
  data() {
    return {
      user: this.userData,
      bartenderTypeOptions: [
        { name: "Hobbysta", code: 1 },
        { name: "Zawodowiec", code: 2 },
      ],
    };
  },
  methods: {
    emitSaveProfile() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      this.$emit("submit", {
        firstName: this.user.firstName,
        lastName: this.user.lastName,
        displayName: this.user.lastName,
        email: this.user.email,
        bartenderType: this.user.bartenderType.code,
      });
    },
    emitOpenChangePassword() {
      this.$emit("open-password-dialog");
    },
  },
  validations() {
    return {
      user: {
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
        bartenderType: {
          required,
        },
      },
    };
  },
};
</script>

<style scoped>
.change-password-button {
  width: 100%;
}

.save-button {
  width: 100%;
}
</style>
