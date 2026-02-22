<script setup>
import { useToastStore } from '../../stores/toastStore';

const toastStore = useToastStore();
</script>

<template>
    <div class="toast-container">
        <TransitionGroup name="toast-list">
            <div v-for="toast in toastStore.toasts" :key="toast.id" class="toast" :class="`toast-${toast.type}`">
                <span>{{ toast.message }}</span>
                <button class="close-btn" @click="toastStore.remove(toast.id)">✕</button>
            </div>
        </TransitionGroup>
    </div>
</template>

<style scoped>
.toast-container {
    position: fixed;
    bottom: 20px;
    right: 20px;
    display: flex;
    flex-direction: column;
    gap: 10px;
    z-index: 9999;
}

.toast {
    min-width: 250px;
    padding: 14px 20px;
    border-radius: 8px;
    color: white;
    display: flex;
    justify-content: space-between;
    align-items: center;
    box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
    font-weight: 500;
    font-size: 14px;
}

.toast-error {
    background-color: var(--error-color);
}

.toast-success {
    background-color: #00ba7c;
}

.toast-info {
    background-color: var(--twitter-blue);
}

.close-btn {
    background: none;
    border: none;
    color: white;
    cursor: pointer;
    font-size: 16px;
    opacity: 0.8;
    margin-left: 15px;
}

.close-btn:hover {
    opacity: 1;
}

.toast-list-enter-active,
.toast-list-leave-active {
    transition: all 0.3s ease;
}

.toast-list-enter-from {
    opacity: 0;
    transform: translateX(30px);
}

.toast-list-leave-to {
    opacity: 0;
    transform: scale(0.9);
}
</style>