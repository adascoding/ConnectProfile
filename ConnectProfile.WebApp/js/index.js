import { isLoggedIn } from '../services/sessionService.js';

const handleAuthButtons = () => {
    if (isLoggedIn()) {
        document.getElementById('authButtons').style.display = 'none';
        document.getElementById('mainButtonDiv').style.display = 'block';

        document.getElementById('mainButton').addEventListener('click', () => {
            window.location.href = './main.html';
        });
    } else {
        document.getElementById('loginButton').addEventListener('click', () => {
            window.location.href = './login.html';
        });

        document.getElementById('registerButton').addEventListener('click', () => {
            window.location.href = './register.html';
        });
    }
};

const init = () => {
    handleAuthButtons();
};

init();
