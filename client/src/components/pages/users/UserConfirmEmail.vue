<template>
  <div class="p-d-flex p-jc-center p-px-2">
    <div style="max-width: 60rem; width: 100%">
      <header class="p-mb-2">
        <h3 class="main-font secondary-heading-font">
          Potwierdzenie adresu email
        </h3>
      </header>
      <div v-if="confirmationSuccessful">
        <p>
          Twój adres email został potwierdzony. Możesz się teraz zalogować na
          swoje konto.
          <router-link class="primary-color" to="/login"
            >Przejdź do logowania</router-link
          >
        </p>
      </div>
      <div v-if="confirmationNotSuccessful">
        <p>
          Link jest niepoprawny, wygasł lub adres email został już potwierdzony.
        </p>
      </div>
    </div>
  </div>

  <Spinner v-if="isLoading"></Spinner>
</template>

<script>
import Spinner from "../../utilities/Spinner.vue";

export default {
  components: {
    Spinner,
  },
  data() {
    return {
      token: null,
      email: null,
      confirmationSuccessful: false,
      confirmationNotSuccessful: false,
      isLoading: false,
    };
  },
  created() {
    this.token = this.$route.query.token;
    this.email = this.$route.query.email;

    if (this.token && this.email) {
      this.confirmEmail();
    } else {
      this.confirmationNotSuccessful = true;
    }
  },
  methods: {
    async confirmEmail() {
      this.isLoading = true;

      const response = await fetch(
        `${process.env.VUE_APP_API_URL}/api/accounts/ConfirmEmail`,
        {
          method: "PATCH",
          headers: {
            "Content-Type": "application/json",
          },
          body: JSON.stringify({
            token: this.token,
            email: this.email,
          }),
        }
      );

      this.isLoading = false;

      if (response.ok) {
        this.confirmationSuccessful = true;
      } else {
        this.confirmationNotSuccessful = true;
      }
    },
  },
};
</script>
