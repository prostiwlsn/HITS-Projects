import routes from './routes.json'
import router from '../router'

export async function deleteGroup(id){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.courses + id, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            }
        });

        if(response.status == 401){
            router.push('/login');
        }
        
        if (!response.ok) {
            console.error('Ошибка удаления:', response.status);
            return response.status;
        }

        const data = await response.json();
        return data;
    
    } catch (error) {
        console.error('Ошибка редактирования:', error);
    }
}