<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 44rem; width: 100%">
      <header class="p-m-4">
        <h3 class="p-text-center main-font secondary-heading-font">
          Informacje o Tobie
        </h3>
      </header>

      <section v-if="userData">
        <save-profile-form
          :userData="userData"
          v-on:submit="submitSaveProfile"
          v-on:open-password-dialog="openChangePasswordDialog"
        >
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
    <change-password-form
      v-on:close-password-dialog="closeChangePasswordDialog"
      v-on:submit-change-password="submitChangePassword"
    ></change-password-form>
  </Dialog>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import SaveProfileForm from "../UserProfile/SaveProfileForm.vue";
import ChangePasswordForm from "../UserProfile/ChangePasswordForm.vue";
import Spinner from "../../../utilities/Spinner.vue";
import Dialog from "primevue/dialog";

export default {
  components: {
    SaveProfileForm,
    ChangePasswordForm,
    Spinner,
    Dialog,
  },
  data() {
    return {
      userData: {},
      isLoading: false,
      displayChangePasswordDialog: false,
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
    async submitChangePassword(model) {
      this.isLoading = true;

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
