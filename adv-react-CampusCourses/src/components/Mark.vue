<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { editUserMark } from "../requests/editUserMark.js"
const emit = defineEmits(['close', 'editMark'])

const props = defineProps(['isActive', 'courseObj', 'studentId', 'markType'])

const statusArray = ["Passed", "Failed"]
const statusBoolArray = ref([false, false])
const chosenStatus = ref(0)

async function editStatusOnClick(status){
    const result = await editUserMark(props.courseObj.id, props.studentId, statusArray[chosenStatus.value], props.markType)
    if(typeof result != "number"){
        emit('editMark', statusArray[chosenStatus.value])
        emit('close')
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

})
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Оценки</div>
                    <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                    <div class="is-flex is-is-flex-direction-row mb-2">
                        <label class="checkbox ml-2" v-for="status in [0,1]">
                            <input type="checkbox" v-model="statusBoolArray[status]" @click="changeStatus(status)"/>
                            {{statusArray[status]}}
                        </label>
                    </div>
                    <a class="button is-primary mr-2" @click="editStatusOnClick(statusArray[chosenStatus])">
                        <strong>Сохранить</strong>
                    </a>
                </div>
            </div>
        </div>
    </div>
</template>