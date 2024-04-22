<template>
    <div class="container-lg">
        <div class="row justify-content-center">
            <div class="col-lg-4 col-md-6 col-sm-12">
                <div class="card custom-card">
                    <div class="card-title"><h1 class="display-4">Вход</h1></div>
                    <div class="card-body">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">Email</span>
                            </div>
                            <input type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="email">
                        </div>

                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">Пароль</span>
                            </div>
                            <input type="password" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="password">
                        </div>

                        <button type="button" class="btn btn-primary" @click="login">Войти</button>
                        <router-link to="/registration"><a class="nav-link" href="#">Регистрация</a></router-link>
                    </div>
                </div>
                <div class="alert alert-warning d-flex align-items-center" role="alert" v-if="isAlerted">
                  <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Warning:"><use xlink:href="#exclamation-triangle-fill"/></svg>
                  <div>
                    Неправильные данные для входа
                  </div>
                </div>
            </div>
        </div>
    </div>
</template>
  
<script>
  import { store } from '@/store/store.js';

  export default {
  data() {
    return {
      email: '',
      password: '',
      isAlerted: false
    };
  },
  setup(){
    return{
      store
    }
  },
  methods: {
    async login() {
      try {
        const response = await fetch('https://blog.kreosoft.space/api/account/login', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            email: this.email,
            password: this.password,
          }),
        });

        if (!response.ok) {
          this.isAlerted = true;
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        const data = await response.json();

        const token = data.token;

        //use vuex
        localStorage.setItem('token', token);

        this.$router.push('/');

        console.log(token);
        this.store.authorize(this.email);
      } catch (error) {
        console.error('Ошибка входа:', error);
      }
    },
  },
};
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