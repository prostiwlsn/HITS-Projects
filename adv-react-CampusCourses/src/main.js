import { createApp } from 'vue'
import router from './router.js'
//import './style.css'
import App from './App.vue'
import PrimeVue from 'primevue/config';
import './../node_modules/Bulma/CSS/bulma.css';

const app = createApp(App).use(router).use(PrimeVue, { unstyled: true }).mount('#app')