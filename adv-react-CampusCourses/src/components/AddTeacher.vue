<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { addTeacher } from "../requests/addTeacher.js"
import UserDropdown from './UserDropdown.vue';
const emit = defineEmits(['close', 'create'])

const props = defineProps(['isActive', 'courseObj'])

let id = ""

const isWarningShown = ref(false)

async function addOnClick(){
    const result = await addTeacher(props.courseObj.id, id)
    if(typeof result != "number"){
        emit('create', result)
        emit('close')
        isWarningShown.value = false
    }
    else{
        isWarningShown.value = true
    }
}
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Добавить</div>
                    <button class="delete is-large" aria-label="close" @click="$emit('close');isWarningShown=false;" style="position: absolute; top: 10px; right: 10px;"></button>
                    <UserDropdown class="mb-3" @choose="(i) => id=i"/>
                    <a class="button is-primary mr-2" @click="addOnClick()">
                        <strong>Добавить</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        Невозможно назначить студента или уже существующего преподавателя
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>