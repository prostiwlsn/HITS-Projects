import routes from './routes.json'

export async function login(email, password){
    try {
        const response = await fetch(routes.host + routes.login, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
          },
          body: JSON.stringify({
            email: email,
            password: password,
          }),
        });

        if (!response.ok) {
          throw new Error('Ошибка HTTP: ' + response.status);
        }

        const data = await response.json();

        const token = data.token;

        console.log(token)
        return token;
    }catch (error) {
        console.error('Ошибка входа:', error);
        return false;
    }

}

/*
        localStorage.setItem('token', token);

        this.$router.push('/');

        console.log(token);
        this.store.authorize(this.email);
*/