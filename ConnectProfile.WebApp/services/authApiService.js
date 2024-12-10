import { saveSession } from './sessionService.js';

const API_BASE_URL = 'https://localhost:7187/api/account';

async function login(loginData) {
    const response = await fetch(`${API_BASE_URL}/login`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(loginData),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Login failed');
    }

    return response.json();
}

async function register(registerData) {
    const response = await fetch(`${API_BASE_URL}/register`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify(registerData),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Registration failed');
    }

    return response.json();
}

async function handleLogin(username, password) {
    try {
        const response = await login({ username, password });
        console.log(response);
        saveSession(response.token, response.userName, response.userId);
        alert(`Welcome, ${response.userName}!`);
    } catch (error) {
        alert(`Login failed: ${error.message}`);
    }
}

async function handleRegister(username, email, password) {
    try {
        await register({ username, email, password });
        alert('Registration successful! Please log in.');
        window.location.href = '../pages/login.html';
    } catch (error) {
        alert(`Registration failed: ${error.message}`);
    }
}

export { login, register, handleLogin, handleRegister };
