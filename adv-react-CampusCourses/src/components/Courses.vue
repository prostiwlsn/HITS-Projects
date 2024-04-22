<script setup>
import { getCourses } from '../requests/getCourses';
import { ref, onMounted, computed } from 'vue'
import course from './MyCourse.vue'
import { useRouter, useRoute } from 'vue-router'
import { state } from '../state.js';
import CreateCourse from './CreateCourse.vue';

const route = useRoute()
const id = route.params.id;

const groups = ref([])

const isCreateShown = ref(0)
function openCreate()
{
    console.log(isCreateShown.value)
    isCreateShown.value = 0
    isCreateShown.value = 1
}

onMounted(async () => {
    groups.value = await getCourses(id)
})
</script>

<template>
    <div class="columns is-flex is-justify-content-center">
        <div class="column is-two-thirds-widescreen is-full-tablet">
            <div class="is-flex is-flex-direction-column"> 
                <h3 class="title is-1">Курсы</h3>
                <CreateCourse :isActive="isCreateShown==1" @close="isCreateShown = 0" :groupId="id" @create="(r) => groups = r"/>
                <div class="mb-3" v-if="state.isAdmin">
                    <button class="button is-primary" @click="openCreate()">Создать</button>
                </div>
                <course v-for="groupOb in groups" :courseObj="groupOb"/>
            </div>
        </div>
    </div>
</template>