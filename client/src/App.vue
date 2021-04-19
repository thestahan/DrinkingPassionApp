<template>
  <div class="max-w-7xl mx-auto">
    <navbar v-if="isAuthenticated || !isHomePage"></navbar>
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
  },
};
</script>
