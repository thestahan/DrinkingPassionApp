export default {
  setUser(state, payload) {
    state.displayName = payload.displayName;
    state.token = payload.token;
    state.isAuthenticated = true;
  },
};
