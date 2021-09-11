import { createApp } from "vue";
import App from "./App.vue";
import router from "./router";
import store from "./store";
import ToastService from "primevue/toastservice";

import PrimeVue from "primevue/config";

import "./assets/css/index.css";
import "primevue/resources/themes/saga-green/theme.css"; //theme
import "primevue/resources/primevue.min.css"; //core css
import "primeicons/primeicons.css"; //icons
import "primeflex/primeflex.css";

const app = createApp(App);

app.use(store);
app.use(router);
app.use(PrimeVue);
app.use(ToastService);

app.mount("#app");
