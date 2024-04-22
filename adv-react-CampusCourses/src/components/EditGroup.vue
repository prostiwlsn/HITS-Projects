<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { editGroup } from "../requests/editGroup.js"
const emit = defineEmits(['close', 'edit'])

const props = defineProps(['isActive', 'groupObj'])

const newGroupName = ref("")

const isWarningShown = ref(false)

function editGroupOnClick(){
    if(newGroupName.value.length == 0){
        isWarningShown.value = true
        console.log("missing name")
        return
    }
    editGroup(props.groupObj.id, newGroupName.value)
    emit('edit', newGroupName.value)
    emit('close')
}

onMounted(async () => {
    newGroupName.value = props.groupObj.name
})
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Редактировать</div>
                    <input class="input mb-3" type="text" placeholder="Название" v-model="newGroupName">
                    <a class="button is-primary" @click="editGroupOnClick()">
                        <strong>Применить</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        Введите название группы
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
</style>