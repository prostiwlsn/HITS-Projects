<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { createNotification } from "../requests/createNotification.js"
const emit = defineEmits(['close', 'create'])

const props = defineProps(['isActive', 'courseObj'])

let message = ""
let isImportant = false

const isWarningShown = ref(false)

async function createOnClick(){
    if(message.length == 0){
        isWarningShown.value = true
        return
    }

    const result = await createNotification(props.courseObj.id, message, isImportant)
    if(typeof result != "number"){
        isWarningShown.value = false
        message = ""
        
        emit('create', result)
        emit('close')
    }
}
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Создание</div>
                    <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                    <input class="input mb-2" type="text" placeholder="Сообщение" v-model="message">
                    <label class="checkbox mb-2">
                        <input type="checkbox" v-model="isImportant"/>
                        Важное сообщение
                    </label>
                    <a class="button is-primary mr-2" @click="createOnClick()">
                        <strong>Опубликовать</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        Введите название группы
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>