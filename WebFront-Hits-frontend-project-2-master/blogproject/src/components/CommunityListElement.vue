<template>
    <div class="card">
      <div class="container">   
        <div class="row justify-content-start">
          <div class="col-6 col-sm-12 text-start">
            <h5 class="card-title" href="#"><router-link :to="'/communities/'+this.community.id ">{{ community.name }}</router-link></h5>
          </div>
          <div class="col-1">
            <div v-if="!isAdmin && !community.isClosed && store.isAuthorized">
              <button type="button" class="btn btn-danger" v-if="subscribed" @click="unSubscribe">Отписаться</button>
              <button type="button" class="btn btn-primary" v-else @click="subscribe">Подписаться</button>
            </div>
          </div>
        </div>
        <div class="row text-start">
          <p>{{ community.isClosed ? 'Закрытое сообщество ' : 'Открытое сообщество ' }}</p>
          <p>Подписчиков: {{ community.subscribersCount }}</p>
        </div>
      </div>
  </div>
</template>
  
<script>
import { store } from '@/store/store.js';

  export default {
    name: 'CommunityListElement',
    props: {
      community: {
        type: Object,
        required: true
      },
      communityId:{
        type: String,
        required: false
      }
    },
    data(){
      return {
        subscribed: false,
        isAdmin: false,
      }
    },
    computed:{
    },
    setup(){
      return{
        store
      }
    },
    mounted(){
      console.log(this.community);
      if(this.store.isAuthorized){
        this.getRole();
      }
    },
    methods:{
      async getRole(){
        const token = localStorage.getItem('token');

        try {
        const response = await fetch('https://blog.kreosoft.space/api/community/'+ this.community.id  +'/role', {
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
        if(!data){
          this.subscribed = false;
        }
        else if(data == 'Administrator')
        {
          this.isAdmin = true;
        }
        else{
          this.subscribed = true;
        }

        this.$emit('roleEvent', this.isAdmin);

        } catch (error) {
          console.error('Ошибка:', error);
        }
      },

      async subscribe(){
        const token = localStorage.getItem('token');

        try {
        const response = await fetch('https://blog.kreosoft.space/api/community/'+ this.community.id  +'/subscribe', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token 
          },
        });

        if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        //const data = await response.json();

        this.subscribed = true;

        } catch (error) {
          console.error('Ошибка:', error);
        }
      },

      async unSubscribe(){
        const token = localStorage.getItem('token');

        try {
        const response = await fetch('https://blog.kreosoft.space/api/community/'+ this.community.id  +'/unsubscribe', {
          method: 'DELETE',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token 
          },
        });

        if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        //const data = await response.json();
        
        this.subscribed = false;

        } catch (error) {
          console.error('Ошибка:', error);
        }
      },

      toCommunityPage(){
        this.$router.push('/communities/'+this.community.id );
      }
    }
  }
</script>