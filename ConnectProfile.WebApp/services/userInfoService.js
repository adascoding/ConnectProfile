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
        const result = await response.json();

        if (!result.success) {
            throw new Error(result.message || 'Error fetching user info');
        }
        return result.data;
    } else if (response.status === 404) {
        return null;
    } else {
        const error = await response.json();
        throw new Error(error.message || 'Error fetching user info');
    }
}

async function saveUserInfo(userInfo) {
    const token = sessionStorage.getItem('jwt_token');
    if (!token) {
        throw new Error('JWT token not found.');
    }

    const accountId = sessionStorage.getItem('user_id');
    if (!accountId) {
        throw new Error('User ID not found in session storage.');
    }

    userInfo.accountId = accountId;

    const response = await fetch(`${API_BASE_URL}/${accountId}`, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify(userInfo),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Failed to save user information.');
    }

    const result = await response.json();

    if (!result.success) {
        throw new Error(result.message || 'Failed to save user information.');
    }

    return result.data;
}

async function updateField(accountId, fieldName, fieldValue) {
    const token = sessionStorage.getItem('jwt_token');
    if (!token) {
        throw new Error('JWT token not found.');
    }

    const response = await fetch(`${API_BASE_URL}/${accountId}/UpdateField`, {
        method: 'PATCH',
        headers: {
            'Content-Type': 'application/json',
            'Authorization': `Bearer ${token}`,
        },
        body: JSON.stringify({ fieldName, fieldValue }),
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || `Error updating field ${fieldName}`);
    }

    const result = await response.json();

    if (!result.success) {
        throw new Error(result.message || `Error updating field ${fieldName}`);
    }

    return result;
}


export { getUserInfo, updateField, saveUserInfo };
