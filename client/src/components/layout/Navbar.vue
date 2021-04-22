<template>
  <Disclosure as="nav" class="relative bg-white">
    <div class="max-w-7xl mx-auto">
      <div class="relative z-10 pb-8 bg-white lg:max-w-2xl lg:w-full">
        <svg
          class="hidden lg:block absolute right-0 inset-y-0 h-full w-48 text-white transform translate-x-1/2"
          fill="currentColor"
          viewBox="0 0 100 100"
          preserveAspectRatio="none"
          aria-hidden="true"
        >
          <polygon points="50,0 100,0 50,100 0,100" />
        </svg>

        <div class="relative pt-6 px-4 sm:px-6 lg:px-8">
          <nav
            class="relative flex items-center justify-between sm:h-10 lg:justify-start"
            aria-label="Global"
          >
            <div
              class="flex items-center flex-grow flex-shrink-0 lg:flex-grow-0"
            >
              <div class="flex items-center justify-between w-full md:w-auto">
                <a href="#">
                  <span class="sr-only">Drinking Passion</span>
                  <img
                    class="h-8 w-auto sm:h-10"
                    src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
                  />
                </a>
                <div class="-mr-2 flex items-center md:hidden">
                  <DisclosureButton
                    class="bg-white rounded-md p-2 inline-flex items-center justify-center text-gray-400 hover:text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-indigo-500"
                  >
                    <span class="sr-only">Otwórz główne menu</span>
                    <MenuIcon class="h-6 w-6" aria-hidden="true" />
                  </DisclosureButton>
                </div>
              </div>
            </div>
            <div class="hidden md:block md:ml-10 md:pr-4 md:space-x-8">
              <router-link
                v-for="item in navigation"
                :key="item.name"
                :to="item.path"
                class="font-medium text-gray-500 hover:text-gray-900"
                >{{ item.displayName }}</router-link
              >
              <router-link
                to="/login"
                class="font-medium text-indigo-600 hover:text-indigo-500"
                >Zaloguj się</router-link
              >
            </div>
          </nav>
        </div>

        <transition
          enter-active-class="duration-150 ease-out"
          enter-from-class="opacity-0 scale-95"
          enter-to-class="opacity-100 scale-100"
          leave-active-class="duration-100 ease-in"
          leave-from-class="opacity-100 scale-100"
          leave-to-class="opacity-0 scale-95"
        >
          <DisclosurePanel
            focus
            class="absolute top-0 inset-x-0 p-2 transition transform origin-top-right md:hidden"
          >
            <div
              class="rounded-lg shadow-md bg-white ring-1 ring-black ring-opacity-5 overflow-hidden"
            >
              <div class="px-5 pt-4 flex items-center justify-between">
                <div>
                  <img
                    class="h-8 w-auto"
                    src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
                    alt=""
                  />
                </div>
                <div class="-mr-2">
                  <DisclosureButton
                    class="bg-white rounded-md p-2 inline-flex items-center justify-center text-gray-400 hover:text-gray-500 hover:bg-gray-100 focus:outline-none focus:ring-2 focus:ring-inset focus:ring-indigo-500"
                  >
                    <span class="sr-only">Zamknij główne menu</span>
                    <XIcon class="h-6 w-6" aria-hidden="true" />
                  </DisclosureButton>
                </div>
              </div>
              <div class="px-2 pt-2 pb-3 space-y-1">
                <router-link
                  v-for="item in navigation"
                  :key="item.name"
                  :to="item.path"
                  class="block px-3 py-2 rounded-md text-base font-medium text-gray-700 hover:text-gray-900 hover:bg-gray-50"
                  >{{ item.displayName }}</router-link
                >
              </div>
              <router-link
                to="/login"
                class="block w-full px-5 py-3 text-center font-medium text-indigo-600 bg-gray-50 hover:bg-gray-100"
              >
                Zaloguj się
              </router-link>
            </div>
          </DisclosurePanel>
        </transition>
      </div>
    </div>
  </Disclosure>
</template>

<script>
import { Disclosure, DisclosurePanel, DisclosureButton } from "@headlessui/vue";
import { MenuIcon, XIcon } from "@heroicons/vue/outline";

export default {
  components: {
    MenuIcon,
    XIcon,
    Disclosure,
    DisclosurePanel,
    DisclosureButton,
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
};
</script>
