import mutations from "./mutations.js";
import actions from "./actions.js";
import getters from "./getters.js";

export default {
  state() {
    return {
      displayName: null,
      token: null,
      tokenExpiration: null,
      isAuthenticated: false,
    };
  },
  mutations,
  actions,
  getters,
};
