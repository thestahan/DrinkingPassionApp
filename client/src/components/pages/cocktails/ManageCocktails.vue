<template>
  <header class="p-mb-6">
    <h2 class="p-text-center main-font heading-font">Zarządzanie koktajlami</h2>
  </header>

  <section class="cocktails-toolbar p-ml-4">
    <Button
      label="Dodaj koktajl"
      icon="pi pi-plus"
      class="p-button-success p-mr-2"
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
            @click="console.log(slotProps)"
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
</template>

<script>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import CocktailService from "../../../services/CocktailService";

export default {
  components: {
    DataTable,
    Column,
    Button,
  },
  data() {
    return {
      cocktails: [],
      cocktailsTotal: null,
      pageIndex: 1,
      pageSize: 12,
      isLoading: false,
      deleteCocktailDialog: false,
      cocktail: {},
    };
  },
  cocktailService: null,
  created() {
    this.cocktailService = new CocktailService();
  },
  mounted() {
    this.getCocktails();
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
        console.log(err.toJSON());
      }

      this.isLoading = false;
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
