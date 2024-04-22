<template>
    <div class="overflow-auto" style="max-height: 92vh;">
        <div class="container">
            <UserPost :post="post" class="mb-2" v-if="isRecieved"/> 
            <ul class="list-group mb-2">
                <div class="col-lg-6 col-md-12">
                    <li v-for="comment in comments" :key="comment.id" class="list-group-item">
                        <UserComment :comment="comment" :postId="post.id" v-if="(comment.subComments > 0 || comment.content != '')" @reloadComments="getPost"/>
                    </li>
                </div>
            </ul>
            <div v-if="store.isAuthorized">
                <div class="row justify-content-start">
                    <div class="col-lg-6 col-md-12">
                        <div class="input-group mb-2">
                            <span class="input-group-text">Комментарий</span>
                            <textarea class="form-control" aria-label="With textarea" v-model="comment"></textarea>
                        </div>
                    </div>
                </div>
                <div class="d-flex justify-content-start"><button type="button" class="btn btn-primary" @click="createComment">Отправить</button></div>
            </div>
        </div>
    </div>
</template>

<script>
    import UserPost from './UserPost.vue'
    import UserComment from './UserComment.vue'
    import { store } from '@/store/store.js';

    export default {
        name: 'PostPage',
        components: {
            UserPost,
            UserComment
        },
        setup(){
            return{
                store
            }
        },
        data() {
            return {
                id: this.$route.params.id,
                post: {},
                comments: [],
                comment: '',
                isRecieved: false
            };
        },
        methods:{
            async getPost(){
                const token = localStorage.getItem('token');

                try {
                    const response = await fetch('https://blog.kreosoft.space/api/post/' + this.id, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': 'Bearer ' + token 
                    },
                    });

                    if (response.status == 401){
                        this.$router.push('/');
                    }
                    else if (response.status == 404){
                        this.$router.push('/404');
                    }
                    else if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }
                    

                    const data = await response.json();
                    console.log(data);

                    this.post = data;

                    this.comments = data.comments;

                    this.isRecieved=true;
                            
                } catch (error) {
                    console.error('Ошибка HTTP:', error);
                }
            },
            async createComment(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/post/'+ this.id +'/comment', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token 
                        },
                        body: JSON.stringify({
                            content: this.comment,
                            parentId: null,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.comment = '';
                    this.getPost();
                } catch (error) {
                    console.error('Ошибка входа:', error);
                }
            }
        },
        mounted(){
            this.getPost();
        }
    }
</script>