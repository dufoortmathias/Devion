<script setup>
import { RouterView } from 'vue-router';
import Sidebar from './components/Sidebar.vue';
</script>

<template>
  <div id="app" class="c-app">
    <Sidebar class="sidebar"/>
    <div class="c-app_body">
        <RouterView />
    </div>
  </div>
</template>

<style lang="scss">
:root {
  --primary: #007bff;
  --primary-alt: #0056b3;
  --grey: #6c757d;
  --dark: #343a40;
  --dark-alt: #495057;
  --light: #f8f9fa;
  --sidebar-width: 300px;
}

* {
  margin: 0;
  padding: 0;
  box-sizing: border-box; 
  font-family: "sofia pro", sans-serif;
}

body {
  background-color: var(--light);
}

button {
  cursor: pointer;
  appearance: none;
  border: none;
  outline: none;
  background: none;
}

.c-app {
  display: flex;
  min-width: 100vw;

  main {
    flex: 1 1 0;
    padding: 2rem;

    @media (max-width: 1024px) {
      padding-left: 6rem;
    }
  }
}

.sidebar {
  height: 100vh;
}
</style>