import { getUserInfo, updateField } from '../services/userInfoService.js';
import { isLoggedIn } from '../services/sessionService.js';

if (!isLoggedIn()) {
    window.location.href = './index.html';
};

const init = async () => {
    const form = document.getElementById('editUserInfoForm');
    const backButton = document.getElementById('backButton');
    const errorMessageDiv = document.getElementById('error-message');
    
    backButton.addEventListener('click', handleBackButtonClick);

    const accountId = sessionStorage.getItem('user_id');

    if (!accountId) {
        alert('User ID not found. Please log in again.');
        return;
    }

    try {
        const userInfo = await getUserInfo();

        if (userInfo) {
            form.elements['firstName'].value = userInfo.firstName || '';
            form.elements['lastName'].value = userInfo.lastName || '';
            form.elements['personalCode'].value = userInfo.personalCode || '';
            form.elements['phoneNumber'].value = userInfo.phoneNumber || '';
            form.elements['email'].value = userInfo.email || '';
            form.elements['city'].value = userInfo.address?.city || '';
            form.elements['street'].value = userInfo.address?.street || '';
            form.elements['houseNumber'].value = userInfo.address?.houseNumber || '';
            form.elements['apartmentNumber'].value = userInfo.address?.apartmentNumber || '';
        } else {
            window.location.href = 'userinfoadd.html';
        }
    } catch (error) {
        errorMessageDiv.style.display = 'block';
        errorMessageDiv.textContent = `Failed to load user info: ${error.message}`;
        return;
    }

    form.querySelectorAll('button').forEach(button => {
        button.addEventListener('click', async () => {
            const field = button.dataset.field;
            const value = form.elements[field].value;

            try {
                await updateField(accountId, field, value);
                form.elements[field].disabled = true;
            } catch (error) {
                errorMessageDiv.style.display = 'block';
                errorMessageDiv.textContent = `Failed to update ${field}: ${error.message}`;
            }
        });
    });

    const formInputs = document.querySelectorAll('#editUserInfoForm input');
    formInputs.forEach(input => input.addEventListener('focus', hideErrorMessage));
};

const handleBackButtonClick = () => {
    window.location.href = './main.html';
};

const hideErrorMessage = () => {
    const errorMessageDiv = document.getElementById('error-message');
    errorMessageDiv.style.display = 'none';
};

init();
