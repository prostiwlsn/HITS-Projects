<template>
    <div class="container-lg">
        <div class="row justify-content-center">
            <div class="col-lg-4 col-md-6 col-sm-12">
                <div class="card custom-card">
                    <div class="card-title"><h1 class="display-4">Регистрация</h1></div>
                    <div class="card-body">
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">ФИО</span>
                            </div>
                            <input type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="fullName">
                        </div>
                        
                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <label class="input-group-text" for="inputGroupSelect01">Пол</label>
                            </div>
                            <select class="custom-select form-control" id="inputGroupSelect01" v-model="gender">
                                <option selected>Выберите</option>
                                <option value="Male">Женщина</option>
                                <option value="Female">Мужчина</option>
                            </select>
                        </div>

                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">Дата рождения</span>
                            </div>
                            <input type="date" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" placeholder="дд.мм.гг" v-model="birthDate">
                        </div>

                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">Телефон</span>
                            </div>
                            <input @input="formatPhoneNumber" type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" placeholder="+7 (xxx) xxx-xx-xx" required v-model="phoneNumber">
                        </div>


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

                        <button type="button" class="btn btn-primary" @click="register">Зарегистрироваться</button>
                        <router-link to="/login"><a class="nav-link" href="#">Вход</a></router-link>
                    </div>
                </div>
                <div class="alert alert-warning d-flex align-items-center" role="alert" v-if="isAlerted">
                  <svg class="bi flex-shrink-0 me-2" width="24" height="24" role="img" aria-label="Warning:"><use xlink:href="#exclamation-triangle-fill"/></svg>
                  <div>
                    Неправильные данные для регистрации
                  </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script>
import { store } from '@/store/store.js';

export default {
  name: 'Registration-Form',
  data() {
    return {
      fullname: '',
      password: '',
      email: '',
      birthDate: '',
      gender: '',
      phoneNumber: '',
      isAlerted: false
    };
  },
  setup(){
    return{
      store
    }
  },
  methods: {
    async register() {
      try {
        const response = await fetch('https://blog.kreosoft.space/api/account/register', {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            fullName: this.fullName,
            email: this.email,
            password: this.password,
            birthDate: this.birthDate,
            gender: this.gender,
            phoneNumber: this.phoneNumber,
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
    formatPhoneNumber(event) {
      let input = event.target.value.replace(/\D/g, '');

      if (input.length > 11) {
        input = input.substr(0, 11);
      }

      let formatted = '+7 (';

      if(input.length > 0){
        input = input.slice(1);
      }

      for (let i = 0; i < input.length; i++) {
        if (i === 3) {
          formatted += ') ';
        } else if (i === 6 || i === 8) {
          formatted += '-';
        }
        formatted += input[i];
      }

      this.phoneNumber = formatted;
    },
  },
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