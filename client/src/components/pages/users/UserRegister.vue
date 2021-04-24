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
      <form class="mt-8 space-y-6" @submit.prevent="submitSignUp">
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
              minlength="8"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              v-model="password"
            />
          </div>
          <div class="pb-4">
            <label for="passwordConfirm" class="sr-only">Powtórz hasło</label>
            <label for="passwordConfirm" class="text-gray-700"
              >Powtórz hasło</label
            >
            <input
              id="passwordConfirm"
              type="password"
              required=""
              minlength="8"
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              v-model="passwordConfirm"
            />
          </div>
        </div>
        <div class="flex justify-start mt-3 ml-4 p-1" v-if="errors.length">
          <ul>
            <li
              class="flex items-center py-1"
              v-bind:key="error"
              v-for="error in errors"
            >
              <XCircleIcon class="h-5 w-5 text-red-700"></XCircleIcon>
              <span class="text-red-700 font-medium text-sm ml-3">{{
                error
              }}</span>
            </li>
          </ul>
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
import { XCircleIcon } from "@heroicons/vue/solid";

export default {
  components: {
    LockClosedIcon,
    XCircleIcon,
  },
  data() {
    return {
      email: "",
      password: "",
      passwordConfirm: "",
      displayName: "",
      errors: [],
    };
  },
  methods: {
    submitSignUp() {
      this.errors = [];
      this.checkForm();
      console.log(this.errors);
      if (this.errors.length) return;

      this.$store.dispatch("signUp", {
        email: this.email.trim(),
        password: this.password.trim(),
        displayName: this.displayName.trim(),
      });
    },

    checkForm() {
      if (!this.isEmailValid(this.email.trim())) {
        this.errors.push("Wprowadzony adres email jest niepoprawny");
      }
      if (this.displayName.trim().length < 3) {
        this.errors.push("Nazwa wyświetlana musi zawierać minimum 3 znaki");
      }
      if (this.password.trim().length < 8) {
        this.errors.push("Hasło musi zawierać minimum 8 znaków");
      }
      if (this.password.trim() != this.passwordConfirm.trim()) {
        this.errors.push("Wprowadzone hasła nie są identyczne");
      }
    },

    isEmailValid(email) {
      var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(email);
    },
  },
};
</script>