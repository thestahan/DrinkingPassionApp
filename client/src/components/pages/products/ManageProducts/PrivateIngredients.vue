<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">
      Składniki prywatne (widoczne tylko dla Ciebie)
    </h3>
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
