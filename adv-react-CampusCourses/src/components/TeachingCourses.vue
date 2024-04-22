<script setup>
import { getTeachingCourses } from '../requests/getTeachingCourses';
import { ref, onMounted, computed } from 'vue'
import course from './MyCourse.vue'

const groups = ref([])

onMounted(async () => {
    groups.value = await getTeachingCourses()
})
</script>

<template>
    <div class="columns is-flex is-justify-content-center">
        <div class="column is-two-thirds-widescreen is-full-tablet">
            <div class="is-flex is-flex-direction-column"> 
                <h3 class="title is-1">Мои курсы</h3>
                <course v-for="groupOb in groups" :courseObj="groupOb"/>
            </div>
        </div>
    </div>
</template>