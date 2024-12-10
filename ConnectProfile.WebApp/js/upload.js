import { uploadProfileImage } from '../services/imageService.js';
import { getUserId } from '../services/sessionService.js';

const handleFileUpload = async (event) => {
    event.preventDefault();

    const fileInput = document.getElementById('image-file');
    const file = fileInput.files[0];

    if (!file) {
        alert('Please select an image to upload.');
        return;
    }

    const formData = new FormData();
    formData.append('file', file);
    formData.append('accountId', getUserId());

    try {
        await uploadProfileImage(formData);
        alert('Profile image uploaded successfully');
        window.location.href = 'main.html';
    } catch (error) {
        alert(`Upload failed: ${error.message}`);
    }
};

const init = () => {
    document.getElementById('upload-form').addEventListener('submit', handleFileUpload);
};

init();
