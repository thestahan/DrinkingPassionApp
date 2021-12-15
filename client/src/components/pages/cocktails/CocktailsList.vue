<template>
  <section class="p-grid p-m-4">
    <cocktail-card
      v-for="cocktail in cocktailsData.cocktails"
      :key="cocktail.id"
      :cocktail="cocktail"
      :cocktailType="cocktailsType"
    ></cocktail-card>
  </section>
</template>

<script>
import CocktailCard from "./CocktailCard.vue";

export default {
  components: { CocktailCard },
  props: {
    cocktailsData: {
      type: Object,
      required: true,
    },
    cocktailsType: {
      type: String,
      required: true,
    },
  },
};
</script>
