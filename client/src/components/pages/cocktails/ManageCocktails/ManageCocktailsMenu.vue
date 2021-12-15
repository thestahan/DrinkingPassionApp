<template>
  <div class="card p-mb-6">
    <header class="p-mb-6">
      <h2 class="p-text-center main-font heading-font">
        Zarządzanie koktajlami
      </h2>
    </header>
    <TabMenu :model="cocktailsTypes" v-model:activeIndex="activeIndex" />

    <router-view></router-view>
  </div>
</template>

<script>
import TabMenu from "primevue/tabmenu";

export default {
  components: {
    TabMenu,
  },
  data() {
    return {
      activeIndex: 0,
      cocktailsTypes: [
        {
          label: "Prywatne",
          icon: "pi pi-fw pi-lock",
          to: "/cocktails/manage/private",
        },
      ],
    };
  },
  computed: {
    isAdmin() {
      return this.$store.getters.roles.includes("admin");
    },
    token() {
      return this.$store.getters.token;
    },
  },
  mounted() {
    this.getPrivateCocktails();
    this.getPublicProducts();
    this.getPrivateProducts();

    if (this.isAdmin) {
      this.getPublicCocktails();

      this.cocktailsTypes.unshift({
        label: "Publiczne",
        icon: "pi pi-fw pi-lock-open",
        to: "/cocktails/manage/public",
      });
    }
  },
  methods: {
    async getPublicCocktails() {
      this.isLoading = true;

      const queryParams = {
        pageIndex: 1,
        pageSize: 10,
      };

      this.$store.dispatch("fetchPublicCocktails", {
        queryParams: queryParams,
      });

      this.isLoading = false;
    },
    async getPrivateCocktails() {
      this.isLoading = true;

      const queryParams = {
        pageIndex: 1,
        pageSize: 10,
      };

      this.$store.dispatch("fetchPrivateCocktails", {
        token: this.token,
        queryParams: queryParams,
      });

      this.isLoading = false;
    },
    async getPublicProducts() {
      this.$store.dispatch("fetchPublicProducts");
    },
    async getPrivateProducts() {
      this.$store.dispatch("fetchPrivateProducts", { token: this.token });
    },
  },
};
</script>
