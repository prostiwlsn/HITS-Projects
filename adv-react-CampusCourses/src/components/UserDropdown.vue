<script setup>
import { state } from '../state.js';
import { ref, onMounted, computed, watch } from 'vue'
import { getUsers } from '../requests/getUsers.js'

const emit = defineEmits(['choose'])

const username = ref("")
const isListShown = ref(0)
let users = []

const filteredUsers = ref([])

watch(username, async (newValue, oldValue) => {
    filteredUsers.value = users.filter(u => u.fullName.includes(newValue))
})

function choose(user){
    username.value = user.fullName;
    emit("choose", user.id)
    isListShown.value = 0
}

onMounted(async () => {
    users = await getUsers()
})
</script>

<template>
    <nav class="panel" style="width: 100%">
        <div class="panel-block">
            <p class="control">
                <input class="input" type="text" placeholder="Преподаватель" v-model="username" @input="isListShown = 1"/>
            </p>
        </div>
        <div style="max-height: 10em; overflow: hidden scroll;">
            <a class="panel-block" v-if="isListShown == 1" v-for="user in filteredUsers" @click="choose(user)">
                {{user.fullName}}
            </a>
        </div>
    </nav>
</template>