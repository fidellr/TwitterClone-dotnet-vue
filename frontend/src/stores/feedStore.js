import { defineStore } from "pinia";
import axios from "axios";
import { useAuthStore } from "./authStore";

const API_URL = "http://localhost:5032/api/tweets";
const WS_URL = "ws://localhost:5032/ws";

export const useFeedStore = defineStore("feed", {
  state: () => ({
    tweets: [],
    ws: null,
  }),
  actions: {
    async fetchFeed() {
      const response = await axios.get(`${API_URL}/feed`);
      this.tweets = response.data;
    },

    async createTweet(content, userId) {
      await axios.post(API_URL, { user_id: userId, content });
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
      const response = await axios.post(`${API_URL}/${tweetId}/comments`, {
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
    },
  },
});
