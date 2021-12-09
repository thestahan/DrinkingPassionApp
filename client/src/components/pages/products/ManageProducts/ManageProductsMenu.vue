<template>
  <div class="card p-mb-6">
    <header class="p-mb-6">
      <h2 class="p-text-center main-font heading-font">
        Zarządzanie składnikami koktajli
      </h2>
    </header>
    <TabMenu :model="ingredientsTypes" v-model:activeIndex="activeIndex" />

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
      ingredientsTypes: [
        {
          label: "Prywatne",
          icon: "pi pi-fw pi-lock",
          to: "/ingredients/manage/private",
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
    this.getPrivateProducts();
    this.getUnits();
    this.getTypes();

    if (this.isAdmin) {
      this.getPublicProducts();

      this.ingredientsTypes.unshift({
        label: "Publiczne",
        icon: "pi pi-fw pi-lock-open",
        to: "/ingredients/manage/public",
      });
    }
  },
  methods: {
    async getPublicProducts() {
      this.isLoading = true;

      this.$store.dispatch("fetchPublicProducts");

      this.isLoading = false;
    },
    async getPrivateProducts() {
      this.isLoading = true;

      this.$store.dispatch("fetchPrivateProducts", { token: this.token });

      this.isLoading = false;
    },
    async getUnits() {
      this.$store.dispatch("fetchProductUnits");
    },
    async getTypes() {
      this.$store.dispatch("fetchProductTypes");
    },
  },
};
</script>
