<script setup>
import { state } from '../state.js';
import { ref, onMounted, computed } from 'vue'
import EditGroup from './EditGroup.vue';
import DeleteGroup from './DeleteGroup.vue';

const props = defineProps(['groupObj'])

const isEditShown = ref(0)
const isDeleteShown = ref(0)

let name = props.groupObj.name
let isDeleted = false

function openEdit()
{
    console.log(isEditShown.value)
    isEditShown.value = 0
    isEditShown.value = 1
}
function openDelete()
{
    console.log(isDeleteShown.value)
    isDeleteShown.value = 0
    isDeleteShown.value = 1
}
</script>

<template>
    <div class="card is-flex is-flex-direction-row is-justify-content-space-between mb-3 is-desktop pl-3" v-if="!isDeleted">
        <a class="m-3" @click="this.$router.push('/courses/'+groupObj.id)"><strong>{{ name }}</strong></a>
        <div v-if="state.isAdmin" class="columns is-mobile">
            <div class="column is-half"><button class="button is-warning m-3 " @click="openEdit()">Редактировать</button></div>
            <div class="column is-half"><button class="button is-danger m-3" @click="openDelete()">Удалить</button></div>
        </div>
        <EditGroup :isActive="isEditShown==1" :groupObj="groupObj" @close="isEditShown = 0" @edit="(n) => name = n"/>
        <DeleteGroup :isActive="isDeleteShown==1" :groupObj="groupObj" @closeDelete="isDeleteShown = 0" @delete="isDeleted=true"/>
    </div>
</template>