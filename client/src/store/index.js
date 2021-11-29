import { createStore } from "vuex";
import authModule from "./modules/auth/index";
import manageCocktailsModule from "./modules/manageCocktails/index";

export default createStore({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    auth: authModule,
    manageCocktails: manageCocktailsModule,
  },
});
