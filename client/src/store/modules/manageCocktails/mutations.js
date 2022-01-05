export default {
  setAllAvailableCocktails(state, payload) {
    state.allAvailableCocktails = payload.cocktails;
  },
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
  deletePublicCocktail(state, payload) {
    state.publicCocktailsData.data = state.publicCocktailsData.data.filter(
      (cocktail) => cocktail.id != payload.id
    );
  },
  deletePrivateCocktail(state, payload) {
    state.privateCocktailsData.data = state.privateCocktailsData.data.filter(
      (cocktail) => cocktail.id != payload.id
    );
  },
};
