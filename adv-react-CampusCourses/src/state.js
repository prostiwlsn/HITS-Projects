import { reactive } from 'vue'
import { getRoles } from './requests/getRoles.js'

export const state = reactive({
    isAuthorized: false,
    isAdmin: false,
    email: 'email@email.com',
    async authorize(email){
        this.isAuthorized = true;
        this.email=email;

        const roles = await getRoles()
        this.isAdmin = roles.isAdmin
    },
    unAuthorize(){
        this.isAuthorized = false;
        this.email='email@email.com';
    },
    accessAuthState(){
        return this.isAuthorized;
    }
})