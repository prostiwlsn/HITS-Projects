import routes from './routes.json'
import router from '../router'

export async function getGroups(){
    const token = localStorage.getItem('token');

    try {
    const response = await fetch(routes.host + routes.groups, {
      method: 'GET',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token 
      },
    });

    if(response.status == 401){
      router.push('/login');
    }
    
    if (!response.ok) {
      throw new Error('Ошибка HTTP: ' + response.status);
    }

    const data = await response.json();
    return data;
    
  } catch (error) {
    console.error('Ошибка входа:', error);
  }
}