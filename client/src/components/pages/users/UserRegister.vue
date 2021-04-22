<template>
  <div class="flex items-center justify-center py-12 px-4 sm:px-6 lg:px-8">
    <div class="max-w-md w-full space-y-">
      <div>
        <img
          class="mx-auto h-12 w-auto"
          src="https://www.svgrepo.com/show/119676/cocktail-glass.svg"
          alt="DrinkingPassion"
        />
        <h2 class="mt-6 text-center text-3xl font-extrabold text-gray-900">
          Załóż konto
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          lub
          {{ " " }}
          <router-link
            to="/login"
            class="font-medium text-indigo-600 hover:text-indigo-500"
          >
            zaloguj na istniejące konto
          </router-link>
        </p>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="submitRegister">
        <input type="hidden" name="remember" value="true" />
        <div class="rounded-md shadow-sm -space-y-px">
          <div class="pb-4">
            <label for="email-address" class="sr-only">Nazwa wyświetlana</label>
            <label for="email-address" class="text-gray-700"
              >Nazwa wyświetlana</label
            >
            <input
              id="displayName"
              type="text"
              required=""
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              v-model="displayName"
            />
          </div>
          <div class="pb-4">
            <label for="email-address" class="sr-only">Adres email</label>
            <label for="email-address" class="text-gray-700">Adres email</label>
            <input
              id="email-address"
              type="email"
              required=""
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              v-model="email"
            />
          </div>
          <div class="pb-4">
            <label for="password" class="sr-only">Hasło</label>
            <label for="password" class="text-gray-700">Hasło</label>
            <input
              id="password"
              type="password"
              required=""
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              v-model="password"
            />
          </div>
        </div>

        <div>
          <button
            type="submit"
            class="group relative w-full flex justify-center py-2 px-4 border border-transparent text-sm font-medium rounded-md text-white bg-indigo-600 hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-offset-2 focus:ring-indigo-500"
          >
            <span class="absolute left-0 inset-y-0 flex items-center pl-3">
              <LockClosedIcon
                class="h-5 w-5 text-indigo-500 group-hover:text-indigo-400"
                aria-hidden="true"
              />
            </span>
            Zarejestruj się
          </button>
        </div>
      </form>
    </div>
  </div>
</template>

<script>
import { LockClosedIcon } from "@heroicons/vue/solid";

export default {
  components: {
    LockClosedIcon,
  },
  data() {
    return {
      email: "",
      password: "",
      displayName: "",
    };
  },
  methods: {
    submitRegister() {
      fetch("https://localhost:5001/api/accounts/register", {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
        },
        body: JSON.stringify({
          email: this.email,
          password: this.password,
          displayName: this.displayName,
        }),
      })
        .then((response) => response.json())
        .then((data) => {
          console.log(data);
        })
        .catch((error) => {
          console.log(error);
        });
    },
  },
};
</script>