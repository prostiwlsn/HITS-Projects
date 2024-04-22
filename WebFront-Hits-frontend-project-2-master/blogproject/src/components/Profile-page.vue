<template>
    <div class="container-lg">
        <div class="row justify-content-center">
            <div class="col-lg-4 col-md-6 col-sm-12">
                <div class="card custom-card">
                    <div class="card-title"><h1 class="display-4">Профиль</h1></div>
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
                                <option value="Female">Женщина</option>
                                <option value="Male">Мужчина</option>
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
                                <span class="input-group-text" id="inputGroup-sizing-default">Email</span>
                            </div>
                            <input type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="email">
                        </div>

                        <div class="input-group mb-3">
                            <div class="input-group-prepend">
                                <span class="input-group-text" id="inputGroup-sizing-default">Телефон</span>
                            </div>
                            <input type="text" class="form-control" aria-label="Default" aria-describedby="inputGroup-sizing-default" v-model="phoneNumber" @input="formatPhoneNumber">
                        </div>

                        <button type="button" class="btn btn-primary" @click="editProfile()">Изменить профиль</button>
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
  name: 'Profile-page',
  data() {
    return {
      email: '',
      fullName: '',
      birthDate: new Date().toISOString().substr(0, 10),
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
  mounted(){
    this.fetchData();
  },
  methods:{
    async fetchData(){
        const token = localStorage.getItem('token');

        try {
        const response = await fetch('https://blog.kreosoft.space/api/account/profile', {
          method: 'GET',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token 
          },
        });

        if(response.status == 401){
            console.log(123);
            this.$router.push('/login');
        }
        else if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        const data = await response.json();
        console.log(data);

        this.fullName = data.fullName;
        this.birthDate = new Date(data.birthDate).toISOString().substr(0, 10);
        this.email = data.email;
        this.gender = data.gender;
        this.phoneNumber = data.phoneNumber;
        
      } catch (error) {
        console.error('Ошибка входа:', error);
      }
    },
    async editProfile(){
      const token = localStorage.getItem('token');

      try {
        const response = await fetch('https://blog.kreosoft.space/api/account/profile', {
          method: 'PUT',
          headers: {
            'Content-Type': 'application/json',
            'Authorization': 'Bearer ' + token
          },
          body: JSON.stringify({
            email: this.email,
            fullName: this.fullName,
            birthDate: this.birthDate,
            gender: this.gender,
            phoneNumber: this.phoneNumber
          }),
        });

        if (!response.ok) {
          this.isAlerted = true;
          throw new Error('Ошибка HTTP: ' + response.status);
        }

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
  }
}
</script>

<style>
</style>