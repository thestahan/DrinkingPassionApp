<template>
  <div class="p-d-flex p-jc-center">
    <div class="p-px-3" style="max-width: 38rem; width: 100%">
      <header class="p-m-4">
        <h3 class="p-text-center main-font secondary-heading-font">
          Informacje o Tobie
        </h3>
      </header>

      <section class="p-grid p-flex-column p-m-4" v-if="displayName">
        <div class="p3-col p-mb-4">
          <div class="p-d-flex p-jc-between p-ai-center">
            <div>
              <h4 class="p-text-bold primary-color user-property-title">
                Nazwa wyświetlana:
              </h4>
              <p>{{ displayName }}</p>
            </div>
            <div styl="margin: auto">
              <!-- prettier-ignore -->
              <button
                class="p-button p-component p-button-icon-only p-button-rounded p-button-success p-mr-2"
              >
                <span class="pi pi-pencil p-button-icon"></span>
              </button>
            </div>
          </div>
        </div>

        <div class="p3-col p-mb-4">
          <div class="p-d-flex p-jc-between p-ai-center">
            <div>
              <h4 class="p-text-bold primary-color user-property-title">
                Adres email:
              </h4>
              <p>{{ email }}</p>
            </div>
            <div styl="margin: auto">
              <!-- prettier-ignore -->
              <button
                class="p-button p-component p-button-icon-only p-button-rounded p-button-success p-mr-2"
              >
                <span class="pi pi-pencil p-button-icon"></span>
              </button>
            </div>
          </div>
        </div>

        <div class="p3-col p-mb-4">
          <div class="p-d-flex p-jc-between p-ai-center">
            <div>
              <h4 class="p-text-bold primary-color user-property-title">
                Twoje doświadczenie w bramaństwie:
              </h4>
              <p>{{ bartenderTypeName }}</p>
            </div>
            <div styl="margin: auto">
              <!-- prettier-ignore -->
              <button
                @click="displayEditDialog = true"
                class="p-button p-component p-button-icon-only p-button-rounded p-button-success p-mr-2"
              >
                <span class="pi pi-pencil p-button-icon"></span>
              </button>
            </div>
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

export default {
  components: { Spinner, Dialog, Button },
  data() {
    return {
      isLoading: false,
      displayName: null,
      email: null,
      bartenderType: null,
      displayEditDialog: false,
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
      this.bartenderType = responseData.bartenderType;
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
  font-size: 1.2rem;
  font-weight: 400;
}
</style>
