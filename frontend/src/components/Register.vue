<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const username = ref('');
const email = ref('');
const password = ref('');
const errorMsg = ref('');

const register = async () => {
    try {
        errorMsg.value = '';
        await authStore.register(username.value, email.value, password.value);
        alert('Account created successfully! Please log in.');
        router.push('/login');
    } catch (error) {
        errorMsg.value = error.response?.data || 'Registration failed. Please try again.';
    }
};
</script>

<template>
    <div class="auth-container">
        <div class="auth-box">
            <h2>Create an Account</h2>
            <p v-if="errorMsg" class="error-text">{{ errorMsg }}</p>

            <input v-model="username" placeholder="Username" type="text" class="input-field" />
            <input v-model="email" placeholder="Email" type="email" class="input-field" />
            <input v-model="password" placeholder="Password" type="password" class="input-field" />

            <button @click="register" class="btn-primary full-width">Sign Up</button>

            <p class="switch-auth">
                Already have an account? <router-link to="/login">Log in here</router-link>
            </p>
        </div>
    </div>
</template>