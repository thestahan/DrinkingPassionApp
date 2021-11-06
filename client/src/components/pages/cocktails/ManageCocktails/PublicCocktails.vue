<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">
      Koktajle publiczne (widoczne dla wszystkich)
    </h3>
  </div>

  <manage-cocktails
    cocktailsType="public"
    :cocktailsData="publicCocktailsData"
  ></manage-cocktails>
</template>

<script>
import ManageCocktails from "./ManageCocktails.vue";

export default {
  components: {
    ManageCocktails,
  },
  computed: {
    publicCocktailsData() {
      return this.$store.getters.publicCocktailsData;
    },
  },
};
</script>
