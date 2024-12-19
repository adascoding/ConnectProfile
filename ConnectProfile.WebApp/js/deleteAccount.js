import { getToken, logout, isLoggedIn } from '../services/sessionService.js';

if (!isLoggedIn()) {
    window.location.href = './index.html';
}

const deleteAccountForm = document.getElementById("deleteAccountForm");
const deleteResponseMessage = document.getElementById("response-message");
const loadingErrorMessage = document.getElementById("loading-error-message");
const logoutButton = document.getElementById('logout-button');
const usersTable = document.getElementById("usersList");

const loadUsers = async () => {
    loadingErrorMessage.style.display = "none";

    try {
        const response = await fetch("https://localhost:7187/api/Account/users", {
            method: "GET",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${getToken()}`
            }
        });

        const data = await response.json();

        if (response.ok && data.success) {
            const users = data.data;
            usersTable.innerHTML = '';

            users.forEach(user => {
                const row = document.createElement('tr');
                const userIdCell = document.createElement('td');
                const userNameCell = document.createElement('td');
                userIdCell.textContent = user.id;
                userNameCell.textContent = user.userName;
                row.appendChild(userIdCell);
                row.appendChild(userNameCell);
                usersTable.appendChild(row);
            });
        } else {
            loadingErrorMessage.style.display = "block";
            loadingErrorMessage.style.color = "red";
            loadingErrorMessage.textContent = data.message || "Failed to load users.";
        }
    } catch (error) {
        loadingErrorMessage.style.display = "block";
        loadingErrorMessage.style.color = "red";
        loadingErrorMessage.textContent = `An error occurred: ${error.message}`;
    }
};

loadUsers();

deleteAccountForm.addEventListener("submit", async (e) => {
    e.preventDefault();

    const accountId = document.getElementById("accountId").value.trim();
    deleteResponseMessage.style.display = "none";
    deleteResponseMessage.textContent = "";

    if (!accountId) {
        deleteResponseMessage.style.display = "block";
        deleteResponseMessage.style.color = "red";
        deleteResponseMessage.textContent = "Please enter a valid Account ID.";
        return;
    }

    try {
        const response = await fetch(`https://localhost:7187/api/Account/${accountId}`, {
            method: "DELETE",
            headers: {
                "Content-Type": "application/json",
                "Authorization": `Bearer ${getToken()}`
            }
        });

        const data = await response.json();

        deleteResponseMessage.style.display = "block";
        if (response.ok) {
            deleteResponseMessage.style.color = "green";
            deleteResponseMessage.textContent = data.message || "Account deleted successfully.";
            loadUsers(); 
        } else {
            deleteResponseMessage.style.color = "red";
            deleteResponseMessage.textContent = data.message || "Failed to delete the account.";
        }
    } catch (error) {
        deleteResponseMessage.style.display = "block";
        deleteResponseMessage.style.color = "red";
        deleteResponseMessage.textContent = `An error occurred: ${error.message}`;
    }
    loadUsers(); 
});

logoutButton.onclick = () => {
    logout();
    window.location.href = './login.html';
};
