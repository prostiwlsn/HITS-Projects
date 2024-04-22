<script setup>
import group from './Group.vue'
import { state } from '../state.js';
import { getGroups } from '../requests/getGroups';
import { ref, onMounted, computed } from 'vue'
import CreateGroup from './CreateGroup.vue';

const groups = ref([])
const isCreateShown = ref(0)

function openCreate()
{
    console.log(isCreateShown.value)
    isCreateShown.value = 0
    isCreateShown.value = 1
}

onMounted(async () => {
    groups.value = await getGroups()
})
</script>

<template>
    <div class="columns is-flex is-justify-content-center is-clipped">
        <div class="column is-two-thirds-widescreen is-full-tablet">
            <div class="is-flex is-flex-direction-column"> 
                <h3 class="title is-1 pl-2">Группы</h3>
                <CreateGroup :isActive="isCreateShown==1" @close="isCreateShown = 0" @create="(g) => groups.push(g)"/>
                <div class="mb-3 ml-3" v-if="state.isAdmin">
                    <button class="button is-primary" @click="openCreate()">Создать</button>
                </div>
                <group v-for="groupOb in groups" :groupObj="groupOb" :key="groupOb.id"/>
            </div>
        </div>
    </div>
</template>

<style scoped>
.btn-mbl-fix{
    padding-left: 2rem;
}

.ttl-mbl-fix{
    padding-left: 3rem;
}

</style>