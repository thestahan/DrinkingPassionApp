<template>
  <header class="p-mb-6">
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
import CocktailService from "../../../services/CocktailService";
import Spinner from "../../utilities/Spinner.vue";
import CocktailCard from "./CocktailCard.vue";

export default {
  components: { Spinner, CocktailCard },
  data() {
    return {
      cocktails: null,
      isLoading: false,
      url: process.env.VUE_APP_API_URL,
    };
  },
  cocktailService: null,
  created() {
    this.cocktailService = new CocktailService();
  },
  mounted() {
    this.getCocktails();
  },
  methods: {
    async getCocktails() {
      this.isLoading = true;

      try {
        const data = await this.cocktailService.getCocktails();

        this.cocktails = data.data;
        // this.cocktailsTotal = data.count;
        // this.pageIndex = data.pageIndex;
        // this.pageSize = data.pageSize;
      } catch (err) {
        console.error(err.toJSON());
      }

      this.isLoading = false;
    },
  },
};
</script>
