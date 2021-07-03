<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 44rem; width: 100%">
      <header class="p-m-4">
        <h3 class="p-text-center main-font secondary-heading-font">
          Informacje o Tobie
        </h3>
      </header>

      <section>
        <form @submit.prevent="submitSaveProfile()">
          <div class="p-grid">
            <div class="p-col-12 p-md-6">
              <div class="p-grid p-fluid">
                <div class="p-col p-field">
                  <label for="firstName" class="p-text-bold primary-color"
                    >Imię</label
                  >
                  <InputText
                    type="text"
                    v-model="firstName"
                    style="width: 100%"
                  />
                </div>
                <div class="p-col p-field">
                  <label for="lastName" class="p-text-bold primary-color"
                    >Nazwisko</label
                  >
                  <InputText
                    type="text"
                    v-model="lastName"
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
                    v-model="displayName"
                    style="width: 100%"
                  />
                </div>
              </div>

              <div class="p-grid p-fliud">
                <div class="p-col p-field">
                  <label for="email" class="p-text-bold primary-color"
                    >Adres email</label
                  >
                  <InputText type="text" v-model="email" style="width: 100%" />
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
                    v-model="bartenderType"
                    :modelValue="bartenderType"
                    :options="bartenderTypeOptions"
                    optionLabel="name"
                  />
                </div>
              </div>
              <div class="p-grid">
                <div class="p-col">
                  <Button
                    class="p-button-secondary change-password-button"
                    label="Zmień hasło"
                    style="margin-bottom: 1rem"
                    @click="openChangePasswordDialog"
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
import Spinner from "../../utilities/Spinner.vue";
import Dialog from "primevue/dialog";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import Dropdown from "primevue/dropdown";
import Password from "primevue/password";

export default {
  components: { Spinner, Dialog, Button, InputText, Dropdown, Password },
  data() {
    return {
      isLoading: false,
      firstName: "Adam",
      lastName: "Nowak",
      displayName: null,
      email: null,
      bartenderType: null,
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

      this.displayName = responseData.displayName;
      this.email = responseData.email;
      if (responseData.bartenderType == 1) {
        this.bartenderType = { name: "Hobbysta", code: 1 };
      } else if (responseData.bartenderType == 2) {
        this.bartenderType = { name: "Zawodowiec", code: 2 };
      }
    },
    async submitSaveProfile() {
      this.isLoading = true;

      const model = {
        firstName: this.firstName,
        lastName: this.lastName,
        displayName: this.displayName,
        email: this.email,
        bartenderType: this.bartenderType.code,
      };

      console.log(model);

      this.isLoading = false;
    },
    async submitChangePassword() {
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
};
</script>

<style scoped>
.change-password-button {
  width: 100%;
}

.save-button {
  width: 100%;
}

.secondary-heading-font {
  font-size: 1.5rem;
  font-weight: 300;
}

.user-property-title {
  font-size: 1.1rem;
  font-weight: 400;
}
</style>
