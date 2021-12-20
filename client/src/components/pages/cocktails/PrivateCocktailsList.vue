<template>
  <header class="p-mb-6">
    <h2 class="p-text-center main-font heading-font">Lista Twoich koktajli</h2>
  </header>

  <cocktails-filters
    @set-filters="setFilters"
    cocktailsType="private"
  ></cocktails-filters>

  <cocktails-list
    v-if="cocktailsData.cocktails"
    :cocktailsData="cocktailsData"
    cocktailsType="private"
  ></cocktails-list>

  <Paginator
    v-if="
      cocktailsData.cocktails &&
      cocktailsData.pageSize < cocktailsData.totalCount
    "
    v-model:rows="cocktailsData.pageSize"
    :totalRecords="cocktailsData.totalCount"
    :rowsPerPageOptions="[3, 9, 18]"
    @page="getPrivateCocktails($event)"
  ></Paginator>

  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import CocktailService from "../../../services/CocktailService";
import Spinner from "../../utilities/Spinner.vue";
import CocktailsList from "./CocktailsList.vue";
import Paginator from "primevue/paginator";
import CocktailsFilters from "./CocktailsFilters.vue";

export default {
  components: { Spinner, CocktailsList, Paginator, CocktailsFilters },
  data() {
    return {
      cocktailsData: {},
      isLoading: false,
      url: process.env.VUE_APP_API_URL,
      currentSort: null,
    };
  },
  cocktailService: null,
  created() {
    this.cocktailService = new CocktailService();
  },
  mounted() {
    this.getPrivateCocktails(null);
  },
  computed: {
    token() {
      return this.$store.getters.token;
    },
  },
  methods: {
    async getPrivateCocktails(event) {
      this.isLoading = true;

      try {
        const queryParams = {
          pageIndex: (event?.page ?? 0) + 1,
          pageSize: event?.rows ?? 9,
          sort: this.currentSort,
        };

        const data = await this.cocktailService.getPrivateCocktails(
          this.token,
          queryParams
        );

        this.cocktailsData.cocktails = data.data;
        this.cocktailsData.totalCount = data.count;
        this.cocktailsData.pageIndex = data.pageIndex - 1;
        this.cocktailsData.pageSize = data.pageSize;
      } catch (err) {
        console.error(err.toJSON());
      }

      this.isLoading = false;
    },
    setFilters(filters) {
      this.currentSort = filters.sort;

      this.getPrivateCocktails(null);
    },
  },
};
</script>
