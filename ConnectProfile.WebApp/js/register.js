import { register } from '../services/authApiService.js';
import { saveSession } from '../services/sessionService.js';

const registerForm = document.getElementById('registerForm');
const errorMessageDiv = document.getElementById('error-message');
const usernameField = document.getElementById('username');
const emailField = document.getElementById('email');
const passwordField = document.getElementById('password');
const confirmPasswordField = document.getElementById('confirmPassword');

const hideErrorMessage = () => {
    errorMessageDiv.style.display = 'none';
}

const init = () => {
    usernameField.addEventListener('focus', hideErrorMessage);
    emailField.addEventListener('focus', hideErrorMessage);
    passwordField.addEventListener('focus', hideErrorMessage);
    confirmPasswordField.addEventListener('focus', hideErrorMessage);

    registerForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        const username = usernameField.value;
        const email = emailField.value;
        const password = passwordField.value;
        const confirmPassword = confirmPasswordField.value;

        if (password !== confirmPassword) {
            errorMessageDiv.style.display = 'block';
            errorMessageDiv.textContent = 'Passwords do not match!';
            return;
        }

        const response = await register({ username, email, password });

        if (response.success) {
            const { token, userName, userId } = response.data;
            saveSession(token, userName, userId);
            errorMessageDiv.style.display = 'none';
            window.location.href = './login.html';
        } else {
            errorMessageDiv.style.display = 'block';
            errorMessageDiv.textContent = response.message;
        }
    });
}

init();
