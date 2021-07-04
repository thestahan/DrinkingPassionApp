<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 44rem; width: 100%">
      <header class="p-m-4">
        <h3 class="p-text-center main-font secondary-heading-font">
          Informacje o Tobie
        </h3>
      </header>

      <section v-if="userData">
        <save-profile-form :userData="userData" v-on:submit="submitSaveProfile">
        </save-profile-form>
      </section>
    </div>
  </div>

  <Dialog
    v-model:visible="displayChangePasswordDialog"
    :modal="true"
    :closeOnEscape="true"
    :dismissableMask="true"
    :breakpoints="{ '960px': '95vw' }"
    :style="{ width: '50vw' }"
    header="Zmiana hasła"
  >
    <form @submit.prevent="submitChangePassword()">
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
  </Dialog>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import SaveProfileForm from "../UserProfile/SaveProfileForm.vue";
import Spinner from "../../../utilities/Spinner.vue";
import Dialog from "primevue/dialog";
import Button from "primevue/button";
import Password from "primevue/password";
import useVuelidate from "@vuelidate/core";
import { required, helpers, sameAs, minLength } from "@vuelidate/validators";

export default {
  components: {
    SaveProfileForm,
    Spinner,
    Dialog,
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
      userData: {},
      isLoading: false,
      displayChangePasswordDialog: false,
      currentPassword: null,
      newPassword: null,
      newPasswordRepeated: null,
      bartenderTypeOptions: [
        { name: "Hobbysta", code: 1 },
        { name: "Zawodowiec", code: 2 },
      ],
    };
  },
  computed: {
    bartenderTypeName() {
      if (this.bartenderType === 2) {
        return "Profesjonalista";
      } else {
        return "Zawodowiec";
      }
    },
  },
  created() {
    this.getUserProfile();
  },
  methods: {
    async getUserProfile() {
      this.isLoading = true;

      const token = this.$store.getters.token;

      const response = await fetch(
        "https://localhost:5001/api/accounts/details",
        {
          method: "GET",
          headers: {
            Authorization: "Bearer " + token,
            "Content-Type": "application/json",
          },
        }
      );

      this.isLoading = false;

      const responseData = await response.json();

      if (!response.ok) {
        const error = new Error(responseData.message || "Wystąpił błąd.");
        throw error;
      }

      this.userData.firstName = "Adam";
      this.userData.lastName = "Nowak";
      this.userData.displayName = responseData.displayName;
      this.userData.email = responseData.email;
      if (responseData.bartenderType == 1) {
        this.userData.bartenderType = { name: "Hobbysta", code: 1 };
      } else if (responseData.bartenderType == 2) {
        this.userData.bartenderType = { name: "Zawodowiec", code: 2 };
      }
    },
    async submitSaveProfile(user) {
      this.isLoading = true;

      console.log(user);

      this.isLoading = false;
    },
    async submitChangePassword() {
      this.v$.$touch();

      if (this.v$.$error) {
        return;
      }

      this.isLoading = true;

      const model = {
        currentPassword: this.currentPassword,
        newPassword: this.newPassword,
        newPasswordRepeated: this.newPasswordRepeated,
      };

      console.log(model);

      this.isLoading = false;
    },
    openChangePasswordDialog() {
      this.displayChangePasswordDialog = true;
    },
    closeChangePasswordDialog() {
      this.displayChangePasswordDialog = false;
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

<style scoped>
.secondary-heading-font {
  font-size: 1.5rem;
  font-weight: 300;
}

.user-property-title {
  font-size: 1.1rem;
  font-weight: 400;
}
</style>
