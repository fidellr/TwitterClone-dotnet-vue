import { defineStore } from "pinia";
import axios from "axios";

const API_URL = "http://localhost:5032/api/auth";

export const useAuthStore = defineStore("auth", {
  state: () => ({
    user: JSON.parse(localStorage.getItem("user")) || null,
    token: localStorage.getItem("token") || null,
  }),
  actions: {
    async register(username, email, password) {
      await axios.post(`${API_URL}/register`, { username, email, password });
    },
    async login(email, password) {
      const response = await axios.post(`${API_URL}/login`, {
        email,
        password,
      });
      this.token = response.data.token;
      this.user = response.data.user;
      localStorage.setItem("token", this.token);
      localStorage.setItem("user", JSON.stringify(this.user));
      axios.defaults.headers.common["Authorization"] = `Bearer ${this.token}`;
    },
    logout() {
      this.user = null;
      this.token = null;
      localStorage.removeItem("token");
      localStorage.removeItem("user");
      delete axios.defaults.headers.common["Authorization"];
    },
  },
});
