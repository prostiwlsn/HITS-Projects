<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { editCourse } from "../requests/editCourse"
import { editReqsAnns } from "../requests/editReqsAnns"
import { validateCourse } from "../functions/validateCourse"
import Editor from 'primevue/editor';
import UserDropdown from './UserDropdown.vue';
import { state } from '../state.js';
const emit = defineEmits(['close', 'create'])

const props = defineProps(['isActive', 'courseId', 'courseObj'])

const requirements = ref("")
const annotations = ref("")

let name = ""
let year = ""
let students = ""
let semester = "Autumn"
let selectedTeacher = ""
let id = ""

const isWarningShown = ref(false)
const warning = ref("Что-то пошло не так")

async function createCourseOnClick(){
    const result = state.isAdmin ? await editCourse(props.courseId, name, Number(year), Number(students), semester, requirements.value, annotations.value, id) : await editReqsAnns(props.courseId, requirements.value, annotations.value)
    if(typeof result != "number"){
        emit('create',{
            name: name,
            startYear: year,
            maximumStudentsCount: students,
            semester: semester,
            requirements: requirements.value,
            annotations: annotations.value,
            mainTeacherId: id
        })
        emit('close')
    }
    else{
        warning.value = validateCourse(name, year, students, semester, id, requirements.value, annotations.value)
        isWarningShown.value=true
    }
}

onMounted(async () =>{
    name = props.courseObj.name
    year = props.courseObj.startYear
    students = props.courseObj.maximumStudentsCount,
    semester = props.courseObj.semester,
    requirements.value = props.courseObj.requirements
    annotations.value = props.courseObj.annotations,
    id = props.courseObj.teachers.filter(t => t.isMain)[0].id
})
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-tree-quarters-widescreen is-flex is-justify-content-center is-full-mobile is-flex is-flex-direction-column is-align-items-center">
                <button class="delete is-large" aria-label="close" @click="$emit('close')" style="position: absolute; top: 10px; right: 10px;"></button>
                <div class="title is-1">Редактирование курса</div>
                <div class="is-flex is-flex-direction-column is-align-items-center" style="width: 65%;"> 
                    <input class="input mb-3" type="text" placeholder="Название" v-model="name" v-if="state.isAdmin">
                    <input class="input mb-3" type="text" placeholder="Год" v-model="year" v-if="state.isAdmin">
                    <input class="input mb-3" type="text" placeholder="Студентов" v-model="students" v-if="state.isAdmin">
                    <div class="select mb-3" v-if="state.isAdmin">
                        <select v-model="semester">
                            <option value="Autumn">Осенний</option>
                            <option value="Spring">Весенний</option>
                        </select>
                    </div>
                    <Editor class="mb-3" v-model="requirements"></Editor>
                    <Editor class="mb-3" v-model="annotations"></Editor>
                    <a class="button is-primary" @click="createCourseOnClick">
                        <strong>Редактировать</strong>
                    </a>
                    <div class="notification is-warning" v-if="isWarningShown" style="width: 100%; margin-top: 1rem;">
                        {{ warning }}
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>