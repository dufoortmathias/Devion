<script setup>
import { RouterView } from 'vue-router';
import HeaderDevion from './components/HeaderDevion.vue'
</script>

<template>
  <div id="app" class="c-app">
    <HeaderDevion />
    <div class="c-app_body">
        <RouterView />
    </div>
  </div>
</template>