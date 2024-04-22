import routes from './routes.json'

export async function register(name, birthDate, email, password, confirmPassword){
    try {
        const response = await fetch(routes.host + routes.registration, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            fullName: name,
            birthDate: birthDate,
            email: email,
            password: password,
            confirmPassword: confirmPassword
          }),
        });

        if (!response.ok) {
          console.error('Ошибка регистрации:', response.status);
          return response.status;
        }

        const data = await response.json();

        const token = data.token;

        console.log(token)
        return token;
    }catch (error) {
        console.error('Ошибка регистрации:', error);
        return response.status;
    }

}