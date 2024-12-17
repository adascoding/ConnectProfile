import { login } from '../services/authApiService.js';
import { saveSession } from '../services/sessionService.js';

const loginForm = document.getElementById('loginForm');
const errorMessageDiv = document.getElementById('error-message');
const usernameField = document.getElementById('username');
const passwordField = document.getElementById('password');

const hideErrorMessage = () => {
    errorMessageDiv.style.display = 'none';
}

const init = () => {
    usernameField.addEventListener('focus', hideErrorMessage);
    passwordField.addEventListener('focus', hideErrorMessage);

    loginForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const username = usernameField.value;
        const password = passwordField.value;

        const response = await login({ username, password });

        if (response.success) {
            const { token, userName, userId, role } = response.data;
            saveSession(token, userName, userId);
            if (role === 'Admin') {
                window.location.href = './deleteAccount.html';
            } else {
                window.location.href = './index.html';
            }
        } else {
            errorMessageDiv.style.display = 'block';
            errorMessageDiv.textContent = response.message;
        }
    });
}

init();
