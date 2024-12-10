import { getUserInfo, updateField } from '../services/userInfoService.js';

const init = async () => {
    const form = document.getElementById('editUserInfoForm');

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
            alert('No user information found.');
            window.location.href = './userinfoadd.html';
        }
    } catch (error) {
        alert(`Failed to load user info: ${error.message}`);
        return;
    }

    form.querySelectorAll('button').forEach(button => {
        button.addEventListener('click', async () => {
            const field = button.dataset.field;
            const value = form.elements[field].value;

            try {
                await updateField(accountId, field, value);
                alert(`${field} updated successfully.`);
            } catch (error) {
                alert(`Failed to update ${field}: ${error.message}`);
            }
        });
    });
};

init();
