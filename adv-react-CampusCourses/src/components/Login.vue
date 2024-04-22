<script setup>
import { login } from '../requests/login.js'
import { state } from '../state.js';
import { useRouter } from 'vue-router';
import { ref } from 'vue'

// добавить маски

const router = useRouter()

let email = ""
let password = ""

const error = ref(0)

async function loginAsync(){
    const result = await login(email, password)
    console.log(result)
    if(result != false){
        localStorage.setItem('token', result);
        localStorage.setItem('email', email);
        console.log(result);
        await state.authorize(email);

        console.log(state.isAdmin)

        router.push('/');
    }
    else{
        error.value = 400
    }
}
</script>

<template>
    <div class="columns is-flex is-justify-content-center is-align-items-center" style="height: 85vh;">
        <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-one-fifth-fullhd is-full-mobile">
            <div class="is-flex is-flex-direction-column is-align-items-center"> 
                <h3 class="title is-1">Вход</h3>
                <input class="input mb-3" type="text" placeholder="Логин" v-model="email">
                <input class="input mb-3" type="password" placeholder="Пароль" v-model="password">
                <a class="button is-primary" @click="loginAsync()">
                    <strong>Войти</strong>
                </a>
                <div class="notification is-warning mt-3" v-if="error!=0">
                    Неправильный логин или пароль
                </div>
            </div>
        </div>
    </div>
</template>