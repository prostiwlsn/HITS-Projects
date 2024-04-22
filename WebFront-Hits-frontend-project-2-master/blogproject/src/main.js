import { createApp } from 'vue'
import App from './App.vue'
import router from './router/index.js'

import "bootstrap/dist/css/bootstrap.min.css"
import "bootstrap"

/* import the fontawesome core */
import { library } from '@fortawesome/fontawesome-svg-core'

/* import font awesome icon component */
import { FontAwesomeIcon } from '@fortawesome/vue-fontawesome'

/* import specific icons */
import { faHeart, faComment, faPenToSquare, faTrash, faCrown } from '@fortawesome/free-solid-svg-icons'
import { faHeart as faHeartRegular} from '@fortawesome/free-regular-svg-icons'

/* add icons to the library */
library.add(faHeart, faComment, faHeartRegular, faPenToSquare, faTrash, faCrown)

createApp(App).use(router).component('font-awesome-icon', FontAwesomeIcon).mount('#app')