<template>
    <div class="container text-start mb-2" v-if="!deleted">
        <div class="card">
            <div class="card-header">
                <div class="container">
                    <div class="row justify-content-between">
                        <div class="col-6 text-start">
                            <div v-for="index in level" :key="index">•</div>
                        </div>
                        <div class="col-6">
                            <div class="row justify-content-end">
                                <div class="col-1" v-if="store.isAuthorized && userId == comment.authorId">
                                    <div class="mr-1"><font-awesome-icon :icon="['fas', 'pen-to-square']" @click="toggleEdit"/></div>
                                </div>
                                <div class="col-1" v-if="store.isAuthorized && userId == comment.authorId && comment.content != ''">
                                    <div class="ml-1"><font-awesome-icon :icon="['fas', 'trash']" @click="deleteComment"/></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <div class="card-body">
                <blockquote class="blockquote mb-0" v-if="!isEditing">
                    <p>{{ comment.content }}</p>
                    <footer class="blockquote-footer">{{ comment.author }}</footer>
                </blockquote>
                <div v-else>
                    <input type="text" class="form-control mb-1" v-model="editContent">
                    <button type="button" class="btn btn-light" @click="editComment">Отправить</button>
                </div>
                <div class="d-inline"><a>{{date + " "}}</a><a href="#" v-if="store.isAuthorized" @click="reply">Ответить</a></div>
                <div v-if="replyOpen">
                    <input type="text" class="form-control mb-1" v-model="content">
                    <button type="button" class="btn btn-light" @click="sendReply">Отправить</button>
                </div>
            </div>
        </div>
        <a v-if="comment.subComments > 0 && !level" href="#" @click="toggleSubcomments()">{{ openCloseString }}</a>
        <ul v-if="!closed" class="list-group">
            <li v-for="comment in subcomments" :key="comment.id" class="list-group-item">
                <UserComment :comment="comment" :level="subLevel" :postId="postId" @reloadComments="getSubcomments"/>
            </li>
        </ul>
    </div>
</template>
  
<script>
    import { store } from '@/store/store.js';

    export default {
        name: 'UserComment',
        props:{
            comment: {
                type: Object,
                required: true
            },
            level: {
                type: Number,
                required: false
            },
            postId:{
                type: String,
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
                subcomments: [],
                subLevel: 0,
                openCloseString: 'Раскрыть ответы',
                closed: true,
                userId: '',
                date: new Date(),
                replyOpen: false,
                content: '',
                deleted: false,
                editContent: '',
                isEditing: false
            }
        },
        mounted(){
            this.getSubcomments();

            if(this.level){
                this.subLevel = this.level+1;
            }
            else{
                this.subLevel = 1;
            }

            this.userId = localStorage.getItem('id');

            this.editContent = this.comment.content;

            this.date = new Date(this.comment.createTime);
            this.date = this.comment.modifiedDate ? 'Изменено ' + new Date(this.comment.modifiedDate) : this.date;
            this.date = this.comment.deleteDate ? 'Удалено ' + new Date(this.comment.deleteDate) : this.date;
        },
        methods:{
            async getSubcomments(){
                try {
                    const response = await fetch('https://blog.kreosoft.space/api/comment/'+ this.comment.id +'/tree', {
                        method: 'GET',
                        headers: {
                            'Content-Type': 'application/json',
                        },
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    const data = await response.json();

                    this.subcomments = data;
                
                } catch (error) {
                    console.error('Ошибка HTTP:', error);
                }
            },
            toggleSubcomments(){
                this.closed = !this.closed;
                this.openCloseString = this.closed ? 'Раскрыть ответы' : 'Скрыть ответы';
            },
            reply(){
                this.replyOpen = !this.replyOpen;
            },
            async sendReply(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/post/'+ this.postId +'/comment', {
                        method: 'POST',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token 
                        },
                        body: JSON.stringify({
                            content: this.content,
                            parentId: this.comment.id,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.content = '';
                    this.getSubcomments();
                    this.$emit('reloadComments')
                } catch (error) {
                    console.error('Ошибка входа:', error);
                }
            },
            async editComment(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/comment/'+ this.comment.id, {
                        method: 'PUT',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token 
                        },
                        body: JSON.stringify({
                            content: this.editContent,
                        }),
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.isEditing = false;
                    this.$emit('reloadComments')
                } catch (error) {
                    console.error('Ошибка входа:', error);
                }
            },
            async deleteComment(){
                try {
                    const token = localStorage.getItem('token');

                    const response = await fetch('https://blog.kreosoft.space/api/comment/'+ this.comment.id, {
                        method: 'DELETE',
                        headers: {
                            'Content-Type': 'application/json',
                            'Authorization': 'Bearer ' + token 
                        },
                    });

                    if (!response.ok) {
                        throw new Error('Ошибка HTTP: ' + response.status);
                    }

                    this.deleted = this.comment.subComments > 0 && !this.level ? false : true;
                    this.$emit('reloadComments')
                } catch (error) {
                    console.error('Ошибка входа:', error);
                }
            },
            toggleEdit(){
                this.isEditing = !this.isEditing;
            }
        }
    }
</script>
