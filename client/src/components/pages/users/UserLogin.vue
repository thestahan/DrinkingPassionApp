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
          Zaloguj się
        </h2>
        <p class="mt-2 text-center text-sm text-gray-600">
          lub
          {{ " " }}
          <router-link
            to="/register"
            class="font-medium text-indigo-600 hover:text-indigo-500"
          >
            załóż konto
          </router-link>
        </p>
      </div>
      <form class="mt-8 space-y-6" @submit.prevent="submitSignIn">
        <input type="hidden" name="remember" value="true" />
        <div class="rounded-md shadow-sm -space-y-px">
          <div>
            <label for="email-address" class="sr-only">Email address</label>
            <input
              id="email-address"
              type="email"
              required=""
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-t-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Adres email"
              v-model="email"
            />
          </div>
          <div>
            <label for="password" class="sr-only">Password</label>
            <input
              id="password"
              type="password"
              required
              class="appearance-none rounded-none relative block w-full px-3 py-2 border border-gray-300 placeholder-gray-500 text-gray-900 rounded-b-md focus:outline-none focus:ring-indigo-500 focus:border-indigo-500 focus:z-10 sm:text-sm"
              placeholder="Hasło"
              v-model="password"
            />
          </div>
        </div>

        <div class="flex items-center justify-between">
          <div class="flex items-center">
            <input
              id="remember_me"
              name="remember_me"
              type="checkbox"
              class="h-4 w-4 text-indigo-600 focus:ring-indigo-500 border-gray-300 rounded"
            />
            <label for="remember_me" class="ml-2 block text-sm text-gray-900">
              Zapamiętaj mnie
            </label>
          </div>

          <div class="text-sm">
            <a
              href="#"
              class="font-medium text-indigo-600 hover:text-indigo-500"
            >
              Zapomniałeś hasła?
            </a>
          </div>
        </div>

        <form-errors :errors="errors"></form-errors>

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
            Zaloguj się
          </button>
        </div>
      </form>
    </div>
  </div>
  <spinner v-if="isLoading"></spinner>
</template>

<script>
import { LockClosedIcon } from "@heroicons/vue/solid";
import FormErrors from "../../utilities/FormErrors.vue";
import Spinner from "../../utilities/Spinner.vue";

export default {
  components: {
    LockClosedIcon,
    FormErrors,
    Spinner,
  },
  data() {
    return {
      email: "",
      password: "",
      errors: [],
      isLoading: false,
      reponseError: null,
    };
  },
  methods: {
    async submitSignIn() {
      this.errors = [];
      this.checkForm();

      if (this.errors.length) return;

      this.isLoading = true;

      try {
        await this.$store.dispatch("signIn", {
          email: this.email,
          password: this.password,
        });

        this.$router.replace("/cocktails");
      } catch (err) {
        this.errors.push(err.message || "Logowanie nie powiodło się");
      }

      this.isLoading = false;
    },

    checkForm() {
      if (!this.isEmailValid(this.email.trim())) {
        this.errors.push("Wprowadzony adres email jest niepoprawny");
      }
      if (this.password.trim().length < 8) {
        this.errors.push("Hasło musi zawierać minimum 8 znaków");
      }
    },

    isEmailValid(email) {
      var re = /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
      return re.test(email);
    },
  },
};
</script>
