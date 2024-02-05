import './assets/main.css'

import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import * as ConfirmDialog from "vuejs-confirm-dialog"

const app = createApp(App)
app.use(ConfirmDialog)

app.use(router)

app.mount('#app')
