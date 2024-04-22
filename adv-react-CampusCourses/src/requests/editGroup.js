import routes from './routes.json'
import router from '../router'

export async function editGroup(id, name){
    const token = localStorage.getItem('token');

    try {
        console.log(routes.host + routes.courses + id)
        const response = await fetch(routes.host + routes.courses + id, {
            method: 'PUT',
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
            console.error('Ошибка редактирования:', response.status);
            return response.status;
        }

        const data = await response.json();
        return data;
    
    } catch (error) {
        console.error('Ошибка редактирования:', error);
    }
}