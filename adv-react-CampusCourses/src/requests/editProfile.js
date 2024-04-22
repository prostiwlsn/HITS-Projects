import routes from './routes.json'
import router from '../router'

export async function editProfile(name, birthDate){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.profile, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                fullName: name,
                birthDate: birthDate
            })
        });
        
        if(response.status == 401){
            router.push('/login');
        }
        
        if (!response.ok) {
            console.error('Ошибка регистрации:', response.status);
            return response.status;
        }

        const data = await response.json();
        return data;
    
    } catch (error) {
        console.error('Ошибка редактирования:', error);
    }
}