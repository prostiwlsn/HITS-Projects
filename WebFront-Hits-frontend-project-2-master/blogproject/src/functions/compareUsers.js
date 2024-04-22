export function compareUsers(a, b) {
    const sumA = a.likes + a.posts;
    const sumB = b.likes + b.posts;

    if (sumA < sumB) {
        return 1; 
    } else if (sumA > sumB) {
        return -1; 
    } else {
        return 0;
    }
}