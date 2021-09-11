export default {
  setUser(state, payload) {
    state.displayName = payload.displayName;
    state.token = payload.token;
    state.didAutoLogout = false;
    state.roles = payload.roles;
  },
  setAutoLogout(state) {
    state.didAutoLogout = true;
  },
};
