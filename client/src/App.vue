<template>
  <div class="max-w-7xl mx-auto">
    <navbar v-if="!isHomePage"></navbar>
    <router-view />
  </div>
</template>

<script>
import Navbar from "./components/layout/Navbar.vue";

export default {
  components: {
    Navbar,
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
