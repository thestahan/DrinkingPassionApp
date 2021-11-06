import mutations from "./mutations.js";
import actions from "./actions.js";
import getters from "./getters.js";

export default {
  state() {
    return {
      publicCocktailsData: null,
      privateCocktailsData: null,
      products: null,
    };
  },
  mutations,
  actions,
  getters,
};
