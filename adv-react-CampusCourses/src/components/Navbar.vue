<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { state } from '../state.js';
import { useRouter } from 'vue-router';
import { checkAuth } from '../functions/checkAuth.js'
import { getMyCourses } from '../requests/getMyCourses.js'
import { getTeachingCourses } from '../requests/getTeachingCourses.js'

const router = useRouter()

const isActive = ref("")

const courses = ref([])
const teachingCourses = ref([])

//проблемы с навбаром (после смены пользователя не убирает ненужные плашки)

const isLoggedIn = ref(0)

const userIsAuthorized = computed(() => {
    return state.isAuthorized;
})

function logOut(){
    state.unAuthorize()
    localStorage.removeItem('token')
    router.push('/');
}

watch(state, async (newAuth, oldAuth) => {
    if(state.isAuthorized){
        courses.value = await getMyCourses()
        teachingCourses.value = await getTeachingCourses()
    }
})

onMounted(async () => {
    await checkAuth()
    if(state.isAuthorized){
        courses.value = await getMyCourses()
        teachingCourses.value = await getTeachingCourses()
    }
})
</script>

<template>
    <nav class="navbar is-fixed-top" role="navigation" aria-label="main navigation">
        <div class="navbar-brand">
            <a class="navbar-item" @click="this.$router.push('/')">
                <strong>Кампусные курсы</strong>
            </a>
            <a role="button" :class="'navbar-burger' + isActive" aria-label="menu" aria-expanded="false" data-target="navbarBasicExample" @click="isActive=isActive==' is-active' ? '' : ' is-active'">
                <span aria-hidden="true"></span>
                <span aria-hidden="true"></span>
                <span aria-hidden="true"></span>
            </a>
        </div>
        <div id="navbarBasicExample" :class="'navbar-menu' + isActive">
            <div class="navbar-start" v-if="state.isAuthorized">
                <a class="navbar-item" @click="this.$router.push('/groups')">
                    Группы курсов
                </a>

                <a class="navbar-item" v-if="courses.length > 0" @click="this.$router.push('/myCourses')">
                    Мои курсы
                </a>

                <a class="navbar-item" v-if="teachingCourses.length > 0" @click="this.$router.push('/teachingCourses')">
                    Преподаваемые курсы
                </a>
            </div>
            <div class="navbar-end">
                <div class="navbar-item" v-if="!state.isAuthorized">
                    <div class="buttons">
                        <a class="button is-primary" @click="this.$router.push('/registration')"> 
                            <strong>Зарегистрироваться</strong>
                        </a>
                        <a class="button is-light" @click="this.$router.push('/login')">
                            Войти
                        </a>
                    </div>
                </div>
                <div class="navbar-item" v-else>
                    <div class="buttons">
                        <a class="button is-primary" @click="this.$router.push('/profile')">
                            <strong>{{ state.email }}</strong>
                        </a>
                        <a class="button is-light" @click="logOut()">
                            Выйти
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </nav>
</template>