const API_BASE_URL = 'https://localhost:7187/api/UserInfo';

async function getUserInfo() {
    const accountId = sessionStorage.getItem('user_id');
    if (!accountId) {
        throw new Error('User ID not found in session storage.');
    }

    const token = sessionStorage.getItem('jwt_token');
    if (!token) {
        throw new Error('JWT token not found.');
    }

    const response = await fetch(`${API_BASE_URL}/${accountId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${token}`,
        },
    });

    if (response.ok) {
        return response.json();
    } else if (response.status === 404) {
        return null;
    } else {
        throw new Error('Error fetching user info');
    }
}

async function saveUserInfo(userInfo) {
    const accountId = sessionStorage.getItem('user_id');
    if (!accountId) {
        throw new Error('User ID not found in session storage.');
    }

    userInfo.AccountId = accountId;

    const token = sessionStorage.getItem('jwt_token');
    if (!token) {
        throw new Error('JWT token not found.');
    }

    const response = await fetch(API_BASE_URL, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(userInfo),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Error saving user info');
    }

    return response.json();
}

async function updateField(accountId, fieldPath, value) {
    const token = sessionStorage.getItem('jwt_token');
    if (!token) {
        throw new Error('JWT token not found.');
    }

    const response = await fetch(`${API_BASE_URL}/${accountId}/${fieldPath}`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(value),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || `Error updating field ${fieldPath}`);
    }
}

export { getUserInfo, saveUserInfo, updateField };