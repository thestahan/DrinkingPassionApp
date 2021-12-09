<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">
      Koktajle prywatne (widoczne tylko dla Ciebie)
    </h3>
  </div>

  <manage-cocktails
    v-if="privateCocktailsData && products"
    cocktailsType="private"
    :cocktailsData="privateCocktailsData"
    :products="products"
  ></manage-cocktails>
</template>

<script>
import ManageCocktails from "./ManageCocktails.vue";

export default {
  components: {
    ManageCocktails,
  },
  computed: {
    privateCocktailsData() {
      return this.$store.getters.privateCocktailsData;
    },
    products() {
      return this.$store.getters.publicProductsData;
    },
  },
};
</script>
