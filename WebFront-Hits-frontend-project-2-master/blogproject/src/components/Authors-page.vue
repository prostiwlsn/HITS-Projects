<template>
    <div class="container">
        <div class="row justify-content-center">
            <div class="col-lg-6 col-md-12">
                <div class="overflow-auto justify-content-center" style="max-height: 94vh;">
                    <ListElement v-for="(item, index) in list" :key="item.id" :author="item" :position="index" class="mb-2"/>
                </div>
            </div>
        </div>
    </div>
  </template>
  
<script>
    import ListElement from './Author-List-Element.vue'
    import { compareUsers } from '@/functions/compareUsers.js'

    export default {
        name: 'Authors-Page',
        data() {
            return {
                list: []
            };
        },
        components: {
            ListElement
        },
        mounted(){
            this.fetchData();
        },
        methods:{
            async fetchData(){
                try {
                    const response = await fetch('https://blog.kreosoft.space/api/author/list', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    const data = await response.json();

                    this.list = data;
                    this.list = this.list.sort(compareUsers);
                } catch (error) {
                    console.error('Ошибка:', error);
                }
            },
        }
    }
</script>