import routes from './routes.json'
import router from '../router'

export async function createNotification(id, message, isImportant){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.crudCourse + id + "/notifications", {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                text: message,
                isImportant: isImportant
            })
        });

        if(response.status == 401){
            router.push('/login');
        }
        
        if (!response.ok) {
            console.error('Ошибка создания:', response.status);
            return response.status;
        }

        const data = await response.json();
        return data;
    
    } catch (error) {
        console.error('Ошибка редактирования:', error);
    }
}