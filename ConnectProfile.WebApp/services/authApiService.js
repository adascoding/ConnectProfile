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
        return {
            success: false,
            message: error.message || 'Login failed'
        };
    }

    const result = await response.json();
    return {
        success: true,
        message: 'Login successful!',
        data: result.data
    };
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
        return {
            success: false,
            message: error.message || 'Registration failed'
        };
    }

    const result = await response.json();
    return {
        success: true,
        message: 'Registration successful!',
        data: result.data
    };
}

export { login, register };
