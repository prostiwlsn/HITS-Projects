<template>
  <div class="container-xxl fixed-top">
    <nav class="navbar sticky-top navbar-expand-md">
      <div class="container">
        <a class="navbar-brand" href="#">MemBlog</a>
        <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav" aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
          <span class="navbar-toggler-icon"></span>
        </button>
        <div class="collapse navbar-collapse" id="navbarNav">
          <div class="container">
            <div class="row justify-content-between">
              <div class="col-6">
                <ul class="navbar-nav">
                  <li class="nav-item">
                    <router-link to="/"><a class="nav-link" href="#">Главная</a></router-link>
                  </li>
                  <li class="nav-item">
                    <router-link to="/communities"><a class="nav-link" href="#">Группы</a></router-link>
                  </li>
                  <li class="nav-item">
                    <router-link to="/authors"><a class="nav-link" href="#">Авторы</a></router-link>
                  </li>
                </ul>
              </div>
              <div class="col-6">
                <div class="dropdown btn-group" v-if="store.isAuthorized">
                  <a class="nav-link dropdown-toggle" href="#" id="navbarDropdownMenuLink" role="button" data-bs-toggle="dropdown">
                    {{store.email}}
                  </a>
                  <ul class="dropdown-menu" aria-labelledby="navbarDropdownMenuLink" >
                    <li><a class="dropdown-item" href="#"><router-link to="/profile">Профиль</router-link></a></li>
                    <li><a class="dropdown-item" href="#" @click="logout">Выйти</a></li>
                  </ul>
                </div>
                <router-link to="/login" v-else><button type="button" class="btn btn-primary">Войти</button></router-link>
              </div>
            </div>
          </div>
        </div>
      </div>
    </nav>
    <router-view/>
  </div>
</template>

<script>
import { parseJwt } from "@/functions/decodeJWT.js";
import { store } from '@/store/store.js';

export default {
  name: 'App',
  components: { 
  },
  setup(){
    return{
      store
    }
  },
  data() {
    return {
      isAuth: true,
      email: 'email@email.com'
    }
  },
  mounted(){
    const token = localStorage.getItem('token');

    if(!token){
      this.store.unAuthorize();
    }
    else{
      const tokenData = parseJwt(token);
      const expired = new Date(tokenData.exp * 1000);

      const newDate = new Date();

      if(expired < newDate){
        localStorage.removeItem("token");
        this.store.unAuthorize();
      }
      else{
        this.store.authorize();
      }
    }

    if(this.store.isAuthorized){
      this.getUserInfo();
    }
  },
  computed: {
  },
  methods:{
    toLogin(){
      this.$router.push('/login');
    },
    async getUserInfo(){
      const token = localStorage.getItem('token');

      try {
        const response = await fetch('https://blog.kreosoft.space/api/account/profile', {
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
        console.log(data);

        this.email = data.email;

        this.store.authorize(this.email);
      } catch (error) {
        console.error('Ошибка входа:', error);
      }
    },
    logout(){
      localStorage.removeItem("token");
      this.isAuth = false;
      this.store.unAuthorize();
    }
  }
}
</script>

<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
  margin-top: 60px;
}
</style>
