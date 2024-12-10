import { saveUserInfo } from '../services/userInfoService.js';

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
            ApartmentNumber: apartmentNumber.value || null,
        },
    };

    try {
        await saveUserInfo(userInfo);
        alert('User information saved successfully!');
        window.location.href = './main.html';
    } catch (error) {
        alert(`Failed to save user information: ${error.message}`);
    }
};

const init = () => {
    const saveButton = document.getElementById('saveButton');
    saveButton.addEventListener('click', handleSaveButtonClick);
};

init();