import { createRouter, createWebHistory } from "vue-router";
import Main from './components/Main.vue'
import Login from './components/Login.vue'
import Registration from './components/Registration.vue'
import Profile from './components/Profile.vue'
import Groups from './components/Groups.vue'
import Courses from './components/Courses.vue'
import MyCourses from './components/MyCourses.vue'
import TeachingCourses from './components/TeachingCourses.vue'
import Course from './components/Course.vue'

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Main
    },
    {
        path: '/login',
        name: 'login',
        component: Login
    },
    {
        path: '/registration',
        name: 'registration',
        component: Registration
    },
    {
        path: '/profile',
        name: 'profile',
        component: Profile
    },
    {
        path: '/groups',
        name: 'groups',
        component: Groups
    },
    {
        path: '/myCourses',
        name: 'myCourses',
        component: MyCourses
    },
    {
        path: '/teachingCourses',
        name: 'teachingCourses',
        component: TeachingCourses
    },
    {
        path: '/courses/:id',
        name: 'courses',
        component: Courses
    },
    {
        path: '/course/:id',
        name: 'course',
        component: Course
    }
]

export default createRouter({
    history: createWebHistory(),
    routes
})