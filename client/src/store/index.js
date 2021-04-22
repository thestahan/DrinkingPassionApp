import { createStore } from "vuex";

export default createStore({
  state: {
    displayName: "",
    token: "",
    navigation: [
      {
        displayName: "Strona główna",
        name: "homePage",
        href: "/",
        current: true,
      },
      {
        displayName: "Koktajle",
        name: "cocktails",
        href: "/cocktails",
        current: false,
      },
    ],
    isAuthenticated: false,
  },
  getters: {
    isAuthenticated: (state) => {
      return state.isAuthenticated;
    },
  },
  mutations: {
    setUser(state, payload) {
      state.displayName = payload.displayName;
      state.token = payload.token;
    },
  },
  actions: {},
  modules: {},
});
