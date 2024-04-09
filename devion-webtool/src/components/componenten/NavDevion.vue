<script setup>
import { RouterLink } from 'vue-router';

</script>

<template>
  <nav class="c-nav">
    <ul class="o-list c-nav__list">
      <li class="c-nav__item">
        <RouterLink to="/Bestelbon" class="c-nav__link">Bestelbonnen</RouterLink>
      </li>
      <li class="c-nav__item">
        <RouterLink to="/artikel" class="c-nav__link">Cebeo artikel toevoegen</RouterLink>
      </li>
      <li class="c-nav__item">
        <RouterLink to="/assembly" class="c-nav__link">Assembly</RouterLink>
      </li>
      <li class="c-nav__item">
        <RouterLink to="/sync" class="c-nav__link">Syncronisatie</RouterLink>
      </li>
    </ul>
  </nav>
</template>

<style scoped>
/*
    Component: Nav
    ---
    Main navigation of the site
*/

.c-nav {
  grid-area: nav;
  font-size: calc(var(--global-fontSize) * 1.5);
  line-height: var(--global-whitespace);
  width: auto;
  display: flex;
}

.c-nav__link {
  font-family: var(--global-fontFamily);
  text-decoration: none;
  font-weight: 400;
  padding: var(--global-whitespace-xs) 0;
}

.c-nav__item {
  display: inline-block;
  margin-right: var(--global-whitespace-md);
}

.c-nav__link:active {
  color: var(--global-color-active);
}

.c-nav__link:hover,
.c-nav__link:focus {
  color: var(--global-color-hover);
  text-decoration: underline;
}

.c-nav__link--active {
  color: var(--global-color-active);
}

@media (min-width: 768px) {
  .c-nav__list {
    display: inline-block;
  }

  .c-nav__link {}
}
</style>