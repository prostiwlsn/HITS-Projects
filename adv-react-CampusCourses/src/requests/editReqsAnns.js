import routes from './routes.json'
import router from '../router'

export async function editReqsAnns(id, requirements, annotations){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.crudCourse + id+"/requirements-and-annotations", {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                annotations: annotations, 
                requirements: requirements
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