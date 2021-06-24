<template>
  <header class="p-m-4">
    <h3 class="p-text-center main-font secondary-heading-font">
      Informacje o Tobie
    </h3>
  </header>

  <section class="p-grid p-flex-column p-m-4" v-if="displayName">
    <div class="p3-col p-mb-2">
      <span class="p-text-bold primary-color">Nazwa wyświetlana:</span>
      <p>{{ displayName }}</p>
    </div>

    <div class="p3-col p-mb-2">
      <span class="p-text-bold primary-color">Adres email:</span>
      <p>{{ email }}</p>
    </div>

    <div class="p3-col p-mb-2">
      <span class="p-text-bold primary-color"
        >Twoje doświadczenie w bramaństwie:</span
      >
      <p>{{ bartenderTypeName }}</p>
    </div>
  </section>

  <spinner v-if="isLoading"></spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";

export default {
  components: { Spinner },
  data() {
    return {
      isLoading: false,
      displayName: null,
      email: null,
      bartenderType: null,
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
</style>
