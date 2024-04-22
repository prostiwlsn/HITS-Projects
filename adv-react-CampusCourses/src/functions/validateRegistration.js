import codes from './validationCodes.json'

export function validateRegistration(name, birthDate, email, password, confirmPassword){
    const today = new Date()
    const emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/
    const numberRegex = /\d/
    const letterRegex = /[a-zA-Z]/

    if (name == ""){
        return codes.filter(c => c.code == 401)[0];
    }
    else if (birthDate > today.toISOString().slice(0, 10)){
        return codes.filter(c => c.code == 402)[0];
    }
    else if (emailRegex.test(email)!=true){
        return codes.filter(c => c.code == 403)[0];
    }
    else if (password != confirmPassword){
        return codes.filter(c => c.code == 404)[0];
    }
    else if (password.length < 6){
        return codes.filter(c => c.code == 405)[0];
    }
    else if (numberRegex.test(password)!=true){
        return codes.filter(c => c.code == 406)[0];
    }
    else if (letterRegex.test(password)!=true){
        return codes.filter(c => c.code == 407)[0];
    }
    else{
        return true
    }
}