<template>
  <Disclosure as="nav" class="bg-white" v-slot="{ open }">
    <div class="max-w-7xl mx-auto px-2 sm:px-6 lg:px-8">
      <div class="relative flex items-center justify-between h-16">
        <div class="absolute inset-y-0 left-0 flex items-center sm:hidden">
          <!-- Mobile menu button-->
          <DisclosureButton
            class="inline-flex items-center justify-center p-2 rounded-md text-gray-400 hover:text-white hover:bg-gray-700 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-white"
          >
            <span class="sr-only">Otwórz główne menu</span>
            <MenuIcon v-if="!open" class="block h-6 w-6" aria-hidden="true" />
            <XIcon v-else class="block h-6 w-6" aria-hidden="true" />
          </DisclosureButton>
        </div>
        <div
          class="flex-1 flex items-center justify-center sm:items-stretch sm:justify-start"
        >
          <div class="flex-shrink-0 flex items-center">
            <img
              class="block h-8 w-auto"
              src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
              alt="Workflow"
            />
          </div>
          <div class="hidden sm:block sm:ml-6">
            <div class="flex space-x-4">
              <router-link
                v-for="item in navigation"
                :key="item.name"
                :to="item.path"
                class="font-medium text-gray-500 hover:text-gray-900"
                >{{ item.displayName }}
              </router-link>
              <router-link
                v-if="!isAuthenticated"
                to="/login"
                class="font-medium"
                style="color: var(--primary-color)"
                >Zaloguj się</router-link
              >
            </div>
          </div>
        </div>

        <div
          class="absolute inset-y-0 right-0 flex items-center pr-2 sm:static sm:inset-auto sm:ml-6 sm:pr-0"
        >
          <!-- Profile dropdown -->
          <Menu as="div" class="ml-3 relative" v-if="isAuthenticated">
            <div>
              <MenuButton
                class="bg-gray-800 flex text-sm rounded-full focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-offset-gray-800 focus:ring-white"
              >
                <span class="sr-only">Otwórz menu profilu</span>
                <img
                  class="h-8 w-8 rounded-full"
                  src="https://images.unsplash.com/photo-1472099645785-5658abf4ff4e?ixlib=rb-1.2.1&ixid=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=2&w=256&h=256&q=80"
                  alt=""
                />
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
                class="origin-top-right absolute right-0 mt-2 w-48 rounded-md shadow-lg py-1 bg-white ring-1 ring-black ring-opacity-5 focus:outline-none"
              >
                <MenuItem v-slot="{ active }">
                  <router-link
                    to="/profile"
                    :class="[
                      active ? 'bg-gray-100' : '',
                      'block px-4 py-2 text-sm text-gray-700',
                    ]"
                    >Twój profil</router-link
                  >
                </MenuItem>
                <MenuItem v-slot="{ active }">
                  <a
                    href="#"
                    @click="logout"
                    :class="[
                      active ? 'bg-gray-100' : '',
                      'block px-4 py-2 text-sm text-gray-700',
                    ]"
                    >Wyloguj się</a
                  >
                </MenuItem>
              </MenuItems>
            </transition>
          </Menu>
        </div>
      </div>
    </div>
    <DisclosurePanel class="sm:hidden">
      <div class="px-2 pt-2 pb-3 space-y-1">
        <router-link
          v-for="item in navigation"
          :key="item.name"
          :to="item.path"
          class="block px-3 py-2 rounded-md text-base font-medium text-gray-700 hover:text-gray-900 hover:bg-gray-50"
          >{{ item.displayName }}
        </router-link>
        <router-link
          v-if="!isAuthenticated"
          to="/login"
          class="block w-full px-5 py-3 text-center font-medium bg-gray-50 hover:bg-gray-100"
          style="color: var(--primary-color)"
        >
          Zaloguj się
        </router-link>
      </div>
    </DisclosurePanel>
  </Disclosure>
</template>

<script>
import {
  Disclosure,
  DisclosurePanel,
  DisclosureButton,
  Menu,
  MenuButton,
  MenuItem,
  MenuItems,
} from "@headlessui/vue";
import { MenuIcon, XIcon } from "@heroicons/vue/outline";

export default {
  components: {
    MenuIcon,
    XIcon,
    Disclosure,
    DisclosurePanel,
    DisclosureButton,
    Menu,
    MenuButton,
    MenuItem,
    MenuItems,
  },
  data() {
    return {
      navigation: [
        {
          displayName: "Strona główna",
          name: "homePage",
          path: "/",
        },
        {
          displayName: "Koktajle",
          name: "cocktails",
          path: "/cocktails",
        },
      ],
      open: false,
    };
  },
  computed: {
    displayName: function () {
      return this.$store.getters.displayName;
    },
    isAuthenticated: function () {
      return this.$store.getters.isAuthenticated;
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
