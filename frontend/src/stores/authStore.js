import { defineStore } from "pinia";
import api from "../services/api";
import { useToastStore } from "./toastStore";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || null,
    token: localStorage.getItem("token") || null,
  }),
  actions: {
    async register(username, email, password) {
      await api.post("/auth/register", { username, email, password });

      const toastStore = useToastStore();
      toastStore.show(
        "Account created successfully! Please log in.",
        "success",
      );
    },
    async login(email, password) {
      const response = await api.post("/auth/login", { email, password });
      this.token = response.data.token;
      this.user = response.data.user;

      localStorage.setItem("token", this.token);
      localStorage.setItem("user", JSON.stringify(this.user));

      const toastStore = useToastStore();
      toastStore.show("Welcome back!", "success");
    },
    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
    },
  },
});
