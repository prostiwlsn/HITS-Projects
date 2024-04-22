import routes from './routes.json'
import router from '../router'

export async function editUserMark(id, studentId, mark, markType){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.crudCourse + id + "/marks/" + studentId, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                markType: markType,
                mark: mark
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