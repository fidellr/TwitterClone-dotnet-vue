import { createRouter, createWebHistory } from "vue-router";
import { useAuthStore } from "./stores/authStore";
import HomeFeed from "./components/HomeFeed.vue";
import Login from "./components/Login.vue";
import Register from "./components/Register.vue";

const routes = [
  { path: "/", component: HomeFeed, meta: { requiresAuth: true } },
  { path: "/login", component: Login },
  { path: "/register", component: Register },
];

export const router = createRouter({ history: createWebHistory(), routes });

router.beforeEach((to, from, next) => {
  const authStore = useAuthStore();
  if (to.meta.requiresAuth && !authStore.token) next("/login");
  else next();
});
