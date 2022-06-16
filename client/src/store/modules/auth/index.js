import mutations from "./mutations.js";
import actions from "./actions.js";
import getters from "./getters.js";

export default {
  state() {
    return {
      displayName: null,
      email: null,
      token: null,
      isAuthenticated: false,
      didAutoLogout: false,
      roles: [],
    };
  },
  mutations,
  actions,
  getters,
};
