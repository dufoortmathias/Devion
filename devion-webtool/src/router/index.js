import { createRouter, createWebHistory } from 'vue-router'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: () => import('../views/BestelbonView.vue')
    },
    {
      path: '/bestelbon',
      name: 'bestelbon',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/BestelbonView.vue')
    },
    {
      path: '/artikel',
      name: 'artikel',
      component: () => import('../views/ArtikelView.vue')
    }
  ]
})

export default router
