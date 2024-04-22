import routes from './routes.json'
import router from '../router'

export async function createGroup(name){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.groups, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                name: name,
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