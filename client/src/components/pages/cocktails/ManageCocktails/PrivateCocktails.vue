<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">
      Koktajle prywatne (widoczne tylko dla Ciebie)
    </h3>
  </div>

  <manage-cocktails
    cocktailsType="private"
    :cocktailsData="privateCocktailsData"
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
  },
};
</script>
