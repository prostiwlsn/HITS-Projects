<template>
    <div class="col-lg-9 col-md-12 col-sm-12">
        <div class="card custom-card">
            <div class="card-title"><h1 class="display-4">Пост</h1></div>
            <div class="card-body">
                <div class="container text-start">
                    <div class="row mb-2">
                        <div class="col-9">
                            <label for="name" class="form-label">Название</label>
                            <div class="input-group">
                                <input type="text" class="form-control" id="name" v-model="title">
                            </div>
                        </div>
                        <div class="col-3">
                            <label for="time" class="form-label">Время</label>
                            <div class="input-group">
                                <input type="number" class="form-control" id="time" v-model="readingTime">
                            </div>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <div class="col-6">
                            <label for="group" class="form-label">Группа</label>
                            <div class="input-group">
                                <select class="form-select" aria-label="Default select example" id="group" v-model="selectedGroup">
                                    <option :value="''">Без группы</option>
                                    <option v-for="group in groups" :key="group.communityId" :value="group.communityId">{{ group.name }}</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-6">
                            <label for="tags" class="form-label">Теги</label>
                            <div class="input-group">
                                <select multiple class="form-select" aria-label="Default select example" id="tags" v-model="selectedTags">
                                    <option v-for="tag in tags" :key="tag.id" :value="tag.id">{{ tag.name }}</option>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="row mb-2">
                        <label for="url" class="form-label">Ссылка на картинку</label>
                        <div class="input-group">
                            <input type="url" class="form-control" id="url" v-model="image">
                        </div>
                    </div>
                    <div class="row mb-2">
                        <label for="exampleFormControlTextarea1" class="form-label">Текст поста</label>
                        <textarea class="form-control" id="exampleFormControlTextarea1" rows="3" v-model="description"></textarea>
                    </div>
                    <div class="text-start">
                        <div class="mb-2">Адрес</div>
                        <div class="overflow-auto" style="max-height: 20vh;">
                            <Address :parentId="0"/>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-footer"><button type="button" class="btn btn-primary" @click="createPost">Создать пост</button></div>
        </div>
    </div>
</template>
  
<script>
import Address from "../components/AddressElement.vue"
import { store } from '@/store/address_store.js';

export default {
    name: 'PostForm',
    components:{
        Address
    },
    props: {
        group: {
            type: String,
            required: false
        }
    },
    setup(){
        return{
            store
        }
    },
    data() {
        return {
            selectedGroup: '',
            title: '',
            description: '',
            readingTime: 0,
            tags: [],
            selectedTags: [],
            image: '',
            address: '',
            groups: ''
        };
    },
    mounted(){
        this.getTags();
        this.getGroups();
    },
    methods:{
        async getTags(){
            try {
                const response = await fetch('https://blog.kreosoft.space/api/tag', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                const data = await response.json();

                this.tags = data;  
            } catch (error) {
                console.error('Ошибка HTTP:', error);
            }
        },
        async getGroups(){
            const token = localStorage.getItem('token');

            try {
                const response = await fetch('https://blog.kreosoft.space/api/community/my', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token 
                    },
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                const data = await response.json();

                this.groups = data.filter(item => item.role === 'Administrator');  

                this.groups.forEach(element => {
                    fetch('https://blog.kreosoft.space/api/community/'+element.communityId)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error('Ошибка HTTP: ' + response.status);
                        }
                        return response.json();
                    })
                    .then(data => {
                        element.name=data.name;
                    })
                });
            } catch (error) {
                console.error('Ошибка HTTP:', error);
            }
        },
        async createPost(){
            const url = this.selectedGroup == '' ? 'https://blog.kreosoft.space/api/post' : 'https://blog.kreosoft.space/api/community/'+ this.selectedGroup +'/post';
            const token = localStorage.getItem('token');

            try {
                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token 
                    },
                    body: JSON.stringify({
                        title: this.title,
                        description: this.description,
                        readingTime: this.readingTime,
                        image: this.image == '' ? null : this.image,
                        addressId: this.store.addresGuid == '' ? null : this.store.addresGuid,
                        tags: this.selectedTags
                    }),
                });

                if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                }

                const data = await response.json();

                this.$router.push('/post/'+data);
            } catch (error) {
                console.error('Ошибка:', error);
            }
        },
    }
}
</script>