<template>
  <header class="p-mb-6">
    <h2 class="p-text-center main-font heading-font">Listy koktajli</h2>
  </header>

  <section class="cocktails-lists-toolbar p-ml-4">
    <Button
      label="Dodaj listę"
      icon="pi pi-plus"
      class="p-button-success p-mr-2"
    />
  </section>

  <section class="cocktails-lists-table p-m-4" v-if="cocktailsLists">
    <DataTable
      :value="cocktailsLists"
      :loading="isLoading"
      stripedRows
      breakpoint="960px"
    >
      <Column field="name" header="Nazwa"></Column>
      <Column field="uniqueLink" header="Link">
        <template #body="{ data }">
          <router-link
            :to="'/guests/cocktailslists/' + data.uniqueLink"
            class="primary-color"
            >Przejdź do listy</router-link
          >
        </template>
      </Column>
      <Column field="cocktailsCount" header="Ilość koktajli"></Column>
      <Column field="createdDate" header="Data utworzenia"></Column>
      <Column header="Edytuj">
        <template #body="slotProps">
          <Button
            icon="pi pi-pencil"
            class="p-button-rounded p-button-warning"
            @click="editCocktailsList(slotProps.data.id)"
          />
        </template>
      </Column>
      <Column header="Usuń">
        <template #body="slotProps">
          <Button
            icon="pi pi-trash"
            class="p-button-rounded p-button-danger"
            @click="deleteCocktailsList(slotProps.data.id)"
          />
        </template>
      </Column>
      <template #empty> Nie znaleziono list koktajli. </template>
    </DataTable>
  </section>

  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import CocktailsListService from "../../../services/CocktailsListService";
import Spinner from "../../utilities/Spinner.vue";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";

export default {
  components: { Spinner, DataTable, Column, Button },
  data() {
    return {
      isLoading: false,
      cocktailsLists: [],
    };
  },
  cocktailListService: null,
  created() {
    this.cocktailListService = new CocktailsListService();
  },
  mounted() {
    this.getCocktailsLists();
  },
  computed: {
    token() {
      return this.$store.getters.token;
    },
  },
  methods: {
    async getCocktailsLists() {
      this.isLoading = true;

      try {
        const lists = await this.cocktailListService.getCocktailsLists(
          this.token
        );

        for (const list of lists) {
          const date = new Date(list.createdDate);
          list.createdDate = date.toLocaleString("pl-pl", {
            year: "numeric",
            month: "numeric",
            day: "numeric",
            hour: "numeric",
            minute: "numeric",
          });
        }

        this.cocktailsLists = lists;
      } catch (err) {
        console.warn(err);
      }

      this.isLoading = false;
    },

    deleteCocktailsList(id) {
      console.log("deletring list of id" + id);
    },

    editCocktailsList(id) {
      console.log("editing list of id" + id);
    },
  },
};
</script>
