<template>
  <h3>Profil</h3>
  <p>{{ displayName }}</p>
  <p>{{ email }}</p>
  <spinner v-if="isLoading"></spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";

export default {
  components: { Spinner },
  data() {
    return {
      isLoading: false,
      displayName: null,
      email: null,
    };
  },
  created() {
    this.getUserProfile();
  },
  methods: {
    async getUserProfile() {
      this.isLoading = true;

      const token = this.$store.getters.token;
      console.log(token);
      const response = await fetch("https://localhost:5001/api/accounts", {
        method: "GET",
        headers: {
          Authorization: "Bearer " + token,
          "Content-Type": "application/json",
        },
      });

      this.isLoading = false;

      const responseData = await response.json();

      if (!response.ok) {
        const error = new Error(responseData.message || "Wystąpił błąd.");
        throw error;
      }

      this.displayName = responseData.displayName;
      this.email = responseData.email;
    },
  },
};
</script>
