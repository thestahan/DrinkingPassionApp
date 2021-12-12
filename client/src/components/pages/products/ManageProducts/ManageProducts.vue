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
    >
      <Column field="name" header="Nazwa" :sortable="true"> </Column>
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
      <template #empty> Nie znaleziono składników. </template>
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

  <Toast position="bottom-right" />
</template>

<script>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import ProductDetailsDialog from "../ManageProducts/ProductDetailsDialog.vue";
import ProductsService from "../../../../services/ProductSerivce";
import Toast from "primevue/toast";

export default {
  components: {
    Button,
    DataTable,
    Column,
    ProductDetailsDialog,
    Toast,
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
    },
    openAddIngredientDialog() {
      this.mode = "add";
      this.showProductsDetailsDialog = true;
      this.productToManage = {};
    },
    async deleteIngredient(ingredientId) {
      try {
        const isPartOfAnyCocktail = await this.$store.dispatch(
          "isPartOfCocktail",
          {
            id: ingredientId,
            isPrivate: this.ingredientsType == "private",
            token: this.token,
          }
        );

        if (isPartOfAnyCocktail) {
          this.showDeleteError(
            "Składnik jest wykorzystywany w co najmniej jednym koktajlu. Usuń najpierw koktajl."
          );

          return;
        }

        const deletedInfo = await this.$store.dispatch("deleteProduct", {
          token: this.token,
          id: ingredientId,
          isPrivate: this.ingredientsType == "private",
        });

        if (deletedInfo.success) {
          this.showDeleteSuccess();
          return;
        }

        this.showDeleteError(deletedInfo.message);
      } catch (err) {
        console.warn(err);
      }
    },
    closeProductDetailsDialog() {
      this.showProductsDetailsDialog = false;
    },
    async manageIngredient(product) {
      try {
        product.isPrivate = this.ingredientsType == "public" ? false : true;

        if (product.id == 0) {
          this.$store.dispatch("addProduct", {
            token: this.token,
            product: product,
          });

          this.showAddSuccess();
        } else {
          this.$store.dispatch("updateProduct", {
            token: this.token,
            product: product,
          });

          this.showEditSuccess();
        }

        this.closeProductDetailsDialog();
      } catch (err) {
        console.warn(err.toJSON());
      }
    },
    showEditSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Dane składnika zostały zapisane",
        life: 3000,
      });
    },
    showAddSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Składnik został pomyślnie dodany",
        life: 3000,
      });
    },
    showDeleteSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Składnik został usunięty",
        life: 3000,
      });
    },
    showDeleteError(errorMessage) {
      this.$toast.add({
        severity: "error",
        summary: "Wystąpił błąd",
        detail: errorMessage,
        life: 10000,
      });
    },
  },
};
</script>
