<script setup>
import { register } from '../requests/register.js'
import { validateRegistration } from '../functions/validateRegistration.js'
import { state } from '../state.js';
import { useRouter } from 'vue-router';
import { ref, reactive } from 'vue'
import codes from '../functions/validationCodes.json'

// добавить маски

const router = useRouter()

let name = ""
let birthDate = ""
let email = ""
let password = ""
let confirmPassword = ""

const error = reactive({
    code:200,
    message:""
})

async function registerAsync(){
    const validationResult = validateRegistration(name, birthDate, email, password, confirmPassword)

    if(validationResult != true){
        error.code=validationResult.code,
        error.message=validationResult.message
        return;
    }

    const result = await register(name, birthDate, email, password, confirmPassword)
    console.log(result)
    if(typeof result != "number"){
        localStorage.setItem('token', result);
        localStorage.setItem('email', email);

        router.push('/');

        console.log(result);
        state.authorize(email);
    }
    else{
        error.code = result
        error.message = codes.filter(c => c.code == result)[0].message;
    }
}

</script>

<template>
    <div class="columns is-flex is-justify-content-center is-align-items-center" style="height: 85vh;">
        <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-one-fifth-fullhd is-full-mobile">
            <div class="is-flex is-flex-direction-column is-align-items-center"> 
                <h3 class="title is-1">Регистрация</h3>
                <input class="input mb-3" type="text" placeholder="ФИО" v-model="name">
                <input class="input mb-3" type="date" v-model="birthDate">
                <input class="input mb-3" type="text" placeholder="Email" v-model="email">
                <input class="input mb-3" type="password" placeholder="Пароль" v-model="password">
                <input class="input mb-3" type="password" placeholder="Подтверждение пароля" v-model="confirmPassword">
                <a class="button is-primary" @click="registerAsync()">
                    <strong>Зарегистрироваться</strong>
                </a>
                <div class="notification is-warning mt-3" v-if="error.code!=200">
                    {{error.code == 400 ? 'Что-то пошло не так' : error.message}}
                </div>
            </div>
        </div>
    </div>
</template>