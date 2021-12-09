import { createStore } from "vuex";
import authModule from "./modules/auth/index";
import manageCocktailsModule from "./modules/manageCocktails/index";
import manageProductsModule from "./modules/manageProducts/index";

export default createStore({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    auth: authModule,
    manageCocktails: manageCocktailsModule,
    manageProducts: manageProductsModule,
  },
});
