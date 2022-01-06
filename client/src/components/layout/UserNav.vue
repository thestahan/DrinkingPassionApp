<template>
  <Menu as="div" class="ml-3 relative">
    <div>
      <MenuButton class="p-mr-2 p-pl-2 p-pr-2">
        <Avatar icon="pi pi-user" class="p-mr-2" shape="circle" />
        <span>{{ displayName }}</span>
      </MenuButton>
    </div>
    <transition
      enter-active-class="transition ease-out duration-100"
      enter-from-class="transform opacity-0 scale-95"
      enter-to-class="transform opacity-100 scale-100"
      leave-active-class="transition ease-in duration-75"
      leave-from-class="transform opacity-100 scale-100"
      leave-to-class="transform opacity-0 scale-95"
    >
      <MenuItems
        class="z-20 origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
      >
        <MenuItem
          v-slot="{ active }"
          v-for="navItem in navItems"
          :key="navItem.key"
        >
          <router-link
            v-if="!navItem.command"
            :to="navItem.to"
            :class="[
              active ? 'bg-gray-100' : '',
              'block px-4 py-2 text-sm text-gray-700',
            ]"
            >{{ navItem.label }}</router-link
          >
          <a
            v-else
            href="#"
            @click="logout"
            :class="[
              active ? 'bg-gray-100' : '',
              'block px-4 py-2 text-sm text-gray-700',
            ]"
          >
            {{ navItem.label }}
          </a>
        </MenuItem>
      </MenuItems>
    </transition>
  </Menu>
</template>

<script>
import { Menu, MenuButton, MenuItem, MenuItems } from "@headlessui/vue";
import Avatar from "primevue/avatar";

export default {
  components: {
    Avatar,
    Menu,
    MenuButton,
    MenuItem,
    MenuItems,
  },
  data() {
    return {
      navItems: [
        {
          label: "Twój profil",
          to: "/profile",
          key: "profile",
        },
        {
          label: "Zarządzaj koktajlami",
          to: "/cocktails/manage/private",
          key: "manageCocktails",
        },
        {
          label: "Zarządzaj składnikami",
          to: "/ingredients/manage/private",
          key: "manageIngredients",
        },
        {
          label: "Listy koktajli",
          to: "/cocktails/lists",
          key: "cocktailsLists",
        },
        {
          label: "Wyloguj się",
          to: "",
          command: this.logout,
          key: "logout",
        },
      ],
    };
  },
  computed: {
    displayName: function () {
      return this.$store.getters.displayName;
    },
  },
  methods: {
    logout() {
      this.$store.dispatch("logout");
      this.$router.replace("/");
    },
  },
};
</script>
