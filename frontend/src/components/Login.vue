<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const email = ref('');
const password = ref('');
const isSubmitting = ref(false);

const login = async () => {
    if (!email.value || !password.value) return;

    isSubmitting.value = true;
    try {
        await authStore.login(email.value, password.value);
        router.push('/');
    } catch (error) { }
    finally {
        isSubmitting.value = false;
    }
};
</script>

<template>
    <div class="auth-container">
        <div class="auth-box">
            <h2>Login</h2>
            <input v-model="email" placeholder="Email" type="email" class="input-field" />
            <input v-model="password" placeholder="Password" type="password" class="input-field" />
            <button @click="login" class="btn-primary full-width">
                {{ isSubmitting ? "Signing In..." : "Sign In" }}
            </button>
            <p class="switch-auth">
                Don't have an account? <router-link to="/register">Sign up here</router-link>
            </p>
        </div>
    </div>
</template>