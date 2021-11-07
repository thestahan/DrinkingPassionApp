<template>
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
          <img
            :src="data.picture"
            :id="'cocktail-picture-' + data.id"
            class="cocktail-image"
          />
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
            @click="deleteCocktail(slotProps.data.id)"
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

  <Toast position="bottom-right" />
</template>

<script>
import DataTable from "primevue/datatable";
import Column from "primevue/column";
import Button from "primevue/button";
import CocktailService from "../../../../services/CocktailService";
import ProductSerivce from "../../../../services/ProductSerivce";
import CocktailDetailsDialog from "../ManageCocktails/CocktailDetailsDialog.vue";
import Toast from "primevue/toast";

export default {
  components: {
    DataTable,
    Column,
    Button,
    CocktailDetailsDialog,
    Toast,
  },
  props: {
    cocktailsType: {
      type: String,
      required: true,
    },
    cocktailsData: [Object, null],
  },
  data() {
    return {
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
      privateCocktailsView: this.cocktailsType == "private",
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
    token() {
      return this.$store.getters.token;
    },
    cocktails() {
      if (!this.cocktailsData) return;

      return this.cocktailsData.data;
    },
  },
  cocktailService: null,
  productService: null,
  created() {
    this.cocktailService = new CocktailService();
    this.productService = new ProductSerivce();
  },
  mounted() {
    this.getProducts();
  },
  methods: {
    async getCocktail(id) {
      this.isLoading = true;

      try {
        this.cocktail = this.privateCocktailsView
          ? await this.cocktailService.getCocktail(id, this.token)
          : await this.cocktailService.getCocktail(id);
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
        const formData = new FormData();

        for (var key in cocktail) {
          if (cocktail[key]) {
            formData.append(key, cocktail[key]);
          }
        }

        formData.set("ingredients", JSON.stringify(cocktail.ingredients));

        if (this.privateCocktailsView) {
          formData.set("isPrivate", true);
        }

        const response = await this.cocktailService.manageCocktail(
          formData,
          this.token
        );

        if (response.status == "201") {
          const newCocktail = response.data;
          this.cocktails.push(newCocktail);

          this.closeCocktailDetailsDialog();
          this.showAddSuccess();
        } else if (response.status == "200") {
          const editedCocktail = this.cocktails.find(
            (x) => x.id == cocktail.id
          );

          const index = this.cocktails.indexOf(editedCocktail);

          if (index === -1) return;

          this.cocktails[index] = response.data;

          this.closeCocktailDetailsDialog();
          this.showEditSuccess();
        }
      } catch (err) {
        console.warn(err.toJSON());
      }
    },
    async deleteCocktail(id) {
      this.isLoading = true;

      const deletedCocktail = this.$store.dispatch("deleteCocktail", {
        id: id,
        token: this.token,
        isPrivate: this.privateCocktailsView,
      });

      if (deletedCocktail) {
        this.showDeleteSuccess();
      }

      this.isLoading = false;
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
    showEditSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Dane koktajlu zostały zapisane",
        life: 3000,
      });
    },
    showAddSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Koktajl został pomyślnie dodany",
        life: 3000,
      });
    },
    showDeleteSuccess() {
      this.$toast.add({
        severity: "success",
        summary: "Sukces",
        detail: "Koktajl został usunięty",
        life: 3000,
      });
    },
  },
};
</script>

<style scoped>
.cocktail-image {
  width: 100px;
  box-shadow: 0 3px 6px rgba(0, 0, 0, 0.16), 0 3px 6px rgba(0, 0, 0, 0.23);
}
</style>
