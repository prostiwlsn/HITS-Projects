<script setup>
import { useRouter, useRoute } from 'vue-router'
import { ref, onMounted, computed } from 'vue'
import { getCourseInfo } from '../requests/getCourseInfo';
import { signup } from '../requests/signup';
import { state } from '../state.js';
import { getMyCourses } from '../requests/getMyCourses';
import EditCourse from './EditCourse.vue'
import DeleteCourse from './DeleteCourse.vue';
import EditStatus from './EditStatus.vue'
import NotificationCreation from './NotificationCreation.vue'
import AddTeacher from './AddTeacher.vue'
import Student from './Student.vue'

const statusMap = new Map();
statusMap.set('Created', 'Создан');
statusMap.set('OpenForAssigning', 'Открыт для записи');
statusMap.set('Started', 'Начался');
statusMap.set('Finished', 'Закончился');

const colorMap = new Map();
colorMap.set('Created', 'black');
colorMap.set('OpenForAssigning', 'blue');
colorMap.set('Started', 'green');
colorMap.set('Finished', 'red');

const router = useRouter()

const route = useRoute()
const id = route.params.id;

const course = ref({})

const chosenTab = ref(0)

const chosenLowerTab = ref(0)

const isCreateShown = ref(0)
const isDeleteShown = ref(0)
const isEditStatusShown = ref(0)
const isNotificationShown = ref(0)
const isTeacherShown = ref(0)

const isMainTeacher = ref(0)
const isStudent = ref(0)
const isTeacher = ref(0)

const notificationsCount = ref(0)

const components = ["Sudents","Teachers"]

function openCreate()
{
    console.log(isCreateShown.value)
    isCreateShown.value = 0
    isCreateShown.value = 1
}

async function signupOnClick(){
    const result = await signup(id)
    if(result == true){
        isStudent.value = 1
    }
}

function addNotification(r){
    course.value = r
    notificationsCount.value++
}

function updateCourse(c){
    course.value.startYear = c.startYear
    course.value.maximumStudentsCount = c.maximumStudentsCount
    course.value.semester = c.semester
    course.value.requirements = c.requirements
    course.value.annotations = c.annotations
}

onMounted(async () => {
    course.value = await getCourseInfo(id)

    const myCourses = await getMyCourses()
    if(state.email == course.value.teachers.filter(t => t.isMain)[0].email){
        isMainTeacher.value = 1
    }
    else if(course.value.teachers.filter(t => t.email == state.email).length > 0){
        isTeacher.value = 1
    }
    else if(myCourses.filter(c => c.id == id).length > 0){
        isStudent.value = 1
    }
    //console.log(course.value.notifications)
    notificationsCount.value = course.value.notifications.length
})
</script>

<template>
    <div class="columns is-flex is-justify-content-center is-clipped">
        <div class="column is-two-thirds-widescreen is-full-tablet p-6">
            <div class="is-flex is-flex-direction-column"> 
                <div class="columns mt-3 mb-3"><div class="column is-full"><h3 class="title is-1 mt-3 mb-3">{{course.name}}</h3></div></div>
                <div class="columns is-vcentered is-mobile mb-5">
                    <div class="column is-half"><h5 class="subtitle is-5">Основная информация</h5></div>
                    <div class="column is-half">
                        <EditCourse :isActive="isCreateShown==1" @close="isCreateShown = 0" :courseId="id" :courseObj="course" @create="(r) => updateCourse(r)" :key="course"/>
                        <DeleteCourse v-if="state.isAdmin" :isActive="isDeleteShown==1" :courseObj="course" @closeDelete="isDeleteShown = 0" @delete="router.push('/')"/>
                        <button class="button is-primary ml-2" v-else :disabled="(isStudent == 1  || course.status!='OpenForAssigning')" @click="signupOnClick" v-show="(isMainTeacher != 1 && isTeacher != 1 && state.isAdmin != 1)">Записаться</button>
                        <div class="is-flex is-flex-direction-row">
                            <div><button class="button is-warning" v-if="state.isAdmin || isMainTeacher == 1" @click="openCreate()">Редактировать</button></div>
                            <div><button class="button is-danger ml-2" v-if="state.isAdmin" @click="isDeleteShown=1">Удалить</button></div>
                        </div>
                    </div>
                </div>
                <div class="columns is-vcentered is-mobile ">
                    <div class="column is-half">
                        <strong>Статус</strong>
                        <div :style="{ color: colorMap.get(course.status) }">{{statusMap.get(course.status)}}</div>
                    </div>
                    <div class="column is-half">
                        <EditStatus :isActive="isEditStatusShown==1" @close="isEditStatusShown = 0" :courseObj="course" :key="course" @editStatus="(r) => course = r"/>
                        <button class="button is-warning" v-if="isMainTeacher == 1 || state.isAdmin" @click="isEditStatusShown=1">Изменить</button>
                    </div>
                </div>
                <div class="columns is-vcentered is-mobile ">
                    <div class="column is-half">
                        <strong>Учебный год</strong>
                        <div class="has-text-grey">{{ course.startYear }}-{{ Number(course.startYear) + 1 }}</div>
                    </div>
                    <div class="column is-half">
                        <strong>Семестр</strong>
                        <div class="has-text-grey">{{course.semester == 'Autumn' ? 'Осенний' : 'Весенний'}}</div>
                    </div>
                </div>
                <div class="columns is-vcentered is-mobile ">
                    <div class="column is-half">
                        <strong>Всего мест</strong>
                        <div class="has-text-grey">{{course.maximumStudentsCount}}</div>
                    </div>
                    <div class="column is-half">
                        <strong>Студентов зачислено</strong>
                        <div class="has-text-grey">{{course.studentsEnrolledCount}}</div>
                    </div>
                </div>
                <div class="tabs">
                    <ul>
                        <li :class="chosenTab == 0 ? 'is-active' : ''" @click="chosenTab=0"><a>Требования</a></li>
                        <li :class="chosenTab == 1 ? 'is-active' : ''" @click="chosenTab=1"><a>Аннотация</a></li>
                        <li :class="chosenTab == 2 ? 'is-active' : ''" @click="chosenTab=2"><a>Уведомления <div style="background-color: red; border-radius: 15px;" class="ml-1 pl-1 pr-1">{{notificationsCount+'+'}}</div></a></li>
                    </ul>
                </div>
                <div>
                    <div v-if="chosenTab == 0">
                        <div v-html="course.requirements"></div>
                    </div>
                    <div v-if="chosenTab == 1">
                        <div v-html="course.annotations"></div>
                    </div>
                    <div v-if="chosenTab == 2">
                        <button class="button is-primary mb-3" v-if="state.isAdmin || isMainTeacher == 1" @click="isNotificationShown=1">Создать</button>
                        <NotificationCreation  :isActive="isNotificationShown==1" @close="isNotificationShown = 0" :courseObj="course" :key="course" @create="(r) => addNotification(r)"/>
                        <div v-for="notification in course.notifications" class="notification notification-fix" :class="notification.isImportant ? 'is-danger' : ''">
                            <div style="overflow: hidden; white-space: normal;">{{ notification.text }}</div>
                        </div>
                    </div>
                </div>
                <div class="tabs">
                    <ul>
                        <li :class="chosenLowerTab == 0 ? 'is-active' : ''" @click="chosenLowerTab=0"><a>Студенты</a></li>
                        <li :class="chosenLowerTab == 1 ? 'is-active' : ''" @click="chosenLowerTab=1"><a>Преподаватели</a></li>
                    </ul>
                </div>
                <div v-if="chosenLowerTab == 0">
                    <Student v-for="student in course.students" :studentObj="student" :courseObj="course" :key="course" :isStudent="!state.isAdmin && isTeacher == 0 && isMainTeacher == 0"/>
                </div>
                <div v-else>
                    <button class="button is-primary mb-3" v-if="state.isAdmin || isMainTeacher == 1" @click="isTeacherShown=1">Добавить</button>
                    <AddTeacher :isActive="isTeacherShown==1" @close="isTeacherShown = 0" :courseObj="course" :key="course" @create="(r) => course = r"/>
                    <div v-for="teacher in course.teachers">
                        <div class="is-flex is-flex-direction-row">
                            <div>{{ teacher.name }} </div>
                            <div v-if="teacher.isMain" style="background-color: limegreen; border-radius: 7px;" class="ml-2 pl-1 pr-1">Главный</div>
                        </div>
                        <div class="has-text-grey">
                            {{ teacher.email }}
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<style scoped>
.column{
    padding: 0;
}

.notification-fix{
    width: 100%;
}

@media only screen and (max-width: 770px) {
    .notification-fix{
        width: 400px;
    }
}
</style>