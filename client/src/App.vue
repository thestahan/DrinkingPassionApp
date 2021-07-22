<template>
  <div class="flex flex-col max-w-7xl h-screen mx-auto justify-between">
    <navbar v-if="!isHomePage"></navbar>
    <div class="mb-auto">
      <router-view />
    </div>
    <app-footer class="h-10"></app-footer>
  </div>
</template>

<script>
import Navbar from "./components/layout/Navbar.vue";
import AppFooter from "./components/layout/AppFooter.vue";

export default {
  components: {
    Navbar,
    AppFooter,
  },
  computed: {
    isAuthenticated() {
      return this.$store.getters.isAuthenticated;
    },
    isHomePage() {
      return this.$route.name === "Home";
    },
    didAutoLogout() {
      return this.$store.getters.didAutoLogout;
    },
  },
  created() {
    this.$store.dispatch("tryLogin");
  },
  watch: {
    didAutoLogout(curValue, oldValue) {
      if (curValue && curValue !== oldValue) {
        this.$router.replace("/");
      }
    },
  },
};
</script>

<style>
@import url("https://fonts.googleapis.com/css2?family=Noto+Sans+JP:wght@100;500&display=swap");
@import url("https://fonts.googleapis.com/css2?family=Lato:wght@100;300;400&display=swap");
</style>
