import routes from './routes.json'

export async function signup(id){
    const token = localStorage.getItem('token');

    try {
    const response = await fetch(routes.host + routes.crudCourse + id + "/sign-up", {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token 
      },
    });
    
    if (!response.ok) {
        console.error('Ошибка записи:', response.status);
        return false;
    }

    return true;
    
  } catch (error) {
    console.error('Ошибка записи:', error);
  }
}