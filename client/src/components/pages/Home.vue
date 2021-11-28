<template>
  <navbar></navbar>

  <div class="relative bg-white overflow-hidden">
    <div
      class="relative z-10 pb-8 bg-white sm:pb-16 md:pb-20 lg:max-w-2xl lg:w-full lg:pb-28 xl:pb-32"
    >
      <svg
        class="hidden lg:block absolute right-0 inset-y-0 h-full w-48 text-white transform translate-x-1/2"
        fill="currentColor"
        viewBox="0 0 100 100"
        preserveAspectRatio="none"
        aria-hidden="true"
      >
        <polygon points="50,0 100,0 50,100 0,100" />
      </svg>

      <div class="relative pt-6 px-4 sm:px-6 lg:px-8"></div>

      <main
        class="mt-10 mx-auto max-w-7xl px-4 sm:mt-12 sm:px-6 md:mt-16 lg:mt-20 lg:px-8 xl:mt-28"
      >
        <div class="sm:text-center lg:text-left">
          <h1
            class="text-4xl tracking-tight font-extrabold text-gray-900 sm:text-5xl md:text-6xl"
          >
            <span class="p-d-inline-block">Najlepsze koktajle </span>
            <span class="p-d-inline-block primary-color">
              w jednym miejscu</span
            >
          </h1>
          <p
            class="mt-3 text-base text-gray-500 sm:mt-5 sm:text-lg sm:max-w-xl sm:mx-auto md:mt-5 md:text-xl lg:mx-0"
          >
            Anim aute id magna aliqua ad ad non deserunt sunt. Qui irure qui
            lorem cupidatat commodo. Elit sunt amet fugiat veniam occaecat
            fugiat aliqua.
          </p>
          <div class="mt-5 sm:mt-8 sm:flex sm:justify-center lg:justify-start">
            <div class="rounded-md shadow">
              <router-link
                to="/register"
                class="w-full flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-md text-white md:py-4 md:text-lg md:px-10"
                style="background-color: var(--primary-color); :hover: var()"
              >
                Utwórz konto
              </router-link>
            </div>
            <div class="mt-3 sm:mt-0 sm:ml-3">
              <router-link
                to="/cocktails"
                class="w-full flex items-center justify-center px-8 py-3 border border-transparent text-base font-medium rounded-md md:py-4 md:text-lg md:px-10"
                style="
                  color: var(--green-700);
                  background-color: var(--green-100);
                "
              >
                Przejdź do koktajli
              </router-link>
            </div>
          </div>
        </div>
      </main>
    </div>
    <div class="lg:absolute lg:inset-y-0 lg:right-0 lg:w-1/2">
      <img
        class="h-56 w-full object-cover sm:h-72 md:h-96 lg:w-full lg:h-full"
        src="@/assets/hero.jpg"
        alt=""
      />
    </div>
  </div>
</template>

<script>
import Navbar from "../layout/Navbar.vue";

export default {
  components: {
    Navbar,
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
  beforeCreate() {
    this.$;
  },
};
</script>
