import routes from './routes.json'
import router from '../router'

export async function deleteCourse(id){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.crudCourse + id, {
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