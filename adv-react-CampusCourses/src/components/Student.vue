<script setup>
import { state } from '../state.js';
import { ref, onMounted, computed } from 'vue'
import { editUserStatus } from "../requests/editUserStatus.js"
import Mark from "./Mark.vue"

const props = defineProps(['studentObj', 'courseObj', 'isStudent'])

const status = ref("")

async function changeStatus(newStatus){
    const result = await editUserStatus(props.courseObj.id, props.studentObj.id, newStatus)
    if(typeof result != "number"){
        status.value = newStatus
    }
}

const isMidtermMarkShown = ref(0)
const isFinalMarkShown = ref(0)

const midtermMark = ref("")
const finalMark = ref("")

async function changeMark(newMark, markType){

}

onMounted(() => {
    status.value = props.studentObj.status

    midtermMark.value = props.studentObj.midtermResult
    finalMark.value = props.studentObj.finalResult
})
</script>

<template>
    <div class="columns v-centered is-mobile">
        <div class="column is-third is-flex is-flex-direction-column">
            <strong>{{ studentObj.name }}</strong>
            <div class="has-text-grey">Статус - {{ status }}</div>
            <div class="has-text-grey">{{ studentObj.email }}</div>
        </div>
        <div class="column is-third">
            <div v-if="status=='Accepted' && !isStudent">
                <a @click="isMidtermMarkShown=1">Промежуточная аттестация - 
                    <div v-if="midtermMark == 'Failed'" style="background-color: red; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">зафейлена</div>
                    <div v-else-if="midtermMark == 'Passed'" style="background-color: #56e387; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">пройдена</div>
                    <div v-else style="background-color: #b3b5b4; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">не выставлена</div>
                </a>
                <Mark :Midterm="'Midterm'" :isActive="isMidtermMarkShown==1" @close="isMidtermMarkShown = 0" :studentId="studentObj.id" :courseObj="courseObj" @editMark="(r) => midtermMark = r"/>
            </div>
            <div v-else-if="isStudent && state.email == studentObj.email">
                <div>
                    Промежуточная аттестация - 
                    <div v-if="midtermMark == 'Failed'" style="background-color: red; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">зафейлена</div>
                    <div v-else-if="midtermMark == 'Passed'" style="background-color: #56e387; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">пройдена</div>
                    <div v-else style="background-color: #b3b5b4; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">не выставлена</div>
                </div>
            </div>
        </div>
        <div class="column is-third">
            <div v-if="status=='InQueue' && !isStudent" class="is-flex is-flex-direction-row is-justify-content-center">
                <button class="button is-primary mr-2" @click="changeStatus('Accepted')">Принять</button>
                <button class="button is-danger" @click="changeStatus('Declined')">Отклонить</button>
            </div>
            <div v-else-if="status=='Accepted' && !isStudent">
                <a @click="isFinalMarkShown=1">Финальная аттестация - 
                    <div v-if="finalMark == 'Failed'" style="background-color: red; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">зафейлена</div>
                    <div v-else-if="finalMark == 'Passed'" style="background-color: #56e387; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">пройдена</div>
                    <div v-else style="background-color: #b3b5b4; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">не выставлена</div>
                </a>
                <Mark :markType="'Final'" :isActive="isFinalMarkShown==1" @close="isFinalMarkShown = 0" :studentId="studentObj.id" :courseObj="courseObj" @editMark="(r) => finalMark = r"/>
            </div>
            <div v-else-if="isStudent && state.email == studentObj.email">
                <div>
                    Финальная аттестация - 
                    <div v-if="finalMark == 'Failed'" style="background-color: red; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">зафейлена</div>
                    <div v-else-if="finalMark == 'Passed'" style="background-color: #56e387; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">пройдена</div>
                    <div v-else style="background-color: #b3b5b4; display: inline-block; border-radius: 10px;" class="pl-1 pr-1">не выставлена</div>
                </div>
            </div>
        </div>
    </div>
</template>