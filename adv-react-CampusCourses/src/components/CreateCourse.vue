<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { createCourse } from "../requests/createCourse"
import { validateCourse } from "../functions/validateCourse"
import Editor from 'primevue/editor';
import UserDropdown from './UserDropdown.vue';
const emit = defineEmits(['close', 'create'])

const props = defineProps(['isActive', 'groupId'])

const requirements = ref("")
const annotations = ref("")

const isWarningShown = ref(false)
const warning = ref("Что-то пошло не так")

let name = ""
let year = ""
let students = ""
let semester = "Autumn"
let selectedTeacher = ""
let id = ""

async function createCourseOnClick(){
    const result = await createCourse(props.groupId, name, Number(year), Number(students), semester, requirements.value, annotations.value, id)
    if(typeof result != "number"){
        emit('create',result)
        emit('close')
        isWarningShown.value = false
    }
    else{
        warning.value = validateCourse(name, year, students, semester, id, requirements.value, annotations.value)
        isWarningShown.value=true
    }
}
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-tree-quarters-widescreen is-flex is-justify-content-center is-full-mobile is-flex is-flex-direction-column is-align-items-center">
                <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                <div class="title is-1">Создание класса</div>
                <div class="is-flex is-flex-direction-column is-align-items-center" style="width: 65%;"> 
                    <input class="input mb-3" type="text" placeholder="Название" v-model="name">
                    <input class="input mb-3" type="text" placeholder="Год" v-model="year">
                    <input class="input mb-3" type="text" placeholder="Студентов" v-model="students">
                    <div class="select mb-3">
                        <select v-model="semester">
                            <option value="Autumn">Осенний</option>
                            <option value="Spring">Весенний</option>
                        </select>
                    </div>
                    <Editor class="mb-3" v-model="requirements"></Editor>
                    <Editor class="mb-3" v-model="annotations"></Editor>
                    <UserDropdown class="mb-3" @choose="(i) => id=i"/>
                    <a class="button is-primary" @click="createCourseOnClick">
                        <strong>Создать</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        {{ warning }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>