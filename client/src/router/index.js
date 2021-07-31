import { createRouter, createWebHistory } from "vue-router";
import Home from "../components/pages/Home.vue";

import UserRegister from "../components/pages/users/UserRegister.vue";
import UserLogin from "../components/pages/users/UserLogin.vue";
import UserProfile from "../components/pages/users/UserProfile/UserProfile.vue";
import CocktailsList from "../components/pages/cocktails/CocktailsList.vue";
import CocktailDetails from "../components/pages/cocktails/CocktailDetails.vue";
import NotFound from "../components/pages/NotFound.vue";
import UserConfirmEmail from "../components/pages/users/UserConfirmEmail.vue";
import store from "../store/index.js";

const routes = [
  {
    path: "/",
    name: "Home",
    component: Home,
  },
  {
    path: "/register",
    name: "Register",
    component: UserRegister,
    meta: { requiresUnauth: true },
  },
  {
    path: "/login",
    name: "Login",
    component: UserLogin,
    meta: { requiresUnauth: true },
  },
  {
    path: "/profile",
    name: "Profile",
    component: UserProfile,
    meta: { requiresAuth: true },
  },
  {
    path: "/cocktails",
    name: "Cocktails",
    component: CocktailsList,
  },
  {
    path: "/cocktails/:id",
    name: "Cocktail",
    component: CocktailDetails,
  },
  {
    path: "/:notFound().*",
    name: "NotFound",
    component: NotFound,
  },
  {
    path: "/confirmEmail",
    name: "ConfirmEmail",
    query: { code: "", email: "" },
    component: UserConfirmEmail,
  },
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

router.beforeEach(function (to, _, next) {
  if (to.meta.requiresAuth && !store.getters.isAuthenticated) {
    next("/login");
  } else if (to.meta.requiresUnauth && store.getters.isAuthenticated) {
    next("/profile");
  } else {
    next();
  }
});

export default router;
