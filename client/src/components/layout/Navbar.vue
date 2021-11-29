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
      <user-nav v-if="isAuthenticated"></user-nav>
    </template>
  </Menubar>
</template>

<script>
import Menubar from "primevue/menubar";
import UserNav from "../layout/UserNav.vue";

export default {
  components: {
    Menubar,
    UserNav,
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
          icon: "pi glass-icon",
          key: "cocktails",
        },
        {
          separator: true,
        },
        {
          label: "Zaloguj się",
          to: "login",
          icon: "pi pi-fw pi-sign-in",
          key: "login",
          visible: () => !this.isAuthenticated,
        },
      ],
    };
  },
  computed: {
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
    },
  },
  mounted() {
    if (this.isAuthenticated) {
      this.updateCocktailsNavItem();
    }
  },
  methods: {
    updateCocktailsNavItem() {
      const cocktailsNavItem = this.navigation.find(
        (i) => i.key == "cocktails"
      );

      if (this.isAuthenticated) {
        cocktailsNavItem.to = "";
        cocktailsNavItem.items = [
          {
            label: "Publiczne",
            to: "/cocktails/public",
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
  },
};
</script>

<style scoped>
.logo {
  height: 2.5rem;
  width: auto;
}

.p-menubar {
  z-index: 20;
  margin-top: 1rem;
  border: none;
  background: white;
}
</style>

<style>
.p-menuitem-text {
  font-weight: 500;
  color: rgb(107, 114, 128);
  font-family: "Lato", sans-serif;
}

.p-submenu-list {
  z-index: 20 !important;
}

.glass-icon {
  background-image: url("~@/assets/custom-icons/whisky.png");
  height: 16px;
  width: 16px;
}
</style>
