const TOKEN_KEY = 'jwt_token';
const USERNAME_KEY = 'username';
const USERID_KEY = 'user_id';

function saveSession(token, username, userId) {
    sessionStorage.setItem(TOKEN_KEY, token);
    sessionStorage.setItem(USERNAME_KEY, username);
    sessionStorage.setItem(USERID_KEY, userId);
}

function getToken() {
    return sessionStorage.getItem(TOKEN_KEY);
}

function getUsername() {
    return sessionStorage.getItem(USERNAME_KEY);
}

function getUserId() {
    return sessionStorage.getItem(USERID_KEY);
}

function isLoggedIn() {
    return !!getToken();
}

function logout() {
    sessionStorage.removeItem(TOKEN_KEY);
    sessionStorage.removeItem(USERNAME_KEY);
    sessionStorage.removeItem(USERID_KEY);
}

export { saveSession, getToken, getUsername, getUserId, isLoggedIn, logout };
