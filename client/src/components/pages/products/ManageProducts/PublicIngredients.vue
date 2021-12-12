<template>
  <div class="p-m-4">
    <h3 class="main-font tertiary-heading-font">
      Składniki publiczne (widoczne dla wszystkich)
    </h3>
  </div>

  <manage-products
    v-if="publicProductsData && productUnits && productTypes"
    ingredientsType="public"
    :ingredientsData="publicProductsData"
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
    publicProductsData() {
      return this.$store.getters.publicProductsData;
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
