<script setup>
import { getProfile } from '../requests/getProfile.js'
import { editProfile } from '../requests/editProfile.js'
import { useRouter } from 'vue-router';
import { ref, reactive, onMounted } from 'vue'
import { validateRegistration } from '../functions/validateRegistration.js'
import codes from '../functions/validationCodes.json'

// добавить маски
//добавить сообщение об успешной смене данных

const router = useRouter()

const name = ref("")
const birthDate = ref("")

const error = reactive({
    code:0,
    message:""
})

async function editProfileAsync(){
    const validationResult = validateRegistration(name.value, birthDate.value, "email@email.com", 'password1', 'password1')

    if(validationResult != true){
        error.code=validationResult.code,
        error.message=validationResult.message
        return;
    }

    const result = await editProfile(name.value, birthDate.value)
    console.log(result)
    if(typeof result == "number"){
        error.code = result
        error.message = codes.filter(c => c.code == result)[0].message;
    }
    else{
        error.code = 200
        error.message = "Данные изменены"
    }
}

onMounted(async () => {
    const profile = await getProfile()
    name.value = profile.fullName
    birthDate.value = profile.birthDate.slice(0,10)
});
</script>

<template>
    <div class="columns is-flex is-justify-content-center is-align-items-center" style="height: 85vh;">
        <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-one-fifth-fullhd is-full-mobile">
            <div class="is-flex is-flex-direction-column is-align-items-center"> 
                <h3 class="title is-1">Профиль</h3>
                <input class="input mb-3" type="text" placeholder="ФИО" v-model="name">
                <input class="input mb-3" type="date" v-model="birthDate">
                <a class="button is-primary" @click="editProfileAsync()">
                    <strong>Отредактировать профиль</strong>
                </a>
                <div class="notification is-warning mt-3" v-if="error.code>200">
                    {{error.code == 400 ? 'Что-то пошло не так' : error.message}}
                </div>
                <div class="notification is-success mt-3" v-else-if="error.code == 200">
                    {{ error.message}}
                </div>
            </div>
        </div>
    </div>
</template>