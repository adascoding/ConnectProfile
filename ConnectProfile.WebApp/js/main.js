import { getProfileImage } from '../services/imageService.js';
import { getUserInfo } from '../services/userInfoService.js';
import { getUserId, getUsername, logout, isLoggedIn } from '../services/sessionService.js';

if (!isLoggedIn()) {
    window.location.href = './index.html';
};

const usernameElement = document.getElementById('username');
const profileImageElement = document.getElementById('profile-image');
const uploadImageButton = document.getElementById('upload-image-button');
const userInfoContainer = document.getElementById('user-info-container');
const editInfoButton = document.getElementById('edit-info-button');
const logoutButton = document.getElementById('logout-button');
const username = getUsername();
const userId = getUserId();

const updateProfileInfo = async () => {

    usernameElement.textContent = username;

    const setProfileImage = async () => {
        try {
            const imageBlob = await getProfileImage(userId);
            profileImageElement.src = imageBlob ? URL.createObjectURL(imageBlob) : '../assets/profile.jpg';
        } catch (error) {
            console.error('Error fetching profile image:', error);
            profileImageElement.src = '../assets/profile.jpg';
        }
    };

    const setUserInfo = async () => {
        try {
            const userInfo = await getUserInfo(userId);

            if (userInfo) {
                userInfoContainer.innerHTML = `
                    <p><strong>First Name:</strong> ${userInfo.firstName || 'N/A'}</p>
                    <p><strong>Last Name:</strong> ${userInfo.lastName || 'N/A'}</p>
                    <p><strong>Personal Code:</strong> ${userInfo.personalCode || 'N/A'}</p>
                    <p><strong>Phone Number:</strong> ${userInfo.phoneNumber || 'N/A'}</p>
                    <p><strong>Email:</strong> ${userInfo.email || 'N/A'}</p>
                    <p><strong>City:</strong> ${userInfo.address.city || 'N/A'}</p>
                    <p><strong>Street:</strong> ${userInfo.address.street || 'N/A'}</p>
                    <p><strong>House Number:</strong> ${userInfo.address.houseNumber || 'N/A'}</p>
                    <p><strong>Apartment Number:</strong> ${userInfo.address.apartmentNumber || 'N/A'}</p>
                `;
                editInfoButton.textContent = 'Edit Info';
                editInfoButton.onclick = () => window.location.href = './userinfoedit.html';
            } else {
                userInfoContainer.innerHTML = `<p>No user information found. Please add your details.</p>`;
                editInfoButton.textContent = 'Add Info';
                editInfoButton.onclick = () => window.location.href = './userinfoadd.html';
            }
        } catch (error) {
            console.error('Error fetching user info:', error);
            userInfoContainer.innerHTML = `<p>Error loading user information.</p>`;
            editInfoButton.textContent = 'Add Info';
            editInfoButton.onclick = () => window.location.href = './userinfoadd.html';
        }
    };

    uploadImageButton.onclick = () => window.location.href = './upload.html';

    logoutButton.onclick = () => {
        logout();
        window.location.href = './login.html';
    };

    await setProfileImage();
    await setUserInfo();
};

const init = () => {
    updateProfileInfo();
};

init();
