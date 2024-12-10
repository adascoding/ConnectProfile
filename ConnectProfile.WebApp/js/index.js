import { isLoggedIn } from '../services/sessionService.js';

const handleAuthButtons = () => {
    if (isLoggedIn()) {
        document.getElementById('authButtons').style.display = 'none';
        document.getElementById('mainButtonDiv').style.display = 'block';

        document.getElementById('mainButton').addEventListener('click', () => {
            window.location.href = 'pages/main.html';
        });
    } else {
        document.getElementById('loginButton').addEventListener('click', () => {
            window.location.href = 'pages/login.html';
        });

        document.getElementById('registerButton').addEventListener('click', () => {
            window.location.href = 'pages/register.html';
        });
    }
};

const init = () => {
    handleAuthButtons();
};

init();
