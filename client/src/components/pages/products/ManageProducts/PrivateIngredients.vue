<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">Składniki prywatne</h3>

    <section class="private-cocktails-description p-mt-2">
      Jeśli w składnikach publicznych brakuje interesujących Cię pozycji, możesz
      dodać swoje własne. Będą one widczone tylko dla Ciebie i będziesz mógł
      wykorzystać je podczas
      <router-link
        to="/cocktails/manage/private"
        class="primary-color p-text-bold"
        >zarządzania koktajlami</router-link
      >.
    </section>
  </div>

  <manage-products
    v-if="privateProductsData && productUnits && productTypes"
    ingredientsType="private"
    :ingredientsData="privateProductsData"
    :units="productUnits"
    :types="productTypes"
  ></manage-products>
</template>

<script>
import ManageProducts from "../ManageProducts/ManageProducts.vue";

export default {
  components: {
    ManageProducts,
  },
  computed: {
    privateProductsData() {
      return this.$store.getters.privateProductsData;
    },
    productUnits() {
      return this.$store.getters.productUnits;
    },
    productTypes() {
      return this.$store.getters.productTypes;
    },
  },
};
</script>
