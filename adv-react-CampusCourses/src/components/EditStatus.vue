<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { editStatus } from "../requests/editStatus.js"
const emit = defineEmits(['close', 'editStatus'])

const props = defineProps(['isActive', 'courseObj'])

const statusArray = ["OpenForAssigning", "Started", "Finished"]
const statusBoolArray = ref([false, false, false])
const chosenStatus = ref(0)

const isWarningShown = ref(false)

async function editStatusOnClick(status){
    const result = await editStatus(props.courseObj.id, status)
    if(typeof result != "number"){
        emit('editStatus', result)
        emit('close')
        isWarningShown.value = false
    }
    else{
        isWarningShown.value = true
    }
}

function changeStatus(index){
    chosenStatus.value = index
    for(let i = 0; i < 3; i++){
        if(i == index){
            statusBoolArray.value[i] = true
        }
        else{
            statusBoolArray.value[i] = false
        }
    }
}

onMounted(() => {
    for(let i = 0; i < 3; i++){
        if(statusArray[i] == props.courseObj.status){
            statusBoolArray.value[i] = true
        }
    }
})
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Редактирование</div>
                    <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                    <div class="is-flex is-is-flex-direction-row mb-2">
                        <label class="checkbox ml-2" v-for="status in [0,1,2]">
                            <input type="checkbox" v-model="statusBoolArray[status]" @click="changeStatus(status)"/>
                            {{statusArray[status]}}
                        </label>
                    </div>
                    <a class="button is-primary mr-2" @click="editStatusOnClick(statusArray[chosenStatus])">
                        <strong>Сохранить</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        Невозможно перевести статус курса в предыдущее положение
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>