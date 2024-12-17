import { getToken } from './sessionService.js';

const API_BASE_URL = 'https://localhost:7187/api/Image';

async function uploadProfileImage(formData) {
    const response = await fetch(`${API_BASE_URL}/upload`, {
        method: 'POST',
        headers: {
            'Authorization': `Bearer ${getToken()}`,
        },
        body: formData,
    });

    if (!response.ok) {
        const error = await response.json();
        throw new Error(error.message || 'Image upload failed');
    }

    return response.json();
}

async function getProfileImage(accountId) {
    const response = await fetch(`${API_BASE_URL}/${accountId}`, {
        method: 'GET',
        headers: {
            'Authorization': `Bearer ${getToken()}`,
        },
    });

    if (!response.ok) {
        if (response.status === 404) {
            return null;
        }
        throw new Error('Error fetching image');
    }

    const imageBlob = await response.blob();
    return imageBlob;
}

export { uploadProfileImage, getProfileImage };
