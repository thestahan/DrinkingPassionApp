import { createRouter, createWebHistory } from "vue-router";
import Home from "../components/pages/Home.vue";

import UserRegister from "../components/pages/users/UserRegister.vue";
import UserLogin from "../components/pages/users/UserLogin.vue";
import UserProfile from "../components/pages/users/UserProfile.vue";
import CocktailsList from "../components/pages/cocktails/CocktailsList.vue";
import CocktailDetails from "../components/pages/cocktails/CocktailDetails.vue";
import NotFound from "../components/pages/NotFound.vue";

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
  },
  {
    path: "/login",
    name: "Login",
    component: UserLogin,
  },
  {
    path: "/profile",
    name: "Profile",
    component: UserProfile,
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
];

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes,
});

export default router;
