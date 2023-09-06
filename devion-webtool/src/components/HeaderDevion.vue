<script setup>
import DevionIcon from './icons/IconDevion.vue'
import DevionNav from './componenten/NavDevion.vue'
</script>

<template>
    <div class="c-header">
        <DevionIcon class="c-icon" />
        <DevionNav class="c-nav" />
    </div>
</template>

<style scoped>
.c-header {
    align-items: center;
    justify-content: center;
    justify-items: center;
    align-self: center;
    width: 100vw;
    grid-area: header;
    display: grid;
    grid-template-areas: "logo nav";
    grid-gap: 20vw;
}

.c-icon {
    padding: 0 var(--global-whitespace-lg);
    height: var(--c-app__topbar-height);
    display: flex;
    align-items: center;
    grid-area: logo;
    height: var(--c-app__topbar-height);
    align-items: center;
    justify-content: center;
    justify-items: center;
    align-self: center;
}

.c-nav {
    grid-area: nav;
}
</style>