<template>
  <router-link
    :to="pathToCocktailDetails"
    tag="div"
    class="p-col-12 p-md-6 p-xl-4"
  >
    <div class="p-card p-component">
      <div class="p-grid">
        <div class="p-col-fixed p-m-2">
          <img
            class="cocktail-picture"
            alt="Zdjęcie koktajlu"
            :src="cocktail.picture"
          />
        </div>
        <div class="p-col">
          <div class="p-card-title">{{ cocktail.name }}</div>
          <div class="p-card-content">
            <div class="p-card-subtitle">
              <span class="card-subtext">Składnik bazowy</span>:
              {{ cocktail.baseIngredient }}
            </div>
            <div class="p-card-subtitle">
              <span class="card-subtext">Ilość składników:</span>
              {{ cocktail.ingredientsCount }}
            </div>
          </div>
        </div>
      </div>
    </div>
  </router-link>
</template>

<script>
export default {
  props: {
    cocktail: {
      type: Object,
      required: true,
    },
    cocktailType: {
      type: String,
      required: true,
    },
  },
  data() {
    return {
      pathToCocktailDetails:
        "/cocktails/" + this.cocktailType + "/" + this.cocktail.id,
    };
  },
};
</script>

<style scoped>
.p-card {
  background-color: var(--green-50);
}

.card-subtext {
  font-weight: 600;
  color: var(--green-600);
}

.cocktail-picture {
  object-fit: cover;
  width: 9rem;
  height: 9rem;
}
</style>
