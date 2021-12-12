<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">Koktajle prywatne</h3>

    <section class="private-cocktails-description p-mt-2">
      Koktajle prywatne po dodaniu są widoczne tylko dla Ciebie. Do ich
      utworzenia możesz wykorzystać zarówno składniki publiczne, zarządzane
      przez administratora, jak i swoje własne, utworzone w zakładce
      <router-link
        to="/ingredients/manage/private"
        class="primary-color p-text-bold"
        >zarządzanie składnikami</router-link
      >. Twoje składniki w widoku edycji koktajlu będą zaznaczone pochyłą
      czcionką.
    </section>
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
    publicProducts() {
      return this.$store.getters.publicProductsData;
    },
    privateProducts() {
      return this.$store.getters.privateProductsData;
    },
    products() {
      if (!this.privateProducts) return this.publicProducts;

      return this.publicProducts
        .concat(
          this.privateProducts.map((product) => ({
            ...product,
            isPrivate: true,
          }))
        )
        .sort((a, b) => a.name.localeCompare(b.name));
    },
  },
};
</script>
