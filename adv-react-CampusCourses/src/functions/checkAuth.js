import { state } from '../state.js';
import { parseJwt } from '../functions/parseJWT.js'

export async function checkAuth(){
    const token = localStorage.getItem('token');

    if(!token){
        state.unAuthorize();
    }
    else{
        const tokenData = parseJwt(token);
        const expired = new Date(tokenData.exp * 1000);

        const newDate = new Date();

        if(expired < newDate){
            localStorage.removeItem("token");
            state.unAuthorize();
        }
        else{
            state.authorize(localStorage.getItem('email'));
        }
    }
}