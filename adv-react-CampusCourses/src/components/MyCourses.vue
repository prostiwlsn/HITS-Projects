<script setup>
import course from './MyCourse.vue'
import { getMyCourses } from '../requests/getMyCourses';
import { ref, onMounted, computed } from 'vue'

const groups = ref([])

onMounted(async () => {
    groups.value = await getMyCourses()
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