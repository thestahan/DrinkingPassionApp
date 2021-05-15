<template>
  <div class="p-col-12 p-md-6 p-xl-4">
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
            <div class="p-card-subtitle">Składnik bazowy: Tequila</div>
            <div class="p-card-subtitle">Ilość składników: 6</div>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
export default {
  props: {
    cocktail: {
      type: Object,
      required: true,
    },
  },
};
</script>

<style scoped>
.p-card {
  background-color: var(--green-50);
}

.cocktail-picture {
  object-fit: cover;
  width: 9rem;
  height: 9rem;
}
</style>
