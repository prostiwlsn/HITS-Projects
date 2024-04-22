import routes from './routes.json'
import router from '../router'

export async function createCourse(id, name, startYear, maximumStudentsCount, semester, requirements, annotations, mainTeacherId){
    const token = localStorage.getItem('token');

    try {
        const response = await fetch(routes.host + routes.groups + '/' + id, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + token 
            },
            body: JSON.stringify({
                name: name,
                startYear: startYear,
                maximumStudentsCount: maximumStudentsCount, 
                semester: semester, 
                requirements: requirements, 
                annotations:annotations, 
                mainTeacherId: mainTeacherId
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