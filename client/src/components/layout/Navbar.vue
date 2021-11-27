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
        <!-- <span>Cześć, {{ displayName }}</span> -->
        <button @click="logout">Wyloguj się</button>
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
          key: "home",
        },
        {
          label: "Koktajle",
          to: "/cocktails",
          icon: "pi pi-fw glass-icon",
          key: "cocktails",
        },
        {
          label: "Zaloguj się",
          to: "login",
          icon: "pi pi-fw pi-arrow-right",
          key: "login",
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
      if (!this.$store.getters.roles) return false;

      return this.$store.getters.roles.includes("admin");
    },
  },
  watch: {
    isAuthenticated: function () {
      this.updateCocktailsNavItem();
      this.updateLoginNavItem();
    },
  },
  mounted() {
    if (this.isAuthenticated) {
      this.updateCocktailsNavItem();
      this.updateLoginNavItem();
    }
  },
  methods: {
    logout() {
      this.$store.dispatch("logout");
      this.$router.replace("/");
    },
    updateCocktailsNavItem() {
      const cocktailsNavItem = this.navigation.find(
        (i) => i.key == "cocktails"
      );

      if (this.isAuthenticated) {
        cocktailsNavItem.to = "";
        cocktailsNavItem.items = [
          {
            label: "Publiczne",
            to: "/cocktails",
            icon: "pi pi-fw pi-lock-open",
          },
          {
            label: "Prywatne",
            to: "/cocktails/private",
            icon: "pi pi-fw pi-lock",
          },
        ];
      } else {
        cocktailsNavItem.to = "/cocktails";
        cocktailsNavItem.items = [];
      }
    },
    updateLoginNavItem() {
      if (this.isAuthenticated) {
        this.navigation = this.navigation.filter((i) => i.key != "login");
      } else {
        const navItem = this.navigation.find((i) => i.key == "login");

        if (!navItem) {
          this.navigation.push({
            label: "Zaloguj się",
            to: "login",
            icon: "pi pi-fw pi-arrow-right",
            key: "login",
          });
        }
      }
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
</style>

<style>
.glass-icon {
  background-image: url("~@/assets/custom-icons/glass-svgrepo.svg");
  height: 20px;
}
</style>
