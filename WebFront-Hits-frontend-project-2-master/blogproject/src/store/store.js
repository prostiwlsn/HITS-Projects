import { reactive } from 'vue'

export const store = reactive({
  isAuthorized: false,
  email: 'email@email.com',
  authorize(email){
    this.saveUserInfo();

    this.isAuthorized = true;
    this.email=email;
  },
  unAuthorize(){
    this.isAuthorized = false;
  },
  async saveUserInfo(){
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

      localStorage.setItem('id', data.id);
      localStorage.setItem('email', data.email);
    } catch (error) {
      console.error('Ошибка входа:', error);
    }
  },
})