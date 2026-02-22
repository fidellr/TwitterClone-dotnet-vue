import axios from "axios";
import { useToastStore } from "../stores/toastStore";
import { useAuthStore } from "../stores/authStore";
import { router } from "../router";

const api = axios.create({
  baseURL: import.meta.env.VITE_API_URL,
});

api.interceptors.request.use((config) => {
  const token = localStorage.getItem("token");
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

api.interceptors.response.use(
  (response) => response,
  (error) => {
    const toastStore = useToastStore();

    if (!error.response) {
      toastStore.show(
        "Cannot connect to server. Please check your internet.",
        "error",
      );
      return Promise.reject(error);
    }

    if (error.response.status === 401) {
      if (error.config.url.includes("/login")) {
        toastStore.show("Invalid email or password.", "error");
        return Promise.reject(error);
      }

      const authStore = useAuthStore();
      authStore.logout();
      router.push("/login");
      toastStore.show("Session expired. Please log in again.", "info");
      return Promise.reject(error);
    }

    if (
      error.response.status === 400 &&
      typeof error.response.data === "string"
    ) {
      toastStore.show(error.response.data, "error");
    } else {
      toastStore.show("An unexpected error occurred.", "error");
    }

    return Promise.reject(error);
  },
);

export default api;
