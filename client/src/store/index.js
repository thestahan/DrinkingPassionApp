import { createStore } from "vuex";
import authModule from "./modules/auth/index";

export default createStore({
  state: {},
  getters: {},
  mutations: {},
  actions: {},
  modules: {
    auth: authModule,
  },
});
