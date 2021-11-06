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
import ProductSerivce from "../../../../services/ProductSerivce";

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
  productService: null,
  created() {
    this.productService = new ProductSerivce();
  },
  mounted() {
    this.getPrivateCocktails();
    // this.getProducts();

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

      this.$store.dispatch("fetchPublicCocktails");

      this.isLoading = false;
    },
    async getPrivateCocktails() {
      this.isLoading = true;

      this.$store.dispatch("fetchPrivateCocktails", { token: this.token });

      this.isLoading = false;
    },
  },
};
</script>
