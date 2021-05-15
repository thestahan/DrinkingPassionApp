<template>
  <header>
    <h2 class="p-text-center main-font heading-font">
      Lista wszystkich koktajli
    </h2>
  </header>

  <section class="p-grid p-m-4" v-if="cocktails">
    <cocktail-card
      v-for="cocktail in cocktails"
      :key="cocktail.id"
      :cocktail="cocktail"
    ></cocktail-card>
  </section>
  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";
import CocktailCard from "./CocktailCard.vue";

export default {
  components: { Spinner, CocktailCard },
  data() {
    return {
      cocktails: null,
      isLoading: false,
    };
  },
  created() {
    this.getCocktails();
  },
  methods: {
    async getCocktails() {
      this.isLoading = true;

      const response = await fetch("https://localhost:5001/api/cocktails", {
        method: "GET",
        headers: {
          "Content-Type": "application/json",
        },
      });

      this.isLoading = false;

      const responseData = await response.json();

      if (!response.ok) {
        const error = new Error(responseData.message || "Wystąpił błąd.");
        throw error;
      }

      this.cocktails = responseData.data;
    },
  },
};
</script>

<style scoped>
.heading-font {
  font-size: 2rem;
  font-weight: 300;
}
</style>
