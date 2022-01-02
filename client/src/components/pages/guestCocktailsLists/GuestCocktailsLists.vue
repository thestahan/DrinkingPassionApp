<template>
  <h5 v-if="notFound">Pod wybranem adresem nie istnieje lista koktajli</h5>

  <div class="list-description" v-if="list">
    <h2 class="p-text-center main-font heading-font">{{ list.name }}</h2>
    <p class="p-text-center created-date">Utworzona: {{ list.createdDate }}</p>
  </div>

  <cocktails-list
    v-if="list"
    :cocktailsData="list"
    cocktailsType="public"
    :listUniqueLink="list.uniqueLink"
  ></cocktails-list>

  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import CocktailsListService from "../../../services/CocktailsListService";
import Spinner from "../../utilities/Spinner.vue";
import CocktailsList from "../cocktails/CocktailsList.vue";

export default {
  components: { Spinner, CocktailsList },
  data() {
    return {
      listId: null,
      list: null,
      isLoading: false,
      notFound: false,
    };
  },
  cocktailListService: null,
  created() {
    this.cocktailListService = new CocktailsListService();
    this.listId = this.$route.params.id;
  },
  mounted() {
    this.getCocktailsList();
  },
  methods: {
    async getCocktailsList() {
      this.isLoading = true;

      try {
        const list = await this.cocktailListService.getCocktailsList(
          this.listId
        );

        list.createdDate = this.formatDate(list.createdDate);

        this.list = list;
      } catch (err) {
        console.warn(err);

        this.notFound = true;
      }

      this.isLoading = false;
    },

    formatDate(date) {
      const dt = new Date(date);

      return dt.toLocaleString("pl-pl", {
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    },
  },
};
</script>

<style scoped>
.created-date {
  font-size: small;
  color: rgb(88, 88, 88);
}
</style>
