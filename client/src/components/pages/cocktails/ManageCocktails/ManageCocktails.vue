<template>
  <header class="p-mb-6">
    <h2 class="p-text-center main-font heading-font">Zarządzanie koktajlami</h2>
  </header>

  <section class="cocktails-toolbar p-ml-4">
    <Button
      label="Dodaj koktajl"
      icon="pi pi-plus"
      class="p-button-success p-mr-2"
      @click="openCocktailDetailsDialog"
    />
  </section>

  <section class="cocktails-table p-m-4" v-if="cocktails">
    <DataTable
      :value="cocktails"
      :paginator="true"
      :rows="10"
      :totalRecords="cocktailsTotal"
      :loading="isLoading"
      :rowsPerPageOptions="[10, 20, 50]"
      stripedRows
      responsiveLayout="stack"
      breakpoint="960px"
    >
      <Column header="Zdjęcie">
        <template #body="{ data }">
          <img :src="data.picture" class="cocktail-image" />
        </template>
      </Column>
      <Column field="name" header="Nazwa"></Column>
      <Column field="baseIngredient" header="Główny składnik"></Column>
      <Column field="ingredientsCount" header="Liczba składników"></Column>
      <Column header="Edytuj">
        <template #body="slotProps">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-warning"
            @click="openEditCocktailDetailsDialog(slotProps.data.id)"
          />
        </template>
      </Column>
      <Column header="Usuń">
        <template #body="slotProps">
          <Button
            icon="pi pi-trash"
            class="p-button-rounded p-button-danger"
            @click="console.log(slotProps)"
          />
        </template>
      </Column>
    </DataTable>
  </section>

  <cocktail-details-dialog
    v-if="renderCocktailDetailsDialog"
    :cocktail="cocktail"
    :products="products"
    @close-modal="closeCocktailDetailsDialog"
    @manage-cocktail="manageCocktail"
    v-model:visible="newCocktailDialog"
  ></cocktail-details-dialog>
</template>

<script>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import CocktailService from "../../../../services/CocktailService";
import ProductSerivce from "../../../../services/ProductSerivce";
import CocktailDetailsDialog from "../ManageCocktails/CocktailDetailsDialog.vue";

export default {
  components: {
    DataTable,
    Column,
    Button,
    CocktailDetailsDialog,
  },
  data() {
    return {
      cocktails: [],
      products: [],
      cocktailsTotal: null,
      pageIndex: 1,
      pageSize: 12,
      isLoading: false,
      deleteCocktailDialog: false,
      newCocktailDialog: false,
      cocktail: {},
      submitted: false,
      mode: null,
    };
  },
  computed: {
    renderCocktailDetailsDialog() {
      if (this.mode == "add") {
        return this.newCocktailDialog;
      }

      if (this.mode == "edit") {
        return this.cocktail.id;
      }

      return false;
    },
  },
  cocktailService: null,
  productService: null,
  created() {
    this.cocktailService = new CocktailService();
    this.productService = new ProductSerivce();
  },
  mounted() {
    this.getCocktails();
    this.getProducts();
  },
  methods: {
    async getCocktails() {
      this.isLoading = true;

      try {
        const data = await this.cocktailService.getCocktails();

        this.cocktails = data.data;
        this.cocktailsTotal = data.count;
        this.pageIndex = data.pageIndex;
        this.pageSize = data.pageSize;
      } catch (err) {
        console.error(err.toJSON());
      }

      this.isLoading = false;
    },
    async getCocktail(id) {
      this.isLoading = true;

      try {
        this.cocktail = await this.cocktailService.getCocktail(id);
      } catch (err) {
        console.error(err.toJSON());
      }

      this.isLoading = false;
    },
    async getProducts() {
      try {
        this.products = await this.productService.getProducts();
      } catch (err) {
        console.warning(err.toJSON());
      }
    },
    async manageCocktail(cocktail) {
      try {
        const response = await this.cocktailService.addCocktail(cocktail);
        console.log(response);
      } catch (err) {
        console.warning(err.toJSON());
      }
    },
    openEditCocktailDetailsDialog(cocktailId) {
      this.mode = "edit";
      this.cocktail = this.getCocktail(cocktailId);
      this.submitted = false;
      this.newCocktailDialog = true;
    },
    openCocktailDetailsDialog() {
      this.mode = "add";
      this.cocktail = {};
      this.submitted = false;
      this.newCocktailDialog = true;
    },
    closeCocktailDetailsDialog() {
      this.mode = null;
      this.cocktail = null;
      this.newCocktailDialog = false;
    },
  },
};
</script>

<style scoped>
.cocktail-image {
  width: 60px;
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}
</style>
