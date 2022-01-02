import { createRouter, createWebHistory } from "vue-router";
import Home from "../components/pages/Home.vue";

import UserRegister from "../components/pages/users/UserRegister.vue";
import UserLogin from "../components/pages/users/UserLogin/UserLogin.vue";
import UserProfile from "../components/pages/users/UserProfile/UserProfile.vue";
import PublicCocktailsList from "../components/pages/cocktails/PublicCocktailsList.vue";
import PrivateCocktailsList from "../components/pages/cocktails/PrivateCocktailsList.vue";
import CocktailDetails from "../components/pages/cocktails/CocktailDetails.vue";
import ManageCocktailsMenu from "../components/pages/cocktails/ManageCocktails/ManageCocktailsMenu.vue";
import NotFound from "../components/pages/NotFound.vue";
import UserConfirmEmail from "../components/pages/users/UserConfirmEmail.vue";
import ChangeForgottenPassword from "../components/pages/users/ChangeForgottenPassword.vue";
import PublicCocktails from "../components/pages/cocktails/ManageCocktails/PublicCocktails.vue";
import PrivateCocktails from "../components/pages/cocktails/ManageCocktails/PrivateCocktails.vue";
import ManageProductsMenu from "../components/pages/products/ManageProducts/ManageProductsMenu.vue";
import PublicIngredients from "../components/pages/products/ManageProducts/PublicIngredients.vue";
import PrivateIngredients from "../components/pages/products/ManageProducts/PrivateIngredients.vue";
import CocktailsLists from "../components/pages/cocktailsLists/CocktailsLists.vue";
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
    path: "/cocktails/public",
    name: "PublicCocktailsList",
    component: PublicCocktailsList,
  },
  {
    path: "/cocktails/private",
    name: "PrivateCocktailsList",
    component: PrivateCocktailsList,
  },
  {
    path: "/cocktails/public/:id",
    name: "PublicCocktailDetails",
    component: CocktailDetails,
  },
  {
    path: "/cocktails/private/:id",
    name: "PrivateCocktailDetails",
    component: CocktailDetails,
  },
  {
    path: "/:notFound(.*)*",
    name: "NotFound",
    component: NotFound,
  },
  {
    path: "/confirmEmail",
    name: "ConfirmEmail",
    query: { code: "", email: "" },
    component: UserConfirmEmail,
  },
  {
    path: "/changeForgottenPassword",
    name: "ChangeForgottenPassword",
    query: { token: "", email: "" },
    component: ChangeForgottenPassword,
  },
  {
    path: "/cocktails/manage",
    name: "ManageCocktails",
    component: ManageCocktailsMenu,
    meta: { requiresAuth: true },
    children: [
      {
        path: "public",
        component: PublicCocktails,
        meta: { requiresAdmin: true },
      },
      {
        path: "private",
        component: PrivateCocktails,
      },
    ],
  },
  {
    path: "/cocktails/lists",
    name: "CocktailsLists",
    component: CocktailsLists,
    meta: { requiresAuth: true },
  },
  {
    path: "/ingredients/manage",
    name: "ManageIngredients",
    component: ManageProductsMenu,
    meta: { requiresAuth: true },
    children: [
      {
        path: "public",
        component: PublicIngredients,
        meta: { requiresAdmin: true },
      },
      {
        path: "private",
        component: PrivateIngredients,
      },
    ],
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
  } else if (to.meta.requiresAdmin && !store.getters.roles?.includes("admin")) {
    next("/notFound");
  } else {
    next();
  }
});

export default router;
