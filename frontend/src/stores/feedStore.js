import { defineStore } from "pinia";
import { useAuthStore } from "./authStore";
import api from "../services/api";
import { useToastStore } from "./toastStore";

const WS_URL = `${import.meta.env.VITE_WS_URL}`;

export const useFeedStore = defineStore("feed", {
  state: () => ({
    tweets: [],
    ws: null,
    isLoading: false,
  }),
  actions: {
    async fetchFeed() {
      this.isLoading = true;
      try {
        const response = await api.get(`/tweets/feed`);
        this.tweets = response.data;
      } catch (err) {
      } finally {
        this.isLoading = false;
      }
    },

    async createTweet(content) {
      const authStore = useAuthStore();
      const toastStore = useToastStore();

      await api.post("/tweets", { user_id: authStore.user.id, content });
      toastStore.show("Tweet posted successfully!", "success");
    },

    connectWebSocket() {
      this.ws = new WebSocket(WS_URL);

      this.ws.onmessage = (event) => {
        const data = JSON.parse(event.data);
        if (data.type === "NEW_TWEET") {
          this.tweets.unshift(data.payload);
        }
      };

      this.ws.onclose = () => {
        setTimeout(() => this.connectWebSocket(), 3000); // reconnect ws
      };
    },

    async addComment(tweetId, content) {
      const authStore = useAuthStore();
      const toastStore = useToastStore();

      try {
        const response = await api.post(`/tweets/${tweetId}/comments`, {
          user_id: authStore.user.id,
          content: content,
        });

        const tweet = this.tweets.find((t) => t.id === tweetId);
        if (tweet) {
          if (!tweet.comments) tweet.comments = [];
          tweet.comments.push({
            id: response.data.id,
            content: content,
            user_id: authStore.user.id,
            username: authStore.user.username,
          });
        }

        toastStore.show("Reply posted!", "success");
      } catch (err) {}
    },
  },
});
