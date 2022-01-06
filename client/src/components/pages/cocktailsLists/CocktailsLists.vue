<template>
  <header class="p-mb-6">
    <h2 class="p-text-center main-font heading-font">Listy koktajli</h2>
  </header>

  <section class="cocktails-lists-toolbar p-ml-4">
    <Button
      label="Dodaj listę"
      icon="pi pi-plus"
      class="p-button-success p-mr-2"
      @click="openNewListDetailsDialog"
    />
  </section>

  <section class="cocktails-lists-table p-m-4" v-if="cocktailsLists">
    <DataTable
      :value="cocktailsLists"
      :loading="isLoading"
      :paginator="true"
      :rows="10"
      :rowsPerPageOptions="[10, 20, 50]"
      responsiveLayout="scroll"
      stripedRows
      breakpoint="960px"
    >
      <Column field="name" header="Nazwa" :sortable="true"></Column>
      <Column field="uniqueLink" header="Link">
        <template #body="{ data }">
          <div class="p-d-flex">
            <router-link
              :to="'/guests/cocktailslists/' + data.uniqueLink"
              class="primary-color"
              >Przejdź do listy</router-link
            >
            <i
              class="pi pi-copy p-ml-2"
              style="font-size: 1.5rem; cursor: pointer"
              v-tooltip="'Skopiuj link'"
              @click="
                copyLinkToClipboard('/guests/cocktailslists/' + data.uniqueLink)
              "
            ></i>
          </div>
        </template>
      </Column>
      <Column
        field="cocktailsCount"
        header="Ilość koktajli"
        :sortable="true"
      ></Column>
      <Column
        field="createdDate"
        header="Data utworzenia"
        :sortable="true"
      ></Column>
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

  <list-details-dialog
    v-if="displayListDialog"
    v-model:visible="displayListDialog"
    :list="newList"
    @close-dialog="closeListDetailsDialog"
    @manage-list="manageList"
  >
  </list-details-dialog>

  <Toast position="bottom-right" />
  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import CocktailsListService from "../../../services/CocktailsListService";
import CocktailService from "../../../services/CocktailService";
import Spinner from "../../utilities/Spinner.vue";
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import Toast from "primevue/toast";
import ListDetailsDialog from "./ListDetailsDialog.vue";

export default {
  components: { Spinner, DataTable, Column, Button, Toast, ListDetailsDialog },
  data() {
    return {
      isLoading: false,
      cocktailsLists: [],
      focusedLinkToList: null,
      newList: null,
      displayListDialog: false,
    };
  },
  cocktailListService: null,
  cocktailService: null,
  created() {
    this.cocktailListService = new CocktailsListService();
    this.cocktailService = new CocktailService();
  },
  mounted() {
    this.getCocktailsLists();
    this.getCocktails();
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
            month: "2-digit",
            day: "2-digit",
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

    async getCocktails() {
      await this.$store.dispatch("fetchAllAvailableCocktails", {
        token: this.token,
      });
    },

    async deleteCocktailsList(id) {
      try {
        const response = await this.cocktailListService.deleteCocktailsList(
          id,
          this.token
        );

        if (response.status != 204) {
          throw new Error();
        }

        this.cocktailsLists = this.cocktailsLists.filter((l) => l.id != id);
        this.showDeleteSuccess();
      } catch (err) {
        console.log(err);
      }
    },

    async editCocktailsList(id) {
      try {
        const listToEdit =
          await this.cocktailListService.getCocktailsListDetails(
            id,
            this.token
          );

        this.newList = {
          id: listToEdit.id,
          name: listToEdit.name,
          cocktails: listToEdit.cocktails.map((cocktail) => ({
            id: cocktail.id,
            name: cocktail.name,
            isPrivate: cocktail.isPrivate,
          })),
        };
        this.openListDetailsDialog();
      } catch (err) {
        console.log(err);
      }
    },

    openNewListDetailsDialog() {
      this.newList = {};
      this.openListDetailsDialog();
    },

    openListDetailsDialog() {
      this.displayListDialog = true;
    },

    closeListDetailsDialog() {
      this.displayListDialog = false;
    },

    async copyLinkToClipboard(link) {
      await navigator.clipboard.writeText(
        process.env.VUE_APP_CLIENT_URL + link
      );

      this.$toast.add({
        severity: "info",
        summary: "Skopiowano link do schowka",
      });
    },

    async manageList(list) {
      try {
        var response = await this.cocktailListService.manageCocktailsList(
          list,
          this.token
        );

        const managedList = response.data;
        managedList.createdDate = this.formatDate(managedList.createdDate);

        if (response.status == 201) {
          this.cocktailsLists.push(managedList);
          this.showAddSuccess();
        } else if (response.status == 200) {
          const editedList = this.cocktailsLists.find((l) => l.id == list.id);

          const index = this.cocktailsLists.indexOf(editedList);

          if (index === -1) return;

          this.cocktailsLists[index] = managedList;
          this.showEditSuccess();
        }

        this.sortCocktailsList();
        this.closeListDetailsDialog();
      } catch (err) {
        console.error(err);
      }
    },

    sortCocktailsList() {
      this.cocktailsLists = this.cocktailsLists.sort((a, b) =>
        a.name.localeCompare(b.name)
      );
    },

    showEditSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Dane listy koktajli zostały zapisane",
        life: 3000,
      });
    },

    showAddSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Lista koktajli została pomyślnie dodana",
        life: 3000,
      });
    },

    showDeleteSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "List koktajli została usunięta",
        life: 3000,
      });
    },

    formatDate(date) {
      return new Date(date).toLocaleString("pl-pl", {
        year: "numeric",
        month: "2-digit",
        day: "2-digit",
        hour: "numeric",
        minute: "numeric",
      });
    },
  },
};
</script>
