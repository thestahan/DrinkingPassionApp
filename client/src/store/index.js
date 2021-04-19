import { createStore } from "vuex";

export default createStore({
  state: {
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
    removeCurrentActivePage(state) {
      const elIndex = state.navigation.findIndex((x) => x.current);
      state.navigation[elIndex].current = false;
    },
    setActivePage(state, payload) {
      const elIndex = state.navigation.findIndex(
        (x) => x.name === payload.value
      );
      state.navigation[elIndex].current = true;
    },
  },
  actions: {},
  modules: {},
});
