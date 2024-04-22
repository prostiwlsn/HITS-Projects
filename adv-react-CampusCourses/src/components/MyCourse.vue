<script setup>
import { useRouter } from 'vue-router';
import { state } from '../state.js';

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

const props = defineProps(['courseObj'])
</script>

<template>
    <div class="card is-flex is-flex-direction-column is-justify-content-start mb-3">
        <div class="is-flex is-flex-direction-row is-justify-content-space-between"><a class="m-3" @click="this.$router.push('/course/'+courseObj.id)">
            <strong>{{ courseObj.name }}</strong></a>
            <div class="m-3" :style="{ color: colorMap.get(courseObj.status) }">{{statusMap.get(courseObj.status)}}</div>
        </div>
        <div class="ml-3 mb-3">Учебный год - {{ courseObj.startYear }}-{{ Number(courseObj.startYear) + 1 }}</div>
        <div class="ml-3 mb-3">Семестр - {{ courseObj.semester == "Spring" ? 'Весенний' : 'Осенний' }}</div>
        <div class="ml-3 mb-3">Мест всего - {{ courseObj.maximumStudentsCount }}</div>
        <div class="ml-3 mb-3">Мест осталось - {{ courseObj.remainingSlotsCount }}</div>
    </div>
</template>