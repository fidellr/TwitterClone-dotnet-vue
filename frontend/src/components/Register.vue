<script setup>
import { ref } from 'vue';
import { useAuthStore } from '../stores/authStore';
import { useRouter } from 'vue-router';

const authStore = useAuthStore();
const router = useRouter();
const username = ref('');
const email = ref('');
const password = ref('');
const isSubmitting = ref(false);


const register = async () => {
    if (!username.value || !email.value || !password.value) return;

    isSubmitting.value = true;
    try {
        await authStore.register(username.value, email.value, password.value);
        router.push('/login');
    } catch (err) { }
    finally {
        isSubmitting.value = false;
    }
};
</script>

<template>
    <div class="auth-container">
        <div class="auth-box">
            <h2>Create an Account</h2>

            <input v-model="username" placeholder="Username" type="text" class="input-field" />
            <input v-model="email" placeholder="Email" type="email" class="input-field" />
            <input v-model="password" placeholder="Password" type="password" class="input-field" />

            <button @click="register" class="btn-primary full-width">
                {{ isSubmitting ? "Creating Account..." : "Sign Up" }}
            </button>

            <p class="switch-auth">
                Already have an account? <router-link to="/login">Log in here</router-link>
            </p>
        </div>
    </div>
</template>