<template>
  <section class="ingredients-toolbar p-ml-4">
    <Button
      label="Dodaj składnik"
      icon="pi pi-plus"
      class="p-button-success p-mr-2"
      @click="openAddIngredientDialog"
    ></Button>
  </section>

  <section class="ingredients-table p-m-4">
    <DataTable
      :value="ingredients"
      :rows="10"
      :paginator="true"
      :loading="isLoading"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      stripedRows
      v-if="ingredients.length > 0"
    >
      <Column field="name" header="Nazwa" :sortable="true"></Column>
      <Column field="productType" header="Typ" :sortable="true"></Column>
      <Column field="productUnit" header="Jednostka" :sortable="true"></Column>
      <Column header="Edytuj">
        <template #body="slotProps">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-warning"
            @click="openEditIngredientDialog(slotProps.data.id)"
          ></Button>
        </template>
      </Column>
      <Column header="Usuń">
        <template #body="slotProps">
          <Button
            icon="pi pi-trash"
            class="p-button-rounded p-button-danger"
            @click="deleteIngredient(slotProps.data.id)"
          />
        </template>
      </Column>
    </DataTable>
  </section>

  <product-details-dialog
    v-if="showProductsDetailsDialog"
    :product="productToManage"
    :types="types"
    :units="units"
    @close-modal="closeProductDetailsDialog"
    @manage-ingredient="manageIngredient"
    v-model:visible="showProductsDetailsDialog"
  >
  </product-details-dialog>
</template>

<script>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import ProductDetailsDialog from "../ManageProducts/ProductDetailsDialog.vue";
import ProductsService from "../../../../services/ProductSerivce";

export default {
  components: {
    Button,
    DataTable,
    Column,
    ProductDetailsDialog,
  },
  props: {
    ingredientsType: {
      type: String,
      required: true,
    },
    ingredientsData: [Object, null],
    units: {
      type: Array,
      required: true,
    },
    types: {
      type: Array,
      required: true,
    },
  },
  data() {
    return {
      isLoading: false,
      showProductsDetailsDialog: false,
      productToManage: {},
      mode: null,
      productsService: null,
    };
  },
  computed: {
    token() {
      return this.$store.getters.token;
    },
    ingredients() {
      if (!this.ingredientsData) return;

      return this.ingredientsData;
    },
  },
  created() {
    this.productsService = new ProductsService();
  },
  methods: {
    openEditIngredientDialog(ingredientId) {
      this.productToManage = this.ingredientsData.find(
        (p) => p.id == ingredientId
      );
      this.mode = "edit";
      this.showProductsDetailsDialog = true;
      console.log("editing: " + ingredientId);
      console.log(this.productToManage);
    },
    openAddIngredientDialog() {
      this.mode = "add";
      this.showProductsDetailsDialog = true;
      this.productToManage = {};
    },
    deleteIngredient(ingredientId) {
      console.log("deletnig: " + ingredientId);
    },
    closeProductDetailsDialog() {
      this.showProductsDetailsDialog = false;
    },
    async manageIngredient(product) {
      try {
        product.isPrivate = this.ingredientsType == "public" ? false : true;

        if (product.id == 0) {
          // //   const response = await this.productsService.addProduct(
          // //     this.token,
          // //     product
          // //   );

          //   const newProduct = response.data;
          this.$store.dispatch("addProduct", {
            token: this.token,
            product: product,
          });
        } else {
          //   const response = await this.productsService.updateProduct(
          //     this.token,
          //     product
          //   );
          //   const updatedProduct = response.data;
          //   this.$store.dispatch(
          //     this.product.isPrivate
          //       ? "updatePrivateProduct"
          //       : "updatePublicProduct",
          //     {
          //       product: updatedProduct,
          //     }
          //   );
          this.$store.dispatch("updateProduct", {
            token: this.token,
            product: product,
          });
        }
      } catch (err) {
        console.warn(err.toJSON());
      }
    },
  },
};
</script>
