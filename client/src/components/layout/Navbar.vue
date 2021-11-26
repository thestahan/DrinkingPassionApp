<template>
  <Menubar :model="navigation">
    <template #start>
      <router-link to="/"
        ><img
          class="logo p-mr-2"
          src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
          alt="Workflow"
        />
      </router-link>
    </template>
    <template #end>
      <div v-if="isAuthenticated">
        <span>Cześć, {{ displayName }}</span>
      </div>
    </template>
  </Menubar>
</template>

<script>
import Menubar from "primevue/menubar";

export default {
  components: {
    Menubar,
  },
  data() {
    return {
      navigation: [
        {
          label: "Strona główna",
          to: "/",
          icon: "pi pi-fw pi-home",
        },
        {
          label: "Koktajle",
          to: "cocktails",
          icon: "pi pi-fw glass-icon",
        },
      ],
    };
  },
  computed: {
    displayName: function () {
      return this.$store.getters.displayName;
    },
    isAuthenticated: function () {
      return this.$store.getters.isAuthenticated;
    },
    isAdmin: function () {
      return this.$store.getters.roles.includes("admin");
    },
  },
  mounted() {
    if (this.isAuthenticated) {
      const cocktailsNavItem = this.navigation.find(
        (i) => i.label == "Koktajle"
      );

      if (cocktailsNavItem) {
        cocktailsNavItem.items = this.getCocktailMenuItems();
      }
    } else {
      this.navigation.push({
        label: "Zaloguj się",
        to: "login",
        icon: "pi pi-fw pi-arrow-right primary-color",
      });
    }
  },
  methods: {
    logout() {
      this.$store.dispatch("logout");
      this.$router.replace("/");
    },
    getCocktailMenuItems() {
      if (!this.isAuthenticated) return [];

      return [
        {
          label: "Publiczne",
          to: "/cocktails",
        },
        {
          label: "Prywatne",
          to: "/cocktails/private",
        },
      ];
    },
  },
};
</script>

<style scoped>
.logo {
  height: 2.5rem;
  width: auto;
}

.p-menubar {
  margin-top: 1rem;
  border: none;
  background: white;
}

.primary-color {
  color: var(--primary-color) !important;
}
</style>

<style>
.glass-icon {
  background-image: url("~@/assets/custom-icons/glass-svgrepo.svg");
  height: 20px;
}
</style>
