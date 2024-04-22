<template>
    <div class="container">
        <div class="card mb-3">
            <img class="card-img-bottom" v-if="post.image" :src="post.image">
            <div class="card-header text-start">
                <h4 class=" d-inline">{{ post.author + " "}}</h4>
                <h4 class="text-muted d-inline" v-if="post.communityId">{{ "в " + post.communityName}}</h4>
            </div>
            <div class="card-body text-start">
                <h5 class="card-title text-start"><router-link :to="this.linkURL">{{ post.title }}</router-link></h5>
                <h6 class="card-subtitle mb-2 text-muted text-start">Создан: {{ new Date(post.createTime) }}</h6>
                <h6 class="card-subtitle mb-2 text-muted text-start">Время чтения: {{ post.readingTime }} минут</h6>
                <div class="overflow-hidden" style="max-height: 10vh;" ref="postDescription" v-if="isCollapsed"><p class="card-text text-start">{{ post.description }}</p></div>
                <p class="card-text text-start" v-if="!isCollapsed">{{ post.description }}<br/></p>
                <a href="#" @click="collapse" v-if="isCollapsible">{{collapseText}}<br/></a>
                <div class="d-inline text-muted" v-for="tag in post.tags" :key="tag.id">#{{ tag.name }} </div>
                <div v-if="post.addressId"><div class="d-inline text-muted" v-for="address in addressElements" :key="address">{{ address.text + " " }}</div></div>
            </div>
            <div class="card-footer">
                <div class="container">
                    <div class="row">
                        <div class="col-6" v-if="!store.isAuthorized"><a>{{ likes }}</a><font-awesome-icon :icon="['fas', 'heart']" /></div>
                        <div class="col-6" v-else-if="store.isAuthorized && !hasLike"><a>{{ likes }} </a><font-awesome-icon :icon="['far', 'heart']" @click="like()"/></div>
                        <div class="col-6" v-else><a>{{ likes }} </a><font-awesome-icon :icon="['fas', 'heart']" style="color: #ff0006;" @click="deleteLike()"/></div>
                        <div class="col-6"><a>{{ post.commentsCount }} </a><font-awesome-icon :icon="['fas', 'comment']" /></div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
    import { store } from '@/store/store.js';

    export default {
        name: 'UserPost',
        props: {
            post: {
                type: Object,
                required: true
            }
        },
        setup(){
            return{
                store
            }
        },
        data() {
            return {
                isCollapsed: true,
                isCollapsible: false,
                collapseText: 'Читать далее...',
                linkURL: '/post/',
                hasLike: false,
                likes: 0,
                addressElements: []
            };
        },
        mounted(){
            const element = this.$refs.postDescription;
            if(element.scrollHeight > element.clientHeight){
                this.isCollapsible = true;
            }
            else{
                this.isCollapsed = false;
            }

            this.linkURL += this.post.id;

            this.hasLike = this.post.hasLike;
            this.likes = this.post.likes;
            if(this.post.addressId){
                this.getAddress();
            }
        },
        methods:{
            collapse(){
                this.isCollapsed = !this.isCollapsed;
                this.collapseText = this.isCollapsed ? 'Читать далее...' : 'Свернуть';
            },
            toPost(){
                this.$router.push(this.linkURL);
            },
            async like(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/post/' + this.post.id + '/like', {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token 
                    },
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.hasLike = true;
                    this.likes++
                } catch (error) {
                    console.error('Ошибка HTTP:', error);
                }
            },
            async deleteLike(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/post/' + this.post.id + '/like', {
                    method: 'DELETE',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token 
                    },
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.hasLike = false;
                    this.likes--
                } catch (error) {
                    console.error('Ошибка HTTP:', error);
                }
            },
            async getAddress(){
                try {
                    const response = await fetch('https://blog.kreosoft.space/api/address/chain?objectGuid=' + this.post.addressId, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    });

                    if (!response.ok) {
                    throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    const data = await response.json();

                    this.addressElements = data;
                    console.log(data);
                } catch (error) {
                    console.error('Ошибка HTTP:', error);
                }
            }
        }
    }
</script>
  
<style>
</style>