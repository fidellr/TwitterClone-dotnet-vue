<script setup>
import { ref, onMounted } from 'vue';
import { useFeedStore } from '../stores/feedStore';
import { useAuthStore } from '../stores/authStore';
import { formatDDMMYYYYHHMM } from "../utils/date";
import { useRouter } from 'vue-router';
import axios from 'axios';

const feedStore = useFeedStore();
const authStore = useAuthStore();
const router = useRouter();

const newTweetContent = ref('');
const commentInputs = ref({});
const isTweeting = ref(false);

onMounted(() => {
    if (authStore.token) {
        feedStore.fetchFeed();
        feedStore.connectWebSocket();
    }
});

const submitTweet = async () => {
    if (!newTweetContent.value.trim()) return;

    isTweeting.value = true;
    try {
        await feedStore.createTweet(newTweetContent.value);
        newTweetContent.value = '';
    } catch (err) { }
    finally {
        isTweeting.value = false;
    }
};

const submitComment = async (tweetId) => {
    const content = commentInputs.value[tweetId];
    if (!content || !content.trim()) return;
    await feedStore.addComment(tweetId, content);
    commentInputs.value[tweetId] = '';
};

const logout = () => {
    authStore.logout();
    router.push('/login');
};
</script>

<template>
    <div class="feed-layout">
        <div class="header">
            <h1>Home Feed</h1>
            <button @click="logout" class="btn-secondary">Logout</button>
        </div>

        <div class="compose-section">
            <textarea v-model="newTweetContent" placeholder="What's happening?" class="compose-input"></textarea>
            <div class="compose-actions">
                <button @click="submitTweet" class="btn-primary">
                    {{ isTweeting ? "Posting..." : "Post" }}
                </button>
            </div>
        </div>

        <div v-if="feedStore.isLoading" class="loading-state">
            Loading tweets...
        </div>

        <div v-else class="feed-list">
            <div v-for="tweet in feedStore.tweets" :key="tweet.id" class="tweet-card">

                <div class="tweet-header">
                    <strong>{{ tweet.username }}</strong>
                    <span class="text-muted" style="padding-left: 5px;">@{{ tweet.username }}</span>
                    <span class="text-muted"> · {{ formatDDMMYYYYHHMM(tweet.created_at) }}</span>
                </div>

                <div class="tweet-body">
                    <p class="tweet-content">{{ tweet.content }}</p>
                </div>

                <div class="comments" v-if="tweet.comments?.length">
                    <div v-for="comment in tweet.comments" :key="comment.id" class="comment">
                        <strong>{{ comment.username }}</strong>
                        <span class="text-muted"> · {{ formatDDMMYYYYHHMM(comment.created_at) }}</span>
                        <p style="margin: 5px 0 0 0;">{{ comment.content }}</p>
                    </div>
                </div>

                <div class="comment-input-area">
                    <input v-model="commentInputs[tweet.id]" class="input-field input-rounded"
                        placeholder="Post your reply" @keyup.enter="submitComment(tweet.id)" />
                    <button class="btn-outline" @click="submitComment(tweet.id)">Reply</button>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.loading-state {
    text-align: center;
    padding: 40px;
    color: var(--text-muted);
    font-size: 16px;
}

button:disabled {
    opacity: 0.6;
    cursor: not-allowed;
}

.feed-layout {
    max-width: 600px;
    margin: 0 auto;
    border-left: 1px solid var(--border-color);
    border-right: 1px solid var(--border-color);
    min-height: 100vh;
}

.header {
    display: flex;
    justify-content: space-between;
    align-items: center;
    padding: 15px 20px;
    background: rgba(255, 255, 255, 0.85);
    backdrop-filter: blur(12px);
    position: sticky;
    top: 0;
    border-bottom: 1px solid var(--border-color);
    z-index: 10;
}

.header h1 {
    margin: 0;
    font-size: 20px;
    font-weight: 700;
}

.compose-section {
    padding: 20px;
    border-bottom: 1px solid var(--border-color);
    display: flex;
    flex-direction: column;
}

.compose-input {
    width: 100%;
    border: none;
    resize: none;
    font-size: 20px;
    outline: none;
    min-height: 80px;
    font-family: inherit;
}

.compose-actions {
    display: flex;
    justify-content: flex-end;
    margin-top: 10px;
}

.tweet-card {
    padding: 15px 20px;
    border-bottom: 1px solid var(--border-color);
    transition: background-color 0.2s;
}

.tweet-card:hover {
    background-color: var(--bg-light);
}

.tweet-content {
    font-size: 15px;
    margin: 0 0 12px 0;
    line-height: 1.5;
}

.comments {
    margin-top: 10px;
    padding-left: 15px;
    border-left: 2px solid var(--input-border);
}

.comment {
    font-size: 14px;
    color: var(--text-muted);
    margin-bottom: 10px;
}

.reply-icon {
    color: var(--twitter-blue);
    margin-right: 5px;
}

.comment-input-area {
    display: flex;
    margin-top: 15px;
    gap: 10px;
    align-items: center;
}

.comment-input-area input {
    flex: 1;
    margin-bottom: 0;
}

.tweet-header {
    margin-bottom: 8px;
    font-size: 15px;
}

.text-muted {
    color: var(--text-muted);
    font-size: 14px;
}
</style>