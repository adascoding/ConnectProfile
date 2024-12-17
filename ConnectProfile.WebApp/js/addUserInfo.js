import { saveUserInfo } from '../services/userInfoService.js';
import { isLoggedIn } from '../services/sessionService.js';

if (!isLoggedIn()) {
    window.location.href = './index.html';
};

const handleSaveButtonClick = async (event) => {
    event.preventDefault();

    const form = document.getElementById('addUserInfoForm');
    const { firstName, lastName, personalCode, phoneNumber, email, city, street, houseNumber, apartmentNumber } = form;

    const userInfo = {
        FirstName: firstName.value,
        LastName: lastName.value,
        PersonalCode: personalCode.value,
        PhoneNumber: phoneNumber.value,
        Email: email.value,
        Address: {
            City: city.value,
            Street: street.value,
            HouseNumber: houseNumber.value,
            ApartmentNumber: apartmentNumber.value,
        },
    };

    const errorMessageDiv = document.getElementById('error-message');
    try {
        await saveUserInfo(userInfo);
        window.location.href = './main.html';
    } catch (error) {
        errorMessageDiv.style.display = 'block';
        errorMessageDiv.textContent = `Failed to save user information: ${error.message}`;
    }
};

const handleBackButtonClick = () => {
    window.location.href = './main.html';
};

const hideErrorMessage = () => {
    const errorMessageDiv = document.getElementById('error-message');
    errorMessageDiv.style.display = 'none';
};

const init = () => {
    const saveButton = document.getElementById('saveButton');
    saveButton.addEventListener('click', handleSaveButtonClick);

    const backButton = document.getElementById('backButton');
    backButton.addEventListener('click', handleBackButtonClick);

    const formInputs = document.querySelectorAll('#addUserInfoForm input');
    formInputs.forEach(input => input.addEventListener('focus', hideErrorMessage));
};

init();
