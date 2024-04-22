<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { createGroup } from "../requests/createGroup.js"
const emit = defineEmits(['close', 'create'])

const props = defineProps(['isActive'])

let name = ""

const isWarningShown = ref(false)

async function createGroupOnClick(){
    let result = await createGroup(name)
    if(typeof result != "number"){
        emit('create',result)
        emit('close')
        name=""
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
                <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Создание</div>
                    <input class="input mb-3" type="text" placeholder="Название" v-model="name">
                    <a class="button is-primary" @click="createGroupOnClick">
                        <strong>Создать</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        Введите название группы
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>