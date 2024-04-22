<script setup>
import { ref, onMounted, computed, watch } from 'vue'
import { deleteCourse } from "../requests/deleteCourse.js"
const emit = defineEmits(['closeDelete', 'delete'])

const props = defineProps(['isActive', 'courseObj'])

function deleteCourseOnClick(){
    deleteCourse(props.courseObj.id)
    emit('delete')
    emit('closeDelete')
}
</script>

<template>
    <div class="modal" :class="isActive ? 'is-active' : ''">
        <div class="modal-background"></div>
        <div class="modal-content">
            <div class="column card is-two-thirds-widescreen is-flex is-justify-content-center is-full-mobile">
                <div class="is-flex is-flex-direction-column is-align-items-center"> 
                    <div class="title is-1">Вы уверены?</div>
                    <div class="is-flex is-is-flex-direction-row">
                        <a class="button is-danger mr-2" @click="deleteCourseOnClick()">
                            <strong>Да</strong>
                        </a>
                        <a class="button ml-2" @click="$emit('closeDelete')">
                            <strong >Нет</strong>
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>