export default {
  setPublicCocktails(state, payload) {
    state.publicCocktailsData = payload.publicCocktailsData;
  },
  setPrivateCocktails(state, payload) {
    state.privateCocktailsData = payload.privateCocktailsData;
  },
  setProducts(state, payload) {
    state.products = payload.products;
  },
  addPublicCocktail(state, payload) {
    state.publicCocktailsData.data.push(payload.cocktail);
  },
  addPrivateCocktail(state, payload) {
    state.privateCocktailsData.data.push(payload.cocktail);
  },
};
