import { uploadProfileImage } from '../services/imageService.js';
import { getUserId, isLoggedIn } from '../services/sessionService.js';

if (!isLoggedIn()) {
    window.location.href = './index.html';
};

const uploadForm = document.getElementById('upload-form');
const fileInput = document.getElementById('image-file');
const nameInput = document.getElementById('image-name');
const descriptionInput = document.getElementById('image-description');
const errorMessageDiv = document.getElementById('error-message');
const backButton = document.getElementById('backButton');

const hideErrorMessage = () => {
    errorMessageDiv.style.display = 'none';
}

const showErrorMessage = (message) => {
    errorMessageDiv.textContent = message;
    errorMessageDiv.style.display = 'block';
}

const handleBackButtonClick = () => {
    window.location.href = './main.html';
};

const init = () => {
    fileInput.addEventListener('focus', hideErrorMessage);
    nameInput.addEventListener('focus', hideErrorMessage);
    descriptionInput.addEventListener('focus', hideErrorMessage);
    backButton.addEventListener('click', handleBackButtonClick);

    uploadForm.addEventListener('submit', async (event) => {
        event.preventDefault();
        hideErrorMessage();

        const file = fileInput.files[0];
        const name = nameInput.value.trim();
        const description = descriptionInput.value.trim();

        if (!file) {
            showErrorMessage('Please select an image to upload.');
            return;
        }

        if (!name) {
            showErrorMessage('Image name is required.');
            return;
        }

        const formData = new FormData();
        formData.append('file', file);
        formData.append('name', name);
        formData.append('description', description);
        formData.append('accountId', getUserId());

        try {
            await uploadProfileImage(formData);
            window.location.href = './main.html';
        } catch (error) {
            showErrorMessage(`Upload failed: ${error.message}`);
        }
    });
}

init();
