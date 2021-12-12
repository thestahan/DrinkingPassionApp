<template>
  <div v-if="cocktail">
    <div class="p-d-sm-none p-grid p-flex-column p-ml-2 p-mr-2">
      <div class="p-col-12">
        <header class="p-mb-3 p-text-center">
          <h3
            class="main-font heading-font"
            style="font-weight: 400; font-size: 1.8rem"
          >
            {{ cocktail.name }}
          </h3>
        </header>
      </div>
      <div class="p-col-12">
        <img
          class="cocktail-details-picture-mobile p-m-auto p-shadow-10"
          alt="Zdjęcie koktajlu"
          :src="cocktail.picture"
        />
      </div>
      <div class="p-col-12">
        <section class="ingredients">
          <div
            class="p-grid p-jc-center"
            v-for="ingredient in cocktail.ingredients"
            :key="ingredient.productId"
          >
            <div class="p-col-5 p-text-left">
              <span>{{ ingredient.name }}</span>
            </div>
            <div class="p-col-2 p-text-left">
              <span class="primary-color"
                >{{ ingredient.amount }} {{ ingredient.unit }}</span
              >
            </div>
          </div>
        </section>
      </div>
    </div>
    <div
      class="p-d-none p-d-sm-flex p-grid p-ai-center vertical-container p-m-4"
    >
      <div class="p-col-6">
        <header class="p-mb-4 p-text-center">
          <h3
            class="main-font heading-font"
            style="font-weight: 400; font-size: 1.8rem"
          >
            {{ cocktail.name }}
          </h3>
        </header>

        <section class="ingredients">
          <div
            class="p-grid p-jc-center"
            v-for="ingredient in cocktail.ingredients"
            :key="ingredient.productId"
          >
            <div class="p-col-5 p-text-left">
              <span>{{ ingredient.name }}</span>
            </div>
            <div class="p-col-2 p-text-left">
              <span class="primary-color"
                >{{ ingredient.amount }} {{ ingredient.unit }}</span
              >
            </div>
          </div>
        </section>
      </div>

      <div class="p-col-6">
        <img
          class="cocktail-details-picture p-m-auto p-shadow-10"
          alt="Zdjęcie koktajlu"
          :src="cocktail.picture"
        />
      </div>
    </div>
    <div class="p-grid p-ml-2 p-mr-2 p-text-justify">
      <div class="p-col-12 p-sm-6">
        <h3 class="main-font p-mb-2 primary-color p-text-bold">
          Opis koktajlu
        </h3>
        <span>{{ cocktail.description }}</span>
      </div>
      <div class="p-col-12 p-sm-6">
        <h3 class="main-font p-mb-2 primary-color p-text-bold">
          Przygotowanie
        </h3>
        <span v-if="cocktail.preparationInstruction">{{
          cocktail.preparationInstruction
        }}</span>
        <span v-else class="not-found-color"
          >Nie podano instrukcji przygotowania.</span
        >
      </div>
    </div>
  </div>
</template>

<script>
import CocktailService from "../../../services/CocktailService";

export default {
  data() {
    return {
      cocktail: null,
      isLoading: false,
    };
  },
  cocktailService: null,
  computed: {
    cocktailId() {
      return this.$route.params.id;
    },
    cocktailName() {
      return this.cocktail.name;
    },
    token() {
      return this.$store.getters.token;
    },
  },
  created() {
    this.cocktailService = new CocktailService();
    this.getCocktailDetails();
  },
  methods: {
    async getCocktailDetails() {
      this.isLoading = true;

      const responseData = await this.cocktailService.getCocktail(
        this.cocktailId,
        this.token
      );

      this.isLoading = false;

      this.cocktail = responseData;
    },
  },
};
</script>

<style scoped>
.cocktail-details-picture {
  object-fit: cover;
  width: 32rem;
  height: 18rem;
}

.cocktail-details-picture-mobile {
  object-fit: cover;
  width: 24rem;
  height: 13.5rem;
}

.not-found-color {
  color: grey;
}
</style>
