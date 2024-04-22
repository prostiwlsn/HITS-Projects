import codes from "./validationCodes.json"

export function validateCourse(name, year, students, semester, id, reqs, anns){
    const numberRegex = /^\d+$/

    if (name.length == 0){
        return codes.filter(c => c.code == 410)[0].message;
    }
    else if (numberRegex.test(year) != true){
        return codes.filter(c => c.code == 411)[0].message;
    }
    else if (Number(students) > 200){
        return codes.filter(c => c.code == 412)[0].message;
    }
    else if (id.length == 0){
        return codes.filter(c => c.code == 413)[0].message;
    }
    else if (reqs.length == 0 || anns.length == 0){
        return "Требования и аннотация должны быть заполнены"
    }
    else{
        return codes.filter(c => c.code == 400)[0].message;
    }
}