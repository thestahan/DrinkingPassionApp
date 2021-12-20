<template>
  <section class="p-m-4 p-d-flex p-jc-between">
    <div>
      <Button
        label="Filtry"
        icon="pi pi-filter"
        iconPos="left"
        @click="toggleFilters"
      ></Button>
    </div>
    <div>
      <Dropdown
        v-model="currentSort"
        :options="sortOptions"
        optionLabel="name"
        placeholder="Sortuj według"
        @change="setFilters"
      ></Dropdown>
    </div>
  </section>

  <section class="p-mt-6 p-ml-4 p-mr-4" v-show="displayFilters">
    <div class="p-grid">
      <div class="p-col-6 p-md-4 p-fluid p-mb-3">
        <span class="p-float-label">
          <InputText
            id="filterCocktailName"
            type="text"
            v-model="filterCocktailName"
          />
          <label for="filterCocktailName">Nazwa koktajlu</label>
        </span>
      </div>

      <div class="p-col-6 p-md-4 p-fluid p-mb-3">
        <span class="p-float-label">
          <InputNumber
            id="ingredientsCount"
            v-model="filterCocktailIngredientsCount"
            :min="2"
            :max="15"
            :showButtons="true"
          />
          <label for="ingredientsCount">Ilość składników</label>
        </span>
      </div>

      <div class="p-col-6 p-md-4 p-fluid p-mb-3">
        <span class="p-float-label">
          <Dropdown
            v-model="filterMainIngredient"
            :options="products"
            dataKey="id"
            optionLabel="name"
            forceSelection
          />
          <label for="ingredientsCount">Główny składnik</label>
        </span>
      </div>

      <div class="p-col-12 p-md-10 p-fluid">
        <span class="p-float-label">
          <Autocomplete
            :multiple="true"
            v-model="filterIngredients"
            :suggestions="filteredProducts"
            :dropdown="true"
            @complete="searchProduct($event)"
            field="name"
          />
          <label for="ingredientsCount">Składniki</label>
        </span>
      </div>

      <div class="p-col-12 p-md-2 p-fluid p-pl-4 p-pr-4">
        <Button label="Filtruj" @click="setFilters"></Button>
      </div>
    </div>
  </section>
</template>

<script>
import Button from "primevue/button";
import Dropdown from "primevue/dropdown";
import InputText from "primevue/inputtext";
import InputNumber from "primevue/inputnumber";
import Autocomplete from "primevue/autocomplete";
import ProductService from "../../../services/ProductSerivce";

export default {
  components: { Button, Dropdown, InputText, InputNumber, Autocomplete },
  props: {
    cocktailsType: {
      type: String,
      required: true,
    },
  },
  emits: ["set-filters"],
  data() {
    return {
      filteredProducts: null,
      products: [],
      sortOptions: [
        { name: "Nazwa rosnąco", code: "NAME_ASC" },
        { name: "Nazwa malejąco", code: "NAME_DESC" },
        { name: "Ilość składników rosnąco", code: "INGREDIENTS_COUNT_ASC" },
        { name: "Ilość składników malejąco", code: "INGREDIENTS_COUNT_DESC" },
      ],
      currentSort: null,
      displayFilters: false,
      filterCocktailName: null,
      filterCocktailIngredientsCount: null,
      filterMainIngredient: null,
      filterIngredients: [],
    };
  },
  productService: null,
  created() {
    this.currentSort = this.sortOptions.find((opt) => opt.code == "NAME_ASC");
    this.productService = new ProductService();
    this.setProducts();
  },
  computed: {
    token() {
      return this.$store.getters.token;
    },
  },
  methods: {
    setFilters() {
      const filters = {
        sort: this.currentSort.code,
      };

      this.$emit("set-filters", filters);
    },
    toggleFilters() {
      this.displayFilters = !this.displayFilters;
    },
    async setProducts() {
      const publicProducts = await this.productService.getPublicProducts();

      if (this.cocktailsType == "private") {
        const privateProducts = await this.productService.getPrivateProducts(
          this.token
        );

        this.products = publicProducts
          .concat(
            privateProducts.map((product) => ({
              ...product,
              isPrivate: true,
            }))
          )
          .sort((a, b) => a.name.localeCompare(b.name));
      } else {
        this.products = publicProducts;
      }

      this.products.unshift({ id: 0, name: "-- Wszystkie --" });
    },
    searchProduct(event) {
      if (!event.query.trim().length) {
        this.filteredProducts = [...this.products];
      } else {
        this.filteredProducts = this.products.filter(
          (product) =>
            product.name.toLowerCase().includes(event.query.toLowerCase()) &&
            !this.filterIngredients.includes(product)
        );
      }
    },
  },
};
</script>
