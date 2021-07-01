<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 44rem; width: 100%">
      <header class="p-m-4">
        <h3 class="p-text-center main-font secondary-heading-font">
          Informacje o Tobie
        </h3>
      </header>

      <section>
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
                <InputText type="text" v-model="lastName" style="width: 100%" />
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
                ></Button>
              </div>
            </div>
          </div>
        </div>

        <div class="p-grid">
          <div class="p-col">
            <Button class="save-button" label="Zapisz zmiany"></Button>
          </div>
        </div>
      </section>
    </div>
  </div>
  <Dialog
    v-model:visible="displayEditDialog"
    :modal="true"
    :closeOnEscape="true"
    :dismissableMask="true"
  >
    <template #header>
      <h3>Twoje doświadczenie w barmaństwie</h3>
    </template>

    Tutaj content do wypełnienia

    <template #footer>
      <Button
        label="Anuluj"
        icon="pi pi-times"
        class="p-button-text"
        @click="displayEditDialog = false"
      />
      <Button label="Zapisz" icon="pi pi-check" />
    </template>
  </Dialog>
  <spinner v-if="isLoading"></spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import Dialog from "primevue/dialog";
import Button from "primevue/button";
import InputText from "primevue/inputtext";
import Dropdown from "primevue/dropdown";

export default {
  components: { Spinner, Dialog, Button, InputText, Dropdown },
  data() {
    return {
      isLoading: false,
      firstName: "Adam",
      lastName: "Nowak",
      displayName: null,
      email: null,
      bartenderType: null,
      displayEditDialog: false,
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
