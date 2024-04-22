import { createRouter, createWebHistory } from "vue-router";
import Login from "../components/Login-Form.vue"
import Registration from '../components/Registration-Form.vue'
import Main from "../components/Main-page.vue"
import Profile from "../components/Profile-page.vue"
import Authors from "../components/Authors-page.vue"
import PostPage from "../components/PostPage.vue"
import Communities from "../components/Community-List.vue"
import Community from "../components/Community-page.vue"
import PostCreation from "../components/PostForm.vue"

const routes = [
    {
        path: '/',
        name: 'Home',
        component: Main,
        props: (route) => ({
            author: route.query.author || '', 
            tagsProp: route.query.tags ? route.query.tags.split(',') : [], 
            sortingProp: route.query.sorting || 'CreateDesc', 
            max: parseInt(route.query.max) || Infinity, 
            min: parseInt(route.query.min) || 0, 
            onlyMyCommunities: route.query.onlyMyCommunities === 'true', 
            size: parseInt(route.query.size) || 5, 
            page: parseInt(route.query.page) || 1, 
        }),
    },
    {
        path: '/profile',
        name: 'Profile',
        component: Profile
    },
    {
        path: '/login',
        name: 'Login',
        component: Login
    },
    {
        path: '/registration',
        name: 'Registration',
        component: Registration
    },
    {
        path: '/authors',
        name: 'Authors',
        component: Authors
    },
    {
        path: '/post/:id',
        name: 'PostPage',
        component: PostPage
    },
    {
        path: '/communities',
        name: 'Communities',
        component: Communities
    },
    {
        path: '/communities/:id',
        name: 'Community',
        component: Community
    },
    {
        path: '/post/create',
        name: 'PostCreation',
        component: PostCreation
    },
]

export default createRouter({
    history: createWebHistory(),
    routes
})